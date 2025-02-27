namespace RobinEpple.Common.Forms.Nodes;

public interface IFileNode : IFieldNode
{
	/// <summary>
	/// The name of the file.
	/// </summary>
	public string FileName { get; set; }

	/// <summary>
	/// The contents of the file.
	/// </summary>
	public byte[]? Value { get; set; }
}
