namespace RobinEpple.Common.Forms.Nodes.DefaultImplementation;

using System.Globalization;
using RobinEpple.Common.Forms.Nodes.Formatters;

internal class TimestampNode : FieldNode, ITimestampNode
{
	public TimestampNode(string name, IParentNode parent, CultureInfo displayCulture)
		: base(name, parent, new LocalizedNumberFormatter(displayCulture)) { }

	/// <inheritdoc />
	public DateTime? Value { get; set; }

	/// <inheritdoc />
	public override void Reset()
	{
		base.Reset();
		Value = null;
	}

	/// <inheritdoc />
	public override async Task ResetAsync()
	{
		await base.ResetAsync();
		Value = null;
	}

	/// <inheritdoc />
	public override object Clone()
	{
		var clone = (TimestampNode)base.Clone();
		clone.Value = Value;
		return clone;
	}
}
