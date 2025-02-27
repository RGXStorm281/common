namespace RobinEpple.Common.Forms.Nodes;

public interface ITextNode : IFieldNode
{
	/// <summary>
	/// The value of this node.
	/// </summary>
	public string? Value { get; set; }
}
