namespace RobinEpple.Common.Forms.Building;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.Nodes.DefaultImplementation;

internal class FileNodeBuilder : FieldNodeBuilder<IFileNodeBuilder, FileNode>, IFileNodeBuilder
{
	public FileNodeBuilder(FileNode node)
		: base(node) { }

	/// <inheritdoc />
	public IFileNodeBuilder UseDefaultValue(FileValue defaultValue)
	{
		Node.Value = defaultValue;
		Node.ReplaceDefaultValue(defaultValue);
		return CastThis();
	}

	/// <inheritdoc />
	protected override FileNodeBuilder CastThis() => this;
}
