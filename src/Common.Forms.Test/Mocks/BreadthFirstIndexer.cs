namespace RobinEpple.Common.Forms.Test.Mocks;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.Visitors;

public class BreadthFirstIndexer : BreadthFirstVisitor
{
	public const string IndexTagName = "_index";

	/// <inheritdoc />
	protected override void ExecuteOnNode(IFormNode node, VisitingContext context)
	{
		node.SetTag(IndexTagName, context.NodeIndex);
	}
}
