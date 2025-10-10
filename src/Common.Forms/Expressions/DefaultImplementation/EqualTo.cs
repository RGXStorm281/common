namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

internal partial class EqualTo<TComparable>(
	IFormExpression<TComparable> source,
	IFormExpression<TComparable> target,
	IComparer<TComparable>? comparer = null
) : IFormExpression<bool>
	where TComparable : IComparable
{
	private readonly IFormExpression<TComparable> _source = source;
	private readonly IFormExpression<TComparable> _target = target;
	private readonly IComparer<TComparable>? _comparer = comparer;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public bool EvaluateOn(IFormNode node)
	{
		var sourceValue = _source.EvaluateOn(node);
		var bound = _target.EvaluateOn(node);
		if (_comparer != null)
		{
			return _comparer.Compare(sourceValue, bound) == 0;
		}
		return sourceValue.CompareTo(bound) == 0;
	}
}
