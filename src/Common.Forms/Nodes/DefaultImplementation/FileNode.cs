namespace RobinEpple.Common.Forms.Nodes.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes.Formatters;
using RobinEpple.Common.SourceGenerators.Abstractions;

internal partial class FileNode : FieldNode, IFileNode
{
	public FileNode(string name, IParentNode parent)
		: base(name, parent, new FileSerializer())
	{
		_value = new(new());
	}

	private ResettableProperty<FileValue> _value { get; set; }

	/// <inheritdoc />
	public FileValue Value
	{
		get => _value.CurrentValue;
		set => _value.CurrentValue = value;
	}

	internal void ReplaceDefaultValue(FileValue newDefaultValue) => _value.ReplaceDefault(newDefaultValue);

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public override void Reset()
	{
		base.Reset();
		_value.Reset();
	}

	/// <inheritdoc />
	public override object Clone()
	{
		var clone = (FileNode)base.Clone();
		clone._value = (ResettableProperty<FileValue>)_value.Clone();
		return clone;
	}
}
