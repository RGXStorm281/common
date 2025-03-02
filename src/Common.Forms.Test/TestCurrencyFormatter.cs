namespace Common.Forms.Test;

using System.Globalization;
using RobinEpple.Common.Forms.Nodes;

public class TestCurrencyFormatter(CultureInfo culture) : IValueFormatter
{
	private readonly CultureInfo _culture = culture;

	public string? Format(object? value)
	{
		if (value is not decimal number)
		{
			throw new InvalidOperationException("This formatter can only be used on number nodes.");
		}

		return number.ToString("c", _culture);
	}

	public object? Parse(string? textInput)
	{
		if (!decimal.TryParse(textInput, _culture, out var value))
		{
			throw new InvalidOperationException("Unsupported format.");
		}

		return value;
	}
}
