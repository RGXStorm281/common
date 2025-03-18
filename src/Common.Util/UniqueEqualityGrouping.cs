namespace RobinEpple.Common.Util;

/// <summary>
/// Groups two elements that are considered equal under a unique key.
/// </summary>
/// <typeparam name="TLeft">The type of the element from the left set.</typeparam>
/// <typeparam name="TRight">The type of the element from the right set.</typeparam>
/// <param name="left">The element from the left set.</param>
/// <param name="right">The element from the right set.</param>
public class UniqueEqualityGrouping<TLeft, TRight>(TLeft left, TRight right)
{
	/// <summary>
	/// The element from the left set.
	/// </summary>
	public TLeft Left { get; } = left;

	/// <summary>
	/// The element from the right set.
	/// </summary>
	public TRight Right { get; } = right;
}
