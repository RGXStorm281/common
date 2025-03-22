namespace RobinEpple.Common.Forms.Nodes.DefaultImplementation;

using System.Globalization;
using RobinEpple.Common.Forms.Nodes.Formatters;

internal class TimestampNode : FieldNode, ITimestampNode
{
	public TimestampNode(string name, IParentNode parent, CultureInfo displayCulture)
		: base(name, parent, new LocalizedNumberFormatter(displayCulture))
	{
		_value = new(null);
	}

	private ResettableProperty<DateTime?> _value { get; set; }

	/// <inheritdoc />
	public DateTime? Value
	{
		get => _value.CurrentValue;
		set => _value.CurrentValue = value;
	}

	internal void ReplaceDefaultValue(DateTime? newDefaultValue) => _value.ReplaceDefault(newDefaultValue);

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
		var clone = (TimestampNode)base.Clone();
		clone._value = (ResettableProperty<DateTime?>)_value.Clone();
		return clone;
	}
}
