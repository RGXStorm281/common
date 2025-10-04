namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

internal partial class SelectExpression<TInput, TOutput>(
	IFormExpression<IEnumerable<TInput>> source,
	Func<TInput, TOutput> selector
) : IFormExpression<IEnumerable<TOutput>>
{
	private readonly IFormExpression<IEnumerable<TInput>> _source = source;
	private readonly Func<TInput, TOutput> _selector = selector;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public IEnumerable<TOutput> EvaluateOn(IFormNode node)
	{
		var sourceItems = _source.EvaluateOn(node);
		return sourceItems.Select(_selector);
	}
}
