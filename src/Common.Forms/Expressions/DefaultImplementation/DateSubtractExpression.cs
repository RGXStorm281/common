namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

internal class DateSubtractExpression(IFormExpression<DateTime> target, IFormExpression<TimeSpan> timeSpan)
	: IFormExpression<DateTime>
{
	private readonly IFormExpression<DateTime> _target = target;
	private readonly IFormExpression<TimeSpan> _timeSpan = timeSpan;

	/// <inheritdoc />
	public DateTime EvaluateOn(IFormNode node)
	{
		var targetValue = _target.EvaluateOn(node);
		var timeSpanValue = _timeSpan.EvaluateOn(node);
		return targetValue.Subtract(timeSpanValue);
	}

	/// <inheritdoc />
	public async Task<DateTime> EvaluateOnAsync(IFormNode node)
	{
		var targetValue = await _target.EvaluateOnAsync(node);
		var timeSpanValue = await _timeSpan.EvaluateOnAsync(node);
		return targetValue.Subtract(timeSpanValue);
	}
}
