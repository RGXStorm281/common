namespace RobinEpple.Common.Forms.Nodes.DefaultImplementation;

using System.Collections.Generic;
using System.Threading.Tasks;
using RobinEpple.Common.Forms.Binding;

internal class TemplateNode : NodeBase, ITemplateNode
{
	public TemplateNode(string name, IParentNode parent)
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
		Instance?.ChangeParent(this);
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

	/// <inheritdoc />
	public IForm? Instance { get; private set; }

	/// <inheritdoc />
	public void Clear()
	{
		Instance = null;
	}

	/// <inheritdoc />
	public Task ClearAsync()
	{
		Clear();
		return Task.CompletedTask;
	}

	/// <inheritdoc />
	public override object Clone()
	{
		var clone = (TemplateNode)base.Clone();
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
		clone.Instance = (IForm?)Instance?.Clone();
		clone.Instance?.ChangeParent(clone);

		return clone;
	}

	/// <inheritdoc />
	public void Instantiate(IForm template)
	{
		if (!_templatesByName.TryGetValue(template.Name, out var configuredTemplate) || configuredTemplate != template)
		{
			throw new InvalidOperationException("The given node is not a child of this collection.");
		}

		var child = (IForm)template.Clone();
		child.ChangeParent(this);
		Instance = child;
		child.Reset();
	}

	/// <inheritdoc />
	public async Task InstantiateAsync(IForm template)
	{
		if (!_templatesByName.TryGetValue(template.Name, out var configuredTemplate) || configuredTemplate != template)
		{
			throw new InvalidOperationException("The given node is not a child of this collection.");
		}

		var child = (IForm)template.Clone();
		child.ChangeParent(this);
		Instance = child;
		await child.ResetAsync();
	}

	/// <inheritdoc />
	public string GetChildId(IFormNode child)
	{
		if (Instance != child)
		{
			throw new InvalidOperationException("The given node is not a child of this container");
		}

		return $"{GetId()}{IFormNode.PathSeparator}{child.Name}";
	}

	/// <inheritdoc />
	public override void Reset()
	{
		base.Reset();
		Instance = null;
	}

	/// <inheritdoc />
	public override async Task ResetAsync()
	{
		await base.ResetAsync();
		Instance = null;
	}

	/// <inheritdoc />
	public override void Update()
	{
		base.Update();
		if (Instance != null)
		{
			Instance.Update();
			IsValid = IsValid && Instance.IsValid;
		}
	}

	/// <inheritdoc />
	public override async Task UpdateAsync()
	{
		await base.UpdateAsync();
		if (Instance != null)
		{
			await Instance.UpdateAsync();
			IsValid = IsValid && Instance.IsValid;
		}
	}
}
