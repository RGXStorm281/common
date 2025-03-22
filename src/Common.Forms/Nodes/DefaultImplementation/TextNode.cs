namespace RobinEpple.Common.Forms.Nodes.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes.Formatters;

internal class TextNode : FieldNode, ITextNode
{
	public TextNode(string name, IParentNode parent)
		: base(name, parent, new TrimTextFormatter())
	{
		_value = new(null);
	}

	private ResettableProperty<string?> _value { get; set; }

	/// <inheritdoc />
	public string? Value
	{
		get => _value.CurrentValue;
		set => _value.CurrentValue = value;
	}

	internal void ReplaceDefaultValue(string? newDefaultValue) => _value.ReplaceDefault(newDefaultValue);

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
		var clone = (TextNode)base.Clone();
		clone._value = (ResettableProperty<string?>)_value.Clone();
		return clone;
	}
}
