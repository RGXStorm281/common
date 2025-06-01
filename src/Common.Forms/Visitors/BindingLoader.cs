namespace RobinEpple.Common.Forms.Visitors;

using RobinEpple.Common.Forms.Nodes;

public class BindingLoader : BreadthFirstVisitor
{
	/// <inheritdoc />
	protected override void ExecuteOnNode(IFormNode node, VisitingContext context)
	{
		if (node.Binding is not { } binding)
		{
			return;
		}

		binding.LoadFromModel(node);
	}
}
