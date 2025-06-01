namespace RobinEpple.Common.Forms.Nodes.DefaultImplementation;

using System.Collections.Generic;
using System.Threading.Tasks;
using RobinEpple.Common.Forms.Binding;
using RobinEpple.Common.Forms.Visitors;
using RobinEpple.Common.Util;

internal class CollectionNode : NodeBase, ICollectionNode
{
	public CollectionNode(string name, IParentNode parent)
		: base(name, parent.Root, parent)
	{
		_templatesByName = [];
	}

	/// <inheritdoc />
	public override void ChangeParent(IParentNode parent)
	{
		if (Parent != null)
		{
			foreach (var currentTemplate in _templatesByName.Values.ToList())
			{
				if (!Parent.StackContains(currentTemplate, out var index))
				{
					// No parent reference, so nothing to be done.
					continue;
				}

				// Template is a parent reference -> needs to be fixed.
				var newParent = parent.GetParentAt(index);
				if (newParent is not IForm newTemplate || newTemplate.Name != currentTemplate.Name)
				{
					throw new InvalidOperationException(
						$"This node contains a template reference on parent {currentTemplate.Name}. The given new parent has a different structure and breaks this reference."
					);
				}
				_templatesByName[currentTemplate.Name] = newTemplate;
			}
		}
		base.ChangeParent(parent);
		foreach (var instance in Instances)
		{
			instance.ChangeParent(this);
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

	private Dictionary<string, IForm> _templatesByName;

	/// <inheritdoc />
	public IEnumerable<IForm> Templates => _templatesByName.Values;

	internal void UseTemplate(IForm template)
	{
		if (_templatesByName.ContainsKey(template.Name))
		{
			throw new InvalidOperationException("Template names need to be unique.");
		}
		_templatesByName.Add(template.Name, template);
	}

	private List<IForm> _instances = [];

	/// <inheritdoc />
	public IEnumerable<IForm> Instances => _instances;

	/// <inheritdoc />
	public string GetChildId(IFormNode child)
	{
		if (child is not IForm instance)
		{
			throw new InvalidOperationException("The given node is not a child of this collection.");
		}

		var indexOfChild = _instances.IndexOf(instance);
		if (indexOfChild < 0)
		{
			throw new InvalidOperationException("The given node is not a child of this collection.");
		}

		return $"{GetId()}{IFormNode.IndexIdentifier.Format(indexOfChild)}{child.Name}";
	}

	/// <inheritdoc />
	public IForm Instantiate(IForm template)
	{
		if (!_templatesByName.TryGetValue(template.Name, out var configuredTemplate) || configuredTemplate != template)
		{
			throw new InvalidOperationException("The given node is not a child of this collection.");
		}

		var child = (IForm)template.Clone();
		child.ChangeParent(this);
		_instances.Add(child);
		child.Reset();

		var initializer = new NodeInitializer();
		initializer.Visit(child);
		return child;
	}

	/// <inheritdoc />
	public async Task<IForm> InstantiateAsync(IForm template)
	{
		if (!_templatesByName.TryGetValue(template.Name, out var configuredTemplate) || configuredTemplate != template)
		{
			throw new InvalidOperationException("The given node is not a child of this collection.");
		}

		var child = (IForm)template.Clone();
		child.ChangeParent(this);
		_instances.Add(child);
		await child.ResetAsync();

		var initializer = new NodeInitializer();
		initializer.Visit(child);
		return child;
	}

	/// <inheritdoc />
	public void RemoveItem(IForm instance)
	{
		var indexOfChild = _instances.IndexOf(instance);
		if (indexOfChild < 0)
		{
			throw new InvalidOperationException("The given node is not a child of this collection.");
		}
		_instances.RemoveAt(indexOfChild);
	}

	/// <inheritdoc />
	public Task RemoveItemAsync(IForm instance)
	{
		var indexOfChild = _instances.IndexOf(instance);
		if (indexOfChild < 0)
		{
			throw new InvalidOperationException("The given node is not a child of this collection.");
		}
		_instances.RemoveAt(indexOfChild);
		return Task.CompletedTask;
	}

	/// <inheritdoc />
	public void Clear()
	{
		_instances.Clear();
	}

	/// <inheritdoc />
	public Task ClearAsync()
	{
		_instances.Clear();
		return Task.CompletedTask;
	}

	/// <inheritdoc />
	public IEnumerable<IFormNode> FindNodes(string name, StringComparer? comparer = null)
	{
		// Default comparer is case sensitive.
		comparer ??= StringComparer.Ordinal;

		// Search all instances in order.
		foreach (var instance in Instances)
		{
			foreach (var target in instance.FindNodes(name, comparer))
			{
				yield return target;
			}
		}
	}

	/// <inheritdoc />
	public override object Clone()
	{
		var clone = (CollectionNode)base.Clone();
		clone._templatesByName = _templatesByName.ToDictionary(
			item => item.Key,
			item =>
			{
				if (Parent?.StackContains(item.Value, out _) == true)
				{
					// Do not clone parents.
					return item.Value;
				}
				return (IForm)item.Value.Clone();
			}
		);
		clone._instances = _instances.CloneAll().ToList();
		foreach (var clonedChild in clone._instances)
		{
			clonedChild.ChangeParent(clone);
		}

		return clone;
	}

	/// <inheritdoc />
	public override void Reset()
	{
		base.Reset();
		Clear();
	}

	/// <inheritdoc />
	public override async Task ResetAsync()
	{
		await base.ResetAsync();
		await ClearAsync();
	}

	/// <inheritdoc />
	public override void Update()
	{
		base.Update();
		foreach (var instance in _instances)
		{
			instance.Update();
			IsValid = IsValid && instance.IsValid;
		}
	}

	/// <inheritdoc />
	public override async Task UpdateAsync()
	{
		await base.UpdateAsync();
		foreach (var instance in _instances)
		{
			await instance.UpdateAsync();
			IsValid = IsValid && instance.IsValid;
		}
	}
}
