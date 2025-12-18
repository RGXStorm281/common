namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

[StaticFactoryMethodName("Multiply")]
internal partial class NumberMultiply([StaticFactoryThis] IFormExpression<IEnumerable<decimal>> factors)
	: IFormExpression<decimal>
{
	private readonly IFormExpression<IEnumerable<decimal>> _factors = factors;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public decimal EvaluateOn(IFormNode node)
	{
		var factorValues = _factors.EvaluateOn(node);
		return factorValues.Aggregate(1m, (intermediateResult, next) => intermediateResult * next);
	}
}
