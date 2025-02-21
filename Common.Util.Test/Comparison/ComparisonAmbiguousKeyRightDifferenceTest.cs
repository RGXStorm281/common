namespace RobinEpple.Common.Util.Test.Comparison;

using RobinEpple.Common.Util.Comparison;

[TestClass]
public class ComparisonAmbiguousKeyRightDifferenceTest : ComparisonTestBase
{
	private IDictionary<int, List<Right>> GetRightDifference(IEnumerable<Left> left, IEnumerable<Right> right)
		=> Comparison.GetRightDifferenceByAmbiguousKey(
			left,
			right,
			leftItem => leftItem.Key,
			rightItem => rightItem.Key);

	[TestMethod]
	public void RightEmpty_ShouldReturnEmpty()
	{
		var left = LeftSet(1, 2, 1);
		var right = RightSet();

		var rightDiff = GetRightDifference(left, right);
		AssertSetEquals(rightDiff, []);
	}

	[TestMethod]
	public void LeftEmpty_ShouldReturnRight()
	{
		var left = LeftSet();
		var right = RightSet(1, 1, 3);

		var rightDiff = GetRightDifference(left, right);
		AssertSetEquals(rightDiff, [1, 1, 3]);
	}

	[TestMethod]
	public void SomeIntersection_ShouldOnlyReturnDifference()
	{
		var left = LeftSet(1, 2, 2);
		var right = RightSet(1, 1, 3);

		var rightDiff = GetRightDifference(left, right);
		AssertSetEquals(rightDiff, [3]);
	}

	[TestMethod]
	public void FullIntersection_ShouldReturnEmpty()
	{
		var left = LeftSet(1, 2, 2);
		var right = RightSet(1, 1, 2);

		var rightDiff = GetRightDifference(left, right);
		AssertSetEquals(rightDiff, []);
	}

	[TestMethod]
	public void EqualityComparer_ShouldTakeEffect()
	{
		string[] left =
		[
			"O'ne",
			"Tw#o",
			"O-ne"
		];
		string[] right =
		[
			"On*e",
			"One",
			"THREE"
		];

		var rightDiff = Comparison.GetRightDifferenceByAmbiguousKey(left, right, new AlphanumericStringEqualityComparer());
		if (rightDiff.Count != 1)
		{
			Assert.Fail("The set has the wrong multiplicity.");
		}

		if (rightDiff["THREE"].Count != 1)
		{
			Assert.Fail("The grouping for 'THREE' has the wrong multiplicity.");
		}

		if (rightDiff["THREE"][0] != right[2])
		{
			Assert.Fail("Mismatch in the first element in 'THREE'.");
		}
	}
}