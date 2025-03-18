namespace RobinEpple.Common.Forms.Nodes;

/// <summary>
/// This interface represents a boolean input in the form.
/// </summary>
public interface IBooleanNode : IFieldNode
{
	/// <summary>
	/// The value of this node.
	/// </summary>
	public bool? Value { get; set; }
}
