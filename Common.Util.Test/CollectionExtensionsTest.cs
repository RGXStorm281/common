namespace RobinEpple.Common.Util.Test;

[TestClass]
public class CollectionExtensionsTest
{
	private List<int> List(params int[] items) => items.ToList();

	private void AssertListEquals(List<int> left, List<int> right)
	{
		if (left.Count != right.Count)
		{
			Assert.Fail("");
		}
	}

	public void WithPrepended_ShouldPrependItem()
	{
		var initial = List(1, 2, 3);
	}
}
