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

	/// <summary>
	/// Checks if the given node appears somewhere in the parent hierarchy.
	/// </summary>
	/// <param name="node">The node to check for.</param>
	/// <param name="index">If <see langword="true"/>, this is the number of layers to go up. The direct parent has index 0.</param>
	/// <returns><see langword="true"/> if the <paramref name="node"/> is somewhere in the parent stack.</returns>
	public bool StackContains(IParentNode node, out int index);

	/// <summary>
	/// Travels up <paramref name="index"/> layers in the parent stack and returns that parent.
	/// </summary>
	/// <param name="index">The index of the parent layer. The direct parent is index 0.</param>
	/// <returns>The parent at the given index.</returns>
	/// <exception cref="InvalidOperationException">If the given <paramref name="index"/> is negative.</exception>
	/// <exception cref="IndexOutOfRangeException">If the given <paramref name="index"/> is greater than the actual parent stack.</exception>
	public IParentNode GetParentAt(int index);
}
