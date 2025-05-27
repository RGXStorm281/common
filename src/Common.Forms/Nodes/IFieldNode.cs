namespace RobinEpple.Common.Forms.Nodes;

/// <summary>
/// This is the base interface for an input of any type in the form.
/// </summary>
public interface IFieldNode : IFormNode
{
	/// <summary>
	/// A flag, indicating whether the field has been touched by the user yet (for showing validation errors).
	/// </summary>
	public bool HasUserInteraction { get; set; }

	/// <summary>
	/// The formatter responsible for converting values to string and parse them again.
	/// </summary>
	public IValueFormatter Formatter { get; }
}
