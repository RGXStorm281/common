namespace RobinEpple.Common.Forms.Nodes;

/// <summary>
/// This interface defines API for formatters that export field values to or parse them from strings.
/// </summary>
public interface IValueFormatter
{
	/// <summary>
	/// Translates the current value into its string representation.
	/// </summary>
	/// <param name="value">The current value.</param>
	/// <returns>The string representation.</returns>
	public string? Format(object? value);

	/// <inheritdoc cref="Format"/>
	public Task<string?> FormatAsync(object? value);

	/// <summary>
	/// Parses a text input into the corresponding value.
	/// </summary>
	/// <param name="textInput">The text input.</param>
	/// <returns>The corresponding value.</returns>
	public object? Parse(string? textInput);

	/// <inheritdoc cref="Parse"/>
	public Task<object?> ParseAsync(string? textInput);
}
