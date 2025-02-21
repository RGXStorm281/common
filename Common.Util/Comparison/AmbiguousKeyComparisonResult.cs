namespace RobinEpple.Common.Util.Comparison;

/// <summary>
/// The result of a comparison of two sets, consisting of the intersection and the two differences.<br />
/// Matching elements can have an M:N relationship. Difference sets are grouped by their key.
/// </summary>
/// <typeparam name="TLeft">The type of the elements in the left set.</typeparam>
/// <typeparam name="TRight">The type of the elements in the right set.</typeparam>
/// <typeparam name="TKey">The type of the key used for comparison.</typeparam>
public class AmbiguousKeyComparisonResult<TLeft, TRight, TKey>(
	IDictionary<TKey, List<TLeft>> leftDifference,
	IDictionary<TKey, AmbiguousEqualityGrouping<TLeft, TRight>> intersection,
	IDictionary<TKey, List<TRight>> rightDifference)
	where TKey : notnull
{
	/// <summary>
	/// All elements that occur in the left but not in the right set.
	/// </summary>
	public IDictionary<TKey, List<TLeft>> LeftDifference { get; } = leftDifference;

	/// <summary>
	/// The tuples of left and right elements that are part of the intersection according to the equality condition.
	/// </summary>
	public IDictionary<TKey, AmbiguousEqualityGrouping<TLeft, TRight>> Intersection { get; } = intersection;

	/// <summary>
	/// All elements that occur in the right set but not in the left set.
	/// </summary>
	public IDictionary<TKey, List<TRight>> RightDifference { get; } = rightDifference;
}