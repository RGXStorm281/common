namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

public class AndExpression(IFormExpression<bool> left, IFormExpression<bool> right) : IFormExpression<bool>
{
	private readonly IFormExpression<bool> _left = left;
	private readonly IFormExpression<bool> _right = right;

	/// <inheritdoc />
	public bool EvaluateOn(IFormNode node)
	{
		var leftValue = _left.EvaluateOn(node);
		var rightValue = _right.EvaluateOn(node);
		return leftValue && rightValue;
	}

	/// <inheritdoc />
	public async Task<bool> EvaluateOnAsync(IFormNode node)
	{
		var leftValue = await _left.EvaluateOnAsync(node);
		var rightValue = await _right.EvaluateOnAsync(node);
		return leftValue && rightValue;
	}
}
