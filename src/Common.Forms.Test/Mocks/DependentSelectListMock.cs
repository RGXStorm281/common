namespace RobinEpple.Common.Forms.Test.Mocks;

using System.Collections.Generic;
using System.Threading.Tasks;
using RobinEpple.Common.Forms.SelectLists;

public class DependentSelectListMock<TParentValue, TValue>(
	IDictionary<TParentValue, ISelectListSource<TValue>> listByParentValue
) : ISelectListSource<TValue>
{
	public static string ParentValueKey = "parent";
	private readonly IDictionary<TParentValue, ISelectListSource<TValue>> _listByParentValue = listByParentValue;

	public IEnumerable<ISelectListItem<TValue>> LoadItems(IDictionary<string, object?>? dependencies = null)
	{
		if (
			dependencies == null
			|| !dependencies.TryGetValue(ParentValueKey, out var parent)
			|| parent is not TParentValue parentValue
		)
		{
			return Enumerable.Empty<ISelectListItem<TValue>>();
		}

		if (!_listByParentValue.TryGetValue(parentValue, out var childList))
		{
			return Enumerable.Empty<ISelectListItem<TValue>>();
		}

		return childList.LoadItems();
	}

	public async Task<IEnumerable<ISelectListItem<TValue>>> LoadItemsAsync(
		IDictionary<string, object?>? dependencies = null
	)
	{
		if (
			dependencies == null
			|| !dependencies.TryGetValue(ParentValueKey, out var parent)
			|| parent is not TParentValue parentValue
		)
		{
			return Enumerable.Empty<ISelectListItem<TValue>>();
		}

		if (!_listByParentValue.TryGetValue(parentValue, out var childList))
		{
			return Enumerable.Empty<ISelectListItem<TValue>>();
		}

		return await childList.LoadItemsAsync();
	}
}
