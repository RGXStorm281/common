#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace RobinEpple.Common.Forms.Wrappers;

using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using RobinEpple.Common.Forms.Wrappers.Abstractions;

[Generator]
public class FormWrapperGenerator : IIncrementalGenerator
{
	private static readonly DiagnosticDescriptor _globalFailure = new DiagnosticDescriptor(
		id: "REFWRAPGEN01",
		title: "Global exception at form wrapper generation",
		messageFormat: "A global exception has occurred during the generation of form wrappers: {0}",
		category: "SourceGeneration",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true
	);
	private static readonly DiagnosticDescriptor _methodSpecificFailure = new DiagnosticDescriptor(
		id: "REFWRAPGEN02",
		title: "Exception at form wrapper generation",
		messageFormat: "An exception has been thrown during the generation of form wrapper {0}: {1}",
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
				transform: static (context, _) => GetGenerationTaskForTarget(context)
			)
			.Where(m => m is not null);

		// Process all methods in one run, because one async method needs to know what other async
		// methods will be generated around it.

		// Register the source code factory.
		context.RegisterSourceOutput(methods, Generate);
	}

	/// <summary>
	/// Checks if the syntax node is a method with attributes.
	/// </summary>
	private static bool IsAttributedMethod(SyntaxNode node) =>
		node is MethodDeclarationSyntax m && m.AttributeLists.Count > 0;

	/// <summary>
	/// Checks whether the given method has an <see cref="WrapFormStructureAttribute"/> and if so,<br/>
	/// casts the method declaration syntax for further processing.
	/// </summary>
	private static FormWrapperGenerationTask? GetGenerationTaskForTarget(GeneratorSyntaxContext context)
	{
		// Get the information on the method declaration.
		var methodDeclaration = (MethodDeclarationSyntax)context.Node;
		var methodSymbol = context.SemanticModel.GetDeclaredSymbol(methodDeclaration);
		if (methodSymbol == null)
		{
			return null;
		}

		// Check that the method is actually decorated with the WrapFormStructureAttribute and not any other attribute.
		var formWrapperAttribute = methodSymbol
			.GetAttributes()
			.FirstOrDefault(attr =>
				attr.AttributeClass?.ToDisplayString() == typeof(WrapFormStructureAttribute).FullName
			);
		if (formWrapperAttribute == null)
		{
			// Otherwise skip this method.
			return null;
		}

		// Try to isolate the form property name from the constructor arguments.
		if (formWrapperAttribute.ConstructorArguments.Length < 1)
		{
			return null;
		}

		var formPropertyName = formWrapperAttribute.ConstructorArguments[0].Value as string;
		if (formPropertyName == null)
		{
			// If the argument is not a string literal or nameof() -> unsupported, skip generation.
			return null;
		}

		// The wrapper property name is optional.
		var wrapperPropertyName = formPropertyName + "Wrapper";
		if (formWrapperAttribute.ConstructorArguments.Length > 1)
		{
			var wrapperPropertyNameArg = formWrapperAttribute.ConstructorArguments[1].Value as string;
			wrapperPropertyName = wrapperPropertyNameArg ?? wrapperPropertyName;
		}

		// Return the generation task.
		return new FormWrapperGenerationTask(
			methodDeclaration,
			methodSymbol,
			context.SemanticModel,
			formPropertyName,
			wrapperPropertyName
		);
	}

	/// <summary>
	/// Generates each async overload in a dedicated partial class.
	/// </summary>
	/// <param name="context">The source production context to register the generated partial classes.</param>
	/// <param name="generationTask">The generation task for a single form wrapper.</param>
	private static void Generate(SourceProductionContext context, FormWrapperGenerationTask? generationTask)
	{
		var logger = new MessageLogger(context);
		if (generationTask == null)
		{
			// skip.
			return;
		}
		try
		{
			var methodDeclaration = generationTask.MethodDeclaration;
			var methodSymbol = generationTask.MethodSymbol;
			var semanticModel = generationTask.SemanticModel;
			var formPropertyName = generationTask.FormPropertyName;
			var wrapperPropertyName = generationTask.WrapperPropertyName;

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
			// Always enable nullable.
			sb.AppendLine("#nullable enable");
			sb.AppendLine();

			// Namespace declaration.
			sb.AppendLine($"namespace {typeNamespace};");
			sb.AppendLine();

			// Rebuild class declaration with modifiers
			sb.AppendLine(BuildClassDeclarationHeader(typeDeclaration));
			sb.AppendLine("{");

			var structure = new StaticFormStructureParser().ParseStaticFormStructure(
				methodDeclaration,
				semanticModel,
				logger,
				typeSymbol
			);
			var wrapperTypeName = structure.GetFullyQualifiedTypeName();
			sb.Append(IndentHelper.Indent(structure.PrintWrapperType()));
			sb.AppendLine(
				IndentHelper.Indent(
					$"public {wrapperTypeName} {wrapperPropertyName} => new {wrapperTypeName}({formPropertyName});"
				)
			);

			// Close class.
			sb.AppendLine("}");

			// Build a unique name for each generated wrapper.
			var sourceName = $"{typeDeclaration.Identifier.Text}.{wrapperPropertyName}.g.cs";

			// Add the source code to the compilation.
			context.AddSource(sourceName, SourceText.From(sb.ToString(), Encoding.UTF8));
		}
		catch (Exception ex)
		{
			var diagnostic = Diagnostic.Create(
				_methodSpecificFailure,
				Location.None,
				generationTask.WrapperPropertyName,
				FlattenException(ex)
			);
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
}
