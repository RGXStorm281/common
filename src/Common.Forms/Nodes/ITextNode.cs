namespace RobinEpple.Common.Forms.Nodes;

/// <summary>
/// This interface represents a text input in the form.
/// </summary>
public interface ITextNode : IFieldNode
{
	/// <summary>
	/// The value of this node.
	/// </summary>
	public string? Value { get; set; }
}
