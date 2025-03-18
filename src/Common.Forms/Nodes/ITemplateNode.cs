namespace RobinEpple.Common.Forms.Nodes;

using RobinEpple.Common.Forms.Binding;

/// <summary>
/// This interface represents an (optional) subsection in the form.<br/>
/// The subsection may be polymorphic by providing multiple templates.
/// </summary>
public interface ITemplateNode : IParentNode
{
	/// <summary>
	/// The list of templates, that can be instantiated to live in this collection.
	/// </summary>
	public IEnumerable<IForm> Templates { get; }

	/// <summary>
	/// The current instance, if it exists.
	/// </summary>
	public IForm? Instance { get; }

	/// <summary>
	/// Optional binding to load the state from and save changes to.
	/// </summary>
	public ITemplateNodeBinding? Binding { get; }

	/// <summary>
	/// Creates a new instance of the given template.
	/// </summary>
	/// <param name="template">The template.</param>
	public void Instantiate(IForm template);

	/// <inheritdoc cref="Instantiate"/>
	public Task InstantiateAsync(IForm template);

	/// <summary>
	/// Removes the current instance.
	/// </summary>
	public void Clear();

	/// <inheritdoc cref="Clear"/>
	public Task ClearAsync();
}
