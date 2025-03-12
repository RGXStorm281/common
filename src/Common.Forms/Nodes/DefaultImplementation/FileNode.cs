namespace RobinEpple.Common.Forms.Nodes.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes.Formatters;

internal class FileNode : FieldNode, IFileNode
{
	public FileNode(string name, IParentNode parent)
		: base(name, parent, new FileSerializer())
	{
		Value = new();
	}

	public FileValue Value { get; set; }

	/// <inheritdoc />
	public override void Reset()
	{
		base.Reset();
		Value.FileContents = null;
		Value.FileName = null;
	}

	/// <inheritdoc />
	public override async Task ResetAsync()
	{
		await base.ResetAsync();
		Value.FileContents = null;
		Value.FileName = null;
	}

	/// <inheritdoc />
	public override object Clone()
	{
		var clone = (FileNode)base.Clone();
		clone.Value = new FileValue()
		{
			FileContents = (byte[]?)Value.FileContents?.Clone(),
			FileName = Value.FileName,
		};
		return clone;
	}
}
