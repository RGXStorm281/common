namespace RobinEpple.Common.Forms.Wrappers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class FormWrapperGenerationTask
{
	public FormWrapperGenerationTask(
		MethodDeclarationSyntax methodDeclaration,
		IMethodSymbol methodSymbol,
		SemanticModel semanticModel,
		string formPropertyName,
		string wrapperPropertyName
	)
	{
		MethodDeclaration = methodDeclaration;
		MethodSymbol = methodSymbol;
		SemanticModel = semanticModel;
		FormPropertyName = formPropertyName;
		WrapperPropertyName = wrapperPropertyName;
	}

	public MethodDeclarationSyntax MethodDeclaration { get; set; }
	public IMethodSymbol MethodSymbol { get; set; }
	public SemanticModel SemanticModel { get; set; }
	public string FormPropertyName { get; }
	public string WrapperPropertyName { get; }
}
