namespace RobinEpple.Common.Forms.Nodes;

/// <summary>
/// This interface represents a section in a form. It may be the root.
/// </summary>
public interface IForm : IScopeProvider
{
	/// <summary>
	/// The list of nodes in this form.<br/>
	/// The order does not imply a visual arrangement.
	/// </summary>
	public IEnumerable<IFormNode> Nodes { get; }
}
