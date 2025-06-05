namespace RobinEpple.Common.Forms.Nodes;

/// <summary>
/// This is a marker interface that defines that the current node provides a scope for contained nodes.
/// </summary>
public interface IScopeProvider : IParentNode
{
	/// <summary>
	/// Searches for the first node in this scope, that matches the given predicate.<br/>
	/// Layers are traversed down, but not up.<br/>
	/// Hint: One layer will be searched fully before moving to the next lower one, so the uppermost result will be returned.
	/// </summary>
	/// <param name="predicate">The predicate, identifying the given node.</param>
	/// <param name="maxDepth">Optional maximum number of child-layers the search traverses down.</param>
	/// <returns>A single node or none.</returns>
	public IFormNode? FindFirst(Func<IFormNode, bool> predicate, int? maxDepth = null);

	/// <summary>
	/// Searches for the first node in this scope, that has the given name.<br/>
	/// Layers are traversed down, but not up.<br/>
	/// Hint: One layer will be searched fully before moving to the next lower one, so the uppermost result will be returned.
	/// </summary>
	/// <param name="name">The name of the desired node.</param>
	/// <param name="comparer">Optional comparer for the name search.</param>
	/// <param name="maxDepth">Optional maximum number of child-layers the search traverses down.</param>
	/// <returns>A single node or none.</returns>
	public IFormNode? FindFirst(string name, StringComparer? comparer = null, int? maxDepth = null);

	/// <summary>
	/// Searches for all nodes in this scope, that match the given predicate.<br/>
	/// Layers are traversed down, but not up. <br/>
	/// </summary>
	/// <param name="predicate">The predicate, identifying the given node.</param>
	/// <param name="maxDepth">Optional maximum number of child-layers the search traverses down.</param>
	/// <returns>The list of all nodes.</returns>
	public IEnumerable<IFormNode> FindAll(Func<IFormNode, bool> predicate, int? maxDepth = null);

	/// <summary>
	/// Searches for all instances of a node with the given name.<br/>
	/// Layers are traversed down, but not up. <br/>
	/// </summary>
	/// <param name="name">The name of the template node, that is searched.</param>
	/// <param name="comparer">Optional comparer for the name search.</param>
	/// <param name="maxDepth">Optional maximum number of child-layers the search traverses down.</param>
	/// <returns>The list of all nodes.</returns>
	public IEnumerable<IFormNode> FindAll(string name, StringComparer? comparer = null, int? maxDepth = null);
}
