namespace Common.Forms.Test;

using System.Globalization;
using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

public class TestEuroFormatter() : IValueFormatter
{
	private readonly CultureInfo _culture = new CultureInfo("de-DE");

	public string? Format(object? value)
	{
		if (value is not decimal number)
		{
			throw new InvalidOperationException("This formatter can only be used on number nodes.");
		}

		return number.ToString("c", _culture);
	}

	public Task<string?> FormatAsync(object? value) => Task.FromResult(Format(value));

	public object? Parse(string? textInput)
	{
		textInput = textInput?.TrimEnd('€').Trim();
		if (!decimal.TryParse(textInput, _culture, out var value))
		{
			throw new InvalidOperationException("Unsupported format.");
		}

		return value;
	}

	public Task<object?> ParseAsync(string? textInput) => Task.FromResult(Parse(textInput));
}
