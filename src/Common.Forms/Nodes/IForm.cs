namespace RobinEpple.Common.Forms.Nodes;

using RobinEpple.Common.Forms.Binding;

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

	/// <summary>
	/// Optional embedded model in this form.
	/// </summary>
	public IFormModel? EmbeddedModel { get; }
}
