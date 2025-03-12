namespace RobinEpple.Common.Forms.Nodes.DefaultImplementation;

using System.Collections.Generic;
using System.Threading.Tasks;

internal class Form : NodeBase, IForm
{
	public Form(string name, IParentNode? parent)
#pragma warning disable CS8604 // Possible null reference argument.
		// Root may be empty shortly and is then set to a recursive reference below.
		: base(name, parent?.Root, parent)
#pragma warning restore CS8604 // Possible null reference argument.

	{
		Root ??= this;
		_nodesByName = [];
	}

	private Dictionary<string, IFormNode> _nodesByName { get; set; }

	/// <inheritdoc />
	public IEnumerable<IFormNode> Nodes => _nodesByName.Values;

	internal void AddNode(IFormNode node)
	{
		if (_nodesByName.ContainsKey(node.Name))
		{
			throw new InvalidOperationException("Node names need to be unique.");
		}

		_nodesByName.Add(node.Name, node);
	}

	/// <inheritdoc />
	public IFormNode? FindNode(string name) => throw new NotImplementedException();

	/// <inheritdoc />
	public IEnumerable<IFormNode> FindNodes(string name) => throw new NotImplementedException();

	/// <inheritdoc />
	public string GetChildId(IFormNode child)
	{
		if (!_nodesByName.TryGetValue(child.Name, out var node) || !ReferenceEquals(node, child))
		{
			throw new InvalidOperationException("The given node is not a direct child of this form.");
		}

		return $"{GetId()}{IFormNode.PathSeparator}{child.Name}";
	}

	/// <inheritdoc />
	public override void ChangeParent(IParentNode parent)
	{
		base.ChangeParent(parent);
		foreach (var node in _nodesByName.Values)
		{
			node.ChangeParent(this);
		}
	}

	/// <inheritdoc />
	public override object Clone()
	{
		var clone = (Form)base.Clone();
		clone._nodesByName = _nodesByName.ToDictionary(item => item.Key, item => (IFormNode)item.Value.Clone());
		foreach (var clonedChild in clone._nodesByName.Values)
		{
			clonedChild.ChangeParent(clone);
		}
		return clone;
	}

	/// <inheritdoc />
	public override void Reset()
	{
		base.Reset();
		_nodesByName.Clear();
	}

	/// <inheritdoc />
	public override async Task ResetAsync()
	{
		await base.ResetAsync();
		_nodesByName.Clear();
	}

	/// <inheritdoc />
	public override void Update()
	{
		base.Update();
		foreach (var node in _nodesByName.Values)
		{
			node.Update();
		}
	}

	/// <inheritdoc />
	public override async Task UpdateAsync()
	{
		await base.UpdateAsync();
		foreach (var node in _nodesByName.Values)
		{
			await node.UpdateAsync();
		}
	}
}
