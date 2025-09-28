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
	private static GenerationTask? GetMethodSyntaxIfTarget(GeneratorSyntaxContext context)
	{
		// Get the information on the method declaration.
		var methodDeclaration = (MethodDeclarationSyntax)context.Node;
		var methodSymbol = context.SemanticModel.GetDeclaredSymbol(methodDeclaration);
		if (methodSymbol == null)
		{
			return null;
		}
		var asyncName = methodSymbol.Name + "Async";

		// Build the generation task.
		var generationTask = new GenerationTask(methodDeclaration, methodSymbol, context.SemanticModel, asyncName);

		// Check that the method is actually decorated with the GenerateAsyncOverloadAttribute and not any other attribute.
		foreach (var attr in methodDeclaration.AttributeLists.SelectMany(attributeList => attributeList.Attributes))
		{
			var constructor = context.SemanticModel.GetSymbolInfo(attr).Symbol;
			if (
				constructor is IMethodSymbol attributeConstructor
				&& attributeConstructor.ContainingType.ToDisplayString()
					== typeof(GenerateAsyncOverloadAttribute).FullName
			)
			{
				// If so, add the generation task.
				return generationTask;
			}
		}

		// Otherwise skip this method.
		return null;
	}

	private record GenerationTask
	{
		public GenerationTask(
			MethodDeclarationSyntax methodDeclaration,
			IMethodSymbol methodSymbol,
			SemanticModel semanticModel,
			string asyncName
		)
		{
			MethodDeclaration = methodDeclaration;
			MethodSymbol = methodSymbol;
			SemanticModel = semanticModel;
			AsyncName = asyncName;
		}

		public MethodDeclarationSyntax MethodDeclaration { get; set; }
		public IMethodSymbol MethodSymbol { get; set; }
		public SemanticModel SemanticModel { get; set; }
		public string AsyncName { get; set; }
	}

	/// <summary>
	/// Generates each async overload in a dedicated partial class.
	/// </summary>
	/// <param name="context">The source production context to register the generated partial classes.</param>
	/// <param name="generatorInformation">The list of method declarations and the compilation for interpretation of their semantics.</param>
	private static void Generate(SourceProductionContext context, ImmutableArray<GenerationTask?> generationTasks)
	{
		try
		{
			var nullSafeGenerationTasks = generationTasks.OfType<GenerationTask>().ToList();

			// First map out all async methods that will be generated.
			// This serves as information for other methods, that there will be an async overload that can be called.

			var toBeGeneratedAsyncMethodNamesBySyncOverload = nullSafeGenerationTasks.ToDictionary<
				GenerationTask,
				IMethodSymbol,
				string
			>(task => task.MethodSymbol!, task => task.AsyncName!, SymbolEqualityComparer.Default);

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

					// Auto generated marker.
					sb.AppendLine("// <auto-generated/>");
					sb.AppendLine();

					// Namespace declaration.
					sb.AppendLine($"namespace {typeNamespace};");
					sb.AppendLine();

					// Usings are not needed, because types are spelled out with their fully qualified names.

					// Rebuild class declaration with modifiers
					sb.AppendLine(BuildClassDeclarationHeader(typeDeclaration));
					sb.AppendLine("{");

					// Inherit the documentation from the original.
					sb.AppendLine(IndentHelper.Indent(BuildInheritdoc(methodSymbol)));

					// Find all awaitable overloads in the methods.
					var awaitableOverloadLocator = new AwaitableOverloadLocator(
						toBeGeneratedAsyncMethodNamesBySyncOverload
					);
					var awaitableOverloads = awaitableOverloadLocator.FindAwaitableOverloadsInMethod(
						methodDeclaration,
						semanticModel
					);

					// Build the async signature.
					sb.AppendLine(IndentHelper.Indent(BuildAsyncMethodSignature(generationTask, awaitableOverloads)));

					// Body: naive clone + replace Foo() -> await FooAsync()
					sb.AppendLine(IndentHelper.Indent(BuildAsyncMethodBody(generationTask, awaitableOverloads)));

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

	private static string BuildAsyncMethodSignature(
		GenerationTask generationTask,
		Dictionary<IMethodSymbol, string> awaitableOverloads
	)
	{
		// Return type.
		var hasYieldStatements = generationTask
			.MethodDeclaration!.DescendantNodes()
			.OfType<YieldStatementSyntax>()
			.Any();
		string? returnType;
		if (generationTask.MethodSymbol!.ReturnsVoid)
		{
			returnType = "System.Threading.Tasks.Task";
		}
		else if (hasYieldStatements)
		{
			returnType = generationTask.MethodSymbol!.ReturnType.ToDisplayString();
			returnType = returnType.Replace("IEnumerable", "IAsyncEnumerable");
		}
		else
		{
			returnType = $"System.Threading.Tasks.Task<{generationTask.MethodSymbol!.ReturnType.ToDisplayString()}>";
		}

		// Modifiers.

		var methodModifiers = generationTask.MethodDeclaration!.Modifiers.Select(m => m.Text).ToList();

		// Only add "async" if at least one call is awaited.
		if (awaitableOverloads.Any() || hasYieldStatements)
		{
			methodModifiers.Add("async");
		}

		// Parameters
		var parameters = string.Join(
			", ",
			generationTask.MethodSymbol!.Parameters.Select(p => $"{p.Type.ToDisplayString()} {p.Name}")
		);
		var typeParameterString = string.Empty;
		if (generationTask.MethodSymbol.TypeParameters.ToList() is { Count: > 0 } typeParameters)
		{
			typeParameterString =
				"<" + string.Join(", ", typeParameters.Select(typeParameter => typeParameter.ToDisplayString())) + ">";
		}

		return $"{string.Join(" ", methodModifiers)} {returnType} {generationTask.AsyncName}{typeParameterString}({parameters})";
	}

	private static string BuildAsyncMethodBody(
		GenerationTask generationTask,
		Dictionary<IMethodSymbol, string> awaitableOverloads
	)
	{
		var translator = new AsyncTranslator();
		return translator.TranslateMethodBody(
			generationTask.MethodDeclaration!,
			generationTask.SemanticModel,
			generationTask.MethodSymbol!,
			awaitableOverloads
		);
	}
}
