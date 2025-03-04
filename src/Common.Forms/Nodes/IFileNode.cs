namespace RobinEpple.Common.Forms.Nodes;

public interface IFileNode : IFieldNode
{
	/// <summary>
	/// The contained file.
	/// </summary>
	public FileValue Value { get; set; }
}
