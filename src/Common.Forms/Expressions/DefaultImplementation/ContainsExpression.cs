namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

internal class ContainsExpression<TElement>(
	IFormExpression<IEnumerable<TElement>> list,
	IFormExpression<TElement> item,
	IEqualityComparer<TElement>? equalityComparer = null
) : IFormExpression<bool>
{
	private readonly IFormExpression<IEnumerable<TElement>> _list = list;
	private readonly IFormExpression<TElement> _item = item;
	private readonly IEqualityComparer<TElement>? _equalityComparer = equalityComparer;

	/// <inheritdoc />
	public bool EvaluateOn(IFormNode node)
	{
		var items = _list.EvaluateOn(node);
		var searchTarget = _item.EvaluateOn(node);
		return items.Contains(searchTarget, _equalityComparer);
	}

	/// <inheritdoc />
	public async Task<bool> EvaluateOnAsync(IFormNode node)
	{
		var items = await _list.EvaluateOnAsync(node);
		var searchTarget = await _item.EvaluateOnAsync(node);
		return items.Contains(searchTarget, _equalityComparer);
	}
}
