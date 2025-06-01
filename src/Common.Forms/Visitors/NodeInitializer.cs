namespace RobinEpple.Common.Forms.Visitors;

using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// Initializes all nodes in the given tree section of a form.
/// </summary>
public class NodeInitializer : BreadthFirstVisitor
{
	protected override void ExecuteOnNode(IFormNode node, VisitingContext context)
	{
		foreach (var extension in node.Extensions)
		{
			extension.OnInitialize(node);
		}
	}
}
