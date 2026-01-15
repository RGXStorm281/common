#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace RobinEpple.Common.SourceGenerators;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using RobinEpple.Common.SourceGenerators.Abstractions;

/// <summary>
/// Generates a static factory method for each constructor of each implementing type of the interface specified in the attribute.
/// </summary>
[Generator]
public class StaticFactoryGenerator : IIncrementalGenerator
{
	private static bool _waitForDebugger = false;

	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		while (_waitForDebugger && !Debugger.IsAttached)
		{
			Thread.Sleep(100);
		}

		// Find candidate classes that have a [StaticFactory] attribute.
		var factoryClasses = context
			.SyntaxProvider.CreateSyntaxProvider(
				predicate: FilterClassesWithAttributes,
				transform: ExtractStaticFactoryAttributes
			)
			.Where(generationTask => generationTask != null);

		// Combine with the bigger compilation context to have access to other classes in the assembly.
		var compilationAndFactories = factoryClasses.Combine(context.CompilationProvider);

		// Register the source code factory.
		context.RegisterSourceOutput(compilationAndFactories, ExecuteSourceProduction);
	}

	private bool FilterClassesWithAttributes(SyntaxNode node, CancellationToken cancellationToken)
	{
		return node is ClassDeclarationSyntax cds && cds.AttributeLists.Count > 0;
	}

	private StaticFactoryGenerationTask? ExtractStaticFactoryAttributes(
		GeneratorSyntaxContext ctx,
		CancellationToken cancellationToken
	)
	{
		var classDeclarationSyntax = (ClassDeclarationSyntax)ctx.Node;
		var classDeclaration = ctx.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax);

		if (classDeclaration is null)
		{
			return null;
		}

		var factoryAttributes = classDeclaration
			.GetAttributes()
			.Where(attr => attr.AttributeClass?.ToDisplayString() == typeof(StaticFactoryAttribute).FullName)
			.ToList();
		if (factoryAttributes.Count == 0)
		{
			return null;
		}

		return new StaticFactoryGenerationTask(
			classDeclarationSyntax,
			classDeclaration,
			factoryAttributes,
			ctx.SemanticModel
		);
	}

	private void ExecuteSourceProduction(
		SourceProductionContext spc,
		(StaticFactoryGenerationTask? GenerationTask, Compilation Compilation) generatorInformation
	)
	{
		if (generatorInformation.GenerationTask == null)
		{
			return;
		}

		var compilation = generatorInformation.Compilation;
		var factoryClass = generatorInformation.GenerationTask.FactoryClass;
		var attributes = generatorInformation.GenerationTask.Attributes;
		var generationTask = generatorInformation.GenerationTask;

		foreach (var attribute in attributes)
		{
			if (attribute == null)
			{
				continue;
			}

			if (attribute.ConstructorArguments.Length != 1)
			{
				continue;
			}

			var markerInterface = attribute.ConstructorArguments[0].Value as INamedTypeSymbol;
			if (markerInterface is null)
			{
				continue;
			}

			// Find all types that implement the marker interface
			var allTypes = GetAllTypes(generatorInformation.Compilation.Assembly.GlobalNamespace);
			var implementingTypes = allTypes.Where(t => IsImplementationOf(t, markerInterface));

			foreach (var implementingType in implementingTypes)
			{
				GenerateFactoryMethodForType(spc, compilation, factoryClass, markerInterface, implementingType);
			}
		}
	}

	private IEnumerable<INamedTypeSymbol> GetAllTypes(INamespaceSymbol ns)
	{
		foreach (var member in ns.GetMembers())
		{
			if (member is INamespaceSymbol childNs)
			{
				foreach (var type in GetAllTypes(childNs))
				{
					yield return type;
				}
			}
			else if (member is INamedTypeSymbol type)
			{
				yield return type;
				foreach (var nested in type.GetTypeMembers())
				{
					yield return nested;
				}
			}
		}
	}

	private bool IsImplementationOf(INamedTypeSymbol type, INamedTypeSymbol markerInterface)
	{
		if (type.IsAbstract)
		{
			return false;
		}

		if (type.TypeKind != TypeKind.Class && type.TypeKind != TypeKind.Struct)
		{
			return false;
		}

		foreach (var implementedInterface in type.AllInterfaces)
		{
			if (IsMarkerInterface(implementedInterface, markerInterface))
			{
				return true;
			}
		}

		return false;
	}

	private bool IsMarkerInterface(INamedTypeSymbol implementedInterface, INamedTypeSymbol markerInterface)
	{
		if (Normalize(implementedInterface).Equals(Normalize(markerInterface), SymbolEqualityComparer.Default))
		{
			return true;
		}
		return false;
	}

	private INamedTypeSymbol Normalize(INamedTypeSymbol interfaceType)
	{
		return interfaceType.OriginalDefinition;
	}

	private void GenerateFactoryMethodForType(
		SourceProductionContext spc,
		Compilation compilation,
		INamedTypeSymbol factoryClass,
		INamedTypeSymbol markerInterface,
		INamedTypeSymbol interfaceImplementation
	)
	{
		var sb = new StringBuilder();

		// Get the syntax for the implementing type.
		var implementationSyntax = interfaceImplementation.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax();
		if (implementationSyntax is not ClassDeclarationSyntax classDeclaration)
		{
			return;
		}

		// Auto generated marker.
		sb.AppendLine("// <auto-generated/>");
		sb.AppendLine();

		// Enable nullable when the source is nullable enabled.
		var semanticModel = compilation.GetSemanticModel(classDeclaration.SyntaxTree);
		if (semanticModel.GetNullableContext(classDeclaration.SpanStart) != NullableContext.Disabled)
		{
			sb.AppendLine("#nullable enable");
		}

		// Namespace declaration.
		sb.AppendLine($"namespace {factoryClass.ContainingNamespace.ToDisplayString()};");
		sb.AppendLine();

		// Usings.
		var usingsInFile = classDeclaration
			.SyntaxTree.GetRoot()
			.DescendantNodes()
			.OfType<UsingDirectiveSyntax>()
			.ToList();
		foreach (var classUsing in usingsInFile)
		{
			sb.AppendLine(classUsing.WithoutTrivia().ToFullString());
		}
		sb.AppendLine();

		// Factory class declaration.
		var accessModifier = string.Join(
			string.Empty,
			factoryClass.DeclaredAccessibility.ToString().Select(char.ToLowerInvariant)
		);
		sb.AppendLine($"{accessModifier} static partial class {factoryClass.Name}");
		sb.AppendLine("{");

		foreach (
			var constructor in interfaceImplementation.Constructors.Where(c =>
				c.DeclaredAccessibility is Accessibility.Public or Accessibility.Internal
			)
		)
		{
			// Reference the documentation of the constructor.
			sb.AppendLine(IndentHelper.Indent(BuildConstructorInheritdoc(constructor)));

			// Create the method signature.
			var parameterList = GetParameterList(constructor);
			var parameters = RenderParameters(semanticModel, parameterList);
			var constructorAccessModifier = constructor.DeclaredAccessibility.ToString().ToLower();
			var typeParameterString = string.Empty;
			if (constructor.ContainingType.TypeParameters.ToList() is { Count: > 0 } typeParameters)
			{
				typeParameterString =
					"<"
					+ string.Join(", ", typeParameters.Select(typeParameter => typeParameter.ToDisplayString()))
					+ ">";
			}

			var visibleType =
				interfaceImplementation.DeclaredAccessibility >= markerInterface.DeclaredAccessibility
					? interfaceImplementation
					: interfaceImplementation.AllInterfaces.First(implementedInterface =>
						IsMarkerInterface(implementedInterface, markerInterface)
					);

			var methodName = GetFactoryMethodName(interfaceImplementation);

			// Obsolete propagation.
			var obsolete = GetObsoleteAttribute(constructor) ?? GetObsoleteAttribute(constructor.ContainingType);
			if (obsolete != null)
			{
				sb.AppendLine(IndentHelper.Indent(obsolete));
			}

			sb.AppendLine(
				IndentHelper.Indent(
					$"{constructorAccessModifier} static {visibleType.ToDisplayString(_fullyQualifiedTypeFormat)} {methodName}{typeParameterString}({parameters})"
				)
			);

			if (classDeclaration.ConstraintClauses.Any())
			{
				sb.Append(IndentHelper.Indent(GetConstraintClauses(classDeclaration), 2));
			}

			// Call the constructor and return the new object (type parameters are already included in "ToDisplayString").
			var args = string.Join(", ", constructor.Parameters.Select(p => p.Name));
			sb.AppendLine(IndentHelper.Indent($"=> new {interfaceImplementation.ToDisplayString()}({args});", 2));
			sb.AppendLine();
		}

		// Close the class declaration.
		sb.AppendLine("}");

		// And add the file to the source code.
		spc.AddSource(
			$"{factoryClass.Name}.{interfaceImplementation.Name}.g.cs",
			SourceText.From(sb.ToString(), Encoding.UTF8)
		);
	}

	private static string GetConstraintClauses(ClassDeclarationSyntax classSyntax)
	{
		var sb = new StringBuilder();
		foreach (var constraintClause in classSyntax.ConstraintClauses)
		{
			sb.AppendLine(constraintClause.WithoutTrivia().NormalizeWhitespace().ToFullString());
		}
		return sb.ToString();
	}

	private static string BuildConstructorInheritdoc(IMethodSymbol constructor)
	{
		var typeRef = constructor
			.ContainingType.ToDisplayString(_fullyQualifiedTypeFormat)
			.Replace('<', '{')
			.Replace('>', '}');
		var constructorRef = constructor
			.ToDisplayString(_fullyQualifiedInheritdocFormat)
			.Replace('<', '{')
			.Replace('>', '}');
		return $"/// <inheritdoc cref=\"{typeRef}.{constructorRef}\"/>";
	}

	public static ParameterListSyntax? GetParameterList(IMethodSymbol methodSymbol)
	{
		// Constructors, methods, local functions all have declarations
		var syntaxRef = methodSymbol.DeclaringSyntaxReferences.FirstOrDefault();
		if (syntaxRef == null)
		{
			return null; // No source (e.g. metadata only)
		}

		var syntaxNode = syntaxRef.GetSyntax();

		// For constructors specifically:
		if (syntaxNode is ConstructorDeclarationSyntax constructorSyntax)
		{
			return constructorSyntax.ParameterList;
		}

		// For normal methods:
		if (syntaxNode is MethodDeclarationSyntax methodSyntax)
		{
			return methodSyntax.ParameterList;
		}

		// For primary constructors:
		if (syntaxNode is ClassDeclarationSyntax classDeclaration)
		{
			return classDeclaration.ParameterList;
		}

		return null;
	}

	public static string GetFactoryMethodName(INamedTypeSymbol implementingType)
	{
		var explicitNameAttribute = implementingType
			.GetAttributes()
			.Where(attr => attr.AttributeClass?.ToDisplayString() == typeof(StaticFactoryMethodNameAttribute).FullName)
			.FirstOrDefault();
		if (explicitNameAttribute == null)
		{
			return implementingType.Name;
		}

		if (explicitNameAttribute.ConstructorArguments.Length != 1)
		{
			return implementingType.Name;
		}

		var explicitName = explicitNameAttribute.ConstructorArguments[0].Value as string;
		if (explicitName is null)
		{
			return implementingType.Name;
		}

		return explicitName;
	}

	private static string RenderParameters(SemanticModel semanticModel, ParameterListSyntax? parameterList)
	{
		if (parameterList == null)
		{
			return string.Empty;
		}

		var parameterStrings = new List<string>();
		foreach (var parameter in parameterList.Parameters)
		{
			var parameterString = Print(semanticModel, parameter);
			parameterStrings.Add(parameterString);
		}

		return string.Join(", ", parameterStrings);
	}

	private static readonly SymbolDisplayFormat _fullyQualifiedTypeFormat = new SymbolDisplayFormat(
		globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Omitted,
		typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
		genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
		miscellaneousOptions: SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier
			| SymbolDisplayMiscellaneousOptions.UseSpecialTypes
	);

	private static readonly SymbolDisplayFormat _fullyQualifiedInheritdocFormat = new SymbolDisplayFormat(
		globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Omitted,
		typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
		genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
		memberOptions: SymbolDisplayMemberOptions.IncludeParameters,
		parameterOptions: SymbolDisplayParameterOptions.IncludeType,
		miscellaneousOptions: SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier
			| SymbolDisplayMiscellaneousOptions.UseSpecialTypes
	);

	private static string Print(SemanticModel model, ParameterSyntax parameter)
	{
		// Get the semantics.
		var symbol = model.GetDeclaredSymbol(parameter)!;

		// Preserve attributes.
		bool isThis = false;
		var attributes = new List<string>();
		foreach (var attr in symbol.GetAttributes())
		{
			var attributeType = attr.AttributeClass!.ToDisplayString(_fullyQualifiedTypeFormat);
			if (attributeType.EndsWith(".StaticFactoryThisAttribute", StringComparison.Ordinal))
			{
				// Convert [StaticFactoryThis] attributes to an extension method.
				isThis = true;
				continue;
			}

			var attributeArgs =
				attr.ConstructorArguments.Length == 0
					? ""
					: "("
						+ string.Join(
							", ",
							attr.ConstructorArguments.Select(a =>
								a.Kind == TypedConstantKind.Primitive ? a.ToCSharpString() : a.ToString()
							)
						)
						+ ")";

			attributes.Add($"[{attributeType}{attributeArgs}]");
		}

		// Preserve modifiers.
		var modifiers = "";

		if (symbol.IsParams)
			modifiers += "params ";

		modifiers += symbol.RefKind switch
		{
			RefKind.Ref => "ref ",
			RefKind.Out => "out ",
			RefKind.In => "in ",
			_ => "",
		};

		// Get fully qualified type name.
		var type = symbol.Type.ToDisplayString(_fullyQualifiedTypeFormat);

		// Preserve default value.
		var defaultValue = symbol.HasExplicitDefaultValue
			? " = " + DefaultValueToCSharp(symbol.ExplicitDefaultValue)
			: "";

		// And put it all together.
		var full =
			$"{string.Join(" ", attributes)} {(isThis ? "this " : modifiers)}{type} {symbol.Name}{defaultValue}".Trim();

		return full;
	}

	private static string DefaultValueToCSharp(object? value)
	{
		if (value == null)
			return "null";

		return value switch
		{
			string s => $"\"{s.Replace("\"", "\\\"")}\"",
			char c => $"'{c}'",
			bool b => b ? "true" : "false",
			float f => f.ToString(System.Globalization.CultureInfo.InvariantCulture) + "f",
			double d => d.ToString(System.Globalization.CultureInfo.InvariantCulture),
			decimal m => m.ToString(System.Globalization.CultureInfo.InvariantCulture) + "m",
			Enum e => $"global::{e.GetType().FullName}.{e}",
			_ => value.ToString(),
		};
	}

	private static string? GetObsoleteAttribute(ISymbol symbol)
	{
		var obsolete = symbol
			.GetAttributes()
			.FirstOrDefault(a => a.AttributeClass?.ToDisplayString() == "System.ObsoleteAttribute");

		if (obsolete == null)
		{
			return null;
		}

		var args = obsolete.ConstructorArguments;

		return args.Length switch
		{
			0 => "[Obsolete]",
			1 => $"[Obsolete({Literal(args[0])})]",
			2 => $"[Obsolete({Literal(args[0])}, {args[1].Value!.ToString()!.ToLower()})]",
			_ => "[Obsolete]",
		};
	}

	private static string Literal(TypedConstant c)
	{
		if (c.Value == null)
		{
			return "null";
		}
		if (c.Type?.SpecialType == SpecialType.System_String)
		{
			return "@\"" + c.Value.ToString()!.Replace("\"", "\"\"") + "\"";
		}
		return c.Value.ToString()!;
	}
}
