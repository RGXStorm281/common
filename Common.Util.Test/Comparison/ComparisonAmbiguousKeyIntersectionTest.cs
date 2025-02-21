namespace RobinEpple.Common.Util.Test.Comparison;

using RobinEpple.Common.Util.Comparison;

[TestClass]
public class ComparisonAmbiguousKeyIntersectionTest : ComparisonTestBase
{
	private IDictionary<int, AmbiguousEqualityGrouping<Left, Right>> GetIntersection(IEnumerable<Left> left, IEnumerable<Right> right)
		=> Comparison.GetIntersectionByAmbiguousKey(
			left,
			right,
			leftItem => leftItem.Key,
			rightItem => rightItem.Key);


	[TestMethod]
	public void LeftEmpty_ShouldReturnEmpty()
	{
		var left = LeftSet();
		var right = RightSet(1, 1, 3);

		var intersection = GetIntersection(left, right);
		AssertSetEquals(intersection, [], []);
	}

	[TestMethod]
	public void RightEmpty_ShouldReturnEmpty()
	{
		var left = LeftSet(1, 2, 1);
		var right = RightSet();

		var intersection = GetIntersection(left, right);
		AssertSetEquals(intersection, [], []);
	}

	[TestMethod]
	public void SomeIntersection_ShouldReturnMatching()
	{
		var left = LeftSet(1, 2, 1);
		var right = RightSet(3, 2);

		var intersection = GetIntersection(left, right);
		AssertSetEquals(intersection, [2], [2]);
	}

	[TestMethod]
	public void FullIntersection_ShouldReturnAll()
	{
		var left = LeftSet(1, 2, 1);
		var right = RightSet(2, 1, 2);

		var intersection = GetIntersection(left, right);
		AssertSetEquals(intersection, [1, 1, 2], [1, 2, 2]);
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

		var intersection = Comparison.GetIntersectionByAmbiguousKey(left, right, new AlphanumericStringEqualityComparer());
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
	}
}