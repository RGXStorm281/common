namespace RobinEpple.Common.Forms.Building;

using RobinEpple.Common.Forms.Nodes.DefaultImplementation;

internal class NumberNodeBuilder : FieldNodeBuilder<INumberNodeBuilder, NumberNode>, INumberNodeBuilder
{
	public NumberNodeBuilder(NumberNode node)
		: base(node) { }

	/// <inheritdoc />
	public INumberNodeBuilder UseDefaultValue(decimal? defaultValue)
	{
		Node.Value = defaultValue;
		Node.ReplaceDefaultValue(defaultValue);
		return CastThis();
	}

	/// <inheritdoc />
	protected override NumberNodeBuilder CastThis() => this;
}
