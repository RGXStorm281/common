namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

internal class TimeSpanMultiplyByExpression(IFormExpression<TimeSpan> target, IFormExpression<decimal> factor)
	: IFormExpression<TimeSpan>
{
	private readonly IFormExpression<TimeSpan> _target = target;
	private readonly IFormExpression<decimal> _factor = factor;

	/// <inheritdoc />
	public TimeSpan EvaluateOn(IFormNode node)
	{
		var targetValue = _target.EvaluateOn(node);
		var factorValue = _factor.EvaluateOn(node);
		return TimeSpan.FromTicks((long)(targetValue.Ticks * factorValue));
	}

	/// <inheritdoc />
	public async Task<TimeSpan> EvaluateOnAsync(IFormNode node)
	{
		var targetValue = await _target.EvaluateOnAsync(node);
		var factorValue = await _factor.EvaluateOnAsync(node);
		return TimeSpan.FromTicks((long)(targetValue.Ticks * factorValue));
	}
}
