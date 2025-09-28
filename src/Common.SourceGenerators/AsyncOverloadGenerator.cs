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

		// Combine with the compilation context to have access class semantics.
		var compilationAndMethods = context.CompilationProvider.Combine(methods.Collect());

		// Register the source code factory.
		context.RegisterSourceOutput(compilationAndMethods, Generate);
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
	private static MethodDeclarationSyntax? GetMethodSyntaxIfTarget(GeneratorSyntaxContext context)
	{
		var methodDeclaration = (MethodDeclarationSyntax)context.Node;

		foreach (var attr in methodDeclaration.AttributeLists.SelectMany(attributeList => attributeList.Attributes))
		{
			var symbol = context.SemanticModel.GetSymbolInfo(attr).Symbol;
			if (
				symbol is IMethodSymbol methodSymbol
				&& methodSymbol.ContainingType.ToDisplayString() == typeof(GenerateAsyncOverloadAttribute).FullName
			)
			{
				return methodDeclaration;
			}
		}

		return null;
	}

	private record GenerationTask
	{
		public MethodDeclarationSyntax? MethodDeclaration { get; set; }
		public IMethodSymbol? MethodSymbol { get; set; }
		public SemanticModel? SemanticModel { get; set; }
		public string? AsyncName { get; set; }
		public bool IsComplete =>
			MethodDeclaration != null && MethodSymbol != null && SemanticModel != null && AsyncName != null;
	}

	/// <summary>
	/// Generates each async overload in a dedicated partial class.
	/// </summary>
	/// <param name="context">The source production context to register the generated partial classes.</param>
	/// <param name="generatorInformation">The list of method declarations and the compilation for interpretation of their semantics.</param>
	private static void Generate(
		SourceProductionContext context,
		(Compilation Compilation, ImmutableArray<MethodDeclarationSyntax?> MethodDeclarations) generatorInformation
	)
	{
		var compilation = generatorInformation.Compilation;

		// First map out all async methods that will be generated.
		// This serves as information for other methods, that there will be an async overload that can be called.
		var generationTasks = generatorInformation
			.MethodDeclarations.Select(methodDeclaration =>
			{
				if (methodDeclaration == null)
				{
					return new GenerationTask();
				}

				var semanticModel = compilation.GetSemanticModel(methodDeclaration.SyntaxTree);
				var methodSymbol = semanticModel.GetDeclaredSymbol(methodDeclaration);
				if (methodSymbol == null)
				{
					return new GenerationTask();
				}

				var asyncName = methodSymbol.Name + "Async";
				return new GenerationTask
				{
					MethodDeclaration = methodDeclaration,
					MethodSymbol = methodSymbol,
					SemanticModel = semanticModel,
					AsyncName = asyncName,
				};
			})
			.Where(task => task.IsComplete)
			.ToList();

		var toBeGeneratedAsyncMethodNamesBySyncOverload = generationTasks.ToDictionary<
			GenerationTask,
			IMethodSymbol,
			string
		>(task => task.MethodSymbol!, task => task.AsyncName!, SymbolEqualityComparer.Default);

		// Then generate each method iteratively.
		foreach (var generationTask in generationTasks)
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

			var typeSymbol = compilation
				.GetSemanticModel(typeDeclaration.SyntaxTree)
				.GetDeclaredSymbol(typeDeclaration);
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
			var awaitableOverloadLocator = new AwaitableOverloadLocator(toBeGeneratedAsyncMethodNamesBySyncOverload);
			var awaitableOverloads = awaitableOverloadLocator.FindAwaitableOverloadsInMethod(
				methodDeclaration,
				semanticModel
			);

			// Build the async signature.
			sb.AppendLine(IndentHelper.Indent(BuildAsyncMethodSignature(generationTask, awaitableOverloads)));

			// Body: naive clone + replace Foo() -> await FooAsync()
			sb.AppendLine(IndentHelper.Indent(BuildAsyncMethodBody(generationTask, compilation, awaitableOverloads)));

			// Close class.
			sb.AppendLine("}");

			// Add the source code to the compilation.
			context.AddSource(
				$"{typeDeclaration.Identifier.Text}.{methodSymbol.Name}.Async.g.cs",
				SourceText.From(sb.ToString(), Encoding.UTF8)
			);
		}
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
		return $"/// <inheritdoc cref=\"{methodSymbol.Name}({parameterTypes})\"/>";
	}

	private static string BuildAsyncMethodSignature(
		GenerationTask generationTask,
		Dictionary<IMethodSymbol, string> awaitableOverloads
	)
	{
		// Return type.
		var returnType = generationTask.MethodSymbol!.ReturnsVoid
			? "System.Threading.Tasks.Task"
			: $"System.Threading.Tasks.Task<{generationTask.MethodSymbol!.ReturnType.ToDisplayString()}>";

		// Modifiers.
		var methodModifiers = generationTask.MethodDeclaration!.Modifiers.Select(m => m.Text).ToList();

		// Only add "async" if at least one call is awaited.
		if (awaitableOverloads.Any())
		{
			methodModifiers.Add("async");
		}

		// Parameters
		var parameters = string.Join(
			", ",
			generationTask.MethodSymbol!.Parameters.Select(p => $"{p.Type.ToDisplayString()} {p.Name}")
		);

		return $"{string.Join(" ", methodModifiers)} {returnType} {generationTask.AsyncName}({parameters})";
	}

	private static string BuildAsyncMethodBody(
		GenerationTask generationTask,
		Compilation compilation,
		Dictionary<IMethodSymbol, string> awaitableOverloads
	)
	{
		var semanticModel = compilation.GetSemanticModel(generationTask.MethodDeclaration!.SyntaxTree);

		var translator = new AsyncTranslator();
		return translator.TranslateMethodBody(
			generationTask.MethodDeclaration!,
			semanticModel,
			generationTask.MethodSymbol!,
			awaitableOverloads
		);
	}
}
