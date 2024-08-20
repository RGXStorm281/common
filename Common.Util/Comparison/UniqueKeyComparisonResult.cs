namespace RobinEpple.HomeSuite.Common.Util.Comparison;

/// <summary>
/// The result of a comparison of two sets, consisting of the intersection and the two differences.<br/>
/// Matching elements have a 1:1 relationship.
/// </summary>
/// <typeparam name="TLeft">The type of the elements in the left set.</typeparam>
/// <typeparam name="TRight">The type of the elements in the right set.</typeparam>
public class UniqueKeyComparisonResult<TLeft, TRight>(
	List<TLeft> leftDifference, 
	List<(TLeft Left, TRight Right)> intersection, 
	List<TRight> rightDifference)
{
	/// <summary>
	/// All elements that occur in the left but not in the right set.
	/// </summary>
	public List<TLeft> LeftDifference { get; } = leftDifference;

	/// <summary>
	/// The tuples of left and right elements that are part of the intersection according to the equality condition.
	/// </summary>
	public List<(TLeft Left, TRight Right)> Intersection { get; } = intersection;

	/// <summary>
	/// All elements that occur in the right set but not in the left set.
	/// </summary>
	public List<TRight> RightDifference { get; } = rightDifference;

	/// <summary>
	/// Uses the comparison result, to execute synchronization.<br/>
	/// The sets are synchronized from left to right, so.: <br/>
	/// * Elements in the <see cref="RightDifference"/> are removed with <paramref name="removeObsolete"/>.<br/>
	/// * Elements in the <see cref="Intersection"/> are updated with <paramref name="updateMatching"/>.<br/>
	/// * Elements in the <see cref="LeftDifference"/> are created with <paramref name="createMissing"/>.
	/// </summary>
	/// <param name="removeObsolete">The function to remove obsolete right elements.</param>
	/// <param name="updateMatching">The function to update right elements with the values from a left match.</param>
	/// <param name="createMissing">The function to create a missing right element from a left one.</param>
	public void ApplyLeftToRight(
		Action<TRight> removeObsolete,
		Action<TLeft, TRight> updateMatching,
		Action<TLeft> createMissing)
	{
		// First delete obsolete -> "make room" to avoid conflicts due to unique constraints.
		foreach (var obsolete in RightDifference)
		{
			removeObsolete(obsolete);
		}

		// Continue by updating existing.
		foreach (var (updated, existing) in Intersection)
		{
			updateMatching(updated, existing);
		}

		// Lastly create new records.
		foreach (var missing in LeftDifference)
		{
			createMissing(missing);
		}
	}

	/// <summary>
	/// Uses the comparison result, to execute synchronization. A state object is passed into each action.<br/>
	/// The sets are synchronized from left to right, so.: <br/>
	/// * Elements in the <see cref="RightDifference"/> are removed with <paramref name="removeObsolete"/>.<br/>
	/// * Elements in the <see cref="Intersection"/> are updated with <paramref name="updateMatching"/>.<br/>
	/// * Elements in the <see cref="LeftDifference"/> are created with <paramref name="createMissing"/>.
	/// </summary>
	/// <typeparam name="TState">The type of a state object.</typeparam>
	/// <param name="removeObsolete">The function to remove obsolete right elements.</param>
	/// <param name="updateMatching">The function to update right elements with the values from a left match.</param>
	/// <param name="createMissing">The function to create a missing right element from a left one.</param>
	/// <param name="state">A state object, that is passed into each action.</param>
	public void ApplyLeftToRight<TState>(
		Action<TRight, TState> removeObsolete,
		Action<TLeft, TRight, TState> updateMatching,
		Action<TLeft, TState> createMissing,
		TState state)
	{
		// First delete obsolete -> "make room" to avoid conflicts due to unique constraints.
		foreach (var obsolete in RightDifference)
		{
			removeObsolete(obsolete, state);
		}

		// Continue by updating existing.
		foreach (var (updated, existing) in Intersection)
		{
			updateMatching(updated, existing, state);
		}

		// Lastly create new records.
		foreach (var missing in LeftDifference)
		{
			createMissing(missing, state);
		}
	}

	/// <inheritdoc cref="ApplyLeftToRight"/>
	public async Task ApplyLeftToRightAsync(
		Func<TRight, Task> removeObsoleteAsync,
		Func<TLeft, TRight, Task> updateMatchingAsync,
		Func<TLeft, Task> createMissingAsync)
	{
		// First delete obsolete -> "make room" to avoid conflicts due to unique constraints.
		foreach (var obsolete in RightDifference)
		{
			await removeObsoleteAsync(obsolete);
		}

		// Continue by updating existing.
		foreach (var (updated, existing) in Intersection)
		{
			await updateMatchingAsync(updated, existing);
		}

		// Lastly create new records.
		foreach (var missing in LeftDifference)
		{
			await createMissingAsync(missing);
		}
	}

	/// <inheritdoc cref="ApplyLeftToRight{TState}"/>
	public async Task ApplyLeftToRightAsync<TState>(
		Func<TRight, TState, Task> removeObsoleteAsync,
		Func<TLeft, TRight, TState, Task> updateMatchingAsync,
		Func<TLeft, TState, Task> createMissingAsync,
		TState state)
	{
		// First delete obsolete -> "make room" to avoid conflicts due to unique constraints.
		foreach (var obsolete in RightDifference)
		{
			await removeObsoleteAsync(obsolete, state);
		}

		// Continue by updating existing.
		foreach (var (updated, existing) in Intersection)
		{
			await updateMatchingAsync(updated, existing, state);
		}

		// Lastly create new records.
		foreach (var missing in LeftDifference)
		{
			await createMissingAsync(missing, state);
		}
	}
}