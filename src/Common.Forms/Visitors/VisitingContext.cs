namespace RobinEpple.Common.Forms.Visitors;

/// <summary>
/// A context providing context about the current state of the tree traversal in the visiting process.
/// </summary>
public class VisitingContext
{
	/// <summary>
	/// The index of the current node in the traversal process.
	/// </summary>
	public int NodeIndex { get; set; } = 0;

	/// <summary>
	/// When set to true, the visitor will stop after this node.
	/// </summary>
	public bool BreakLoop { get; set; } = false;
}
