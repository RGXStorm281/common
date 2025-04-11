namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

public class EqualsExpression<TValue>(
	IFormExpression<TValue> left,
	IFormExpression<TValue> right,
	IEqualityComparer<TValue>? equalityComparer
) : IFormExpression<bool>
{
	private readonly IFormExpression<TValue> _left = left;
	private readonly IFormExpression<TValue> _right = right;
	private readonly IEqualityComparer<TValue>? _equalityComparer = equalityComparer;

	/// <inheritdoc />
	public bool EvaluateOn(IFormNode node)
	{
		var leftValue = _left.EvaluateOn(node);
		var rightValue = _right.EvaluateOn(node);

		if (_equalityComparer == null)
		{
			return Equals(leftValue, rightValue);
		}
		else
		{
			return _equalityComparer.Equals(leftValue, rightValue);
		}
	}

	/// <inheritdoc />
	public async Task<bool> EvaluateOnAsync(IFormNode node)
	{
		var leftValue = await _left.EvaluateOnAsync(node);
		var rightValue = await _right.EvaluateOnAsync(node);

		if (_equalityComparer == null)
		{
			return Equals(leftValue, rightValue);
		}
		else
		{
			return _equalityComparer.Equals(leftValue, rightValue);
		}
	}
}
