namespace RobinEpple.Common.Forms;

using System.Linq.Expressions;
using RobinEpple.Common.Forms.Binding;

public interface ITemplateNodeBuilder : ITemplatedNodeBuilder<ITemplateNodeBuilder>
{
	/// <summary>
	/// Creates a model binding from the custom getter and setter functions.
	/// </summary>
	/// <typeparam name="TModel">The type of the model this templated node represents. This may be a supertype for different implementations in different templates.</typeparam>
	/// <param name="getter">The method loading the value from some model available in the building context.</param>
	/// <param name="setter">The method writing the value to some model available in the building context.</param>
	/// <param name="emptyValue">The value to write to the model, if the template instance does not yield a model.</param>
	/// <returns>The node for further configurations.</returns>
	public ITemplateNodeBuilder UseGetterSetterBinding<TModel>(
		Func<TModel?> getter,
		Action<TModel?> setter,
		TModel? emptyValue
	);

	/// <inheritdoc cref="UseGetterSetterBinding"/>
	public ITemplateNodeBuilder UseGetterSetterBinding<TModel>(Func<TModel?> getter, Action<TModel?> setter)
		where TModel : class;

	/// <inheritdoc cref="UseGetterSetterBinding"/>
	public ITemplateNodeBuilder UseGetterSetterBinding<TModel>(Func<TModel?> getter, Action<TModel?> setter)
		where TModel : struct;

	/// <summary>
	/// Creates a model binding by constructing getter and setter methods from the given <paramref name="propertyAccessor"/>
	/// </summary>
	/// <typeparam name="TModel">The type of the model this templated node represents. This may be a supertype for different implementations in different templates.</typeparam>
	/// <param name="propertyAccessor">An expression pointing to some property accessible from the building context.</param>
	/// <param name="emptyValue">The value to write to the model, if the template instance does not yield a model.</param>
	/// <returns>The node for further configurations.</returns>
	public ITemplateNodeBuilder UsePropertyBinding<TModel>(
		Expression<Func<TModel?>> propertyAccessor,
		TModel? emptyValue
	);

	/// <inheritdoc cref="UsePropertyBinding{TModel}"/>
	public ITemplateNodeBuilder UsePropertyBinding<TModel>(Expression<Func<TModel?>> propertyAccessor)
		where TModel : class;

	/// <inheritdoc cref="UsePropertyBinding{TModel}"/>
	public ITemplateNodeBuilder UsePropertyBinding<TModel>(Expression<Func<TModel?>> propertyAccessor)
		where TModel : struct;

	/// <summary>
	/// Creates a binding referencing an embedded model using custom getter and setter methods.
	/// </summary>
	/// <param name="modelReference">The binding factory for referencing an instance model.</param>
	/// <param name="getter">The method loading the value from the model.</param>
	/// <param name="setter">The method writing the value to the model.</param>
	/// <returns>The node for adding further configurations.</returns>
	public ITemplateNodeBuilder UseEmbeddedModelGetterSetterBinding<TModel, TInnerModel>(
		EmbeddedModelReference<TModel> modelReference,
		Func<TModel, TInnerModel?> getter,
		Action<TModel, TInnerModel?> setter,
		TInnerModel? emptyValue
	);

	/// <inheritdoc cref="UseEmbeddedModelGetterSetterBinding"/>
	public ITemplateNodeBuilder UseEmbeddedModelGetterSetterBinding<TModel, TInnerModel>(
		EmbeddedModelReference<TModel> modelReference,
		Func<TModel, TInnerModel?> getter,
		Action<TModel, TInnerModel?> setter
	)
		where TInnerModel : class;

	/// <inheritdoc cref="UseEmbeddedModelGetterSetterBinding"/>
	public ITemplateNodeBuilder UseEmbeddedModelGetterSetterBinding<TModel, TInnerModel>(
		EmbeddedModelReference<TModel> modelReference,
		Func<TModel, TInnerModel?> getter,
		Action<TModel, TInnerModel?> setter
	)
		where TInnerModel : struct;

	/// <summary>
	/// Creates a binding referencing  an embedded model by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="modelReference">The binding factory for referencing an instance model.</param>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The node for adding further configurations.</returns>
	public ITemplateNodeBuilder UseEmbeddedModelPropertyBinding<TModel, TInnerModel>(
		EmbeddedModelReference<TModel> modelReference,
		Expression<Func<TModel, TInnerModel?>> propertyAccessor,
		TInnerModel? emptyItem
	);

	/// <inheritdoc cref="UseEmbeddedModelPropertyBinding"/>
	public ITemplateNodeBuilder UseEmbeddedModelPropertyBinding<TModel, TInnerModel>(
		EmbeddedModelReference<TModel> modelReference,
		Expression<Func<TModel, TInnerModel?>> propertyAccessor
	)
		where TInnerModel : class;

	/// <inheritdoc cref="UseEmbeddedModelPropertyBinding"/>
	public ITemplateNodeBuilder UseEmbeddedModelPropertyBinding<TModel, TInnerModel>(
		EmbeddedModelReference<TModel> modelReference,
		Expression<Func<TModel, TInnerModel?>> propertyAccessor
	)
		where TInnerModel : struct;
}
