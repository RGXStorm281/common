namespace RobinEpple.Common.Forms.Nodes;

/// <summary>
/// This interface represents a number input in the form.
/// </summary>
public interface INumberNode : IFieldNode
{
	/// <summary>
	/// The value of this node.
	/// </summary>
	public decimal? Value { get; set; }
}
