namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

public class NumberAddExpression(IFormExpression<decimal> left, IFormExpression<decimal> right)
	: IFormExpression<decimal>
{
	private readonly IFormExpression<decimal> _left = left;
	private readonly IFormExpression<decimal> _right = right;

	/// <inheritdoc />
	public decimal EvaluateOn(IFormNode node)
	{
		var leftValue = _left.EvaluateOn(node);
		var rightValue = _right.EvaluateOn(node);
		return leftValue + rightValue;
	}

	/// <inheritdoc />
	public async Task<decimal> EvaluateOnAsync(IFormNode node)
	{
		var leftValue = await _left.EvaluateOnAsync(node);
		var rightValue = await _right.EvaluateOnAsync(node);
		return leftValue + rightValue;
	}
}
