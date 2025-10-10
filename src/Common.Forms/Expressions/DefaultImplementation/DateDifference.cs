namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

internal partial class DateDifference(
	[StaticFactoryThis] IFormExpression<DateTime> start,
	IFormExpression<DateTime> end
) : IFormExpression<TimeSpan>
{
	private readonly IFormExpression<DateTime> _start = start;
	private readonly IFormExpression<DateTime> _end = end;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public TimeSpan EvaluateOn(IFormNode node)
	{
		var startValue = _start.EvaluateOn(node);
		var endValue = _end.EvaluateOn(node);
		return endValue - startValue;
	}
}
