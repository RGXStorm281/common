namespace RobinEpple.Common.Forms;

using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// An interface for a (generated) wrapper type for a form.
/// </summary>
public interface IFormWrapper
{
	/// <summary>
	/// The wrapped form.
	/// </summary>
	public IForm? Node { get; }
}
