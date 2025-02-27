namespace RobinEpple.Common.Forms.Nodes;

public interface INumberNode : IFieldNode
{
	/// <summary>
	/// The value of this node.
	/// </summary>
	public decimal? Value { get; set; }
}
