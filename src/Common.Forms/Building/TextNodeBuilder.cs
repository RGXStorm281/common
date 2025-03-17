namespace RobinEpple.Common.Forms.Building;

using RobinEpple.Common.Forms.Nodes.DefaultImplementation;

internal class TextNodeBuilder : FieldNodeBuilder<ITextNodeBuilder, TextNode>, ITextNodeBuilder
{
	public TextNodeBuilder(TextNode node)
		: base(node) { }

	/// <inheritdoc />
	public ITextNodeBuilder UseDefaultValue(string? defaultValue)
	{
		Node.Value = defaultValue;
		Node.ReplaceDefaultValue(defaultValue);
		return CastThis();
	}

	/// <inheritdoc />
	protected override TextNodeBuilder CastThis() => this;
}
