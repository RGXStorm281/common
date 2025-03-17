namespace RobinEpple.Common.Forms.Building;

using System;
using RobinEpple.Common.Forms.Nodes.DefaultImplementation;

internal class TimestampNodeBuilder : FieldNodeBuilder<ITimestampNodeBuilder, TimestampNode>, ITimestampNodeBuilder
{
	public TimestampNodeBuilder(TimestampNode node)
		: base(node) { }

	public void UseDefaultValue(DateTime? defaultValue) => throw new NotImplementedException();

	/// <inheritdoc />
	protected override TimestampNodeBuilder CastThis() => this;
}
