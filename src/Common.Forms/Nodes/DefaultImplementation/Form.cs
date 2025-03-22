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
	public IFormNode? FindNode(string name, StringComparer? comparer = null)
	{
		// Default comparer is case sensitive.
		comparer ??= StringComparer.Ordinal;

		// First search the current layer.
		foreach (var node in Nodes)
		{
			if (comparer.Equals(node.Name, name))
			{
				return node;
			}
		}

		// Then search all subsections in order.
		foreach (var node in Nodes)
		{
			if (node is IScopeProvider subsection && subsection.FindNode(name, comparer) is { } subsectionTarget)
			{
				return subsectionTarget;
			}

			if (node is ITemplateNode template && template.Instance?.FindNode(name, comparer) is { } templateTarget)
			{
				return templateTarget;
			}

			// Never search collections for unique nodes.
		}

		// If none is found return null.
		return null;
	}

	/// <inheritdoc />
	public IEnumerable<IFormNode> FindNodes(string name, StringComparer? comparer = null)
	{
		// Default comparer is case sensitive.
		comparer ??= StringComparer.Ordinal;

		// First return all matches in this node.
		foreach (var node in Nodes)
		{
			if (comparer.Equals(node.Name, name))
			{
				yield return node;
			}
		}

		// Then search all subsections in order.
		foreach (var node in Nodes)
		{
			if (node is IScopeProvider subsection)
			{
				foreach (var target in subsection.FindNodes(name, comparer))
				{
					yield return target;
				}
			}

			if (node is ITemplateNode template)
			{
				foreach (var target in template.Instance?.FindNodes(name, comparer) ?? [])
				{
					yield return target;
				}
			}

			if (node is ICollectionNode collection)
			{
				foreach (var instance in collection.Instances)
				{
					foreach (var target in instance.FindNodes(name, comparer))
					{
						yield return target;
					}
				}
			}
		}
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
