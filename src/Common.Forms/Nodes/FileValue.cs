namespace RobinEpple.Common.Forms.Nodes;

/// <summary>
/// This struct represents a file with name and bytes.<br/>
/// It is a value type.
/// </summary>
public struct FileValue
{
	/// <summary>
	/// The byte content of the file.
	/// </summary>
	public byte[]? FileContents { get; set; }

	/// <summary>
	/// The name of the file.
	/// </summary>
	public string? FileName { get; set; }
}
