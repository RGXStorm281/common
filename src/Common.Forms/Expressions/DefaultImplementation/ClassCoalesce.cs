namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

[StaticFactoryMethodName("Coalesce")]
internal partial class ClassCoalesce<TValue>(
	[StaticFactoryThis] IFormExpression<TValue?> source,
	IFormExpression<TValue> fallbackValue
) : IFormExpression<TValue>
	where TValue : class
{
	public ClassCoalesce([StaticFactoryThis] IFormExpression<TValue?> source, TValue fallbackValue)
		: this(source, FormExpression.StaticValue(fallbackValue)) { }

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
