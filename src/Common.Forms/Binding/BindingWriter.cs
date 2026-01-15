namespace RobinEpple.Common.Forms.Binding;

using RobinEpple.Common.Forms.Nodes;

internal class BindingWriter : BreadthFirstTraversal
{
	/// <inheritdoc />
	protected override void Visit(IFormNode node, TraversalContext context)
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
