namespace RobinEpple.Common.Forms.Nodes;

public interface IForm : IFormNode
{
	/// <summary>
	/// The list of nodes in this form.<br/>
	/// The order does not imply a visual arrangement.
	/// </summary>
	public IEnumerable<IFormNode> Nodes { get; }

	/// <summary>
	/// Searches for a unique node in this form.<br/>
	/// Layers are traversed down, but not up.
	/// </summary>
	/// <param name="name">The name of the desired node.</param>
	/// <returns>A single node or none.</returns>
	public IFormNode? FindNode(string name);

	/// <summary>
	/// Searches for all instances of a node with the given name.<br/>
	/// Layers are traversed down, but not up.
	/// </summary>
	/// <param name="name">The name of the template node.</param>
	/// <returns>The list of all instances.</returns>
	public IEnumerable<IFormNode> FindNodes(string name);
}
