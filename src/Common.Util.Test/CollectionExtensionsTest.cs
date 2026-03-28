namespace RobinEpple.Common.Util.Test;

using RobinEpple.Common.Util;

[TestClass]
public class CollectionExtensionsTest
{
	private List<int> List(params int[] items) => items.ToList();

	private void AssertListEquals<TItem>(IEnumerable<TItem> left, IEnumerable<TItem> right)
	{
		var leftList = left.ToList();
		var rightList = right.ToList();

		if (leftList.Count != rightList.Count)
		{
			Assert.Fail("");
		}

		for (int i = 0; i < leftList.Count; i++)
		{
			Assert.AreEqual(leftList[i], rightList[i]);
		}
	}

	[TestMethod]
	public void None()
	{
		var list = List(1, 2, 5, 1, 3);
		Assert.IsTrue(list.None(item => item > 5));
		Assert.IsFalse(list.None(item => item >= 5));
		Assert.IsFalse(list.None(item => item > 0));
	}

	[TestMethod]
	public void ToList_ShouldApplySelector()
	{
		var initial = List(1, 2, 5, 1, 3);
		var transformed = initial.ToList(item => item % 2);

		AssertListEquals(transformed, List(1, 0, 1, 1, 1));
	}

	private class CloneableInt(int item) : ICloneable
	{
		public int Item { get; } = item;

		public object Clone() => new CloneableInt(Item);

		public override bool Equals(object? obj)
		{
			if (obj is not CloneableInt other)
			{
				return false;
			}

			return other.Item == Item;
		}

		public override int GetHashCode()
		{
			return Item.GetHashCode();
		}
	}

	[TestMethod]
	public void Clone_ShouldRespectOrderAndAllElements()
	{
		var source = new CloneableInt[]
		{
			new CloneableInt(1),
			new CloneableInt(2),
			new CloneableInt(3),
			new CloneableInt(4),
		};
		var clone = source.CloneAll();
		AssertListEquals(source, clone);
	}

	[TestMethod]
	public void ToMultiDict_ShouldRespectEqualityComparer()
	{
		var source = List(1, 2, 5, 1, 3);
		var dict = source.ToMultiDict(item => item, new ModEqualityComparer(2));

		AssertListEquals(dict[0], List(2));
		AssertListEquals(dict[1], List(1, 5, 1, 3));
	}

	[TestMethod]
	public void TrySetValue_ShouldSetIfKeyAvailable()
	{
		var target = new Dictionary<int, bool> { { 0, false }, { 1, false } };

		var success = target.TrySetValue(1, true);
		Assert.IsTrue(success);
		Assert.IsTrue(target[1]);
		Assert.IsFalse(target[0]);
	}

	[TestMethod]
	public void TrySetValue_ShouldNotSetIfKeyNotAvailable()
	{
		var target = new Dictionary<int, bool> { { 0, false }, { 1, false } };

		var success = target.TrySetValue(2, true);
		Assert.IsFalse(success);
		Assert.IsFalse(target[0]);
		Assert.IsFalse(target[1]);
		Assert.AreEqual(target.Count, 2);
	}

	[TestMethod]
	public void GetOrAdd_ShouldGetIfKeyPresent()
	{
		var target = new Dictionary<int, bool> { { 0, false }, { 1, false } };

		var item = target.GetOrAdd(1, () => true, out var created);
		Assert.IsFalse(item);
		Assert.IsFalse(created);
		Assert.AreEqual(target.Count, 2);
	}

	[TestMethod]
	public void GetOrAdd_ShouldAddIfKeyNotPresent()
	{
		var target = new Dictionary<int, bool> { { 0, false }, { 1, false } };

		var item = target.GetOrAdd(2, () => true, out var created);
		Assert.IsTrue(item);
		Assert.IsTrue(created);
		Assert.AreEqual(target.Count, 3);
	}
}
