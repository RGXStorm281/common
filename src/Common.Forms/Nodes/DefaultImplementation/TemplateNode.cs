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
		base.ChangeParent(parent);
		Instance?.ChangeParent(this);
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
	public ITemplateNodeBinding? Binding { get; private set; }

	internal void UseBinding(ITemplateNodeBinding binding) => Binding = binding;

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
		clone._templatesByName = _templatesByName.ToDictionary(item => item.Key, item => (IForm)item.Value.Clone());
		clone.Instance = (IForm?)Instance?.Clone();
		clone.Instance?.ChangeParent(clone);

		// Do not clone stateless decorators.
		clone.Binding = Binding;
		return clone;
	}

	/// <inheritdoc />
	public void Instantiate(IForm template) => throw new NotImplementedException();

	/// <inheritdoc />
	public Task InstantiateAsync(IForm template) => throw new NotImplementedException();

	/// <inheritdoc />
	public IFormNode? FindNode(string name) => throw new NotImplementedException();

	/// <inheritdoc />
	public IEnumerable<IFormNode> FindNodes(string name) => throw new NotImplementedException();

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
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public override async Task UpdateAsync()
	{
		await base.UpdateAsync();
		throw new NotImplementedException();
	}
}
