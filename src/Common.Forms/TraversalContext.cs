namespace RobinEpple.Common.Forms;

/// <summary>
/// A context providing context about the current state of the tree traversal in the visiting process.
/// </summary>
public class TraversalContext
{
	/// <summary>
	/// The index of the current node in the traversal process.
	/// </summary>
	public int NodeIndex { get; set; } = 0;

	/// <summary>
	/// The depth of the current node in the tree, measured from the starting node.
	/// </summary>
	public int CurrentDepth { get; set; } = 0;

	/// <summary>
	/// When set to true, the traversal will stop entirely after this node.
	/// </summary>
	public bool Quit { get; set; } = false;

	/// <summary>
	/// When this property is set to true, the children of the current node are not traversed.<br/>
	/// This does not affect other branches.
	/// </summary>
	public bool TraverseChildren { get; set; } = true;
}
