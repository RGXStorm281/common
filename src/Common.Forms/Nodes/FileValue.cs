namespace RobinEpple.Common.Forms.Nodes;

/// <summary>
/// This struct represents a file with name and bytes.<br/>
/// It is a value type.
/// </summary>
public struct FileValue(string? fileName, byte[]? fileContents)
{
	/// <summary>
	/// The byte content of the file.
	/// </summary>
	public byte[]? FileContents { get; set; } = fileContents;

	/// <summary>
	/// The name of the file.
	/// </summary>
	public string? FileName { get; set; } = fileName;
}
