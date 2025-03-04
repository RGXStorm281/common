namespace RobinEpple.Common.Forms.Nodes.Formatters;

public class BooleanFormatter(string trueText, string falseText) : IValueFormatter
{
	private readonly string _trueText = trueText;

	private readonly string _falseText = falseText;

	/// <inheritdoc />
	public string? Format(object? value)
	{
		if (value is not bool isTrue)
		{
			return null;
		}

		return isTrue ? _trueText : _falseText;
	}

	/// <inheritdoc />
	public Task<string?> FormatAsync(object? value) => Task.FromResult(Format(value));

	/// <inheritdoc />
	public object? Parse(string? textInput)
	{
		if (_trueText.Equals(textInput))
		{
			return true;
		}
		if (_falseText.Equals(textInput))
		{
			return false;
		}

		return null;
	}

	/// <inheritdoc />
	public Task<object?> ParseAsync(string? textInput) => Task.FromResult(Parse(textInput));
}
