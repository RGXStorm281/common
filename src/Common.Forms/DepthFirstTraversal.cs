namespace RobinEpple.Common.Forms;

using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// Traverses the tree, by first exploring one branch to its end before going to the next one.
/// </summary>
public abstract class DepthFirstTraversal
{
	private Stack<(int Depth, IFormNode Node)> _nextVisits = [];

	/// <summary>
	/// Traverses the form tree starting with the given node.
	/// </summary>
	/// <param name="node">The starting point of the breadth-first traversal.</param>
	public void RunOn(IFormNode node)
	{
		var context = new TraversalContext();
		_nextVisits.Push((context.CurrentDepth, node));

		while (_nextVisits.TryPop(out var nextVisit))
		{
			context.CurrentDepth = nextVisit.Depth;
			Visit(nextVisit.Node, context);
			if (context.Quit)
			{
				return;
			}

			// Then enqueue its children if enabled.
			if (context.TraverseChildren)
			{
				EnqueueChildren(nextVisit.Node, context);
			}
			else
			{
				// If not, re-enable it for the next node.
				context.TraverseChildren = true;
			}

			context.NodeIndex++;
		}
	}

	private void EnqueueChildren(IFormNode node, TraversalContext context)
	{
		var nextDepth = context.CurrentDepth + 1;
		if (node is IParentNode parent)
		{
			// Reverse the order of lists, such that the first child is at the top of the stack.
			foreach (var child in parent.GetChildren().Reverse())
			{
				_nextVisits.Push((nextDepth, child));
			}
		}
	}

	/// <summary>
	/// Execute the functionality on the current node.
	/// </summary>
	/// <param name="node">The current node.</param>
	/// <param name="context">
	/// The context providing information about the state in the tree traversal.<br/>
	/// Also allows breaking the loop (e.g. when the desired element is found).
	/// </param>
	protected abstract void Visit(IFormNode node, TraversalContext context);
}
