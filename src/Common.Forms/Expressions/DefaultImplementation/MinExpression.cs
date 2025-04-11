namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

internal class MinExpression<TComparable>(
	IFormExpression<IEnumerable<TComparable>> items,
	IComparer<TComparable>? comparer = null
) : IFormExpression<TComparable?>
	where TComparable : IComparable
{
	private readonly IFormExpression<IEnumerable<TComparable>> _items = items;
	private readonly IComparer<TComparable>? _comparer = comparer;

	/// <inheritdoc />
	public TComparable? EvaluateOn(IFormNode node)
	{
		var itemValues = _items.EvaluateOn(node);
		return itemValues.Min(_comparer);
	}

	/// <inheritdoc />
	public async Task<TComparable?> EvaluateOnAsync(IFormNode node)
	{
		var itemValues = await _items.EvaluateOnAsync(node);
		return itemValues.Min(_comparer);
	}
}
