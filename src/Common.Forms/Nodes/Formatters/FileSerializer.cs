namespace RobinEpple.Common.Forms.Nodes.Formatters;

using RobinEpple.Common.SourceGenerators.Abstractions;

/// <summary>
/// A formatter for file values.
/// </summary>
public partial class FileSerializer : IValueFormatter
{
	/// <summary>
	/// The separator between file name and base64 contents.
	/// </summary>
	public const char Separator = ':';

	/// <summary>
	/// A list of allowed characters in file names.
	/// </summary>
	public const string AllowedFileNameCharacters = "ABCDEFGHIKLMNOPQRSTUVXYZ abcdefghiklmnopqrstuvxyz0123456789()-_.";

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public string? Format(object? value)
	{
		if (value is not FileValue file)
		{
			return null;
		}

		// Serialize the file contents.
		var byteRepresentation = file.FileContents == null ? string.Empty : Convert.ToBase64String(file.FileContents);

		// Then combine with the file name into a merged string representation.
		return $"{file.FileName}{Separator}{byteRepresentation}";
	}

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public object? Parse(string? textInput)
	{
		if (textInput is not string text)
		{
			return new FileValue();
		}

		// Split the parts.
		var parts = text.Split(Separator);
		if (parts.Length != 2)
		{
			return new FileValue();
		}

		// Filter the file name for allowed characters.
		var fileName = new string(parts[0].Where(AllowedFileNameCharacters.Contains).ToArray());

		// Deserialize the file contents.
		byte[]? fileContents;
		try
		{
			fileContents = Convert.FromBase64String(parts[2]);
		}
		catch (Exception)
		{
			fileContents = null;
		}

		return new FileValue() { FileName = fileName, FileContents = fileContents };
	}
}
