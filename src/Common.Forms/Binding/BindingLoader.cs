namespace RobinEpple.Common.Forms.Binding;

using RobinEpple.Common.Forms.Nodes;

public class BindingLoader : BreadthFirstTraversal
{
	/// <inheritdoc />
	protected override void ExecuteOnNode(IFormNode node, TraversalContext context)
	{
		if (node.Binding is not { } binding)
		{
			return;
		}

		binding.LoadFromModel(node);
	}
}
