#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace RobinEpple.Common.SourceGenerators;

using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using RobinEpple.Common.SourceGenerators.Abstractions;

/// <summary>
/// Generates async overloads for all marked methods.
/// </summary>
[Generator]
public class AsyncOverloadGenerator : IIncrementalGenerator
{
	private static readonly DiagnosticDescriptor _globalFailure = new DiagnosticDescriptor(
		id: "REASYNCGEN01",
		title: "Global exception at async overload generation",
		messageFormat: "A global exception has occurred during the generation of async methods: {0}",
		category: "SourceGeneration",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true
	);
	private static readonly DiagnosticDescriptor _methodSpecificFailure = new DiagnosticDescriptor(
		id: "REASYNCGEN02",
		title: "Exception at async overload generation",
		messageFormat: "An exception has been thrown during the generation of method {0}: {1}",
		category: "SourceGeneration",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true
	);

	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		// Find candidate methods that have a [GenerateAsyncOverload] attribute.
		var methods = context
			.SyntaxProvider.CreateSyntaxProvider(
				predicate: static (syntaxNode, _) => IsAttributedMethod(syntaxNode),
				transform: static (context, _) => GetMethodSyntaxIfTarget(context)
			)
			.Where(m => m is not null);

		// Process all methods in one run, because one async method needs to know what other async
		// methods will be generated around it.
		var allMethods = methods.Collect();

		// Register the source code factory.
		context.RegisterSourceOutput(allMethods, Generate);
	}

	/// <summary>
	/// Checks if the syntax node is a method with attributes.
	/// </summary>
	private static bool IsAttributedMethod(SyntaxNode node) =>
		node is MethodDeclarationSyntax m && m.AttributeLists.Count > 0;

	/// <summary>
	/// Checks whether the given method has an <see cref="GenerateAsyncOverloadAttribute"/> and if so,<br/>
	/// casts the method declaration syntax for further processing.
	/// </summary>
	private static AsyncOverloadGenerationTask? GetMethodSyntaxIfTarget(GeneratorSyntaxContext context)
	{
		// Get information about the method declaration.
		var methodDeclaration = (MethodDeclarationSyntax)context.Node;
		var methodSymbol = context.SemanticModel.GetDeclaredSymbol(methodDeclaration);
		if (methodSymbol == null)
		{
			return null;
		}
		var asyncName = methodSymbol.Name + "Async";
		var typeDeclaration = methodDeclaration.FirstAncestorOrSelf<TypeDeclarationSyntax>();

		// Collect all attributes.
		var methodAttributes = methodDeclaration.AttributeLists.SelectMany(attributeList => attributeList.Attributes);
		var typeAttributes =
			typeDeclaration?.AttributeLists.SelectMany(attributeList => attributeList.Attributes) ?? [];
		var allAttributes = methodAttributes.Concat(typeAttributes);

		// Scan attributes for extension namespace whitelisting and generator attribute.
		var extensionNamespaces = new List<string>();
		var hasGenerateAsyncOverloadAttribute = false;

		// Scan the method's own attributes for GenerateAsyncOverloadAttribute and AsyncOverloadExtensionNamespaceAttribute.
		foreach (var attr in allAttributes)
		{
			var constructor = context.SemanticModel.GetSymbolInfo(attr).Symbol;
			if (constructor is not IMethodSymbol attributeConstructor)
			{
				continue;
			}

			var attributeType = attributeConstructor.ContainingType.ToDisplayString();
			if (attributeType == typeof(GenerateAsyncOverloadAttribute).FullName)
			{
				hasGenerateAsyncOverloadAttribute = true;
			}
			else if (attributeType == typeof(AsyncOverloadExtensionNamespaceAttribute).FullName)
			{
				var namespaceName = GetFirstStringArgument(attr, context.SemanticModel);
				if (namespaceName != null)
				{
					extensionNamespaces.Add(namespaceName);
				}
			}
		}

		// If a GenerateAsyncOverloadAttribute was found, return a generation task, otherwise skip this method.
		if (!hasGenerateAsyncOverloadAttribute)
		{
			return null;
		}

		return new AsyncOverloadGenerationTask(
			methodDeclaration,
			methodSymbol,
			context.SemanticModel,
			asyncName,
			[.. extensionNamespaces]
		);
	}

	private static string? GetFirstStringArgument(AttributeSyntax attr, SemanticModel semanticModel)
	{
		var argument = attr.ArgumentList?.Arguments.FirstOrDefault();
		if (argument == null)
		{
			return null;
		}
		var constant = semanticModel.GetConstantValue(argument.Expression);
		return constant.HasValue ? constant.Value as string : null;
	}

	/// <summary>
	/// Generates each async overload in a dedicated partial class.
	/// </summary>
	/// <param name="context">The source production context to register the generated partial classes.</param>
	/// <param name="generationTasks">The list of generation tasks.</param>
	private static void Generate(
		SourceProductionContext context,
		ImmutableArray<AsyncOverloadGenerationTask?> generationTasks
	)
	{
		try
		{
			var nullSafeGenerationTasks = generationTasks.OfType<AsyncOverloadGenerationTask>().ToList();

			// First map out all async methods that will be generated.
			// This serves as information for other methods, that there will be an async overload that can be called.

			var toBeGeneratedAsyncMethodNamesBySyncOverload = nullSafeGenerationTasks.ToDictionary<
				AsyncOverloadGenerationTask,
				IMethodSymbol,
				string
			>(task => task.MethodSymbol!, task => task.AsyncName!, SymbolEqualityComparer.Default);
			var awaitableOverloadLocator = new AwaitableOverloadLocator(toBeGeneratedAsyncMethodNamesBySyncOverload);

			// Then generate each method iteratively.
			foreach (var generationTask in nullSafeGenerationTasks)
			{
				try
				{
					var methodDeclaration = generationTask.MethodDeclaration!;
					var methodSymbol = generationTask.MethodSymbol!;
					var semanticModel = generationTask.SemanticModel!;
					var asyncName = generationTask.AsyncName!;

					// Ground the method in its context:
					// Get the class name and namespace for the new partial class.
					var typeDeclaration = methodDeclaration.FirstAncestorOrSelf<TypeDeclarationSyntax>();
					if (typeDeclaration == null)
					{
						return;
					}

					var typeSymbol = semanticModel.GetDeclaredSymbol(typeDeclaration);
					if (typeSymbol == null)
					{
						return;
					}

					var typeNamespace = typeSymbol.ContainingNamespace.ToDisplayString();

					// Start building the file.
					var sb = new StringBuilder();

					// Enable nullable when the source is nullable enabled.
					if (semanticModel.GetNullableContext(methodDeclaration.SpanStart) != NullableContext.Disabled)
					{
						sb.AppendLine("#nullable enable");
					}

					// Auto generated marker.
					sb.AppendLine("// <auto-generated/>");
					sb.AppendLine();

					// Namespace declaration.
					sb.AppendLine($"namespace {typeNamespace};");
					sb.AppendLine();

					// Usings.
					var usingsInFile = typeDeclaration
						.SyntaxTree.GetRoot()
						.DescendantNodes()
						.OfType<UsingDirectiveSyntax>()
						.ToList();
					if (!usingsInFile.Any(usingInFile => usingInFile.Name?.ToString() == "System.Threading.Tasks"))
					{
						sb.AppendLine("using System.Threading.Tasks;");
					}
					foreach (var classUsing in usingsInFile)
					{
						sb.AppendLine(classUsing.WithoutTrivia().ToFullString());
					}
					sb.AppendLine();

					// Rebuild class declaration with modifiers
					sb.AppendLine(BuildClassDeclarationHeader(typeDeclaration));
					sb.AppendLine("{");

					// Inherit the documentation from the original.
					sb.AppendLine(IndentHelper.Indent(BuildInheritdoc(methodSymbol)));

					// Find all awaitable overloads in the methods.
					awaitableOverloadLocator.FindAwaitableOverloadsInMethod(generationTask);

					// Build the async signature.
					sb.AppendLine(IndentHelper.Indent(BuildAsyncMethodSignature(generationTask)));

					// Body: naive clone + replace Foo() -> await FooAsync()
					sb.AppendLine(IndentHelper.Indent(BuildAsyncMethodBody(generationTask)));

					// Close class.
					sb.AppendLine("}");

					// Build a unique name for each overload.
					var sourceName = $"{typeDeclaration.Identifier.Text}.{methodSymbol.Name}";
					foreach (var typeParam in methodSymbol.TypeParameters)
					{
						sourceName += "_" + typeParam.Name;
					}
					foreach (var param in methodSymbol.Parameters)
					{
						sourceName += "_" + param.Name;
					}
					sourceName += $".Async.g.cs";

					// Add the source code to the compilation.
					context.AddSource(sourceName, SourceText.From(sb.ToString(), Encoding.UTF8));
				}
				catch (Exception ex)
				{
					var diagnostic = Diagnostic.Create(
						_methodSpecificFailure,
						Location.None,
						generationTask.AsyncName,
						FlattenException(ex)
					);
					context.ReportDiagnostic(diagnostic);
				}
			}
		}
		catch (Exception ex)
		{
			var diagnostic = Diagnostic.Create(_globalFailure, Location.None, FlattenException(ex));
			context.ReportDiagnostic(diagnostic);
		}
	}

	private static string FlattenException(Exception ex)
	{
		return ex.ToString()
			+ (ex.InnerException != null ? "\nInner Exception:\n" + FlattenException(ex.InnerException) : "");
	}

	private static string BuildClassDeclarationHeader(TypeDeclarationSyntax typeDeclaration)
	{
		var sb = new StringBuilder();

		// Declare modifiers.
		var classModifiers = string.Join(" ", typeDeclaration.Modifiers.Select(m => m.Text));
		sb.Append(classModifiers);

		// Declare the class.
		sb.Append($" {typeDeclaration.Keyword.WithoutTrivia().ToFullString()} ");
		sb.Append(typeDeclaration.Identifier.Text);

		// Add type parameters if they exist.
		if (typeDeclaration.TypeParameterList != null)
		{
			sb.Append(typeDeclaration.TypeParameterList.ToFullString());
		}

		// Build the class declaration.
		return sb.ToString();
	}

	private static string BuildInheritdoc(IMethodSymbol methodSymbol)
	{
		var parameterTypes = string.Join(",", methodSymbol.Parameters.Select(p => p.Type.ToDisplayString()));
		var typeParameterString = string.Empty;
		if (methodSymbol.TypeParameters.ToList() is { Count: > 0 } typeParameters)
		{
			typeParameterString =
				"{" + string.Join(",", typeParameters.Select(typeParameter => typeParameter.ToDisplayString())) + "}";
		}
		return $"/// <inheritdoc cref=\"{methodSymbol.Name}{typeParameterString}({parameterTypes})\"/>";
	}

	private static string BuildAsyncMethodSignature(AsyncOverloadGenerationTask generationTask)
	{
		// Return type.
		string? returnType;
		if (generationTask.MethodSymbol!.ReturnsVoid)
		{
			returnType = "Task";
		}
		else if (generationTask.HasYieldStatements)
		{
			returnType = Print(generationTask.MethodDeclaration.ReturnType);
			returnType = returnType.Replace("IEnumerable", "IAsyncEnumerable");
		}
		else
		{
			returnType = $"Task<{Print(generationTask.MethodDeclaration.ReturnType)}>";
		}

		// Modifiers.
		var methodModifiers = generationTask.MethodDeclaration!.Modifiers.Select(m => m.Text).ToList();

		// Only add "async" if at least one call is awaited.
		if (generationTask.IsRunningAsync)
		{
			methodModifiers.Add("async");
		}

		// Parameters
		var parameters = Print(generationTask.MethodDeclaration.ParameterList);
		var typeParameterString = Print(generationTask.MethodDeclaration.TypeParameterList);

		return $"{string.Join(" ", methodModifiers)} {returnType} {generationTask.AsyncName}{typeParameterString}{parameters}";
	}

	private static string BuildAsyncMethodBody(AsyncOverloadGenerationTask generationTask)
	{
		var translator = new AsyncTranslator();
		return translator.TranslateMethodBody(generationTask);
	}

	private static string Print(SyntaxNode? node)
	{
		if (node == null)
		{
			return string.Empty;
		}
		return node.WithoutTrivia().ToFullString();
	}

	private static string Print(SyntaxToken? token)
	{
		if (token == null)
		{
			return string.Empty;
		}
		return token.Value.WithoutTrivia().ToFullString();
	}
}
