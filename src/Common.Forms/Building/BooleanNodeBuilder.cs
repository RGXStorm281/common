namespace RobinEpple.Common.Forms.Building;

using RobinEpple.Common.Forms.Nodes.DefaultImplementation;

internal class BooleanNodeBuilder : FieldNodeBuilder<IBooleanNodeBuilder, BooleanNode>, IBooleanNodeBuilder
{
	public BooleanNodeBuilder(BooleanNode node)
		: base(node) { }

	/// <inheritdoc />
	public IBooleanNodeBuilder UseDefaultValue(bool? defaultValue)
	{
		Node.Value = defaultValue;
		Node.ReplaceDefaultValue(defaultValue);
		return CastThis();
	}

	/// <inheritdoc />
	protected override BooleanNodeBuilder CastThis() => this;
}
