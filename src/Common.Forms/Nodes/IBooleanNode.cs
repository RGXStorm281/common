namespace RobinEpple.Common.Forms.Nodes;

public interface IBooleanNode : IFieldNode
{
	/// <summary>
	/// The value of this node.
	/// </summary>
	public bool? Value { get; set; }
}
