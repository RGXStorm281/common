namespace RobinEpple.Common.Forms.BasicForm.Forms;

using RobinEpple.Common.Forms.BasicApi;
using RobinEpple.Common.Forms.BasicApi.DataContainers;
using RobinEpple.Common.Forms.BasicApi.Nodes;
using RobinEpple.Common.Forms.BasicForm.DataContainers;

/// <summary>
/// Represents a sub-form on a complex property in a parent model.
/// </summary>
/// <inheritdoc cref="NodeBase{TNodeType}" />
internal class SubForm(
	string id,
	IParentNode parent,
	ILabel? label,
	IExpression<bool>? visibilityCondition,
	IEnumerable<INodeValidator<SubForm>> validators
) : NodeBase<SubForm>(id, parent, label, visibilityCondition, validators), IContainerNode
{
	private readonly Dictionary<string, IFormNode> _nodesById = [];

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
	public IEnumerable<IFormNode> Children => _nodesById.Values;

	/// <inheritdoc />
	public void SetData(IContainerDataContainer context)
	{
		_nodesById.UpdateFrom(context);
	}

	/// <inheritdoc />
	public IContainerDataContainer GetData() =>
		new BasicContainerDataContainer { ChildDataContainersByChildId = _nodesById.GetDataContainers() };

	/// <summary>
	/// Adds a node to the root.
	/// </summary>
	/// <param name="node">The node.</param>
	public void AddNode(IFormNode node) => _nodesById.Add(node.Id, node);
}
