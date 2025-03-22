namespace RobinEpple.Common.Forms.Nodes.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes.Formatters;

internal class BooleanNode : FieldNode, IBooleanNode
{
	public BooleanNode(string name, IParentNode parent)
		: base(name, parent, new BooleanFormatter("yes", "no"))
	{
		_value = new(null);
	}

	private ResettableProperty<bool?> _value { get; set; }

	/// <inheritdoc />
	public bool? Value
	{
		get => _value.CurrentValue;
		set => _value.CurrentValue = value;
	}

	internal void ReplaceDefaultValue(bool? newDefaultValue) => _value.ReplaceDefault(newDefaultValue);

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
		var clone = (BooleanNode)base.Clone();
		clone._value = (ResettableProperty<bool?>)_value.Clone();
		return clone;
	}
}
