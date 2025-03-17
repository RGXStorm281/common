namespace RobinEpple.Common.Forms.Building;

using RobinEpple.Common.Forms.Nodes.DefaultImplementation;

internal class FileNodeBuilder : FieldNodeBuilder<IFileNodeBuilder, FileNode>, IFileNodeBuilder
{
	public FileNodeBuilder(FileNode node)
		: base(node) { }

	public void UseDefaultValue(string? fileName, byte[]? fileContent) => throw new NotImplementedException();

	/// <inheritdoc />
	protected override FileNodeBuilder CastThis() => this;
}
