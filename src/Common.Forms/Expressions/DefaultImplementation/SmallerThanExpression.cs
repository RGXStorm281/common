namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

internal class SmallerThanExpression<TComparable>(
	IFormExpression<TComparable> source,
	IFormExpression<TComparable> exclusiveUpperBound,
	IComparer<TComparable>? comparer = null
) : IFormExpression<bool>
	where TComparable : IComparable
{
	private readonly IFormExpression<TComparable> _source = source;
	private readonly IFormExpression<TComparable> _exclusiveUpperBound = exclusiveUpperBound;
	private readonly IComparer<TComparable>? _comparer = comparer;

	/// <inheritdoc />
	public bool EvaluateOn(IFormNode node)
	{
		var sourceValue = _source.EvaluateOn(node);
		var bound = _exclusiveUpperBound.EvaluateOn(node);
		if (_comparer != null)
		{
			return _comparer.Compare(sourceValue, bound) < 0;
		}
		return sourceValue.CompareTo(bound) < 0;
	}

	/// <inheritdoc />
	public async Task<bool> EvaluateOnAsync(IFormNode node)
	{
		var sourceValue = await _source.EvaluateOnAsync(node);
		var bound = await _exclusiveUpperBound.EvaluateOnAsync(node);
		if (_comparer != null)
		{
			return _comparer.Compare(sourceValue, bound) < 0;
		}
		return sourceValue.CompareTo(bound) < 0;
	}
}
