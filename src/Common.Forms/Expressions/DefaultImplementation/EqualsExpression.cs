namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

internal partial class EqualsExpression<TValue>(
	IFormExpression<TValue> left,
	IFormExpression<TValue> right,
	IEqualityComparer<TValue>? equalityComparer
) : IFormExpression<bool>
{
	private readonly IFormExpression<TValue> _left = left;
	private readonly IFormExpression<TValue> _right = right;
	private readonly IEqualityComparer<TValue>? _equalityComparer = equalityComparer;

	/// <inheritdoc />
	[GenerateAsyncOverload]
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
}
