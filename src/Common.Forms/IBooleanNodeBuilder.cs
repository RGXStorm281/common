namespace RobinEpple.Common.Forms;

using System.Linq.Expressions;
using RobinEpple.Common.Forms.Binding;

/// <summary>
/// A builder for a boolean node.
/// </summary>
public interface IBooleanNodeBuilder
	: IFieldNodeBuilder<IBooleanNodeBuilder>,
		IValueNodeBuilder<bool?, IBooleanNodeBuilder>
{
	/// <summary>
	/// Sets a default value that the node starts with and is resetted to.
	/// </summary>
	/// <param name="defaultValue">The default value to use.</param>
	/// <returns>The node builder for further configurations.</returns>
	public IBooleanNodeBuilder UseDefaultValue(bool? defaultValue);

	/// <summary>
	/// Creates a model binding from the custom getter and setter functions.
	/// </summary>
	/// <param name="getter">The method loading the value from some model available in the building context.</param>
	/// <param name="setter">The method writing the value to some model available in the building context.</param>
	/// <returns>The node builder for further configurations.</returns>
	public IBooleanNodeBuilder UseGetterSetterBinding(Func<bool?> getter, Action<bool?> setter);

	/// <summary>
	/// Creates a model binding by constructing getter and setter methods from the given <paramref name="propertyAccessor"/>
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property accessible from the building context.</param>
	/// <returns>The node builder for further configurations.</returns>
	public IBooleanNodeBuilder UsePropertyBinding(Expression<Func<bool?>> propertyAccessor);

	/// <summary>
	/// Creates a binding referencing an embedded model using custom getter and setter methods.
	/// </summary>
	/// <param name="modelReference">The binding factory for referencing an instance model.</param>
	/// <param name="getter">The method loading the value from the model.</param>
	/// <param name="setter">The method writing the value to the model.</param>
	/// <returns>The node builder for adding further configurations.</returns>
	public IBooleanNodeBuilder UseEmbeddedModelGetterSetterBinding<TModel>(
		EmbeddedModelReference<TModel> modelReference,
		Func<TModel, bool?> getter,
		Action<TModel, bool?> setter
	);

	/// <summary>
	/// Creates a binding referencing an embedded model by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="modelReference">The binding factory for referencing an instance model.</param>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The node builder for adding further configurations.</returns>
	public IBooleanNodeBuilder UseEmbeddedModelPropertyBinding<TModel>(
		EmbeddedModelReference<TModel> modelReference,
		Expression<Func<TModel, bool?>> propertyAccessor
	);
}
