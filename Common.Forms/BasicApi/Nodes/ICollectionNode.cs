using RobinEpple.Common.Forms.BasicApi.DataContainers;

namespace RobinEpple.Common.Forms.BasicApi.Nodes;

/// <summary>
/// The interface for nodes that store a list of values.
/// </summary>
/// <typeparam name="TItem">The value type of the items.</typeparam>
// ReSharper disable once PossibleInterfaceMemberAmbiguity
public interface ICollectionNode<out TItem> : IFormNode
{
	/// <summary>
	/// The value collection of the node.
	/// </summary>
	public IEnumerable<TItem> Values { get; }

	/// <summary>
	/// Updates the data in the node with the data from the container.
	/// </summary>
	/// <param name="context">The context with the data.</param>
	public void SetData(ICollectionDataContainer context);

	/// <summary>
	/// Emits the current data from this node as a data container.
	/// </summary>
	/// <returns>The data container.</returns>
	public ICollectionDataContainer GetData();
}