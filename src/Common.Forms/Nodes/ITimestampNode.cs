namespace RobinEpple.Common.Forms.Nodes;

/// <summary>
/// This interface represents a timestamp input in the form.
/// </summary>
public interface ITimestampNode : IFieldNode
{
	/// <summary>
	/// The value of this node.
	/// </summary>
	public DateTime? Value { get; set; }
}
