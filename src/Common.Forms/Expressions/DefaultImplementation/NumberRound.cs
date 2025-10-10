namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

[StaticFactoryMethodName("Round")]
internal partial class NumberRound([StaticFactoryThis] IFormExpression<decimal> target) : IFormExpression<decimal>
{
	private readonly IFormExpression<decimal> _target = target;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public decimal EvaluateOn(IFormNode node)
	{
		var targetValue = _target.EvaluateOn(node);
		return Math.Round(targetValue);
	}
}
