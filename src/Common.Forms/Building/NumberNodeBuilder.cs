namespace RobinEpple.Common.Forms.Building;

using RobinEpple.Common.Forms.Nodes.DefaultImplementation;

internal class NumberNodeBuilder : FieldNodeBuilder<INumberNodeBuilder, NumberNode>, INumberNodeBuilder
{
	public NumberNodeBuilder(NumberNode node)
		: base(node) { }

	public void UseDefaultValue(decimal? defaultValue) => throw new NotImplementedException();

	/// <inheritdoc />
	protected override NumberNodeBuilder CastThis() => this;
}
