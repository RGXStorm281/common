namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

internal partial class Median<TComparable>(
	[StaticFactoryThis] IFormExpression<IEnumerable<TComparable>> items,
	bool preferLowerIndex = false,
	IComparer<TComparable>? comparer = null
) : IFormExpression<TComparable?>
	where TComparable : IComparable
{
	private readonly IFormExpression<IEnumerable<TComparable>> _items = items;
	private readonly bool _preferLowerIndex = preferLowerIndex;
	private readonly IComparer<TComparable>? _comparer = comparer;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public TComparable? EvaluateOn(IFormNode node)
	{
		var itemValues = _items.EvaluateOn(node);
		var orderedList = itemValues.Order(_comparer).ToList();
		if (orderedList.Count == 0)
		{
			return default;
		}

		// Division by default has a bias towards the bigger index on even number of elements.
		// 1 : 2 = 0 [x]
		// 2 : 2 = 1 [_, x]
		// 3 : 2 = 1 [_, x, _]
		// 4 : 2 = 2 [_, _, x, _]
		// 5 : 2 = 2 [_, _, x, _, _]
		var medianIndex = orderedList.Count / 2;

		if (_preferLowerIndex && orderedList.Count % 2 == 0)
		{
			// There are an even number of elements. To adhere to the configured bias the index needs to be decremented by one.
			medianIndex--;
		}

		return orderedList[medianIndex];
	}
}
