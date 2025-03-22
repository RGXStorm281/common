namespace RobinEpple.Common.Forms.Nodes.DefaultImplementation;

using System.Globalization;
using RobinEpple.Common.Forms.Nodes.Formatters;

internal class NumberNode : FieldNode, INumberNode
{
	public NumberNode(string name, IParentNode parent, CultureInfo displayCulture)
		: base(name, parent, new LocalizedNumberFormatter(displayCulture))
	{
		_value = new(null);
	}

	private ResettableProperty<decimal?> _value { get; set; }

	/// <inheritdoc />
	public decimal? Value
	{
		get => _value.CurrentValue;
		set => _value.CurrentValue = value;
	}

	internal void ReplaceDefaultValue(decimal? newDefaultValue) => _value.ReplaceDefault(newDefaultValue);

	/// <inheritdoc />
	public override void Reset()
	{
		base.Reset();
		_value.Reset();
	}

	/// <inheritdoc />
	public override async Task ResetAsync()
	{
		await base.ResetAsync();
		_value.Reset();
	}

	/// <inheritdoc />
	public override object Clone()
	{
		var clone = (NumberNode)base.Clone();
		clone._value = (ResettableProperty<decimal?>)_value.Clone();
		return clone;
	}
}
