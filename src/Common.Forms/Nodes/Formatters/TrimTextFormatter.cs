namespace RobinEpple.Common.Forms.Nodes.Formatters;

using System.Threading.Tasks;

public class TrimTextFormatter : IValueFormatter
{
	/// <inheritdoc />
	public string? Format(object? value)
	{
		if (value is not string text)
		{
			return null;
		}

		return text.Trim();
	}

	/// <inheritdoc />
	public Task<string?> FormatAsync(object? value) => Task.FromResult(Format(value));

	/// <inheritdoc />
	public object? Parse(string? textInput)
	{
		if (textInput is not string text)
		{
			return null;
		}

		return text.Trim();
	}

	/// <inheritdoc />
	public Task<object?> ParseAsync(string? textInput) => Task.FromResult(Parse(textInput));
}
