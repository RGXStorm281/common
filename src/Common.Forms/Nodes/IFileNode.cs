namespace RobinEpple.Common.Forms.Nodes;

/// <summary>
/// This interface represents a file input in the form.
/// </summary>
public interface IFileNode : IFieldNode
{
	/// <summary>
	/// The contained file.
	/// </summary>
	public FileValue Value { get; set; }
}
