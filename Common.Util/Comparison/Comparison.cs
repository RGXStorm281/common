namespace RobinEpple.HomeSuite.Common.Util.Comparison;

public static class Comparison
{
	#region Compare

	/// <summary>
	/// Compares the sets using keys of identical type and returns the left-hand difference.<br/>
	/// The keys must be unique.
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
		IEqualityComparer<TKey>? comparer = null)
		where TKey : notnull
	{
		// Group and index right set by key.
		var rightDictionary = right.ToDictionary(rightKeySelector, comparer);

		// Iterate over the left set and return only those that are not contained in the right set.
		foreach (var left1 in left)
		{
			if (!rightDictionary.ContainsKey(leftKeySelector(left1)))
			{
				yield return left1;
			}
		}
	}

	/// <summary>
	/// Compares the sets using keys of identical type and returns the left-hand difference.<br/>
	/// The keys must be unique.
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
		IEqualityComparer<TKey>? comparer = null)
		where TKey : notnull
	{
		// Group and index left set by key.
		var leftDictionary = left.ToDictionary(leftKeySelector, comparer);

		// Iterate over the right set and return only those that are not contained in the left set.
		foreach (var right1 in right)
		{
			if (!leftDictionary.ContainsKey(rightKeySelector(right1)))
			{
				yield return right1;
			}
		}
	}

	/// <summary>
	/// Compares the sets using keys of identical type and returns the left-hand difference.<br/>
	/// The keys must be unique.
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
	public static IEnumerable<(TLeft Left, TRight Right)> GetIntersectionByUniqueKey<TLeft, TRight, TKey>(
		IEnumerable<TLeft> left,
		IEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		IEqualityComparer<TKey>? comparer = null)
		where TKey : notnull
	{
		// Group and index left set by key.
		var leftDictionary = left.ToDictionary(leftKeySelector, comparer);

		// Iterate over the right set and return only those that are also contained in the left set.
		foreach (var rightItem in right)
		{
			if (leftDictionary.TryGetValue(rightKeySelector(rightItem), out var leftItem))
			{
				yield return (leftItem, rightItem);
			}
		}
	}

	/// <summary>
	/// Compares the sets using keys of identical type and returns the left-hand difference.<br/>
	/// The keys must be unique.
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
		IEqualityComparer<TKey>? comparer = null)
		where TKey : notnull
	{
		var leftDifference = new List<TLeft>();
		var intersection = new List<(TLeft Left, TRight Right)>();

		// Grouping and indexing sets by key.
		var leftDictionary = left.ToDictionary(leftKeySelector, comparer);
		var rightDictionary = right.ToDictionary(rightKeySelector, comparer);

		// Iterate over the left dictionary to form left difference and intersection.
		foreach (var pair in leftDictionary)
		{
			if (rightDictionary.TryGetValue(pair.Key, out var rightItem))
			{
				// Hit -> Intersection.
				intersection.Add((pair.Value, rightItem));
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
		return new(leftDifference, intersection, rightDictionary.Values.ToList());
	}

	/// <summary>
	/// Compares the sets using keys of identical type and returns the left-hand difference.<br/>
	/// The keys must not be unique.
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
		IEqualityComparer<TKey>? comparer = null)
		where TKey : notnull
	{
		var leftDifference = new Dictionary<TKey, List<TLeft>>();
		var intersection = new List<(List<TLeft> Left, List<TRight> Right)>();

		// Grouping and indexing sets by key.
		var leftDictionary = left.ToMultiDict(leftKeySelector, comparer);
		var rightDictionary = right.ToMultiDict(rightKeySelector, comparer);

		// Iterate over the left dictionary to form left difference and intersection.
		foreach (var pair in leftDictionary)
		{
			if (rightDictionary.TryGetValue(pair.Key, out var rightItem))
			{
				// Hit -> Intersection.
				intersection.Add((pair.Value, rightItem));
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
		return new(leftDifference, intersection, rightDictionary);
	}

	#endregion

	#region Synchronize unique key

	/// <summary>
	/// Compares the sets using keys of identical type. The keys must be unique.<br/>
	/// The comparison result is then used to synchronize the two sets by modifying the right one using the given functions.
	/// </summary>
	/// <typeparam name="TLeft">The type of elements in the left collection.</typeparam>
	/// <typeparam name="TRight">The type of elements in the right collection.</typeparam>
	/// <typeparam name="TKey">The type of the shared key.</typeparam>
	/// <param name="left">The left collection.</param>
	/// <param name="right">The right collection.</param>
	/// <param name="leftKeySelector">The function to select the key from the left elements.</param>
	/// <param name="rightKeySelector">The function to select the key from the right elements.</param>
	/// <param name="removeObsolete">The function to remove obsolete right elements.</param>
	/// <param name="updateMatching">The function to update right elements with the values from a left match.</param>
	/// <param name="createMissing">The function to create a missing right element from a left one.</param>
	/// <param name="comparer">Optional comparer for individual equality definitions on the key.</param>
	public static void SynchronizeLeftToRightByUniqueKey<TLeft, TRight, TKey>(
		IEnumerable<TLeft> left,
		IEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Action<TRight> removeObsolete,
		Action<TLeft, TRight> updateMatching,
		Action<TLeft> createMissing,
		IEqualityComparer<TKey>? comparer = null) 
		where TKey : notnull
	{
		var result = CompareByUniqueKeyEquality(left, right, leftKeySelector, rightKeySelector, comparer);
		result.ApplyLeftToRight(removeObsolete, updateMatching, createMissing);
	}

	/// <summary>
	/// Compares the sets using keys of identical type. The keys must be unique.<br/>
	/// The comparison result is then used to synchronize the two sets by modifying the right one using the given functions.
	/// </summary>
	/// <typeparam name="TLeft">The type of elements in the left collection.</typeparam>
	/// <typeparam name="TRight">The type of elements in the right collection.</typeparam>
	/// <typeparam name="TKey">The type of the shared key.</typeparam>
	/// <typeparam name="TState">The type of state object.</typeparam>
	/// <param name="left">The left collection.</param>
	/// <param name="right">The right collection.</param>
	/// <param name="leftKeySelector">The function to select the key from the left elements.</param>
	/// <param name="rightKeySelector">The function to select the key from the right elements.</param>
	/// <param name="removeObsolete">The function to remove obsolete right elements.</param>
	/// <param name="updateMatching">The function to update right elements with the values from a left match.</param>
	/// <param name="createMissing">The function to create a missing right element from a left one.</param>
	/// <param name="state">A state object, that is passed into each action.</param>
	/// <param name="comparer">Optional comparer for individual equality definitions on the key.</param>
	public static void SynchronizeLeftToRightByUniqueKey<TLeft, TRight, TKey, TState>(
		IEnumerable<TLeft> left,
		IEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Action<TRight, TState> removeObsolete,
		Action<TLeft, TRight, TState> updateMatching,
		Action<TLeft, TState> createMissing,
		TState state,
		IEqualityComparer<TKey>? comparer = null) 
		where TKey : notnull
	{
		var result = CompareByUniqueKeyEquality(left, right, leftKeySelector, rightKeySelector, comparer);
		result.ApplyLeftToRight(removeObsolete, updateMatching, createMissing, state);
	}

	/// <inheritdoc cref="SynchronizeLeftToRightByUniqueKey{TLeft, TRight, TKey}"/>
	public static async Task SynchronizeLeftToRightByUniqueKeyAsync<TLeft, TRight, TKey>(
		IEnumerable<TLeft> left,
		IEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Func<TRight, Task> removeObsoleteAsync,
		Func<TLeft, TRight, Task> updateMatchingAsync,
		Func<TLeft, Task> createMissingAsync,
		IEqualityComparer<TKey>? comparer = null) 
		where TKey : notnull
	{
		var result = CompareByUniqueKeyEquality(left, right, leftKeySelector, rightKeySelector, comparer);
		await result.ApplyLeftToRightAsync(removeObsoleteAsync, updateMatchingAsync, createMissingAsync);
	}

	/// <inheritdoc cref="SynchronizeLeftToRightByUniqueKey{TLeft, TRight, TKey,TState}"/>
	public static async Task SynchronizeLeftToRightByUniqueKeyAsync<TLeft, TRight, TKey, TState>(
		IEnumerable<TLeft> left,
		IEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Func<TRight, TState, Task> removeObsoleteAsync,
		Func<TLeft, TRight, TState, Task> updateMatchingAsync,
		Func<TLeft, TState, Task> createMissingAsync,
		TState state,
		IEqualityComparer<TKey>? comparer = null) 
		where TKey : notnull
	{
		var result = CompareByUniqueKeyEquality(left, right, leftKeySelector, rightKeySelector, comparer);
		await result.ApplyLeftToRightAsync(removeObsoleteAsync, updateMatchingAsync, createMissingAsync, state);
	}

	#endregion

	#region Synchronize ambigous key

	/// <summary>
	/// Compares the sets using keys of identical type. The sets may contain the same key multiple times.<br/>
	/// The comparison result is then used to synchronize the two sets by modifying the right one using the given functions.
	/// </summary>
	/// <typeparam name="TLeft">The type of elements in the left collection.</typeparam>
	/// <typeparam name="TRight">The type of elements in the right collection.</typeparam>
	/// <typeparam name="TKey">The type of the shared key.</typeparam>
	/// <param name="left">The left collection.</param>
	/// <param name="right">The right collection.</param>
	/// <param name="leftKeySelector">The function to select the key from the left elements.</param>
	/// <param name="rightKeySelector">The function to select the key from the right elements.</param>
	/// <param name="removeObsolete">The function to remove obsolete right elements.</param>
	/// <param name="updateMatching">The function to update right elements with the values from a left match.</param>
	/// <param name="createMissing">The function to create a missing right element from a left one.</param>
	/// <param name="comparer">Optional comparer for individual equality definitions on the key.</param>
	public static void SynchronizeLeftToRightByAmbiguousKey<TLeft, TRight, TKey>(
		IEnumerable<TLeft> left,
		IEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Action<TKey, List<TRight>> removeObsolete,
		Action<List<TLeft>, List<TRight>> updateMatching,
		Action<TKey, List<TLeft>> createMissing,
		IEqualityComparer<TKey>? comparer = null)
		where TKey : notnull
	{
		var result = CompareByAmbiguousKeyEquality(left, right, leftKeySelector, rightKeySelector, comparer);
		result.ApplyLeftToRight(removeObsolete, updateMatching, createMissing);
	}

	/// <summary>
	/// Compares the sets using keys of identical type. The sets may contain the same key multiple times.<br/>
	/// The comparison result is then used to synchronize the two sets by modifying the right one using the given functions.
	/// </summary>
	/// <typeparam name="TLeft">The type of elements in the left collection.</typeparam>
	/// <typeparam name="TRight">The type of elements in the right collection.</typeparam>
	/// <typeparam name="TKey">The type of the shared key.</typeparam>
	/// <typeparam name="TState">The type of state object.</typeparam>
	/// <param name="left">The left collection.</param>
	/// <param name="right">The right collection.</param>
	/// <param name="leftKeySelector">The function to select the key from the left elements.</param>
	/// <param name="rightKeySelector">The function to select the key from the right elements.</param>
	/// <param name="removeObsolete">The function to remove obsolete right elements.</param>
	/// <param name="updateMatching">The function to update right elements with the values from a left match.</param>
	/// <param name="createMissing">The function to create a missing right element from a left one.</param>
	/// <param name="state">A state object, that is passed into each action.</param>
	/// <param name="comparer">Optional comparer for individual equality definitions on the key.</param>
	public static void SynchronizeLeftToRightByAmbiguousKey<TLeft, TRight, TKey, TState>(
		IEnumerable<TLeft> left,
		IEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Action<TKey, List<TRight>, TState> removeObsolete,
		Action<List<TLeft>, List<TRight>, TState> updateMatching,
		Action<TKey, List<TLeft>, TState> createMissing,
		TState state,
		IEqualityComparer<TKey>? comparer = null)
		where TKey : notnull
	{
		var result = CompareByAmbiguousKeyEquality(left, right, leftKeySelector, rightKeySelector, comparer);
		result.ApplyLeftToRight(removeObsolete, updateMatching, createMissing, state);
	}

	/// <inheritdoc cref="SynchronizeLeftToRightByAmbiguousKey{TLeft, TRight, TKey}"/>
	public static async Task SynchronizeLeftToRightByAmbiguousKeyAsync<TLeft, TRight, TKey>(
		IEnumerable<TLeft> left,
		IEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Func<TKey, List<TRight>, Task> removeObsoleteAsync,
		Func<List<TLeft>, List<TRight>, Task> updateMatchingAsync,
		Func<TKey, List<TLeft>, Task> createMissingAsync,
		IEqualityComparer<TKey>? comparer = null) 
		where TKey : notnull
	{
		var result = CompareByAmbiguousKeyEquality(left, right, leftKeySelector, rightKeySelector, comparer);
		await result.ApplyLeftToRightAsync(removeObsoleteAsync, updateMatchingAsync, createMissingAsync);
	}

	/// <inheritdoc cref="SynchronizeLeftToRightByAmbiguousKey{TLeft, TRight, TKey,TState}"/>
	public static async Task SynchronizeLeftToRightByAmbiguousKeyAsync<TLeft, TRight, TKey, TState>(
		IEnumerable<TLeft> left,
		IEnumerable<TRight> right,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Func<TKey, List<TRight>, TState, Task> removeObsoleteAsync,
		Func<List<TLeft>, List<TRight>, TState, Task> updateMatchingAsync,
		Func<TKey, List<TLeft>, TState, Task> createMissingAsync,
		TState state,
		IEqualityComparer<TKey>? comparer = null) 
		where TKey : notnull
	{
		var result = CompareByAmbiguousKeyEquality(left, right, leftKeySelector, rightKeySelector, comparer);
		await result.ApplyLeftToRightAsync(removeObsoleteAsync, updateMatchingAsync, createMissingAsync, state);
	}

	#endregion
}