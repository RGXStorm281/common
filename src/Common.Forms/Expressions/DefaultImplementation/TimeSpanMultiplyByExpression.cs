namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

internal partial class TimeSpanMultiplyByExpression(IFormExpression<TimeSpan> target, IFormExpression<decimal> factor)
	: IFormExpression<TimeSpan>
{
	private readonly IFormExpression<TimeSpan> _target = target;
	private readonly IFormExpression<decimal> _factor = factor;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public TimeSpan EvaluateOn(IFormNode node)
	{
		var targetValue = _target.EvaluateOn(node);
		var factorValue = _factor.EvaluateOn(node);
		return TimeSpan.FromTicks((long)(targetValue.Ticks * factorValue));
	}
}
