namespace RobinEpple.Common.Forms.Nodes.DefaultImplementation;

using System.Globalization;
using RobinEpple.Common.Forms.Nodes.Formatters;
using RobinEpple.Common.Forms.SelectLists;
using RobinEpple.Common.SourceGenerators.Abstractions;

internal partial class TimestampNode : FieldNode, ITimestampNode
{
	public TimestampNode(string name, IParentNode parent, CultureInfo displayCulture)
		: base(name, parent, new LocalizedTimestampFormatter(displayCulture))
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

	/// <inheritdoc />
	public ISelectListSource<DateTime?>? SelectList { get; private set; }

	/// <inheritdoc />
	public IEnumerable<ISelectListItem<DateTime?>>? CurrentSelectListItems { get; private set; }

	public void UseSelectList(ISelectListSource<DateTime?>? selectList)
	{
		SelectList = selectList;
	}

	internal void ReplaceDefaultValue(DateTime? newDefaultValue) => _value.ReplaceDefault(newDefaultValue);

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
		var clone = (TimestampNode)base.Clone();
		clone._value = (ResettableProperty<DateTime?>)_value.Clone();
		return clone;
	}
}
