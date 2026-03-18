namespace RobinEpple.Common.Forms.Nodes.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes.Formatters;
using RobinEpple.Common.Forms.SelectLists;
using RobinEpple.Common.SourceGenerators.Abstractions;

internal partial class TextNode : FieldNode, ITextNode
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

	/// <inheritdoc />
	public ISelectListSource<string?>? SelectList { get; private set; }

	/// <inheritdoc />
	public IEnumerable<ISelectListItem<string?>>? CurrentSelectListItems { get; private set; }

	public void UseSelectList(ISelectListSource<string?>? selectList)
	{
		SelectList = selectList;
	}

	internal void ReplaceDefaultValue(string? newDefaultValue) => _value.ReplaceDefault(newDefaultValue);

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public override void Reset()
	{
		base.Reset();
		_value.Reset();
	}

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public override void Update()
	{
		CurrentSelectListItems = SelectList?.LoadFor(this);
		base.Update();
	}

	/// <inheritdoc />
	public override object Clone()
	{
		var clone = (TextNode)base.Clone();
		clone._value = (ResettableProperty<string?>)_value.Clone();
		return clone;
	}
}
