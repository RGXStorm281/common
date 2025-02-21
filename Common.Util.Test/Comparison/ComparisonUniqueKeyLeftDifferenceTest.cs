namespace RobinEpple.Common.Util.Test.Comparison;

using RobinEpple.Common.Util.Comparison;

[TestClass]
public class ComparisonUniqueKeyLeftDifferenceTest : ComparisonTestBase
{
	private IEnumerable<Left> GetLeftDifference(IEnumerable<Left> left, IEnumerable<Right> right)
		=> Comparison.GetLeftDifferenceByUniqueKey(
			left,
			right,
			leftItem => leftItem.Key,
			rightItem => rightItem.Key).ToList();

	[TestMethod]
	public void LeftEmpty_ShouldReturnEmpty()
	{
		IEnumerable<Left> left = LeftSet();
		IEnumerable<Right> right = RightSet(1, 2, 3);

		IEnumerable<Left> leftDiff = GetLeftDifference(left, right);

		AssertSetEquals(leftDiff, []);
	}

	[TestMethod]
	public void RightEmpty_ShouldReturnLeft()
	{
		IEnumerable<Left> left = LeftSet(1, 2, 3);
		IEnumerable<Right> right = RightSet();

		IEnumerable<Left> leftDiff = GetLeftDifference(left, right);
		AssertSetEquals(leftDiff, [1, 2, 3]);
	}

	[TestMethod]
	public void SomeIntersection_ShouldOnlyReturnDifference()
	{
		IEnumerable<Left> left = LeftSet(1, 2, 3);
		IEnumerable<Right> right = RightSet(3, 1);

		IEnumerable<Left> leftDiff = GetLeftDifference(left, right);
		AssertSetEquals(leftDiff, [2]);
	}

	[TestMethod]
	public void FullIntersection_ShouldReturnEmpty()
	{
		IEnumerable<Left> left = LeftSet(1, 2, 3);
		IEnumerable<Right> right = RightSet(1, 2, 3);

		IEnumerable<Left> leftDiff = GetLeftDifference(left, right);
		AssertSetEquals(leftDiff, []);
	}

	[TestMethod]
	public void EqualityComparer_ShouldTakeEffect()
	{
		string[] left =
		[
			"One",
			"Tw#o",
			"Three"
		];
		string[] right =
		[
			"One",
			"T#wo",
			"THREE"
		];

		var leftDiff = Comparison.GetLeftDifferenceByUniqueKey(left, right, new AlphanumericStringEqualityComparer()).ToList();
		if (leftDiff.Count != 1)
		{
			Assert.Fail("The set has the wrong multiplicity.");
		}

		if (leftDiff[0] != left[2])
		{
			Assert.Fail("Mismatch in the first element.");
		}
	}
}