namespace RobinEpple.Common.Forms.SelectLists;

using RobinEpple.Common.SourceGenerators.Abstractions;

public partial class StaticSelectListSource<TValue>(IEnumerable<SelectListItem<TValue>> items)
	: ISelectListSource<TValue>
{
	private readonly List<SelectListItem<TValue>> _items = items.ToList();

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public IEnumerable<ISelectListItem<TValue>> LoadItems(IDictionary<string, object?>? dependencies = null) => _items;
}
