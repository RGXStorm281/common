namespace RobinEpple.Common.Forms.Nodes;

using RobinEpple.Common.Forms.SelectLists;

/// <summary>
/// A base interface for all nodes representing a single value.
/// </summary>
public interface IValueNode<TValue> : IFieldNode
{
	/// <summary>
	/// The value of this node.
	/// </summary>
	public TValue Value { get; set; }

	/// <summary>
	/// A provider to load named value suggestions.
	/// </summary>
	public ISelectListSource<TValue>? SelectList { get; }

	/// <summary>
	/// The select list items currently available for this node, based on the current dependencies.
	/// This list is updated every time on <see cref="IFormNode.Update"/>.
	/// </summary>
	public IEnumerable<ISelectListItem<TValue>>? CurrentSelectListItems { get; }
}
