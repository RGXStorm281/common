namespace RobinEpple.Common.Forms.Building;

using RobinEpple.Common.Forms.Binding;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.Nodes.DefaultImplementation;

internal class CollectionNodeBuilder : NodeBuilder<ICollectionNodeBuilder, CollectionNode>, ICollectionNodeBuilder
{
	public CollectionNodeBuilder(CollectionNode node)
		: base(node) { }

	/// <inheritdoc />
	public ICollectionNodeBuilder UseBinding(ICollectionNodeBinding binding)
	{
		Node.UseBinding(binding);
		return CastThis();
	}

	/// <inheritdoc />
	public ICollectionNodeBuilder UsePreconfiguredTemplate(IForm template)
	{
		Node.UseTemplate(template);
		return CastThis();
	}

	/// <inheritdoc />
	public ICollectionNodeBuilder UseTemplate(
		string name,
		ITemplatedNodeBuilder<ICollectionNodeBuilder>.TemplateBuilder? configure = null
	)
	{
		var builder = new FormBuilder(name);
		configure?.Invoke(builder);
		Node.UseTemplate(builder.Build());
		return CastThis();
	}

	/// <inheritdoc />
	protected override CollectionNodeBuilder CastThis() => this;
}
