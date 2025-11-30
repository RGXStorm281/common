namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

internal partial class OnNotFound<TValue>(
	[StaticFactoryThis] IFormExpression<TValue> source,
	IFormExpression<TValue> fallbackValue
) : IFormExpression<TValue>
{
	private readonly IFormExpression<TValue> _source = source;
	private readonly IFormExpression<TValue> _fallbackValue = fallbackValue;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public TValue EvaluateOn(IFormNode node)
	{
		try
		{
			return _source.EvaluateOn(node);
		}
		catch (NodeNotFoundException)
		{
			return _fallbackValue.EvaluateOn(node);
		}
	}
}
