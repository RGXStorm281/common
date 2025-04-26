namespace RobinEpple.Common.Forms.Nodes.DefaultImplementation;

using System.Collections.Generic;
using System.Linq.Expressions;
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
		_valid = new(true);
		_validationErrorsByKey = [];
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
	public IForm Root { get; protected set; }

	/// <inheritdoc />
	public IScopeProvider GetScope()
	{
		if (this is IScopeProvider scope)
		{
			return scope;
		}

		if (Parent == null)
		{
			throw new InvalidOperationException("The parent stack does not contain a scope provider.");
		}

		return Parent.GetScope();
	}

	/// <inheritdoc />
	public virtual void ChangeParent(IParentNode parent)
	{
		Parent = parent;
		Root = Parent.Root;
	}

	private ResettableProperty<string> _label { get; set; }

	/// <inheritdoc />
	public string Label
	{
		get => _label.CurrentValue;
		set => _label.CurrentValue = value;
	}

	internal void ReplaceDefaultLabel(string newDefaultLabel) => _label.ReplaceDefault(newDefaultLabel);

	private ResettableProperty<bool> _visibility { get; set; }

	/// <inheritdoc />
	public bool IsVisible
	{
		get => _visibility.CurrentValue;
		private set => _visibility.CurrentValue = value;
	}

	internal void ReplaceDefaultVisibility(bool newDefaultVisibility)
	{
		_visibility.ReplaceDefault(newDefaultVisibility);
		IsVisible = newDefaultVisibility;
	}

	/// <inheritdoc />
	public IFormExpression<bool>? VisibilityCondition { get; private set; }

	internal void UseVisibilityCondition(IFormExpression<bool> condition) => VisibilityCondition = condition;

	private ResettableProperty<bool> _readonly { get; set; }

	/// <inheritdoc />
	public bool IsReadonly
	{
		get => _readonly.CurrentValue;
		private set => _readonly.CurrentValue = value;
	}

	internal void ReplaceDefaultReadonly(bool newDefaultReadonly)
	{
		_readonly.ReplaceDefault(newDefaultReadonly);
		IsReadonly = newDefaultReadonly;
	}

	/// <inheritdoc />
	public IFormExpression<bool>? ReadonlyCondition { get; private set; }

	internal void UseReadonlyCondition(IFormExpression<bool> condition) => ReadonlyCondition = condition;

	private Dictionary<string, string> _validationErrorsByKey { get; set; }

	private ResettableProperty<bool> _valid { get; set; }

	/// <inheritdoc />
	public bool IsValid
	{
		get => _valid.CurrentValue;
		protected set => _valid.CurrentValue = value;
	}

	/// <inheritdoc />
	public IReadOnlyDictionary<string, string> ValidationErrorsByKey => _validationErrorsByKey;

	private List<INodeValidator> _validators { get; set; }

	/// <inheritdoc />
	public IEnumerable<INodeValidator> NodeValidators => _validators;

	internal void UseValidator(INodeValidator validator) => _validators.Add(validator);

	private List<IFormNodeExtension> _extensions { get; set; }

	/// <inheritdoc />
	public IEnumerable<IFormNodeExtension> Extensions => _extensions;

	internal void UseExtension(IFormNodeExtension extension) => _extensions.Add(extension);

	/// <inheritdoc />
	public void SetValidationError(string id, string error) => _validationErrorsByKey[id] = error;

	/// <inheritdoc />
	public virtual object Clone()
	{
		var clone = (NodeBase)MemberwiseClone();
		clone.Name = Name;
		clone.Parent = Parent;
		clone.Root = Root;
		clone._label = (ResettableProperty<string>)_label.Clone();
		clone._visibility = (ResettableProperty<bool>)_visibility.Clone();
		clone._readonly = (ResettableProperty<bool>)_readonly.Clone();
		clone._valid = (ResettableProperty<bool>)_valid.Clone();
		clone._validationErrorsByKey = _validationErrorsByKey.ToDictionary(error => error.Key, error => error.Value);

		// Do not clone stateless decorators.
		clone.VisibilityCondition = VisibilityCondition;
		clone._validators = _validators.ToList();
		clone._extensions = _extensions.ToList();
		return clone;
	}

	/// <inheritdoc />
	public void RemoveValidationError(string id)
	{
		if (_validationErrorsByKey.ContainsKey(id))
		{
			_validationErrorsByKey.Remove(id);
		}
	}

	/// <inheritdoc />
	public virtual void Reset()
	{
		_label.Reset();
		_visibility.Reset();
		_readonly.Reset();
		_valid.Reset();
		_validationErrorsByKey.Clear();
	}

	/// <inheritdoc />
	public virtual Task ResetAsync()
	{
		Reset();
		return Task.CompletedTask;
	}

	/// <inheritdoc />
	public virtual void Update()
	{
		CallExtensionEvent(extension => extension.OnBeforeReadonlyStateEvaluation(this));
		UpdateReadonlyState();
		CallExtensionEvent(extension => extension.OnAfterReadonlyStateEvaluation(this));

		CallExtensionEvent(extension => extension.OnBeforeVisibilityEvaluation(this));
		UpdateVisibility();
		CallExtensionEvent(extension => extension.OnAfterVisibilityEvaluation(this));

		if (!IsVisible)
		{
			IsValid = true;
			return;
		}

		CallExtensionEvent(extension => extension.OnBeforeValidation(this));
		Validate();
		CallExtensionEvent(extension => extension.OnAfterValidation(this));
	}

	/// <inheritdoc />
	public virtual async Task UpdateAsync()
	{
		if (Parent != null && !Parent.IsVisible)
		{
			// If the parent of this node is not visible, this node is also not visible.
			// That manual changes get lost is an accepted behavior.
			IsVisible = false;
		}
		else if (VisibilityCondition != null)
		{
			// If the node does not have a parent or the parent is visible
			// visibility is determined by the visibility condition (if set)
			// or stays at the value defined externally.
			IsVisible = await VisibilityCondition.EvaluateOnAsync(this);
		}

		// Run all validators.
		foreach (var validator in _validators)
		{
			await validator.ValidateAsync(this);
		}

		if (!IsVisible)
		{
			// Invisible Nodes are always valid.
			IsValid = true;
		}
		else
		{
			// Visible Nodes are valid if they have no validation errors.
			IsValid = ValidationErrorsByKey.Count == 0;
		}
	}

	private void CallExtensionEvent(Action<IFormNodeExtension> callEvent)
	{
		foreach (var extension in Extensions)
		{
			callEvent(extension);
		}
	}

	private async Task CallExtensionEventAsync(Func<IFormNodeExtension, Task> callEventAsync)
	{
		foreach (var extension in Extensions)
		{
			await callEventAsync(extension);
		}
	}

	private void UpdateReadonlyState()
	{
		// Reset to default.
		_readonly.Reset();

		// If the parent of this node is readonly, this node is also readonly.
		if (Parent != null && Parent.IsReadonly)
		{
			IsReadonly = true;
			// Parent visibility overrules visibility condition.
			return;
		}

		// If the node does not have a parent or the parent is not readonly
		// this nodes state is determined by the readonly condition (if set)
		// or stays at the default value.
		if (ReadonlyCondition != null)
		{
			IsReadonly = ReadonlyCondition.EvaluateOn(this);
		}
	}

	private async Task UpdateReadonlyStateAsync()
	{
		// Reset to default.
		_readonly.Reset();

		// If the parent of this node is readonly, this node is also readonly.
		if (Parent != null && Parent.IsReadonly)
		{
			IsReadonly = true;
			// Parent visibility overrules visibility condition.
			return;
		}

		// If the node does not have a parent or the parent is not readonly
		// this nodes state is determined by the readonly condition (if set)
		// or stays at the default value.
		if (ReadonlyCondition != null)
		{
			IsReadonly = await ReadonlyCondition.EvaluateOnAsync(this);
		}
	}

	private void UpdateVisibility()
	{
		// Reset to default.
		_visibility.Reset();

		// If the parent of this node is not visible, this node is also not visible.
		if (Parent != null && !Parent.IsVisible)
		{
			IsVisible = false;
			// Parent visibility overrules visibility condition.
			return;
		}

		// If the node does not have a parent or the parent is visible
		// visibility is determined by the visibility condition (if set)
		// or stays at the default value.
		if (VisibilityCondition != null)
		{
			IsVisible = VisibilityCondition.EvaluateOn(this);
		}
	}

	private async Task UpdateVisibilityAsync()
	{
		// Reset to default.
		_visibility.Reset();

		// If the parent of this node is not visible, this node is also not visible.
		if (Parent != null && !Parent.IsVisible)
		{
			IsVisible = false;
			// Parent visibility overrules visibility condition.
			return;
		}

		// If the node does not have a parent or the parent is visible
		// visibility is determined by the visibility condition (if set)
		// or stays at the default value.
		if (VisibilityCondition != null)
		{
			IsVisible = await VisibilityCondition.EvaluateOnAsync(this);
		}
	}

	private void Validate()
	{
		// Reset validation to true.
		_valid.Reset();
		_validationErrorsByKey.Clear();

		// Validate and collect validation errors.
		foreach (var validator in NodeValidators)
		{
			validator.Validate(this);
		}

		// If there are validation errors the node is not valid.
		if (_validationErrorsByKey.Count > 0)
		{
			IsValid = false;
		}
	}

	private async Task ValidateAsync()
	{
		// Reset validation to true.
		_valid.Reset();
		_validationErrorsByKey.Clear();

		// Validate and collect validation errors.
		foreach (var validator in NodeValidators)
		{
			await validator.ValidateAsync(this);
		}

		// If there are validation errors the node is not valid.
		if (_validationErrorsByKey.Count > 0)
		{
			IsValid = false;
		}
	}
}
