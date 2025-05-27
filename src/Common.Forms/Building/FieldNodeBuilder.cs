namespace RobinEpple.Common.Forms.Building;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.Nodes.DefaultImplementation;

internal abstract class FieldNodeBuilder<TSpecificNodeBuilder, TNode>
	: NodeBuilder<TSpecificNodeBuilder, TNode>,
		IFieldNodeBuilder<TSpecificNodeBuilder>
	where TNode : FieldNode
{
	public FieldNodeBuilder(TNode node)
		: base(node) { }

	/// <inheritdoc/>
	public TSpecificNodeBuilder UseFormatter(IValueFormatter formatter)
	{
		Node.UseFormatter(formatter);
		return CastThis();
	}
}
