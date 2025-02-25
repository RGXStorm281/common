namespace RobinEpple.Common.Forms.BasicApi.Nodes;

/// <summary>
/// The base interface for all nodes in the form tree.
/// </summary>
public interface IFormNode : ICloneable
{
	#region form structure

	/// <summary>
	/// The id of the node.
	/// </summary>
	public string Id { get; }

	/// <summary>
	/// The node that contains this one.
	/// </summary>
	public IParentNode Parent { get; }

	#endregion

	#region node definition

	/// <summary>
	/// Optional label of the field.
	/// </summary>
	public ILabel? Label { get; }

	/// <summary>
	/// Optional Condition for when the node is visible.
	/// </summary>
	public IExpression<bool>? VisibilityCondition { get; }

	#endregion

	#region state

	/// <summary>
	/// Whether the node is currently visible.
	/// </summary>
	public bool IsVisible { get; }

	/// <summary>
	/// Whether the node was validated.
	/// </summary>
	public bool IsValidated { get; }

	/// <summary>
	/// Whether the node is valid.
	/// </summary>
	public bool IsValid { get; }

	/// <summary>
	/// The errors attached to the node.
	/// </summary>
	public IEnumerable<string> ValidationErrors { get; }

	/// <summary>
	/// Adds a validation error to the node.
	/// </summary>
	/// <param name="error">The error.</param>
	public void AddValidationError(string error);

	/// <summary>
	/// Removes all validation errors from this node.
	/// </summary>
	public void SetValid();

	#endregion

	#region working with the form

	/// <summary>
	/// Resets the state of the node (validation, ...). This does NOT touch the data in the node.
	/// </summary>
	public void ResetState();

	/// <summary>
	/// Updates the state of the node and all nodes below.
	/// </summary>
	/// <param name="context"></param>
	public void UpdateState(StateUpdateContext context);

	#endregion
}
