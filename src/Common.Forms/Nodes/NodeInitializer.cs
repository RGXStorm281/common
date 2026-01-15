namespace RobinEpple.Common.Forms.Nodes;

/// <summary>
/// Initializes all nodes in the given tree section of a form.
/// </summary>
public class NodeInitializer : BreadthFirstTraversal
{
	/// <inheritdoc />
	protected override void Visit(IFormNode node, TraversalContext context)
	{
		foreach (var extension in node.Extensions)
		{
			extension.OnInitialize(node);
		}
	}
}
