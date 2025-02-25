namespace RobinEpple.Common.Forms.BasicApi.Nodes;

using RobinEpple.Common.Forms.BasicApi.DataContainers;

/// <summary>
/// The interface for a node that groups a set of child nodes into a complex structure in the form.
/// </summary>

// ReSharper disable once PossibleInterfaceMemberAmbiguity
public interface IContainerNode : IFormNode, IParentNode
{
	/// <summary>
	/// The children of the node.
	/// </summary>
	public IEnumerable<IFormNode> Children { get; }

	/// <summary>
	/// Updates the data in the node with the data from the container.
	/// </summary>
	/// <param name="context">The context with the data.</param>
	public void SetData(IContainerDataContainer context);

	/// <summary>
	/// Emits the current data from this node as a data container.
	/// </summary>
	/// <returns>The data container.</returns>
	public IContainerDataContainer GetData();
}
