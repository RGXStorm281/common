namespace RobinEpple.Common.Forms;

using System.Linq.Expressions;
using RobinEpple.Common.Forms.Binding;

public interface INumberNodeBuilder
	: IFieldNodeBuilder<INumberNodeBuilder>,
		IValueNodeBuilder<decimal?, INumberNodeBuilder>
{
	/// <summary>
	/// Sets a default value that the node starts with and is resetted to.
	/// </summary>
	/// <param name="defaultValue">The default value to use.</param>
	public INumberNodeBuilder UseDefaultValue(decimal? defaultValue);

	/// <summary>
	/// Creates a model binding from the custom getter and setter functions.
	/// </summary>
	/// <param name="getter">The method loading the value from some model available in the building context.</param>
	/// <param name="setter">The method writing the value to some model available in the building context.</param>
	/// <returns>The node builder for further configurations.</returns>
	public INumberNodeBuilder UseGetterSetterBinding(Func<decimal?> getter, Action<decimal?> setter);

	/// <summary>
	/// Creates a model binding by constructing getter and setter methods from the given <paramref name="propertyAccessor"/>
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property accessible from the building context.</param>
	/// <returns>The node builder for further configurations.</returns>
	public INumberNodeBuilder UsePropertyBinding(Expression<Func<decimal?>> propertyAccessor);

	/// <inheritdoc cref="UsePropertyBinding"/>
	public INumberNodeBuilder UsePropertyBinding(Expression<Func<double?>> propertyAccessor);

	/// <inheritdoc cref="UsePropertyBinding"/>
	public INumberNodeBuilder UsePropertyBinding(Expression<Func<float?>> propertyAccessor);

	/// <inheritdoc cref="UsePropertyBinding"/>
	public INumberNodeBuilder UsePropertyBinding(Expression<Func<long?>> propertyAccessor);

	/// <inheritdoc cref="UsePropertyBinding"/>
	public INumberNodeBuilder UsePropertyBinding(Expression<Func<int?>> propertyAccessor);

	/// <summary>
	/// Creates a binding referencing an embedded model using custom getter and setter methods.
	/// </summary>
	/// <param name="modelReference">The binding factory for referencing an instance model.</param>
	/// <param name="getter">The method loading the value from the model.</param>
	/// <param name="setter">The method writing the value to the model.</param>
	/// <returns>The node builder for adding further configurations.</returns>
	public INumberNodeBuilder UseEmbeddedModelGetterSetterBinding<TModel>(
		EmbeddedModelReference<TModel> modelReference,
		Func<TModel, decimal?> getter,
		Action<TModel, decimal?> setter
	);

	/// <summary>
	/// Creates a binding referencing an embedded model by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="modelReference">The binding factory for referencing an instance model.</param>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The node this for adding further configurations.</returns>
	public INumberNodeBuilder UseEmbeddedModelPropertyBinding<TModel>(
		EmbeddedModelReference<TModel> modelReference,
		Expression<Func<TModel, decimal?>> propertyAccessor
	);

	/// <inheritdoc cref="UseEmbeddedModelPropertyBinding"/>
	public INumberNodeBuilder UseEmbeddedModelPropertyBinding<TModel>(
		EmbeddedModelReference<TModel> modelReference,
		Expression<Func<TModel, double?>> propertyAccessor
	);

	/// <inheritdoc cref="UseEmbeddedModelPropertyBinding"/>
	public INumberNodeBuilder UseEmbeddedModelPropertyBinding<TModel>(
		EmbeddedModelReference<TModel> modelReference,
		Expression<Func<TModel, float?>> propertyAccessor
	);

	/// <inheritdoc cref="UseEmbeddedModelPropertyBinding"/>
	public INumberNodeBuilder UseEmbeddedModelPropertyBinding<TModel>(
		EmbeddedModelReference<TModel> modelReference,
		Expression<Func<TModel, long?>> propertyAccessor
	);

	/// <inheritdoc cref="UseEmbeddedModelPropertyBinding"/>
	public INumberNodeBuilder UseEmbeddedModelPropertyBinding<TModel>(
		EmbeddedModelReference<TModel> modelReference,
		Expression<Func<TModel, int?>> propertyAccessor
	);
}
