namespace RobinEpple.Common.Util.Test.Comparison;

using RobinEpple.Common.Util.Comparison;

public abstract class ComparisonTestBase
{
	protected IEnumerable<Left> LeftSet(params int[] keys) => keys.Select(key => new Left(key));

	protected IEnumerable<Right> RightSet(params int[] keys) => keys.Select(key => new Right(key));

	protected record Left(int Key);

	protected record Right(int Key);

	protected void AssertSetEquals(IEnumerable<Left> left, int[] keys)
	{
		var leftList = left.ToList();
		if (leftList.Count != keys.Length)
		{
			Assert.Fail("The set does not have the right multiplicity.");
		}

		foreach (int expectedKey in keys)
		{
			if (leftList.All(item => item.Key != expectedKey))
			{
				Assert.Fail($"The list does not contain the expected item '{expectedKey}'.");
			}
		}
	}

	protected void AssertSetEquals(IEnumerable<Right> right, int[] keys)
	{
		var rightList = right.ToList();
		if (rightList.Count != keys.Length)
		{
			Assert.Fail("The set does not have the right multiplicity.");
		}

		foreach (int expectedKey in keys)
		{
			if (rightList.All(item => item.Key != expectedKey))
			{
				Assert.Fail($"The list does not contain the expected item '{expectedKey}'.");
			}
		}
	}

	protected void AssertSetEquals(IEnumerable<UniqueEqualityGrouping<Left, Right>> intersection, int[] keys)
	{
		var intersectionList = intersection.ToList();
		if (intersectionList.Count != keys.Length)
		{
			Assert.Fail("The set does not have the right multiplicity.");
		}

		foreach (int expectedKey in keys)
		{
			if (intersectionList.All(item => item.Left.Key != expectedKey || item.Right.Key != expectedKey))
			{
				Assert.Fail($"The list does not contain the expected item '{expectedKey}'.");
			}
		}
	}

	protected void AssertSetEquals(IDictionary<int, List<Left>> left, int[] keys)
	{
		var groupedKeys = keys.ToLookup(key => key).ToDictionary(group => group.Key, group => group.ToList());

		if (left.Count != groupedKeys.Count)
		{
			Assert.Fail("The set does not have the right multiplicity.");
		}

		foreach (var (key, duplicates) in groupedKeys)
		{
			if (!left.TryGetValue(key, out var leftForKey))
			{
				Assert.Fail($"The set is missing the key {key}.");
			}

			if (leftForKey.Count != duplicates.Count)
			{
				Assert.Fail($"The grouping for key {key} does not have the right multiplicity.");
			}
		}
	}

	protected void AssertSetEquals(IDictionary<int, List<Right>> right, int[] keys)
	{
		var groupedKeys = keys.ToLookup(key => key).ToDictionary(group => group.Key, group => group.ToList());

		if (right.Count != groupedKeys.Count)
		{
			Assert.Fail("The set does not have the right multiplicity.");
		}

		foreach (var (key, duplicates) in groupedKeys)
		{
			if (!right.TryGetValue(key, out var rightForKey))
			{
				Assert.Fail($"The set is missing the key {key}.");
			}

			if (rightForKey.Count != duplicates.Count)
			{
				Assert.Fail($"The grouping for key {key} does not have the right multiplicity.");
			}
		}
	}

	protected void AssertSetEquals(
		IDictionary<int, AmbiguousEqualityGrouping<Left, Right>> intersection,
		int[] leftKeys,
		int[] rightKeys
	)
	{
		var groupedLeftKeys = leftKeys.ToLookup(key => key).ToDictionary(group => group.Key, group => group.ToList());
		var groupedRightKeys = rightKeys.ToLookup(key => key).ToDictionary(group => group.Key, group => group.ToList());

		if (groupedLeftKeys.Count != groupedRightKeys.Count)
		{
			throw new ArgumentException("Invalid test configuration: key group multiplicities don't match.");
		}

		if (intersection.Count != groupedLeftKeys.Count)
		{
			Assert.Fail("The set does not have the right multiplicity.");
		}

		foreach (var (key, leftDuplicates) in groupedLeftKeys)
		{
			if (!groupedRightKeys.TryGetValue(key, out var rightDuplicates))
			{
				throw new ArgumentException($"Invalid test configuration: missing right keys for left key {key}.");
			}

			if (!intersection.TryGetValue(key, out var intersectionForKey))
			{
				Assert.Fail($"The set is missing the key {key}.");
			}

			if (intersectionForKey.Left.Count != leftDuplicates.Count)
			{
				Assert.Fail($"The left grouping for key {key} does not have the right multiplicity.");
			}

			if (intersectionForKey.Right.Count != rightDuplicates.Count)
			{
				Assert.Fail($"The right grouping for key {key} does not have the right multiplicity.");
			}
		}
	}
}
