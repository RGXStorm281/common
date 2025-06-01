namespace RobinEpple.Common.Forms.Visitors;

using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// Traverses the tree, by first exploring one branch to its end before going to the next one.
/// </summary>
public abstract class DepthFirstVisitor
{
	private Stack<IFormNode> _nextNodes = [];

	/// <summary>
	/// Traverses the form tree starting with the given node.
	/// </summary>
	/// <param name="node">The starting point of the breadth-first traversal.</param>
	public void Visit(IFormNode node)
	{
		var context = new VisitingContext();
		VisitInternal(node, context);
	}

	private void VisitInternal(IFormNode node, VisitingContext context)
	{
		// Execute on the current node.
		ExecuteOnNode(node, context);
		if (context.BreakLoop)
		{
			return;
		}

		// Then enqueue its children if available.
		// Reverse the order of lists, such that the first child is at the top of the stack.
		switch (node)
		{
			case IForm form:
			{
				foreach (var child in form.Nodes.Reverse())
				{
					_nextNodes.Push(child);
				}
				break;
			}
			case ICollectionNode collection:
			{
				foreach (var child in collection.Instances.Reverse())
				{
					_nextNodes.Push(child);
				}
				break;
			}
			case ITemplateNode templatedSection:
			{
				if (templatedSection.Instance is { } child)
				{
					_nextNodes.Push(child);
				}
				break;
			}
		}

		// Visit the next node if available.
		if (!_nextNodes.TryPop(out var nextNode))
		{
			// All nodes have been visited.
			return;
		}
		context.NodeIndex++;
		VisitInternal(nextNode, context);
	}

	/// <summary>
	/// Execute the functionality on the current node.
	/// </summary>
	/// <param name="node">The current node.</param>
	/// <param name="context">
	/// The context providing information about the state in the tree traversal.<br/>
	/// Also allows breaking the loop (e.g. when the desired element is found).
	/// </param>
	protected abstract void ExecuteOnNode(IFormNode node, VisitingContext context);
}
