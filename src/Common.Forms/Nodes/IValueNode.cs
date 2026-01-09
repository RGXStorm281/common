namespace RobinEpple.Common.Forms.Nodes;

using RobinEpple.Common.Forms.SelectLists;

public interface IValueNode<TValue> : IFormNode
{
	/// <summary>
	/// The value of this node.
	/// </summary>
	public TValue Value { get; set; }

	/// <summary>
	/// A provider to load named value suggestions.
	/// </summary>
	public ISelectListSource<TValue>? Suggestions { get; }
}
