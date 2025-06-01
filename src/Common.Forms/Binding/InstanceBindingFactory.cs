namespace RobinEpple.Common.Forms.Binding;

using System.Linq.Expressions;
using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// This factory can be used to create model bindings for instance models in parent nodes.
/// </summary>
/// <typeparam name="TModel">The type of the instance model.</typeparam>
public class InstanceBindingFactory<TModel>
{
	private readonly string _instanceNodeName;

	public InstanceBindingFactory(string instanceNodeName)
	{
		_instanceNodeName = instanceNodeName;
	}

	/// <summary>
	/// Creates a binding for an <see cref="IBooleanNode"/> using custom getter and setter methods.
	/// </summary>
	/// <param name="getter">The method loading the value from the model.</param>
	/// <param name="setter">The method writing the value to the model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreateGetterSetterBinding(Func<TModel, bool?> getter, Action<TModel, bool?> setter)
	{
		var binding = new FormNodeBinding<bool?>(
			new ValueNodeBinding<bool?>(),
			new EmbeddedModelGetterSetterBinding<TModel, bool?>(_instanceNodeName, getter, setter)
		);
		return binding;
	}

	/// <summary>
	/// Creates a binding for an <see cref="IBooleanNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreatePropertyBinding(Expression<Func<TModel, bool?>> propertyAccessor)
	{
		var binding = new FormNodeBinding<bool?>(
			new ValueNodeBinding<bool?>(),
			new EmbeddedModelPropertyBinding<TModel, bool?, bool?>(_instanceNodeName, propertyAccessor)
		);
		return binding;
	}

	/// <summary>
	/// Creates a binding for an <see cref="IFileNode"/> using custom getter and setter methods.
	/// </summary>
	/// <param name="getter">The method loading the value from the model.</param>
	/// <param name="setter">The method writing the value to the model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreateGetterSetterBinding(
		Func<TModel, FileValue> getter,
		Action<TModel, FileValue> setter
	)
	{
		var binding = new FormNodeBinding<FileValue>(
			new ValueNodeBinding<FileValue>(),
			new EmbeddedModelGetterSetterBinding<TModel, FileValue>(_instanceNodeName, getter, setter)
		);
		return binding;
	}

	/// <summary>
	/// Creates a binding for an <see cref="IFileNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreatePropertyBinding(Expression<Func<TModel, FileValue>> propertyAccessor)
	{
		var binding = new FormNodeBinding<FileValue>(
			new ValueNodeBinding<FileValue>(),
			new EmbeddedModelPropertyBinding<TModel, FileValue, FileValue>(_instanceNodeName, propertyAccessor)
		);
		return binding;
	}

	/// <summary>
	/// Creates a binding for an <see cref="INumberNode"/> using custom getter and setter methods.
	/// </summary>
	/// <param name="getter">The method loading the value from the model.</param>
	/// <param name="setter">The method writing the value to the model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreateGetterSetterBinding(Func<TModel, decimal?> getter, Action<TModel, decimal?> setter)
	{
		var binding = new FormNodeBinding<decimal?>(
			new ValueNodeBinding<decimal?>(),
			new EmbeddedModelGetterSetterBinding<TModel, decimal?>(_instanceNodeName, getter, setter)
		);
		return binding;
	}

	/// <summary>
	/// Creates a binding for an <see cref="INumberNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreatePropertyBinding(Expression<Func<TModel, decimal?>> propertyAccessor)
	{
		var binding = new FormNodeBinding<decimal?>(
			new ValueNodeBinding<decimal?>(),
			new EmbeddedModelPropertyBinding<TModel, decimal?, decimal?>(_instanceNodeName, propertyAccessor)
		);
		return binding;
	}

	/// <summary>
	/// Creates a binding for an <see cref="INumberNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreatePropertyBinding(Expression<Func<TModel, double?>> propertyAccessor)
	{
		var binding = new FormNodeBinding<double?>(
			new ValueNodeBinding<double?>(),
			new EmbeddedModelPropertyBinding<TModel, double?, double?>(_instanceNodeName, propertyAccessor)
		);
		return binding;
	}

	/// <summary>
	/// Creates a binding for an <see cref="INumberNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreatePropertyBinding(Expression<Func<TModel, float?>> propertyAccessor)
	{
		var binding = new FormNodeBinding<float?>(
			new ValueNodeBinding<float?>(),
			new EmbeddedModelPropertyBinding<TModel, float?, float?>(_instanceNodeName, propertyAccessor)
		);
		return binding;
	}

	/// <summary>
	/// Creates a binding for an <see cref="INumberNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreatePropertyBinding(Expression<Func<TModel, long?>> propertyAccessor)
	{
		var binding = new FormNodeBinding<long?>(
			new ValueNodeBinding<long?>(),
			new EmbeddedModelPropertyBinding<TModel, long?, long?>(_instanceNodeName, propertyAccessor)
		);
		return binding;
	}

	/// <summary>
	/// Creates a binding for an <see cref="INumberNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreatePropertyBinding(Expression<Func<TModel, int?>> propertyAccessor)
	{
		var binding = new FormNodeBinding<int?>(
			new ValueNodeBinding<int?>(),
			new EmbeddedModelPropertyBinding<TModel, int?, int?>(_instanceNodeName, propertyAccessor)
		);
		return binding;
	}

	/// <summary>
	/// Creates a binding for an <see cref="ITextNode"/> using custom getter and setter methods.
	/// </summary>
	/// <param name="getter">The method loading the value from the model.</param>
	/// <param name="setter">The method writing the value to the model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreateGetterSetterBinding(Func<TModel, string?> getter, Action<TModel, string?> setter)
	{
		var binding = new FormNodeBinding<string?>(
			new ValueNodeBinding<string?>(),
			new EmbeddedModelGetterSetterBinding<TModel, string?>(_instanceNodeName, getter, setter)
		);
		return binding;
	}

	/// <summary>
	/// Creates a binding for an <see cref="ITextNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreatePropertyBinding(Expression<Func<TModel, string?>> propertyAccessor)
	{
		var binding = new FormNodeBinding<string?>(
			new ValueNodeBinding<string?>(),
			new EmbeddedModelPropertyBinding<TModel, string?, string?>(_instanceNodeName, propertyAccessor)
		);
		return binding;
	}

	/// <summary>
	/// Creates a binding for an <see cref="ITimestampNode"/> using custom getter and setter methods.
	/// </summary>
	/// <param name="getter">The method loading the value from the model.</param>
	/// <param name="setter">The method writing the value to the model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreateGetterSetterBinding(
		Func<TModel, DateTime?> getter,
		Action<TModel, DateTime?> setter
	)
	{
		var binding = new FormNodeBinding<DateTime?>(
			new ValueNodeBinding<DateTime?>(),
			new EmbeddedModelGetterSetterBinding<TModel, DateTime?>(_instanceNodeName, getter, setter)
		);
		return binding;
	}

	/// <summary>
	/// Creates a binding for an <see cref="ITimestampNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreatePropertyBinding(Expression<Func<TModel, DateTime?>> propertyAccessor)
	{
		var binding = new FormNodeBinding<DateTime?>(
			new ValueNodeBinding<DateTime?>(),
			new EmbeddedModelPropertyBinding<TModel, DateTime?, DateTime?>(_instanceNodeName, propertyAccessor)
		);
		return binding;
	}

	/// <summary>
	/// Creates a binding for an <see cref="ITemplateNode"/> using custom getter and setter methods.
	/// </summary>
	/// <param name="getter">The method loading the value from the model.</param>
	/// <param name="setter">The method writing the value to the model.</param>
	/// <param name="emptyValue">The value to write to the model, if the template instance does not yield a model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreateGetterSetterBinding<TInnerModel>(
		Func<TModel, TInnerModel?> getter,
		Action<TModel, TInnerModel?> setter,
		TInnerModel? emptyValue
	)
	{
		var binding = new FormNodeBinding<TInnerModel?>(
			new TemplateNodeBinding<TInnerModel?>(emptyValue),
			new EmbeddedModelGetterSetterBinding<TModel, TInnerModel?>(_instanceNodeName, getter, setter)
		);
		return binding;
	}

	/// <inheritdoc cref="CreateGetterSetterBinding{TInnerModel}(Func{TModel,TInnerModel},Action{TModel,TInnerModel},TInnerModel)"/>
	internal IFormNodeBinding CreateGetterSetterBinding<TInnerModel>(
		Func<TModel, TInnerModel?> getter,
		Action<TModel, TInnerModel?> setter
	)
		where TInnerModel : class => CreateGetterSetterBinding(getter, setter, null);

	/// <inheritdoc cref="CreateGetterSetterBinding{TInnerModel}(Func{TModel,TInnerModel},Action{TModel,TInnerModel},TInnerModel)"/>
	internal IFormNodeBinding CreateGetterSetterBinding<TInnerModel>(
		Func<TModel, TInnerModel?> getter,
		Action<TModel, TInnerModel?> setter
	)
		where TInnerModel : struct => CreateGetterSetterBinding(getter, setter, null);

	/// <summary>
	/// Creates a binding for an <see cref="ITemplateNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <param name="emptyValue">The value to write to the model, if the template instance does not yield a model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreatePropertyBinding<TInnerModel>(
		Expression<Func<TModel, TInnerModel?>> propertyAccessor,
		TInnerModel? emptyValue
	)
	{
		var binding = new FormNodeBinding<TInnerModel?>(
			new TemplateNodeBinding<TInnerModel?>(emptyValue),
			new EmbeddedModelPropertyBinding<TModel, TInnerModel?, TInnerModel?>(_instanceNodeName, propertyAccessor)
		);
		return binding;
	}

	/// <inheritdoc cref="CreatePropertyBinding{TInnerModel}(Expression{Func{TModel,TInnerModel}},TInnerModel)"/>
	internal IFormNodeBinding CreatePropertyBinding<TInnerModel>(
		Func<TModel, TInnerModel?> getter,
		Action<TModel, TInnerModel?> setter
	)
		where TInnerModel : class => CreateGetterSetterBinding(getter, setter, null);

	/// <inheritdoc cref="CreatePropertyBinding{TInnerModel}(Expression{Func{TModel,TInnerModel}},TInnerModel)"/>
	internal IFormNodeBinding CreatePropertyBinding<TInnerModel>(
		Func<TModel, TInnerModel?> getter,
		Action<TModel, TInnerModel?> setter
	)
		where TInnerModel : struct => CreateGetterSetterBinding(getter, setter, null);

	/// <summary>
	/// Creates a binding for an <see cref="ICollectionNode"/> using custom getter and setter methods.
	/// </summary>
	/// <param name="getter">The method loading the value from the model.</param>
	/// <param name="setter">The method writing the value to the model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreateGetterSetterBinding<TItem>(
		Func<TModel, IEnumerable<TItem>> getter,
		Action<TModel, IEnumerable<TItem>> setter
	)
	{
		var binding = new FormNodeBinding<IEnumerable<TItem>>(
			new CollectionNodeBinding<TItem>(),
			new EmbeddedModelGetterSetterBinding<TModel, IEnumerable<TItem>>(_instanceNodeName, getter, setter)
		);
		return binding;
	}

	/// <summary>
	/// Creates a binding for an <see cref="ICollectionNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreatePropertyBinding<TItem>(
		Expression<Func<TModel, IEnumerable<TItem>>> propertyAccessor
	)
	{
		var binding = new FormNodeBinding<IEnumerable<TItem>>(
			new CollectionNodeBinding<TItem>(),
			new EmbeddedModelPropertyBinding<TModel, IEnumerable<TItem>, IEnumerable<TItem>>(
				_instanceNodeName,
				propertyAccessor
			)
		);
		return binding;
	}
}
