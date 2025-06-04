namespace RobinEpple.Common.Forms.Test.Mocks;

using RobinEpple.Common.Forms.Nodes;

public class DepthFirstIndexer : DepthFirstTraversal
{
	public const string IndexTagName = "_index";

	/// <inheritdoc />
	protected override void ExecuteOnNode(IFormNode node, TraversalContext context)
	{
		node.SetTag(IndexTagName, context.NodeIndex);
	}
}
