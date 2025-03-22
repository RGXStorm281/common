namespace RobinEpple.Common.Forms.Nodes;

/// <summary>
/// This is a marker interface that defines that the current node provides a scope for contained nodes.
/// </summary>
public interface IScopeProvider : IParentNode
{
	/// <summary>
	/// Searches for a unique node in this form.<br/>
	/// Layers are traversed down, but not up. Since only unique nodes are considered, <br/>
	/// subsections are searched, but not collections.<br/>
	/// Hint: If recursive templates are used, the search will return the uppermost instance.
	/// </summary>
	/// <param name="name">The name of the desired node.</param>
	/// <param name="comparer">Optional comparer for the name search.</param>
	/// <returns>A single node or none.</returns>
	public IFormNode? FindNode(string name, StringComparer? comparer = null);

	/// <summary>
	/// Searches for all instances of a node with the given name.<br/>
	/// Layers are traversed down, but not up. Both sections and collections <br/>
	/// are searched.
	/// </summary>
	/// <param name="name">The name of the template node.</param>
	/// <param name="comparer">Optional comparer for the name search.</param>
	/// <returns>The list of all instances.</returns>
	public IEnumerable<IFormNode> FindNodes(string name, StringComparer? comparer = null);
}
