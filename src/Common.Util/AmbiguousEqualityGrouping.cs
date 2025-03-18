namespace RobinEpple.Common.Util;

/// <summary>
/// Groups two element lists that are considered equal under an ambiguous key.
/// </summary>
/// <typeparam name="TLeft">The type of the element from the left set.</typeparam>
/// <typeparam name="TRight">The type of the element from the right set.</typeparam>
/// <param name="left">The group of elements from the left set, that are considered equal.</param>
/// <param name="right">The group of elements from the right set, that are considered equal.</param>
public class AmbiguousEqualityGrouping<TLeft, TRight>(IEnumerable<TLeft> left, IEnumerable<TRight> right)
{
	/// <summary>
	/// The group of elements from the left set, that are considered equal.
	/// </summary>
	public IEnumerable<TLeft> Left { get; } = left;

	/// <summary>
	/// The group of elements from the right set, that are considered equal.
	/// </summary>
	public IEnumerable<TRight> Right { get; } = right;
}
