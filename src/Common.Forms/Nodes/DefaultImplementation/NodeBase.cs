namespace RobinEpple.Common.Forms.Nodes.DefaultImplementation;

using System.Collections.Generic;
using System.Threading.Tasks;
using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Extensions;
using RobinEpple.Common.Forms.Validation;

internal abstract class NodeBase : IFormNode
{
	public NodeBase(string name, IForm root, IParentNode? parent)
	{
		Name = name;
		Parent = parent;
		Root = root;
		_label = new(name);
		_visibility = new(true);
		_readonly = new(false);
		_validationErrorsById = [];
		_validators = [];
		_extensions = [];
	}

	/// <inheritdoc />
	public string Name { get; private set; }

	/// <inheritdoc />
	public string GetId() => Parent?.GetChildId(this) ?? Name;

	/// <inheritdoc />
	public IParentNode? Parent { get; private set; }

	/// <inheritdoc />
	public IForm Root { get; private set; }

	private ResetableProperty<string> _label { get; }

	/// <inheritdoc />
	public string Label
	{
		get => _label.CurrentValue;
		set => _label.CurrentValue = value;
	}

	internal void ReplaceDefaultLabel(string newDefaultLabel) => _label.ReplaceDefault(newDefaultLabel);

	private ResetableProperty<bool> _visibility { get; }

	/// <inheritdoc />
	public bool IsVisible
	{
		get => _visibility.CurrentValue;
		set => _visibility.CurrentValue = value;
	}

	internal void ReplaceDefaultVisibility(bool newDefaultVisibility) =>
		_visibility.ReplaceDefault(newDefaultVisibility);

	private ResetableProperty<bool> _readonly { get; }

	/// <inheritdoc />
	public bool IsReadonly
	{
		get => _readonly.CurrentValue;
		set => _readonly.CurrentValue = value;
	}

	internal void ReplaceDefaultReadonly(bool newDefaultReadonly) => _readonly.ReplaceDefault(newDefaultReadonly);

	/// <inheritdoc />
	public IFormExpression<bool>? VisibilityCondition { get; internal set; }

	private Dictionary<string, string> _validationErrorsById { get; }

	/// <inheritdoc />
	public bool IsValid => !_validationErrorsById.Any();

	/// <inheritdoc />
	public IEnumerable<string> ValidationErrors => _validationErrorsById.Values;

	private List<INodeValidator> _validators { get; }

	/// <inheritdoc />
	public IEnumerable<INodeValidator> NodeValidators => _validators;

	internal void AddValidator(INodeValidator validator) => _validators.Add(validator);

	private List<IFormNodeExtension> _extensions { get; }

	/// <inheritdoc />
	public IEnumerable<IFormNodeExtension> Extensions => _extensions;

	internal void AddExtension(IFormNodeExtension extension) => _extensions.Add(extension);

	/// <inheritdoc />
	public void SetValidationError(string id, string error) => _validationErrorsById[id] = error;

	/// <inheritdoc />
	public abstract object Clone();

	/// <inheritdoc />
	public void RemoveValidationError(string id)
	{
		if (_validationErrorsById.ContainsKey(id))
		{
			_validationErrorsById.Remove(id);
		}
	}

	/// <inheritdoc />
	public void Reset()
	{
		_label.Reset();
		_visibility.Reset();
		_readonly.Reset();
		_validationErrorsById.Clear();
	}

	/// <inheritdoc />
	public Task ResetAsync()
	{
		Reset();
		return Task.CompletedTask;
	}

	/// <inheritdoc />
	public void Update()
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public Task UpdateAsync()
	{
		throw new NotImplementedException();
	}
}
