namespace RobinEpple.Common.Forms.Nodes.Formatters;

using System.Globalization;
using RobinEpple.Common.SourceGenerators.Abstractions;

public partial class LocalizedNumberFormatter(CultureInfo culture) : IValueFormatter
{
	private readonly CultureInfo _culture = culture;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public string? Format(object? value)
	{
		if (value is not decimal number)
		{
			return null;
		}

		return number.ToString(_culture);
	}

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public object? Parse(string? textInput)
	{
		if (!decimal.TryParse(textInput, _culture, out var number))
		{
			return null;
		}

		return number;
	}
}
