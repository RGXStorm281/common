namespace RobinEpple.Common.Forms.BasicApi.Nodes;

/// <summary>
/// The interface for all form nodes that contain others.
/// </summary>
public interface IParentNode
{
	/// <summary>
	/// Whether the node is currently visible.
	/// </summary>
	public bool IsVisible { get; }

	/// <summary>
	/// Find the first node that matches the <paramref name="predicate" /> and supersedes the <paramref name="currentNode" />.<br />
	/// This method recursively traverses up the parents until it reaches the root.
	/// </summary>
	/// <param name="currentNode">Optional starting position for the search. If <see langword="null" /> all nodes starting from the last one are searched.</param>
	/// <param name="predicate">The predicate, that the node has to match.</param>
	/// <returns>The first node that matches the predicate.</returns>
	/// <exception cref="NodeNotFoundException">When no node matched the <paramref name="predicate" />.</exception>
	public IFormNode FindClosestBefore(IFormNode currentNode, Func<IFormNode, bool> predicate);

	/// <summary>
	/// Iterates backwards over all children and returns the first node that matches the <paramref name="predicate" />.<br />
	/// This method does NOT search through parent nodes.
	/// </summary>
	/// <param name="predicate">The predicate, that the node has to match.</param>
	/// <returns>The first node that matches the predicate or <see langword="null" />.</returns>
	public IFormNode? FindFirstBackwards(Func<IFormNode, bool> predicate);
}
