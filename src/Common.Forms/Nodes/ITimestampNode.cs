namespace RobinEpple.Common.Forms.Nodes;

public interface ITimestampNode : IFieldNode
{
	/// <summary>
	/// The value of this node.
	/// </summary>
	public DateTime? Value { get; set; }
}
