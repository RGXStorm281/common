namespace RobinEpple.Common.Forms.Nodes;

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
