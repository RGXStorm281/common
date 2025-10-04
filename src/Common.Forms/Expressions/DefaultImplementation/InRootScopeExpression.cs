namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

internal partial class InRootScopeExpression<TValue>(IFormExpression<TValue> expression) : IFormExpression<TValue>
{
	private readonly IFormExpression<TValue> _expression = expression;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public TValue EvaluateOn(IFormNode node)
	{
		return _expression.EvaluateOn(node.Root);
	}
}
