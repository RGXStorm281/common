namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

internal partial class Contains<TElement>(
	[StaticFactoryThis] IFormExpression<IEnumerable<TElement>> list,
	IFormExpression<TElement> item,
	IEqualityComparer<TElement>? equalityComparer = null
) : IFormExpression<bool>
{
	private readonly IFormExpression<IEnumerable<TElement>> _list = list;
	private readonly IFormExpression<TElement> _item = item;
	private readonly IEqualityComparer<TElement>? _equalityComparer = equalityComparer;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public bool EvaluateOn(IFormNode node)
	{
		var items = _list.EvaluateOn(node);
		var searchTarget = _item.EvaluateOn(node);
		return items.Contains(searchTarget, _equalityComparer);
	}
}
