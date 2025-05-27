namespace RobinEpple.Common.Forms.Building;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.Nodes.DefaultImplementation;

internal class TemplateNodeBuilder : NodeBuilder<ITemplateNodeBuilder, TemplateNode>, ITemplateNodeBuilder
{
	public TemplateNodeBuilder(TemplateNode node)
		: base(node) { }

	/// <inheritdoc />
	public ITemplateNodeBuilder UsePreConfiguredTemplate(IForm template)
	{
		Node.UseTemplate(template);
		return CastThis();
	}

	/// <inheritdoc />
	public ITemplateNodeBuilder UseTemplate(
		string name,
		ITemplatedNodeBuilder<ITemplateNodeBuilder>.TemplateBuilder? configure = null
	)
	{
		var builder = new FormBuilder(name);
		configure?.Invoke(builder);
		Node.UseTemplate(builder.Build());
		return CastThis();
	}

	/// <inheritdoc />
	protected override TemplateNodeBuilder CastThis() => this;
}
