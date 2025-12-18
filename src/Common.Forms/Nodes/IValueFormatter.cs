namespace RobinEpple.Common.Forms.Nodes;

using RobinEpple.Common.SourceGenerators.Abstractions;

/// <summary>
/// This interface defines API for formatters that export field values to or parse them from strings.
/// </summary>
public partial interface IValueFormatter
{
	/// <summary>
	/// Translates the current value into its string representation.
	/// </summary>
	/// <param name="value">The current value.</param>
	/// <returns>The string representation.</returns>
	[GenerateAsyncOverload]
	public string? Format(object? value);

	/// <summary>
	/// Parses a text input into the corresponding value.
	/// </summary>
	/// <param name="textInput">The text input.</param>
	/// <returns>The corresponding value.</returns>
	[GenerateAsyncOverload]
	public object? Parse(string? textInput);
}
