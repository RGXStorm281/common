namespace RobinEpple.Common.Util.Test.Comparison;

using RobinEpple.Common.Util.Comparison;

[TestClass]
public class ComparisonUniqueKeyIntersectionTest : ComparisonTestBase
{
	private List<UniqueEqualityGrouping<Left, Right>> GetIntersection(
		IEnumerable<Left> left,
		IEnumerable<Right> right
	) =>
		Comparison
			.GetIntersectionByUniqueKey(left, right, leftItem => leftItem.Key, rightItem => rightItem.Key)
			.ToList();

	[TestMethod]
	public void LeftEmpty_ShouldReturnEmpty()
	{
		var left = LeftSet();
		var right = RightSet(1, 2, 3);

		var intersection = GetIntersection(left, right);
		AssertSetEquals(intersection, []);
	}

	[TestMethod]
	public void RightEmpty_ShouldReturnEmpty()
	{
		var left = LeftSet(1, 2, 3);
		var right = RightSet();

		var intersection = GetIntersection(left, right);
		AssertSetEquals(intersection, []);
	}

	[TestMethod]
	public void SomeIntersection_ShouldReturnMatching()
	{
		var left = LeftSet(1, 2, 3);
		var right = RightSet(3, 2);

		var intersection = GetIntersection(left, right);
		AssertSetEquals(intersection, [2, 3]);
	}

	[TestMethod]
	public void FullIntersection_ShouldReturnAll()
	{
		var left = LeftSet(1, 2, 3);
		var right = RightSet(3, 1, 2);

		var intersection = GetIntersection(left, right);
		AssertSetEquals(intersection, [1, 2, 3]);
	}

	[TestMethod]
	public void EqualityComparer_ShouldTakeEffect()
	{
		string[] left = ["One", "Tw#o", "Three"];
		string[] right = ["One", "T#wo", "THREE"];

		var intersection = Comparison
			.GetIntersectionByUniqueKey(left, right, new AlphanumericStringEqualityComparer())
			.ToList();
		if (intersection.Count != 2)
		{
			Assert.Fail("The set has the wrong multiplicity.");
		}

		if (intersection[0].Left != left[0] || intersection[0].Right != right[0])
		{
			Assert.Fail("Mismatch in the first element.");
		}

		if (intersection[1].Left != left[1] || intersection[1].Right != right[1])
		{
			Assert.Fail("Mismatch in the second element.");
		}
	}
}
