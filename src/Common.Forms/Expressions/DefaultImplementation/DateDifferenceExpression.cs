namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

internal class DateDifferenceExpression(IFormExpression<DateTime> start, IFormExpression<DateTime> end)
	: IFormExpression<TimeSpan>
{
	private readonly IFormExpression<DateTime> _start = start;
	private readonly IFormExpression<DateTime> _end = end;

	/// <inheritdoc />
	public TimeSpan EvaluateOn(IFormNode node)
	{
		var startValue = _start.EvaluateOn(node);
		var endValue = _end.EvaluateOn(node);
		return endValue - startValue;
	}

	/// <inheritdoc />
	public async Task<TimeSpan> EvaluateOnAsync(IFormNode node)
	{
		var startValue = await _start.EvaluateOnAsync(node);
		var endValue = await _end.EvaluateOnAsync(node);
		return endValue - startValue;
	}
}
