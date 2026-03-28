namespace RobinEpple.Common.Forms.Nodes.Formatters;

using RobinEpple.Common.SourceGenerators.Abstractions;

/// <summary>
/// Just outputs the label of the currently instantiated template.
/// On parsing it only trims the text.
/// </summary>
public partial class TemplateFormatter : IValueFormatter
{
	/// <inheritdoc />
	[GenerateAsyncOverload]
	public string? Format(object? value)
	{
		if (value is not IForm instance)
		{
			return null;
		}

		return instance.Label;
	}

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public object? Parse(string? textInput)
	{
		return textInput?.Trim();
	}
}
