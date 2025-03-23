namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

public class InRootScopeExpression<TValue>(IFormExpression<TValue> expression) : IFormExpression<TValue>
{
	private readonly IFormExpression<TValue> _expression = expression;

	/// <inheritdoc />
	public TValue EvaluateOn(IFormNode node)
	{
		return _expression.EvaluateOn(node.Root);
	}

	/// <inheritdoc />
	public Task<TValue> EvaluateOnAsync(IFormNode node)
	{
		return _expression.EvaluateOnAsync(node.Root);
	}
}
