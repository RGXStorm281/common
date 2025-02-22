namespace RobinEpple.Common.Forms.BasicApi.Nodes;

using RobinEpple.Common.Forms.BasicApi.DataContainers;

public interface IRootNode : IParentNode
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
