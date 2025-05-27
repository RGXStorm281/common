namespace RobinEpple.Common.Forms.Nodes;

/// <summary>
/// This interface represents a collection of subsections in the form.<br/>
/// Subsections may be polymorphic by providing multiple templates.
/// </summary>
public interface ICollectionNode : IParentNode
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
	/// Creates a new instance of the given template.
	/// </summary>
	/// <param name="template">The template.</param>
	public void Instantiate(IForm template);

	/// <inheritdoc cref="Instantiate"/>
	public Task InstantiateAsync(IForm template);

	/// <summary>
	/// Removes the given instance from this collection.
	/// </summary>
	/// <param name="instance">The instance.</param>
	public void RemoveItem(IForm instance);

	/// <inheritdoc cref="RemoveItem"/>
	public Task RemoveItemAsync(IForm instance);

	/// <summary>
	/// Removes all instances.
	/// </summary>
	public void Clear();

	/// <inheritdoc cref="Clear"/>
	public Task ClearAsync();
}
