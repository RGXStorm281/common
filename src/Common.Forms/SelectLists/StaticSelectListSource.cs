namespace RobinEpple.Common.Forms.SelectLists;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

/// <summary>
/// A static collection as select list.
/// </summary>
public partial class StaticSelectListSource<TValue>(IEnumerable<SelectListItem<TValue>> items)
	: ISelectListSource<TValue>
{
	private readonly List<SelectListItem<TValue>> _items = items.ToList();

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public IEnumerable<ISelectListItem<TValue>> LoadFor(IFormNode node) => _items;
}
