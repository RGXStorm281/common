namespace RobinEpple.Common.Forms.Nodes;

public interface IValueFormatter
{
	/// <summary>
	/// Translates the current value into its string representation.
	/// </summary>
	/// <param name="value">The current value.</param>
	/// <returns>The string representation.</returns>
	public string? Format(object? value);

	/// <summary>
	/// Parses a text input into the corresponding value.
	/// </summary>
	/// <param name="textInput">The text input.</param>
	/// <returns>The corresponding value.</returns>
	public object? Parse(string? textInput);
}
