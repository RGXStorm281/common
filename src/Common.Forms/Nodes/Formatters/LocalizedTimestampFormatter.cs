namespace RobinEpple.Common.Forms.Nodes.Formatters;

using System.Globalization;

public class LocalizedTimestampFormatter(CultureInfo culture) : IValueFormatter
{
	private readonly CultureInfo _culture = culture;

	/// <inheritdoc />
	public string? Format(object? value)
	{
		if (value is not DateTime timestamp)
		{
			return null;
		}

		return timestamp.ToString(_culture);
	}

	/// <inheritdoc />
	public Task<string?> FormatAsync(object? value) => Task.FromResult(Format(value));

	/// <inheritdoc />
	public object? Parse(string? textInput)
	{
		if (!DateTime.TryParse(textInput, _culture, out var number))
		{
			return null;
		}

		return number;
	}

	/// <inheritdoc />
	public Task<object?> ParseAsync(string? textInput) => Task.FromResult(Parse(textInput));
}
