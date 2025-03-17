namespace RobinEpple.Common.Forms.Building;

using System;
using RobinEpple.Common.Forms.Nodes.DefaultImplementation;

internal class TimestampNodeBuilder : FieldNodeBuilder<ITimestampNodeBuilder, TimestampNode>, ITimestampNodeBuilder
{
	public TimestampNodeBuilder(TimestampNode node)
		: base(node) { }

	/// <inheritdoc />
	public ITimestampNodeBuilder UseDefaultValue(DateTime? defaultValue)
	{
		Node.Value = defaultValue;
		Node.ReplaceDefaultValue(defaultValue);
		return CastThis();
	}

	/// <inheritdoc />
	protected override TimestampNodeBuilder CastThis() => this;
}
