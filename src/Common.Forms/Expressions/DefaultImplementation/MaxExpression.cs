namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

internal partial class MaxExpression<TComparable>(
	IFormExpression<IEnumerable<TComparable>> items,
	IComparer<TComparable>? comparer = null
) : IFormExpression<TComparable?>
	where TComparable : IComparable
{
	private readonly IFormExpression<IEnumerable<TComparable>> _items = items;
	private readonly IComparer<TComparable>? _comparer = comparer;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public TComparable? EvaluateOn(IFormNode node)
	{
		var itemValues = _items.EvaluateOn(node);
		return itemValues.Max(_comparer);
	}
}
