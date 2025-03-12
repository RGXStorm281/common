namespace RobinEpple.Common.Forms.Nodes.DefaultImplementation;

using System.Globalization;
using RobinEpple.Common.Forms.Nodes.Formatters;

internal class NumberNode : FieldNode, INumberNode
{
	public NumberNode(string name, IParentNode parent, CultureInfo displayCulture)
		: base(name, parent, new LocalizedNumberFormatter(displayCulture)) { }

	/// <inheritdoc />
	public decimal? Value { get; set; }

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
		var clone = (NumberNode)base.Clone();
		clone.Value = Value;
		return clone;
	}
}
