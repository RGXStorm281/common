namespace RobinEpple.Common.Forms;

using System.Linq.Expressions;
using RobinEpple.Common.Forms.Binding;

/// <summary>
/// A builder for a collection node.
/// </summary>
public interface ICollectionNodeBuilder : ITemplatedNodeBuilder<ICollectionNodeBuilder>
{
	/// <summary>
	/// Creates a model binding from the custom getter and setter functions.
	/// </summary>
	/// <typeparam name="TItem">The type of the items in the collection, this node represents. This may be a supertype for different implementations in different templates.</typeparam>
	/// <param name="getter">The method loading the value from some model available in the building context.</param>
	/// <param name="setter">The method writing the value to some model available in the building context.</param>
	/// <returns>The node builder for further configurations.</returns>
	public ICollectionNodeBuilder UseGetterSetterBinding<TItem>(
		Func<IEnumerable<TItem>> getter,
		Action<IEnumerable<TItem>> setter
	);

	/// <summary>
	/// Creates a model binding by constructing getter and setter methods from the given <paramref name="propertyAccessor"/>
	/// </summary>
	/// <typeparam name="TItem">The type of the items in the collection, this node represents. This may be a supertype for different implementations in different templates.</typeparam>
	/// <param name="propertyAccessor">An expression pointing to some property accessible from the building context.</param>
	/// <returns>The node builder for further configurations.</returns>
	public ICollectionNodeBuilder UsePropertyBinding<TItem>(Expression<Func<IEnumerable<TItem>>> propertyAccessor);

	/// <summary>
	/// Creates a binding referencing an embedded model using custom getter and setter methods.
	/// </summary>
	/// <param name="modelReference">The binding factory for referencing an instance model.</param>
	/// <param name="getter">The method loading the value from the model.</param>
	/// <param name="setter">The method writing the value to the model.</param>
	/// <returns>The node builder for adding further configurations.</returns>
	public ICollectionNodeBuilder UseEmbeddedModelGetterSetterBinding<TModel, TItem>(
		EmbeddedModelReference<TModel> modelReference,
		Func<TModel, IEnumerable<TItem>> getter,
		Action<TModel, IEnumerable<TItem>> setter
	);

	/// <summary>
	/// Creates a binding referencing an embedded model by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="modelReference">The binding factory for referencing an instance model.</param>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The node builder for adding further configurations.</returns>
	public ICollectionNodeBuilder UseEmbeddedModelPropertyBinding<TModel, TItem>(
		EmbeddedModelReference<TModel> modelReference,
		Expression<Func<TModel, IEnumerable<TItem>>> propertyAccessor
	);
}
