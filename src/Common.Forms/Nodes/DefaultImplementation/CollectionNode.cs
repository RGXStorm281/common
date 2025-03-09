namespace RobinEpple.Common.Forms.Nodes.DefaultImplementation;

using System.Collections.Generic;
using System.Threading.Tasks;
using RobinEpple.Common.Forms.Binding;
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
		base.ChangeParent(parent);
		foreach (var instance in Instances)
		{
			instance.ChangeParent(this);
		}
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
	public ICollectionNodeBinding? Binding { get; private set; }

	internal void UseBinding(ICollectionNodeBinding binding) => Binding = binding;

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

		return $"{GetId()}{IFormNode.IndexIdentifer.Format(indexOfChild)}{child.Name}";
	}

	/// <inheritdoc />
	public void Instantiate(IForm template) => throw new NotImplementedException();

	/// <inheritdoc />
	public Task InstantiateAsync(IForm template) => throw new NotImplementedException();

	/// <inheritdoc />
	public void RemoveItem(IForm instance) => throw new NotImplementedException();

	/// <inheritdoc />
	public Task RemoveItemAsync(IForm instance) => throw new NotImplementedException();

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
	public IFormNode? FindNode(string name) => throw new NotImplementedException();

	/// <inheritdoc />
	public IEnumerable<IFormNode> FindNodes(string name) => throw new NotImplementedException();

	/// <inheritdoc />
	public override object Clone()
	{
		var clone = (CollectionNode)base.Clone();
		clone._templatesByName = _templatesByName.ToDictionary(item => item.Key, item => (IForm)item.Value.Clone());
		clone._instances.Clear();
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
		}
	}

	/// <inheritdoc />
	public override async Task UpdateAsync()
	{
		await base.UpdateAsync();
		foreach (var instance in _instances)
		{
			await instance.UpdateAsync();
		}
	}
}
