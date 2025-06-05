namespace RobinEpple.Common.Forms.Nodes.DefaultImplementation;

using System.Collections.Generic;
using System.Threading.Tasks;
using RobinEpple.Common.Forms.Binding;
using RobinEpple.Common.Forms.Search;

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

	/// <inheritdoc />
	public IEmbeddedModel? EmbeddedModel { get; private set; }

	internal void UseEmbeddedModel(IEmbeddedModel model)
	{
		EmbeddedModel = model;
	}

	internal void AddNode(IFormNode node)
	{
		if (_nodesByName.ContainsKey(node.Name))
		{
			throw new InvalidOperationException("Node names need to be unique.");
		}

		_nodesByName.Add(node.Name, node);
	}

	/// <inheritdoc />
	public IFormNode? FindFirst(Func<IFormNode, bool> predicate, int? maxDepth = null)
	{
		var search = new BreadthFirstSearch(predicate, true);
		search.RunOn(this);
		return search.Results.FirstOrDefault();
	}

	/// <inheritdoc />
	public IFormNode? FindFirst(string name, StringComparer? comparer = null, int? maxDepth = null)
	{
		comparer ??= StringComparer.Ordinal;
		return FindFirst(node => comparer.Equals(node.Name, name), maxDepth);
	}

	/// <inheritdoc />
	public IEnumerable<IFormNode> FindAll(Func<IFormNode, bool> predicate, int? maxDepth = null)
	{
		var search = new BreadthFirstSearch(predicate, false);
		search.RunOn(this);
		return search.Results;
	}

	/// <inheritdoc />
	public IEnumerable<IFormNode> FindAll(string name, StringComparer? comparer = null, int? maxDepth = null)
	{
		comparer ??= StringComparer.Ordinal;
		return FindAll(node => comparer.Equals(node.Name, name), maxDepth);
	}

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
	public bool StackContains(IParentNode node, out int index)
	{
		var parent = (IParentNode)this;
		index = 0;
		while (parent != null)
		{
			if (parent == node)
			{
				return true;
			}
			parent = parent.Parent;
			index++;
		}
		return false;
	}

	/// <inheritdoc />
	public IParentNode GetParentAt(int index)
	{
		if (index < 0)
		{
			throw new InvalidOperationException("Negative indices are not allowed.");
		}
		var parent = (IParentNode)this;
		for (int i = 0; i < index; i++)
		{
			parent = parent?.Parent;
		}
		if (parent == null)
		{
			throw new IndexOutOfRangeException();
		}
		return parent;
	}

	/// <inheritdoc />
	public override object Clone()
	{
		var clone = (Form)base.Clone();
		if (clone.Root == this)
		{
			clone.Root = clone;
		}
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
		foreach (var node in _nodesByName.Values)
		{
			node.Reset();
		}
	}

	/// <inheritdoc />
	public override async Task ResetAsync()
	{
		await base.ResetAsync();
		foreach (var node in _nodesByName.Values)
		{
			await node.ResetAsync();
		}
	}

	/// <inheritdoc />
	public override void Update()
	{
		base.Update();
		foreach (var node in _nodesByName.Values)
		{
			node.Update();
			IsValid = IsValid && node.IsValid;
		}
	}

	/// <inheritdoc />
	public override async Task UpdateAsync()
	{
		await base.UpdateAsync();
		foreach (var node in _nodesByName.Values)
		{
			await node.UpdateAsync();
			IsValid = IsValid && node.IsValid;
		}
	}
}
