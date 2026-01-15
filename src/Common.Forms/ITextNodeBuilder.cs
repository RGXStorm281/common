namespace RobinEpple.Common.Forms;

using System.Linq.Expressions;
using RobinEpple.Common.Forms.Binding;

/// <summary>
/// A builder for a text node.
/// </summary>
public interface ITextNodeBuilder : IFieldNodeBuilder<ITextNodeBuilder>, IValueNodeBuilder<string?, ITextNodeBuilder>
{
	/// <summary>
	/// Sets a default value that the node starts with and is resetted to.
	/// </summary>
	/// <param name="defaultValue">The default value to use.</param>
	public ITextNodeBuilder UseDefaultValue(string? defaultValue);

	/// <summary>
	/// Creates a model binding from the custom getter and setter functions.
	/// </summary>
	/// <param name="getter">The method loading the value from some model available in the building context.</param>
	/// <param name="setter">The method writing the value to some model available in the building context.</param>
	/// <returns>The node builder for further configurations.</returns>
	public ITextNodeBuilder UseGetterSetterBinding(Func<string?> getter, Action<string?> setter);

	/// <summary>
	/// Creates a model binding by constructing getter and setter methods from the given <paramref name="propertyAccessor"/>
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property accessible from the building context.</param>
	/// <returns>The node builder for further configurations.</returns>
	public ITextNodeBuilder UsePropertyBinding(Expression<Func<string?>> propertyAccessor);

	/// <summary>
	/// Creates a binding referencing an embedded model using custom getter and setter methods.
	/// </summary>
	/// <param name="modelReference">The binding factory for referencing an instance model.</param>
	/// <param name="getter">The method loading the value from the model.</param>
	/// <param name="setter">The method writing the value to the model.</param>
	/// <returns>The node builder for adding further configurations.</returns>
	public ITextNodeBuilder UseEmbeddedModelGetterSetterBinding<TModel>(
		EmbeddedModelReference<TModel> modelReference,
		Func<TModel, string?> getter,
		Action<TModel, string?> setter
	);

	/// <summary>
	/// Creates a binding referencing an embedded model by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="modelReference">The binding factory for referencing an instance model.</param>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The node builder for adding further configurations.</returns>
	public ITextNodeBuilder UseEmbeddedModelPropertyBinding<TModel>(
		EmbeddedModelReference<TModel> modelReference,
		Expression<Func<TModel, string?>> propertyAccessor
	);
}
