namespace RobinEpple.Common.Forms.Nodes.Formatters;

using RobinEpple.Common.SourceGenerators.Abstractions;

/// <summary>
/// A formatter for text values.
/// </summary>
public partial class TrimTextFormatter : IValueFormatter
{
	/// <inheritdoc />
	[GenerateAsyncOverload]
	public string? Format(object? value)
	{
		if (value is not string text)
		{
			return null;
		}

		return text.Trim();
	}

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public object? Parse(string? textInput)
	{
		if (textInput is not string text)
		{
			return null;
		}

		return text.Trim();
	}
}
