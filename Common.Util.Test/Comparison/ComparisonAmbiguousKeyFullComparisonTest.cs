namespace RobinEpple.Common.Util.Test.Comparison;

using RobinEpple.Common.Util.Comparison;

[TestClass]
public class ComparisonAmbiguousKeyFullComparisonTest : ComparisonTestBase
{
	private AmbiguousKeyComparisonResult<Left, Right, int> GetComparisonResult(IEnumerable<Left> left, IEnumerable<Right> right)
		=> Comparison.CompareByAmbiguousKeyEquality(
			left,
			right,
			leftItem => leftItem.Key,
			rightItem => rightItem.Key);

	[TestMethod]
	public void LeftEmpty_ShouldSortAllInRightDiff()
	{
		var left = LeftSet();
		var right = RightSet(1, 1, 3);

		var result = GetComparisonResult(left, right);
		AssertSetEquals(result.LeftDifference, []);
		AssertSetEquals(result.Intersection, [], []);
		AssertSetEquals(result.RightDifference, [1, 1, 3]);
	}

	[TestMethod]
	public void RightEmpty_ShouldSortAllInLeftDiff()
	{
		var left = LeftSet(1, 2, 1);
		var right = RightSet();

		var result = GetComparisonResult(left, right);
		AssertSetEquals(result.LeftDifference, [1, 1, 2]);
		AssertSetEquals(result.Intersection, [], []);
		AssertSetEquals(result.RightDifference, []);
	}

	[TestMethod]
	public void AllKeysEqual_ShouldSortAllInIntersection()
	{
		var left = LeftSet(1, 2, 1);
		var right = RightSet(2, 1, 2);

		var result = GetComparisonResult(left, right);
		AssertSetEquals(result.LeftDifference, []);
		AssertSetEquals(result.Intersection, [1, 1, 2], [1, 2, 2]);
		AssertSetEquals(result.RightDifference, []);
	}

	[TestMethod]
	public void Overlapping_ShouldSortCorrectly()
	{
		var left = LeftSet(1, 2, 1);
		var right = RightSet(1, 1, 3);

		var result = GetComparisonResult(left, right);
		AssertSetEquals(result.LeftDifference, [2]);
		AssertSetEquals(result.Intersection, [1, 1], [1, 1]);
		AssertSetEquals(result.RightDifference, [3]);
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

		var comparisonResult = Comparison.CompareByAmbiguousKeyEquality(left, right, new AlphanumericStringEqualityComparer());

		var leftDiff = comparisonResult.LeftDifference;
		if (leftDiff.Count != 1)
		{
			Assert.Fail("The set has the wrong multiplicity.");
		}

		if (leftDiff["Two"].Count != 1)
		{
			Assert.Fail("The grouping for 'Two' has the wrong multiplicity.");
		}

		if (leftDiff["Two"][0] != left[1])
		{
			Assert.Fail("Mismatch in the first element in 'Two'.");
		}

		var intersection = comparisonResult.Intersection;
		if (intersection.Count != 1)
		{
			Assert.Fail("The set has the wrong multiplicity.");
		}

		var intersectionGroupings = intersection["One"];

		if (intersectionGroupings.Left.Count != 2 || intersectionGroupings.Right.Count != 2)
		{
			Assert.Fail("The grouping has the wrong multiplicity.");
		}

		if (intersectionGroupings.Left[0] != left[0]
		    || intersectionGroupings.Left[1] != left[2]
		    || intersectionGroupings.Right[0] != right[0]
		    || intersectionGroupings.Right[1] != right[1])
		{
			Assert.Fail("Mismatch in the intersection elements.");
		}

		var rightDiff = comparisonResult.RightDifference;
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