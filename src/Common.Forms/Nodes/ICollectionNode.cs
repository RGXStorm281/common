namespace RobinEpple.Common.Forms.Nodes;

using RobinEpple.Common.Forms.Binding;

public interface ICollectionNode : IFormNode
{
	/// <summary>
	/// The list of templates, that can be instantiated to live in this collection.
	/// </summary>
	public IEnumerable<IForm> Templates { get; }

	/// <summary>
	/// The list of instances in this collection.
	/// </summary>
	public IEnumerable<IForm> Instances { get; }

	/// <summary>
	/// Optional binding to load the state from and save changes.
	/// </summary>
	public IFormBinding? Binding { get; }

	/// <summary>
	/// Creates a new instance of the given template.
	/// </summary>
	/// <param name="template">The template.</param>
	public void CreateChild(IForm template);

	/// <inheritdoc cref="CreateChild"/>
	public Task CreateChildAsync(IForm template);

	/// <summary>
	/// Removes the given instance from this collection.
	/// </summary>
	/// <param name="instance">The instance.</param>
	public void RemoveChild(IForm instance);

	/// <inheritdoc cref="RemoveChild"/>
	public Task RemoveChildAsync(IForm instance);
}
