namespace RobinEpple.Common.Util.Comparison;

/// <summary>
/// A static helper class to compare two sets of potentially different types by a common key.
/// </summary>
public static class Comparison
{
	#region Unique Key

	/// <summary>
	/// Compares the sets using keys of identical type and returns the left-hand difference.<br />
	/// The keys have to be unique.
	/// </summary>
	/// <typeparam name="TLeft">The type of elements in the left collection.</typeparam>
	/// <typeparam name="TRight">The type of elements in the right collection.</typeparam>
	/// <typeparam name="TKey">The type of the shared key.</typeparam>
	/// <param name="left">The left collection.</param>
	/// <param name="right">The right collection.</param>
	/// <param name="leftKeySelector">The function to select the key from the left elements.</param>
	/// <param name="rightKeySelector">The function to select the key from the right elements.</param>
	/// <param name="comparer">Optional comparer for individual equality definitions on the key.</param>
	/// <returns>The left difference.</returns>
	public static IEnumerable<TLeft> GetLeftDifferenceByUniqueKey<TLeft, TRight, TKey>(
		IEnumerable<TLeft> left,
		IEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		IEqualityComparer<TKey>? comparer = null
	)
		where TKey : notnull
	{
		// Index right set by key.
		var rightKeyIndex = new HashSet<TKey>(right.Select(rightKeySelector), comparer);

		// Iterate over the left set and return only those that are not contained in the right set.
		foreach (TLeft leftItem in left)
		{
			if (!rightKeyIndex.Contains(leftKeySelector(leftItem)))
			{
				yield return leftItem;
			}
		}
	}

	/// <summary>
	/// Compares the sets using keys of identical type and returns the right-hand difference.<br />
	/// The keys have to be unique.
	/// </summary>
	/// <typeparam name="TLeft">The type of elements in the left collection.</typeparam>
	/// <typeparam name="TRight">The type of elements in the right collection.</typeparam>
	/// <typeparam name="TKey">The type of the shared key.</typeparam>
	/// <param name="left">The left collection.</param>
	/// <param name="right">The right collection.</param>
	/// <param name="leftKeySelector">The function to select the key from the left elements.</param>
	/// <param name="rightKeySelector">The function to select the key from the right elements.</param>
	/// <param name="comparer">Optional comparer for individual equality definitions on the key.</param>
	/// <returns>The right difference.</returns>
	public static IEnumerable<TRight> GetRightDifferenceByUniqueKey<TLeft, TRight, TKey>(
		IEnumerable<TLeft> left,
		IEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		IEqualityComparer<TKey>? comparer = null
	)
		where TKey : notnull
	{
		// Index left set by key.
		var leftKeyIndex = new HashSet<TKey>(left.Select(leftKeySelector), comparer);

		// Iterate over the right set and return only those that are not contained in the left set.
		foreach (TRight rightItem in right)
		{
			if (!leftKeyIndex.Contains(rightKeySelector(rightItem)))
			{
				yield return rightItem;
			}
		}
	}

	/// <summary>
	/// Compares the sets using keys of identical type and returns the intersection.<br />
	/// The keys have to be unique.
	/// </summary>
	/// <typeparam name="TLeft">The type of elements in the left collection.</typeparam>
	/// <typeparam name="TRight">The type of elements in the right collection.</typeparam>
	/// <typeparam name="TKey">The type of the shared key.</typeparam>
	/// <param name="left">The left collection.</param>
	/// <param name="right">The right collection.</param>
	/// <param name="leftKeySelector">The function to select the key from the left elements.</param>
	/// <param name="rightKeySelector">The function to select the key from the right elements.</param>
	/// <param name="comparer">Optional comparer for individual equality definitions on the key.</param>
	/// <returns>The intersection.</returns>
	public static IEnumerable<UniqueEqualityGrouping<TLeft, TRight>> GetIntersectionByUniqueKey<TLeft, TRight, TKey>(
		IEnumerable<TLeft> left,
		IEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		IEqualityComparer<TKey>? comparer = null
	)
		where TKey : notnull
	{
		// Index left set by key.
		var leftDictionary = left.ToDictionary(leftKeySelector, comparer);

		// Iterate over the right set and return only those that are also contained in the left set.
		foreach (TRight rightItem in right)
		{
			if (leftDictionary.TryGetValue(rightKeySelector(rightItem), out TLeft? leftItem))
			{
				yield return new(leftItem, rightItem);
			}
		}
	}

	/// <summary>
	/// Compares the sets using keys of identical type and returns a full comparison result.<br />
	/// The keys have to be unique.
	/// </summary>
	/// <typeparam name="TLeft">The type of elements in the left collection.</typeparam>
	/// <typeparam name="TRight">The type of elements in the right collection.</typeparam>
	/// <typeparam name="TKey">The type of the shared key.</typeparam>
	/// <param name="left">The left collection.</param>
	/// <param name="right">The right collection.</param>
	/// <param name="leftKeySelector">The function to select the key from the left elements.</param>
	/// <param name="rightKeySelector">The function to select the key from the right elements.</param>
	/// <param name="comparer">Optional comparer for individual equality definitions on the key.</param>
	/// <returns>The comparison result.</returns>
	public static UniqueKeyComparisonResult<TLeft, TRight> CompareByUniqueKeyEquality<TLeft, TRight, TKey>(
		IEnumerable<TLeft> left,
		IEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		IEqualityComparer<TKey>? comparer = null
	)
		where TKey : notnull
	{
		var leftDifference = new List<TLeft>();
		var intersection = new List<UniqueEqualityGrouping<TLeft, TRight>>();

		// Index both sets by key.
		var leftDictionary = left.ToDictionary(leftKeySelector, comparer);
		var rightDictionary = right.ToDictionary(rightKeySelector, comparer);

		// Iterate over the left dictionary to form left difference and intersection.
		foreach (KeyValuePair<TKey, TLeft> pair in leftDictionary)
		{
			if (rightDictionary.TryGetValue(pair.Key, out TRight? rightItem))
			{
				// Hit -> Intersection.
				intersection.Add(new(pair.Value, rightItem));
				// Remove from the right-hand dictionary -> At the end, only right elements without a match should remain.
				rightDictionary.Remove(pair.Key);
			}
			else
			{
				// No hit -> Left difference
				leftDifference.Add(pair.Value);
			}
		}

		// Return result.
		// In the right-hand dictionary only the elements without a match in the left-hand dictionary remain.
		return new UniqueKeyComparisonResult<TLeft, TRight>(leftDifference, intersection, [.. rightDictionary.Values]);
	}

	#endregion

	#region Ambigous Key

	/// <summary>
	/// Compares the sets using keys of identical type and returns the left-hand difference.<br />
	/// The keys don't have to be unique.
	/// </summary>
	/// <typeparam name="TLeft">The type of elements in the left collection.</typeparam>
	/// <typeparam name="TRight">The type of elements in the right collection.</typeparam>
	/// <typeparam name="TKey">The type of the shared key.</typeparam>
	/// <param name="left">The left collection.</param>
	/// <param name="right">The right collection.</param>
	/// <param name="leftKeySelector">The function to select the key from the left elements.</param>
	/// <param name="rightKeySelector">The function to select the key from the right elements.</param>
	/// <param name="comparer">Optional comparer for individual equality definitions on the key.</param>
	/// <returns>The left difference.</returns>
	public static IDictionary<TKey, List<TLeft>> GetLeftDifferenceByAmbiguousKey<TLeft, TRight, TKey>(
		IEnumerable<TLeft> left,
		IEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		IEqualityComparer<TKey>? comparer = null
	)
		where TKey : notnull
	{
		// Group and index left set by key.
		var leftDictionary = left.ToLookup(leftKeySelector, comparer)
			.ToDictionary(group => group.Key, group => group.ToList(), comparer);
		var rightKeyIndex = new HashSet<TKey>(right.Select(rightKeySelector), comparer);

		// Iterate over the left set and remove those that are also contained in the right set.
		foreach (var leftKey in leftDictionary.Keys)
		{
			if (rightKeyIndex.Contains(leftKey))
			{
				leftDictionary.Remove(leftKey);
			}
		}

		return leftDictionary;
	}

	/// <summary>
	/// Compares the sets using keys of identical type and returns the right-hand difference.<br />
	/// The keys don't have to be unique.
	/// </summary>
	/// <typeparam name="TLeft">The type of elements in the left collection.</typeparam>
	/// <typeparam name="TRight">The type of elements in the right collection.</typeparam>
	/// <typeparam name="TKey">The type of the shared key.</typeparam>
	/// <param name="left">The left collection.</param>
	/// <param name="right">The right collection.</param>
	/// <param name="leftKeySelector">The function to select the key from the left elements.</param>
	/// <param name="rightKeySelector">The function to select the key from the right elements.</param>
	/// <param name="comparer">Optional comparer for individual equality definitions on the key.</param>
	/// <returns>The right difference.</returns>
	public static IDictionary<TKey, List<TRight>> GetRightDifferenceByAmbiguousKey<TLeft, TRight, TKey>(
		IEnumerable<TLeft> left,
		IEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		IEqualityComparer<TKey>? comparer = null
	)
		where TKey : notnull
	{
		// Group and index right set by key.
		var rightDictionary = right
			.ToLookup(rightKeySelector, comparer)
			.ToDictionary(group => group.Key, group => group.ToList(), comparer);
		var leftKeyIndex = new HashSet<TKey>(left.Select(leftKeySelector), comparer);

		// Iterate over the right set and remove those that are also contained in the left set.
		foreach (var rightKey in rightDictionary.Keys)
		{
			if (leftKeyIndex.Contains(rightKey))
			{
				rightDictionary.Remove(rightKey);
			}
		}

		return rightDictionary;
	}

	/// <summary>
	/// Compares the sets using keys of identical type and returns the intersection.<br />
	/// The keys don't have to be unique.
	/// </summary>
	/// <typeparam name="TLeft">The type of elements in the left collection.</typeparam>
	/// <typeparam name="TRight">The type of elements in the right collection.</typeparam>
	/// <typeparam name="TKey">The type of the shared key.</typeparam>
	/// <param name="left">The left collection.</param>
	/// <param name="right">The right collection.</param>
	/// <param name="leftKeySelector">The function to select the key from the left elements.</param>
	/// <param name="rightKeySelector">The function to select the key from the right elements.</param>
	/// <param name="comparer">Optional comparer for individual equality definitions on the key.</param>
	/// <returns>The intersection.</returns>
	public static IDictionary<TKey, AmbiguousEqualityGrouping<TLeft, TRight>> GetIntersectionByAmbiguousKey<
		TLeft,
		TRight,
		TKey
	>(
		IEnumerable<TLeft> left,
		IEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		IEqualityComparer<TKey>? comparer = null
	)
		where TKey : notnull
	{
		var intersection = new Dictionary<TKey, AmbiguousEqualityGrouping<TLeft, TRight>>(comparer);

		// Group and index both sets by key.
		var leftDictionary = left.ToLookup(leftKeySelector, comparer)
			.ToDictionary(group => group.Key, group => group.ToList(), comparer);
		var rightDictionary = right
			.ToLookup(rightKeySelector, comparer)
			.ToDictionary(group => group.Key, group => group.ToList(), comparer);

		// Iterate over the right set and return only those that are also contained in the left set.
		foreach (var rightGrouping in rightDictionary)
		{
			if (leftDictionary.TryGetValue(rightGrouping.Key, out var leftGrouping))
			{
				intersection.Add(rightGrouping.Key, new(leftGrouping, rightGrouping.Value));
			}
		}

		return intersection;
	}

	/// <summary>
	/// Compares the sets using keys of identical type and returns the full comparison result.<br />
	/// The keys don't have to be unique.
	/// </summary>
	/// <typeparam name="TLeft">The type of elements in the left collection.</typeparam>
	/// <typeparam name="TRight">The type of elements in the right collection.</typeparam>
	/// <typeparam name="TKey">The type of the shared key.</typeparam>
	/// <param name="left">The left collection.</param>
	/// <param name="right">The right collection.</param>
	/// <param name="leftKeySelector">The function to select the key from the left elements.</param>
	/// <param name="rightKeySelector">The function to select the key from the right elements.</param>
	/// <param name="comparer">Optional comparer for individual equality definitions on the key.</param>
	/// <returns>The comparison result.</returns>
	public static AmbiguousKeyComparisonResult<TLeft, TRight, TKey> CompareByAmbiguousKeyEquality<TLeft, TRight, TKey>(
		IEnumerable<TLeft> left,
		IEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		IEqualityComparer<TKey>? comparer = null
	)
		where TKey : notnull
	{
		var leftDifference = new Dictionary<TKey, List<TLeft>>(comparer);
		var intersection = new Dictionary<TKey, AmbiguousEqualityGrouping<TLeft, TRight>>(comparer);

		// Grouping and indexing sets by key.
		var leftDictionary = left.ToLookup(leftKeySelector, comparer)
			.ToDictionary(group => group.Key, group => group.ToList(), comparer);
		var rightDictionary = right
			.ToLookup(rightKeySelector, comparer)
			.ToDictionary(group => group.Key, group => group.ToList(), comparer);

		// Iterate over the left dictionary to form left difference and intersection.
		foreach (var pair in leftDictionary)
		{
			if (rightDictionary.TryGetValue(pair.Key, out var rightItem))
			{
				// Hit -> Intersection.
				intersection.Add(pair.Key, new(pair.Value, rightItem));
				// Remove from the right-hand dictionary -> At the end, only right elements without a match should remain.
				rightDictionary.Remove(pair.Key);
			}
			else
			{
				// No hit -> Left difference
				leftDifference.Add(pair.Key, pair.Value);
			}
		}

		// Return result.
		// In the right-hand dictionary only the elements without a match in the left-hand dictionary remain.
		return new AmbiguousKeyComparisonResult<TLeft, TRight, TKey>(leftDifference, intersection, rightDictionary);
	}

	#endregion

	#region UsabilityOverloads

	/// <inheritdoc cref="GetLeftDifferenceByUniqueKey{TLeft,TRight,TKey}"/>
	public static IEnumerable<TItem> GetLeftDifferenceByUniqueKey<TItem, TKey>(
		IEnumerable<TItem> left,
		IEnumerable<TItem> right,
		Func<TItem, TKey> keySelector,
		IEqualityComparer<TKey>? comparer = null
	)
		where TKey : notnull => GetLeftDifferenceByUniqueKey(left, right, keySelector, keySelector, comparer);

	/// <inheritdoc cref="GetLeftDifferenceByUniqueKey{TLeft,TRight,TKey}"/>
	public static IEnumerable<TItem> GetLeftDifferenceByUniqueKey<TItem>(
		IEnumerable<TItem> left,
		IEnumerable<TItem> right,
		IEqualityComparer<TItem>? comparer = null
	)
		where TItem : notnull => GetLeftDifferenceByUniqueKey(left, right, item => item, comparer);

	/// <inheritdoc cref="GetRightDifferenceByUniqueKey{TLeft,TRight,TKey}"/>
	public static IEnumerable<TItem> GetRightDifferenceByUniqueKey<TItem, TKey>(
		IEnumerable<TItem> left,
		IEnumerable<TItem> right,
		Func<TItem, TKey> keySelector,
		IEqualityComparer<TKey>? comparer = null
	)
		where TKey : notnull => GetRightDifferenceByUniqueKey(left, right, keySelector, keySelector, comparer);

	/// <inheritdoc cref="GetRightDifferenceByUniqueKey{TLeft,TRight,TKey}"/>
	public static IEnumerable<TItem> GetRightDifferenceByUniqueKey<TItem>(
		IEnumerable<TItem> left,
		IEnumerable<TItem> right,
		IEqualityComparer<TItem>? comparer = null
	)
		where TItem : notnull => GetRightDifferenceByUniqueKey(left, right, item => item, comparer);

	/// <inheritdoc cref="GetIntersectionByUniqueKey{TLeft,TRight,TKey}"/>
	public static IEnumerable<UniqueEqualityGrouping<TItem, TItem>> GetIntersectionByUniqueKey<TItem, TKey>(
		IEnumerable<TItem> left,
		IEnumerable<TItem> right,
		Func<TItem, TKey> keySelector,
		IEqualityComparer<TKey>? comparer = null
	)
		where TKey : notnull => GetIntersectionByUniqueKey(left, right, keySelector, keySelector, comparer);

	/// <inheritdoc cref="GetIntersectionByUniqueKey{TLeft,TRight,TKey}"/>
	public static IEnumerable<UniqueEqualityGrouping<TItem, TItem>> GetIntersectionByUniqueKey<TItem>(
		IEnumerable<TItem> left,
		IEnumerable<TItem> right,
		IEqualityComparer<TItem>? comparer = null
	)
		where TItem : notnull => GetIntersectionByUniqueKey(left, right, item => item, comparer);

	/// <inheritdoc cref="CompareByUniqueKeyEquality{TLeft,TRight,TKey}"/>
	public static UniqueKeyComparisonResult<TItem, TItem> CompareByUniqueKeyEquality<TItem, TKey>(
		IEnumerable<TItem> left,
		IEnumerable<TItem> right,
		Func<TItem, TKey> keySelector,
		IEqualityComparer<TKey>? comparer = null
	)
		where TKey : notnull => CompareByUniqueKeyEquality(left, right, keySelector, keySelector, comparer);

	/// <inheritdoc cref="CompareByUniqueKeyEquality{TLeft,TRight,TKey}"/>
	public static UniqueKeyComparisonResult<TItem, TItem> CompareByUniqueKeyEquality<TItem>(
		IEnumerable<TItem> left,
		IEnumerable<TItem> right,
		IEqualityComparer<TItem>? comparer = null
	)
		where TItem : notnull => CompareByUniqueKeyEquality(left, right, item => item, comparer);

	/// <inheritdoc cref="GetLeftDifferenceByAmbiguousKey{TLeft,TRight,TKey}"/>
	public static IDictionary<TKey, List<TItem>> GetLeftDifferenceByAmbiguousKey<TItem, TKey>(
		IEnumerable<TItem> left,
		IEnumerable<TItem> right,
		Func<TItem, TKey> keySelector,
		IEqualityComparer<TKey>? comparer = null
	)
		where TKey : notnull => GetLeftDifferenceByAmbiguousKey(left, right, keySelector, keySelector, comparer);

	/// <inheritdoc cref="GetLeftDifferenceByAmbiguousKey{TLeft,TRight,TKey}"/>
	public static IDictionary<TItem, List<TItem>> GetLeftDifferenceByAmbiguousKey<TItem>(
		IEnumerable<TItem> left,
		IEnumerable<TItem> right,
		IEqualityComparer<TItem>? comparer = null
	)
		where TItem : notnull => GetLeftDifferenceByAmbiguousKey(left, right, item => item, comparer);

	/// <inheritdoc cref="GetRightDifferenceByAmbiguousKey{TLeft,TRight,TKey}"/>
	public static IDictionary<TKey, List<TItem>> GetRightDifferenceByAmbiguousKey<TItem, TKey>(
		IEnumerable<TItem> left,
		IEnumerable<TItem> right,
		Func<TItem, TKey> keySelector,
		IEqualityComparer<TKey>? comparer = null
	)
		where TKey : notnull => GetRightDifferenceByAmbiguousKey(left, right, keySelector, keySelector, comparer);

	/// <inheritdoc cref="GetRightDifferenceByAmbiguousKey{TLeft,TRight,TKey}"/>
	public static IDictionary<TItem, List<TItem>> GetRightDifferenceByAmbiguousKey<TItem>(
		IEnumerable<TItem> left,
		IEnumerable<TItem> right,
		IEqualityComparer<TItem>? comparer = null
	)
		where TItem : notnull => GetRightDifferenceByAmbiguousKey(left, right, item => item, comparer);

	/// <inheritdoc cref="GetIntersectionByAmbiguousKey{TLeft,TRight,TKey}"/>
	public static IDictionary<TKey, AmbiguousEqualityGrouping<TItem, TItem>> GetIntersectionByAmbiguousKey<TItem, TKey>(
		IEnumerable<TItem> left,
		IEnumerable<TItem> right,
		Func<TItem, TKey> keySelector,
		IEqualityComparer<TKey>? comparer = null
	)
		where TKey : notnull => GetIntersectionByAmbiguousKey(left, right, keySelector, keySelector, comparer);

	/// <inheritdoc cref="GetIntersectionByAmbiguousKey{TLeft,TRight,TKey}"/>
	public static IDictionary<TItem, AmbiguousEqualityGrouping<TItem, TItem>> GetIntersectionByAmbiguousKey<TItem>(
		IEnumerable<TItem> left,
		IEnumerable<TItem> right,
		IEqualityComparer<TItem>? comparer = null
	)
		where TItem : notnull => GetIntersectionByAmbiguousKey(left, right, item => item, comparer);

	/// <inheritdoc cref="CompareByAmbiguousKeyEquality{TLeft,TRight,TKey}"/>
	public static AmbiguousKeyComparisonResult<TItem, TItem, TKey> CompareByAmbiguousKeyEquality<TItem, TKey>(
		IEnumerable<TItem> left,
		IEnumerable<TItem> right,
		Func<TItem, TKey> keySelector,
		IEqualityComparer<TKey>? comparer = null
	)
		where TKey : notnull => CompareByAmbiguousKeyEquality(left, right, keySelector, keySelector, comparer);

	/// <inheritdoc cref="CompareByAmbiguousKeyEquality{TLeft,TRight,TKey}"/>
	public static AmbiguousKeyComparisonResult<TItem, TItem, TItem> CompareByAmbiguousKeyEquality<TItem>(
		IEnumerable<TItem> left,
		IEnumerable<TItem> right,
		IEqualityComparer<TItem>? comparer = null
	)
		where TItem : notnull => CompareByAmbiguousKeyEquality(left, right, item => item, comparer);

	#endregion
}
