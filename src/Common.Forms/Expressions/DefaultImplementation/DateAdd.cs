namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

internal partial class DateAdd([StaticFactoryThis] IFormExpression<DateTime> target, IFormExpression<TimeSpan> timeSpan)
	: IFormExpression<DateTime>
{
	private readonly IFormExpression<DateTime> _target = target;
	private readonly IFormExpression<TimeSpan> _timeSpan = timeSpan;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public DateTime EvaluateOn(IFormNode node)
	{
		var targetValue = _target.EvaluateOn(node);
		var timeSpanValue = _timeSpan.EvaluateOn(node);
		return targetValue.Add(timeSpanValue);
	}
}
