namespace RobinEpple.Common.Forms.Nodes;

public interface IParentNode : IFormNode
{
	/// <summary>
	/// Computes a unique id for the child.
	/// </summary>
	/// <param name="child">The child.</param>
	/// <returns>The id of the child.</returns>
	internal string GetChildId(IFormNode child);

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
