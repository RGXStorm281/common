namespace RobinEpple.Common.Util.Test.Comparison;

using RobinEpple.Common.Util;

[TestClass]
public class ComparisonUniqueKeyRightDifferenceTest : ComparisonTestBase
{
	private IEnumerable<Right> GetRightDifference(IEnumerable<Left> left, IEnumerable<Right> right) =>
		Comparison
			.GetRightDifferenceByUniqueKey(left, right, leftItem => leftItem.Key, rightItem => rightItem.Key)
			.ToList();

	[TestMethod]
	public void RightEmpty_ShouldReturnEmpty()
	{
		var left = LeftSet(1, 2, 3);
		var right = RightSet();

		var rightDiff = GetRightDifference(left, right);

		AssertSetEquals(rightDiff, []);
	}

	[TestMethod]
	public void LeftEmpty_ShouldReturnRight()
	{
		var left = LeftSet();
		var right = RightSet(1, 2, 3);

		var rightDiff = GetRightDifference(left, right);
		AssertSetEquals(rightDiff, [1, 2, 3]);
	}

	[TestMethod]
	public void SomeIntersection_ShouldOnlyReturnDifference()
	{
		var left = LeftSet(3, 1);
		var right = RightSet(1, 2, 3);

		var rightDiff = GetRightDifference(left, right);
		AssertSetEquals(rightDiff, [2]);
	}

	[TestMethod]
	public void FullIntersection_ShouldReturnEmpty()
	{
		var left = LeftSet(1, 2, 3);
		var right = RightSet(2, 3, 1);

		var rightDiff = GetRightDifference(left, right);
		AssertSetEquals(rightDiff, []);
	}

	[TestMethod]
	public void EqualityComparer_ShouldTakeEffect()
	{
		string[] left = ["One", "Tw#o", "Three"];
		string[] right = ["One", "T#wo", "THREE"];

		var rightDiff = Comparison
			.GetRightDifferenceByUniqueKey(left, right, new AlphanumericStringEqualityComparer())
			.ToList();
		if (rightDiff.Count != 1)
		{
			Assert.Fail("The set has the wrong multiplicity.");
		}

		if (rightDiff[0] != right[2])
		{
			Assert.Fail("Mismatch in the first element.");
		}
	}
}
