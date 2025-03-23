namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

public class BiggerOrEqualExpression<TComparable>(
	IFormExpression<TComparable> source,
	IFormExpression<TComparable> inclusiveLowerBound,
	IComparer<TComparable>? comparer = null
) : IFormExpression<bool>
	where TComparable : IComparable
{
	private readonly IFormExpression<TComparable> _source = source;
	private readonly IFormExpression<TComparable> _inclusiveLowerBound = inclusiveLowerBound;
	private readonly IComparer<TComparable>? _comparer = comparer;

	/// <inheritdoc />
	public bool EvaluateOn(IFormNode node)
	{
		var sourceValue = _source.EvaluateOn(node);
		var bound = _inclusiveLowerBound.EvaluateOn(node);
		if (_comparer != null)
		{
			return _comparer.Compare(sourceValue, bound) >= 0;
		}
		return sourceValue.CompareTo(bound) >= 0;
	}

	/// <inheritdoc />
	public async Task<bool> EvaluateOnAsync(IFormNode node)
	{
		var sourceValue = await _source.EvaluateOnAsync(node);
		var bound = await _inclusiveLowerBound.EvaluateOnAsync(node);
		if (_comparer != null)
		{
			return _comparer.Compare(sourceValue, bound) >= 0;
		}
		return sourceValue.CompareTo(bound) >= 0;
	}
}
