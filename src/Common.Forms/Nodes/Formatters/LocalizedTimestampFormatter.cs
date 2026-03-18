namespace RobinEpple.Common.Forms.Nodes.Formatters;

using System.Globalization;
using RobinEpple.Common.SourceGenerators.Abstractions;

/// <summary>
/// A formatter for timestamp values.
/// </summary>
public partial class LocalizedTimestampFormatter(CultureInfo culture) : IValueFormatter
{
	private readonly CultureInfo _culture = culture;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public string? Format(object? value)
	{
		if (value is not DateTime timestamp)
		{
			return null;
		}

		return timestamp.ToString(_culture);
	}

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public object? Parse(string? textInput)
	{
		if (!DateTime.TryParse(textInput, _culture, out var number))
		{
			return null;
		}

		return number;
	}
}
