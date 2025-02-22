namespace RobinEpple.Common.Util.Test.Comparison;

using RobinEpple.Common.Util.Comparison;

[TestClass]
public class ComparisonUniqueKeyFullComparisonTest : ComparisonTestBase
{
	private UniqueKeyComparisonResult<Left, Right> GetComparisonResult(
		IEnumerable<Left> left,
		IEnumerable<Right> right
	) => Comparison.CompareByUniqueKeyEquality(left, right, leftItem => leftItem.Key, rightItem => rightItem.Key);

	[TestMethod]
	public void LeftEmpty_ShouldSortAllInRightDiff()
	{
		var left = LeftSet();
		var right = RightSet(1, 2, 3);

		var result = GetComparisonResult(left, right);
		AssertSetEquals(result.LeftDifference, []);
		AssertSetEquals(result.Intersection, []);
		AssertSetEquals(result.RightDifference, [1, 2, 3]);
	}

	[TestMethod]
	public void RightEmpty_ShouldSortAllInLeftDiff()
	{
		var left = LeftSet(1, 2, 3);
		var right = RightSet();

		var result = GetComparisonResult(left, right);
		AssertSetEquals(result.LeftDifference, [1, 2, 3]);
		AssertSetEquals(result.Intersection, []);
		AssertSetEquals(result.RightDifference, []);
	}

	[TestMethod]
	public void SetsEqual_ShouldSortAllInIntersection()
	{
		var left = LeftSet(1, 2, 3);
		var right = RightSet(3, 2, 1);

		var result = GetComparisonResult(left, right);
		AssertSetEquals(result.LeftDifference, []);
		AssertSetEquals(result.Intersection, [1, 2, 3]);
		AssertSetEquals(result.RightDifference, []);
	}

	[TestMethod]
	public void Overlapping_ShouldSortCorrectly()
	{
		var left = LeftSet(1, 3);
		var right = RightSet(2, 1);

		var result = GetComparisonResult(left, right);
		AssertSetEquals(result.LeftDifference, [3]);
		AssertSetEquals(result.Intersection, [1]);
		AssertSetEquals(result.RightDifference, [2]);
	}

	[TestMethod]
	public void EqualityComparer_ShouldTakeEffect()
	{
		string[] left = ["One", "Tw#o", "Three"];
		string[] right = ["One", "T#wo", "THREE"];

		var comparisonResult = Comparison.CompareByUniqueKeyEquality(
			left,
			right,
			new AlphanumericStringEqualityComparer()
		);

		var leftDiff = comparisonResult.LeftDifference.ToList();
		if (leftDiff.Count != 1)
		{
			Assert.Fail("The left difference has the wrong multiplicity.");
		}

		if (leftDiff[0] != left[2])
		{
			Assert.Fail("Mismatch in the first left difference element.");
		}

		var intersection = comparisonResult.Intersection.ToList();
		if (intersection.Count != 2)
		{
			Assert.Fail("The intersection has the wrong multiplicity.");
		}

		if (intersection[0].Left != left[0] || intersection[0].Right != right[0])
		{
			Assert.Fail("Mismatch in the first intersection element.");
		}

		if (intersection[1].Left != left[1] || intersection[1].Right != right[1])
		{
			Assert.Fail("Mismatch in the second intersection element.");
		}

		var rightDiff = comparisonResult.RightDifference.ToList();
		if (rightDiff.Count != 1)
		{
			Assert.Fail("The right difference has the wrong multiplicity.");
		}

		if (rightDiff[0] != right[2])
		{
			Assert.Fail("Mismatch in the first right difference element.");
		}
	}
}
