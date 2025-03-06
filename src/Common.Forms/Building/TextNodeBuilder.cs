namespace RobinEpple.Common.Forms.Building;

using RobinEpple.Common.Forms.Nodes.DefaultImplementation;

internal class TextNodeBuilder : FieldNodeBuilder<ITextNodeBuilder, TextNode>, ITextNodeBuilder
{
	public TextNodeBuilder(TextNode node)
		: base(node) { }

	/// <inheritdoc />
	protected override TextNodeBuilder CastThis() => this;
}
