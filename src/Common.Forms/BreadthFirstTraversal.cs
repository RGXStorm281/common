namespace RobinEpple.Common.Forms;

using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// Traverses the form tree by first visiting all nodes in the current depth, before going to the next layer.
/// </summary>
public abstract class BreadthFirstTraversal
{
	private Queue<(int Depth, IFormNode Node)> _nextVisits = [];

	/// <summary>
	/// Traverses the form tree starting with the given node.
	/// </summary>
	/// <param name="node">The starting point of the breadth-first traversal.</param>
	public void Visit(IFormNode node)
	{
		var context = new TraversalContext();
		VisitInternal(node, context);
	}

	private void VisitInternal(IFormNode node, TraversalContext context)
	{
		// Execute on the current node.
		ExecuteOnNode(node, context);
		if (context.Quit)
		{
			return;
		}

		// Then enqueue its children if enabled.
		if (context.TraverseChildren)
		{
			EnqueueChildren(node, context);
		}
		else
		{
			// If not, re-enable it for the next node.
			context.TraverseChildren = true;
		}

		// Visit the next node if available.
		if (!_nextVisits.TryDequeue(out var nextVisit))
		{
			// All nodes have been visited.
			return;
		}
		context.NodeIndex++;
		context.CurrentDepth = nextVisit.Depth;
		VisitInternal(nextVisit.Node, context);
	}

	private void EnqueueChildren(IFormNode node, TraversalContext context)
	{
		var nextDepth = context.CurrentDepth + 1;
		switch (node)
		{
			case IForm form:
			{
				foreach (var child in form.Nodes)
				{
					_nextVisits.Enqueue((nextDepth, child));
				}
				break;
			}
			case ICollectionNode collection:
			{
				foreach (var child in collection.Instances)
				{
					_nextVisits.Enqueue((nextDepth, child));
				}
				break;
			}
			case ITemplateNode templatedSection:
			{
				if (templatedSection.Instance is { } child)
				{
					_nextVisits.Enqueue((nextDepth, child));
				}
				break;
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
	protected abstract void ExecuteOnNode(IFormNode node, TraversalContext context);
}
