namespace RobinEpple.Common.Forms.Nodes.Formatters;

using RobinEpple.Common.SourceGenerators.Abstractions;

public partial class FileSerializer : IValueFormatter
{
	public const char Separator = ':';
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
