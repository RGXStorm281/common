using RobinEpple.Common.Forms.BasicApi;
using RobinEpple.Common.Forms.BasicApi.Nodes;

namespace RobinEpple.Common.Forms.BasicForm;

/// <summary>
/// The abstract base class for a node in the form tree.
/// </summary>
/// <typeparam name="TNodeImplementationType">The implementation type of this node base class.</typeparam>
/// <param name="id">The id of the current node.</param>
/// <param name="parent">The parent node, that contains this node.</param>
/// <param name="label">Optional label for defining the node.</param>
/// <param name="visibilityCondition">Optional condition for when the node is visible.</param>
/// <param name="validators">A list of validators for this node.</param>
internal abstract class NodeBase<TNodeImplementationType>(
	string id,
	IParentNode parent,
	ILabel? label,
	IExpression<bool>? visibilityCondition,
	IEnumerable<INodeValidator<TNodeImplementationType>> validators)
	: IFormNode
	where TNodeImplementationType : IFormNode
{
	#region form structure

	/// <inheritdoc />
	public string Id { get; } = id;

	/// <inheritdoc />
	public IParentNode Parent { get; internal set; } = parent;

	#endregion

	#region node definition

	/// <inheritdoc />
	public ILabel? Label { get; private set; } = label;

	/// <inheritdoc />
	public IExpression<bool>? VisibilityCondition { get; private set; } = visibilityCondition;

	#endregion

	#region state

	/// <inheritdoc />
	public bool IsVisible { get; private set; } = true;

	/// <inheritdoc />
	public bool IsValidated { get; private set; }

	/// <inheritdoc />
	public bool IsValid { get; protected set; } = true;

	private List<string> _validationErrors = [];

	/// <inheritdoc />
	public IEnumerable<string> ValidationErrors
		=> _validationErrors;

	/// <summary>
	/// A list of validators to execute on <see cref="UpdateState" />.
	/// </summary>
	private IEnumerable<INodeValidator<TNodeImplementationType>> Validators { get; set; } = validators;

	/// <inheritdoc />
	public void AddValidationError(string error)
	{
		IsValid = false;
		_validationErrors.Add(error);
	}

	/// <inheritdoc />
	public virtual void SetValid()
	{
		_validationErrors.Clear();
		IsValid = true;
	}

	#endregion

	#region working with the form

	/// <inheritdoc />
	public virtual void ResetState()
	{
		IsVisible = true;
		IsValidated = false;
		SetValid();
	}

	/// <inheritdoc />
	public virtual void UpdateState(StateUpdateContext context)
	{
		if (VisibilityCondition != null)
		{
			IsVisible = Parent.IsVisible && VisibilityCondition.EvaluateOn(this, context);
		}

		if (!IsVisible)
		{
			SetValid();
			return;
		}

		if (this is not TNodeImplementationType validationTarget)
		{
			AddValidationError("Error in form configuration: Validators do not match the node type.");
			IsValidated = true;
			return;
		}

		foreach (var validator in Validators)
		{
			validator.Validate(validationTarget, context);
		}

		IsValidated = true;
	}

	/// <inheritdoc />
	public virtual object Clone()
	{
		var clone = (NodeBase<TNodeImplementationType>) MemberwiseClone();
		clone.Label = (ILabel?) Label?.Clone();
		clone.VisibilityCondition = (IExpression<bool>?) VisibilityCondition?.Clone();
		clone._validationErrors = [.._validationErrors];
		clone.Validators = [..Validators];
		return clone;
	}

	#endregion
}