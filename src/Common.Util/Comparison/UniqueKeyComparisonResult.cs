namespace RobinEpple.Common.Util.Comparison;

/// <summary>
/// The result of a comparison of two sets, consisting of the intersection and the two differences.<br />
/// Matching elements have a 1:1 relationship.
/// </summary>
/// <typeparam name="TLeft">The type of the elements in the left set.</typeparam>
/// <typeparam name="TRight">The type of the elements in the right set.</typeparam>
public class UniqueKeyComparisonResult<TLeft, TRight>(
	IEnumerable<TLeft> leftDifference,
	IEnumerable<UniqueEqualityGrouping<TLeft, TRight>> intersection,
	IEnumerable<TRight> rightDifference
)
{
	/// <summary>
	/// The tuples of left and right elements that are part of the intersection according to the equality condition.
	/// </summary>
	public IEnumerable<UniqueEqualityGrouping<TLeft, TRight>> Intersection { get; } = intersection;

	/// <summary>
	/// All elements that occur in the left but not in the right set.
	/// </summary>
	public IEnumerable<TLeft> LeftDifference { get; } = leftDifference;

	/// <summary>
	/// All elements that occur in the right set but not in the left set.
	/// </summary>
	public IEnumerable<TRight> RightDifference { get; } = rightDifference;
}
