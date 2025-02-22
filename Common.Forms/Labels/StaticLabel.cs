namespace RobinEpple.Common.Forms.Labels;

using RobinEpple.Common.Forms.BasicApi;

/// <summary>
/// Represents a label that always stays the same.
/// </summary>
/// <param name="text">The text to display.</param>
public class StaticLabel(string text) : ILabel
{
	/// <summary>
	/// The text of the label.
	/// </summary>
	public string Text { get; } = text;

	/// <inheritdoc />
	public object Clone() => throw new NotImplementedException();
}
