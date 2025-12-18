namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

internal partial class ScopeName : IFormExpression<string>
{
	/// <inheritdoc />
	[GenerateAsyncOverload]
	public string EvaluateOn(IFormNode node)
	{
		var scope = node.GetScope();
		return scope.Name;
	}
}
