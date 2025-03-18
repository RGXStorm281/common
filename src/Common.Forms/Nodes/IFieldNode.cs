namespace RobinEpple.Common.Forms.Nodes;

using RobinEpple.Common.Forms.Binding;

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
	/// Optional binding to load the state from and save changes to.
	/// </summary>
	public IFieldNodeBinding? Binding { get; }

	/// <summary>
	/// The formatter responsible for converting values to string and parse them again.
	/// </summary>
	public IValueFormatter Formatter { get; }
}
