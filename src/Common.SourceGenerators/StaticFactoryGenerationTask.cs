namespace RobinEpple.Common.SourceGenerators;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

internal class StaticFactoryGenerationTask(
	ClassDeclarationSyntax factoryClassSyntax,
	INamedTypeSymbol factoryClass,
	IEnumerable<AttributeData> attributes,
	SemanticModel semanticModel
)
{
	public ClassDeclarationSyntax FactoryClassSyntax { get; } = factoryClassSyntax;
	public INamedTypeSymbol FactoryClass { get; } = factoryClass;
	public IEnumerable<AttributeData> Attributes { get; } = attributes;
	public SemanticModel SemanticModel { get; } = semanticModel;
}
