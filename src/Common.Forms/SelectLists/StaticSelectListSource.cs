namespace RobinEpple.Common.Forms.SelectLists;

public class StaticSelectListSource<TValue>(IEnumerable<SelectListItem<TValue>> items) : ISelectListSource<TValue>
{
	private readonly List<SelectListItem<TValue>> _items = items.ToList();

	/// <inheritdoc />
	public IEnumerable<ISelectListItem<TValue>> LoadItems(IDictionary<string, object?>? dependencies = null) => _items;

	/// <inheritdoc />
	public Task<IEnumerable<ISelectListItem<TValue>>> LoadItemsAsync(
		IDictionary<string, object?>? dependencies = null
	) => Task.FromResult(LoadItems());
}
