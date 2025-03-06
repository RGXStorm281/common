namespace RobinEpple.Common.Forms.Building;

using RobinEpple.Common.Forms.Nodes.DefaultImplementation;

internal class TimestampNodeBuilder : FieldNodeBuilder<ITimestampNodeBuilder, TimestampNode>, ITimestampNodeBuilder
{
	public TimestampNodeBuilder(TimestampNode node)
		: base(node) { }

	/// <inheritdoc />
	protected override TimestampNodeBuilder CastThis() => this;
}
