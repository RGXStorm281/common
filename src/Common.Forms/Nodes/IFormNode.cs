namespace RobinEpple.Common.Forms.Nodes;

using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Extensions;
using RobinEpple.Common.Forms.Validation;

public interface IFormNode : ICloneable
{
	#region structure

	/// <summary>
	/// The name by that this field can be referenced in the form.<br/>
	/// This should be unique in the structure, but can be instantiated multiple times, <br/>
	/// when it is part of a template.
	/// </summary>
	public string Name { get; }

	/// <summary>
	/// The unique id of this node in the instance tree.<br/>
	/// This is always unique, even if a template gets instantiated multiple times.
	/// </summary>
	public string GetId();

	/// <summary>
	/// The parent of this node in the tree structure.
	/// </summary>
	public IParentNode? Parent { get; }

	/// <summary>
	/// The root node
	/// </summary>
	public IForm Root { get; }

	#endregion

	#region node properties

	/// <summary>
	/// The display name of the node.
	/// </summary>
	public string Label { get; set; }

	/// <summary>
	/// Whether the node is visible.
	/// </summary>
	public bool IsVisible { get; set; }

	/// <summary>
	/// Whether this node and all subordinate fields should be readonly.
	/// </summary>
	public bool IsReadonly { get; set; }

	/// <summary>
	/// An optional condition to specify when the field is visible and when not.
	/// </summary>
	public IFormExpression<bool>? VisibilityCondition { get; }

	#endregion

	#region validation

	/// <summary>
	/// Whether this node and all subordinate nodes are valid.
	/// </summary>
	public bool IsValid { get; }

	/// <summary>
	/// The list of validation errors attached to this node.
	/// </summary>
	public IEnumerable<string> ValidationErrors { get; }

	/// <summary>
	/// The list of validators active on this node.
	/// </summary>
	public IEnumerable<INodeValidator> NodeValidators { get; }

	/// <summary>
	/// Adds or replaces a validation error on this node.
	/// </summary>
	/// <param name="id">
	/// The id of this error. <br/>
	/// This should be unique to the used validator and cause.<br/>
	/// It is used to filter out duplicates and ensures the ability to remove the error again.
	/// </param>
	/// <param name="error">The error text.</param>
	public void SetValidationError(string id, string error);

	/// <summary>
	/// Removes the validation error with the given id from this node.
	/// </summary>
	/// <param name="id">The id of the error. See <see cref="SetValidationError"/> for details.</param>
	public void RemoveValidationError(string id);

	#endregion

	#region state engine

	/// <summary>
	/// Resets this node to its default state.
	/// </summary>
	public void Reset();

	/// <inheritdoc cref="Reset"/>
	public Task ResetAsync();

	/// <summary>
	/// Updates the state of this node by applying internal rules.
	/// </summary>
	public void Update();

	/// <inheritdoc cref="Update"/>
	public Task UpdateAsync();

	#endregion

	#region extensibility

	/// <summary>
	/// The list of extensions registered for this node.
	/// </summary>
	public IEnumerable<IFormNodeExtension> Extensions { get; }

	#endregion
}
