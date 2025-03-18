namespace RobinEpple.Common.Util.Test.Comparison;

using RobinEpple.Common.Util;

[TestClass]
public class ComparisonAmbiguousKeyLeftDifferenceTest : ComparisonTestBase
{
	private IDictionary<int, IEnumerable<Left>> GetLeftDifference(IEnumerable<Left> left, IEnumerable<Right> right) =>
		Comparison.GetLeftDifferenceByAmbiguousKey(left, right, leftItem => leftItem.Key, rightItem => rightItem.Key);

	[TestMethod]
	public void LeftEmpty_ShouldReturnEmpty()
	{
		var left = LeftSet();
		var right = RightSet(1, 1, 3);

		var leftDiff = GetLeftDifference(left, right);
		AssertSetEquals(leftDiff, []);
	}

	[TestMethod]
	public void RightEmpty_ShouldReturnLeft()
	{
		var left = LeftSet(1, 2, 1);
		var right = RightSet();

		var leftDiff = GetLeftDifference(left, right);
		AssertSetEquals(leftDiff, [1, 1, 2]);
	}

	[TestMethod]
	public void SomeIntersection_ShouldOnlyReturnDifference()
	{
		var left = LeftSet(1, 2, 2);
		var right = RightSet(1, 1, 3);

		var leftDiff = GetLeftDifference(left, right);
		AssertSetEquals(leftDiff, [2, 2]);
	}

	[TestMethod]
	public void FullIntersection_ShouldReturnEmpty()
	{
		var left = LeftSet(1, 2, 2);
		var right = RightSet(1, 1, 2);

		var leftDiff = GetLeftDifference(left, right);
		AssertSetEquals(leftDiff, []);
	}

	[TestMethod]
	public void EqualityComparer_ShouldTakeEffect()
	{
		string[] left = ["O'ne", "Tw#o", "O-ne"];
		string[] right = ["On*e", "One", "THREE"];

		var leftDiff = Comparison.GetLeftDifferenceByAmbiguousKey(
			left,
			right,
			new AlphanumericStringEqualityComparer()
		);
		if (leftDiff.Count != 1)
		{
			Assert.Fail("The set has the wrong multiplicity.");
		}

		var twoGrouping = leftDiff["Two"].ToList();
		if (twoGrouping.Count != 1)
		{
			Assert.Fail("The grouping for 'Two' has the wrong multiplicity.");
		}

		if (twoGrouping[0] != left[1])
		{
			Assert.Fail("Mismatch in the first element in 'Two'.");
		}
	}
}
