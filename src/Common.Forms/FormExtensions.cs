namespace RobinEpple.Common.Forms;

using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// A collection of extension methods on the form.
/// </summary>
public static class FormExtensions
{
	private class InteractedHelper : DepthFirstTraversal
	{
		protected override void Visit(IFormNode node, TraversalContext context)
		{
			if (node is IFieldNode fieldNode)
			{
				fieldNode.HasUserInteraction = true;
			}
		}
	}

	/// <summary>
	/// Traverses through this node and all descendants to set all fields interacted.
	/// </summary>
	/// <param name="node">The container or node to process.</param>
	public static void SetAllInteracted(this IFormNode node)
	{
		var helper = new InteractedHelper();
		helper.RunOn(node);
	}
}
