namespace RobinEpple.Common.Forms.Visitors;

using RobinEpple.Common.Forms.Nodes;

public class BindingWriter : BreadthFirstVisitor
{
	/// <inheritdoc />
	protected override void ExecuteOnNode(IFormNode node, VisitingContext context)
	{
		// Only write back visible nodes.
		if (!node.IsVisible)
		{
			return;
		}

		if (node.Binding is not { } binding)
		{
			return;
		}

		binding.WriteToModel(node);
	}
}
