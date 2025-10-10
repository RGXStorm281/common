namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

[StaticFactoryMethodName("MultiplyBy")]
internal partial class NumberMultiplyBy(
	[StaticFactoryThis] IFormExpression<decimal> target,
	IFormExpression<decimal> factor
) : IFormExpression<decimal>
{
	private readonly IFormExpression<decimal> _target = target;
	private readonly IFormExpression<decimal> _factor = factor;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public decimal EvaluateOn(IFormNode node)
	{
		var targetValue = _target.EvaluateOn(node);
		var factorValue = _factor.EvaluateOn(node);
		return targetValue * factorValue;
	}
}
