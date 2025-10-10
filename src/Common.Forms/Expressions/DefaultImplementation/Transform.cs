namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

internal partial class Transform<TInput, TOutput>(
	[StaticFactoryThis] IFormExpression<TInput> source,
	Func<TInput, TOutput> selector
) : IFormExpression<TOutput>
{
	private readonly IFormExpression<TInput> _source = source;
	private readonly Func<TInput, TOutput> _selector = selector;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public TOutput EvaluateOn(IFormNode node)
	{
		var sourceValue = _source.EvaluateOn(node);
		return _selector(sourceValue);
	}
}
