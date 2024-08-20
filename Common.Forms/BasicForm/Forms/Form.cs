using RobinEpple.Common.Forms.BasicApi.DataContainers;
using RobinEpple.Common.Forms.BasicApi.Nodes;
using RobinEpple.Common.Forms.BasicForm.DataContainers;

namespace RobinEpple.Common.Forms.BasicForm.Forms;

/// <summary>
/// The root node of a form tree.
/// </summary>
internal class Form : IRootNode
{
	private readonly Dictionary<string, IFormNode> _nodesById = [];

	/// <inheritdoc />
	public bool IsVisible
		=> true;

	/// <inheritdoc />
	public IFormNode FindClosestBefore(IFormNode currentNode, Func<IFormNode, bool> predicate)
	{
		var nodes = _nodesById.Values.ToList();
		return nodes.FindClosestBefore(currentNode, predicate);
	}

	/// <inheritdoc />
	public IFormNode? FindFirstBackwards(Func<IFormNode, bool> predicate)
	{
		var nodes = _nodesById.Values.ToList();
		return nodes.FindFirstBackwards(predicate);
	}

	/// <inheritdoc />
	public IEnumerable<IFormNode> Children
		=> _nodesById.Values;

	/// <inheritdoc />
	public void SetData(IContainerDataContainer context)
	{
		_nodesById.UpdateFrom(context);
	}

	/// <inheritdoc />
	public IContainerDataContainer GetData()
		=> new BasicContainerDataContainer { ChildDataContainersByChildId = _nodesById.GetDataContainers() };

	/// <summary>
	/// Adds a node to the root.
	/// </summary>
	/// <param name="node">The node.</param>
	public void AddNode(IFormNode node)
		=> _nodesById.Add(node.Id, node);
}