namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

[StaticFactoryMethodName("Add")]
internal partial class NumberAdd([StaticFactoryThis] IFormExpression<decimal> left, IFormExpression<decimal> right)
	: IFormExpression<decimal>
{
	private readonly IFormExpression<decimal> _left = left;
	private readonly IFormExpression<decimal> _right = right;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public decimal EvaluateOn(IFormNode node)
	{
		var leftValue = _left.EvaluateOn(node);
		var rightValue = _right.EvaluateOn(node);
		return leftValue + rightValue;
	}
}
