namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

internal partial class ClassCoalesceExpression<TValue>(
	IFormExpression<TValue?> source,
	IFormExpression<TValue> fallbackValue
) : IFormExpression<TValue>
	where TValue : class
{
	private readonly IFormExpression<TValue?> _source = source;
	private readonly IFormExpression<TValue> _fallbackValue = fallbackValue;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public TValue EvaluateOn(IFormNode node)
	{
		var sourceValue = _source.EvaluateOn(node);
		if (sourceValue == null)
		{
			return _fallbackValue.EvaluateOn(node);
		}
		else
		{
			return sourceValue;
		}
	}
}
