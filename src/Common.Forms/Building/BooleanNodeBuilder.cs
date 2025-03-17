namespace RobinEpple.Common.Forms.Building;

using RobinEpple.Common.Forms.Nodes.DefaultImplementation;

internal class BooleanNodeBuilder : FieldNodeBuilder<IBooleanNodeBuilder, BooleanNode>, IBooleanNodeBuilder
{
	public BooleanNodeBuilder(BooleanNode node)
		: base(node) { }

	public void UseDefaultValue(bool? defaultValue) => throw new NotImplementedException();

	/// <inheritdoc />
	protected override BooleanNodeBuilder CastThis() => this;
}
