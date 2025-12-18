namespace RobinEpple.Common.Forms.Nodes.Formatters;

using RobinEpple.Common.SourceGenerators.Abstractions;

public partial class BooleanFormatter(string trueText, string falseText) : IValueFormatter
{
	private readonly string _trueText = trueText;

	private readonly string _falseText = falseText;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public string? Format(object? value)
	{
		if (value is not bool isTrue)
		{
			return null;
		}

		return isTrue ? _trueText : _falseText;
	}

	/// <inheritdoc />
	[GenerateAsyncOverload]
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
}
