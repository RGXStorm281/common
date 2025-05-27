namespace RobinEpple.Common.Forms.Nodes;

public interface IValueNode<TValue>
{
	/// <summary>
	/// The value of this node.
	/// </summary>
	public TValue Value { get; set; }
}
