namespace RobinEpple.Common.SourceGenerators;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

internal class AsyncOverloadGenerationTask
{
	public AsyncOverloadGenerationTask(
		MethodDeclarationSyntax methodDeclaration,
		IMethodSymbol methodSymbol,
		SemanticModel semanticModel,
		string asyncName,
		string[] whitelistedExtensionNamespaces
	)
	{
		MethodDeclaration = methodDeclaration;
		MethodSymbol = methodSymbol;
		SemanticModel = semanticModel;
		AsyncName = asyncName;
		WhitelistedExtensionNamespaces = whitelistedExtensionNamespaces;
		HasYieldStatements = MethodDeclaration!.DescendantNodes().OfType<YieldStatementSyntax>().Any();
	}

	public MethodDeclarationSyntax MethodDeclaration { get; set; }
	public IMethodSymbol MethodSymbol { get; set; }
	public SemanticModel SemanticModel { get; set; }
	public bool HasYieldStatements { get; set; }
	public string AsyncName { get; set; }
	public string[] WhitelistedExtensionNamespaces { get; }

	public Dictionary<IMethodSymbol, string> AwaitableLocalOverloads { get; set; } = [];
	public Dictionary<IMethodSymbol, string> AwaitableExtensionOverloads { get; set; } = [];
	public bool HasBody => MethodDeclaration.Body != null || MethodDeclaration.ExpressionBody != null;
	public bool IsRunningAsync =>
		HasBody && (AwaitableLocalOverloads.Count > 0 || AwaitableExtensionOverloads.Count > 0 || HasYieldStatements);
}
