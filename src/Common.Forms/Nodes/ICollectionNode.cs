namespace RobinEpple.Common.Forms.Nodes;

using RobinEpple.Common.SourceGenerators.Abstractions;

/// <summary>
/// This interface represents a collection of subsections in the form.<br/>
/// Subsections may be polymorphic by providing multiple templates.
/// </summary>
public partial interface ICollectionNode : IParentNode
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
	/// <returns>The newly created instance.</returns>
	[GenerateAsyncOverload]
	public IForm Instantiate(IForm template);

	/// <summary>
	/// Removes the given instance from this collection.
	/// </summary>
	/// <param name="instance">The instance.</param>
	[GenerateAsyncOverload]
	public void RemoveItem(IForm instance);

	/// <summary>
	/// Removes all instances.
	/// </summary>
	[GenerateAsyncOverload]
	public void Clear();
}
