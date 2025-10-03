namespace RobinEpple.Common.SourceGenerators;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class AsyncOverloadGenerationTask
{
	public AsyncOverloadGenerationTask(
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

		HasYieldStatements = MethodDeclaration!.DescendantNodes().OfType<YieldStatementSyntax>().Any();
	}

	public MethodDeclarationSyntax MethodDeclaration { get; set; }
	public IMethodSymbol MethodSymbol { get; set; }
	public SemanticModel SemanticModel { get; set; }
	public bool HasYieldStatements { get; set; }
	public string AsyncName { get; set; }
	public Dictionary<IMethodSymbol, string> AwaitableOverloads { get; set; } = [];
	public bool HasBody => MethodDeclaration.Body != null || MethodDeclaration.ExpressionBody != null;
	public bool IsRunningAsync => HasBody && (AwaitableOverloads.Count > 0 || HasYieldStatements);
}
