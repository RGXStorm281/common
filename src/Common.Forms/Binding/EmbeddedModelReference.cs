namespace RobinEpple.Common.Forms.Binding;

using System.Linq.Expressions;
using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// This factory can be used to create model bindings for instance models in parent nodes.
/// </summary>
/// <typeparam name="TModel">The type of the instance model.</typeparam>
public class EmbeddedModelReference<TModel>
{
	/// <summary>
	/// The name of the node that contains the embedded model.
	/// </summary>
	public string ModelNodeName { get; }

	/// <inheritdoc cref="EmbeddedModelReference{TModel}"/>
	public EmbeddedModelReference(string modelNodeName)
	{
		ModelNodeName = modelNodeName;
	}

	/// <summary>
	/// Creates a binding for an <see cref="IBooleanNode"/> using custom getter and setter methods.
	/// </summary>
	/// <param name="getter">The method loading the value from the model.</param>
	/// <param name="setter">The method writing the value to the model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreateBooleanGetterSetterBinding(Func<TModel, bool?> getter, Action<TModel, bool?> setter)
	{
		var binding = new FormNodeBinding<bool?>(
			new ValueNodeBinding<bool?>(),
			new EmbeddedModelGetterSetterBinding<TModel, bool?>(ModelNodeName, getter, setter)
		);
		return binding;
	}

	/// <summary>
	/// Creates a binding for an <see cref="IBooleanNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreateBooleanPropertyBinding(Expression<Func<TModel, bool?>> propertyAccessor)
	{
		var binding = new FormNodeBinding<bool?>(
			new ValueNodeBinding<bool?>(),
			new EmbeddedModelPropertyBinding<TModel, bool?, bool?>(ModelNodeName, propertyAccessor)
		);
		return binding;
	}

	/// <summary>
	/// Creates a binding for an <see cref="IFileNode"/> using custom getter and setter methods.
	/// </summary>
	/// <param name="getter">The method loading the value from the model.</param>
	/// <param name="setter">The method writing the value to the model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreateFileGetterSetterBinding(
		Func<TModel, FileValue> getter,
		Action<TModel, FileValue> setter
	)
	{
		var binding = new FormNodeBinding<FileValue>(
			new ValueNodeBinding<FileValue>(),
			new EmbeddedModelGetterSetterBinding<TModel, FileValue>(ModelNodeName, getter, setter)
		);
		return binding;
	}

	/// <summary>
	/// Creates a binding for an <see cref="IFileNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreateFilePropertyBinding(Expression<Func<TModel, FileValue>> propertyAccessor)
	{
		var binding = new FormNodeBinding<FileValue>(
			new ValueNodeBinding<FileValue>(),
			new EmbeddedModelPropertyBinding<TModel, FileValue, FileValue>(ModelNodeName, propertyAccessor)
		);
		return binding;
	}

	/// <summary>
	/// Creates a binding for an <see cref="INumberNode"/> using custom getter and setter methods.
	/// </summary>
	/// <param name="getter">The method loading the value from the model.</param>
	/// <param name="setter">The method writing the value to the model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreateNumberGetterSetterBinding(
		Func<TModel, decimal?> getter,
		Action<TModel, decimal?> setter
	)
	{
		var binding = new FormNodeBinding<decimal?>(
			new ValueNodeBinding<decimal?>(),
			new EmbeddedModelGetterSetterBinding<TModel, decimal?>(ModelNodeName, getter, setter)
		);
		return binding;
	}

	/// <summary>
	/// Creates a binding for an <see cref="INumberNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreateNumberPropertyBinding(Expression<Func<TModel, decimal?>> propertyAccessor)
	{
		var binding = new FormNodeBinding<decimal?>(
			new ValueNodeBinding<decimal?>(),
			new EmbeddedModelPropertyBinding<TModel, decimal?, decimal?>(ModelNodeName, propertyAccessor)
		);
		return binding;
	}

	/// <summary>
	/// Creates a binding for an <see cref="INumberNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreateNumberPropertyBinding(Expression<Func<TModel, double?>> propertyAccessor)
	{
		var binding = new FormNodeBinding<decimal?>(
			new ValueNodeBinding<decimal?>(),
			new EmbeddedModelPropertyBinding<TModel, decimal?, double?>(ModelNodeName, propertyAccessor)
		);
		return binding;
	}

	/// <summary>
	/// Creates a binding for an <see cref="INumberNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreateNumberPropertyBinding(Expression<Func<TModel, float?>> propertyAccessor)
	{
		var binding = new FormNodeBinding<decimal?>(
			new ValueNodeBinding<decimal?>(),
			new EmbeddedModelPropertyBinding<TModel, decimal?, float?>(ModelNodeName, propertyAccessor)
		);
		return binding;
	}

	/// <summary>
	/// Creates a binding for an <see cref="INumberNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreateNumberPropertyBinding(Expression<Func<TModel, long?>> propertyAccessor)
	{
		var binding = new FormNodeBinding<decimal?>(
			new ValueNodeBinding<decimal?>(),
			new EmbeddedModelPropertyBinding<TModel, decimal?, long?>(ModelNodeName, propertyAccessor)
		);
		return binding;
	}

	/// <summary>
	/// Creates a binding for an <see cref="INumberNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreateNumberPropertyBinding(Expression<Func<TModel, int?>> propertyAccessor)
	{
		var binding = new FormNodeBinding<decimal?>(
			new ValueNodeBinding<decimal?>(),
			new EmbeddedModelPropertyBinding<TModel, decimal?, int?>(ModelNodeName, propertyAccessor)
		);
		return binding;
	}

	/// <summary>
	/// Creates a binding for an <see cref="ITextNode"/> using custom getter and setter methods.
	/// </summary>
	/// <param name="getter">The method loading the value from the model.</param>
	/// <param name="setter">The method writing the value to the model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreateTextGetterSetterBinding(
		Func<TModel, string?> getter,
		Action<TModel, string?> setter
	)
	{
		var binding = new FormNodeBinding<string?>(
			new ValueNodeBinding<string?>(),
			new EmbeddedModelGetterSetterBinding<TModel, string?>(ModelNodeName, getter, setter)
		);
		return binding;
	}

	/// <summary>
	/// Creates a binding for an <see cref="ITextNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreateTextPropertyBinding(Expression<Func<TModel, string?>> propertyAccessor)
	{
		var binding = new FormNodeBinding<string?>(
			new ValueNodeBinding<string?>(),
			new EmbeddedModelPropertyBinding<TModel, string?, string?>(ModelNodeName, propertyAccessor)
		);
		return binding;
	}

	/// <summary>
	/// Creates a binding for an <see cref="ITimestampNode"/> using custom getter and setter methods.
	/// </summary>
	/// <param name="getter">The method loading the value from the model.</param>
	/// <param name="setter">The method writing the value to the model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreateTimestampGetterSetterBinding(
		Func<TModel, DateTime?> getter,
		Action<TModel, DateTime?> setter
	)
	{
		var binding = new FormNodeBinding<DateTime?>(
			new ValueNodeBinding<DateTime?>(),
			new EmbeddedModelGetterSetterBinding<TModel, DateTime?>(ModelNodeName, getter, setter)
		);
		return binding;
	}

	/// <summary>
	/// Creates a binding for an <see cref="ITimestampNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreateTimestampPropertyBinding(Expression<Func<TModel, DateTime?>> propertyAccessor)
	{
		var binding = new FormNodeBinding<DateTime?>(
			new ValueNodeBinding<DateTime?>(),
			new EmbeddedModelPropertyBinding<TModel, DateTime?, DateTime?>(ModelNodeName, propertyAccessor)
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
	internal IFormNodeBinding CreateTemplateGetterSetterBinding<TInnerModel>(
		Func<TModel, TInnerModel?> getter,
		Action<TModel, TInnerModel?> setter,
		TInnerModel? emptyValue
	)
	{
		var binding = new FormNodeBinding<TInnerModel?>(
			new TemplateNodeBinding<TInnerModel?>(emptyValue),
			new EmbeddedModelGetterSetterBinding<TModel, TInnerModel?>(ModelNodeName, getter, setter)
		);
		return binding;
	}

	/// <summary>
	/// Creates a binding for an <see cref="ITemplateNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <param name="emptyValue">The value to write to the model, if the template instance does not yield a model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreateTemplatePropertyBinding<TInnerModel>(
		Expression<Func<TModel, TInnerModel?>> propertyAccessor,
		TInnerModel? emptyValue
	)
	{
		var binding = new FormNodeBinding<TInnerModel?>(
			new TemplateNodeBinding<TInnerModel?>(emptyValue),
			new EmbeddedModelPropertyBinding<TModel, TInnerModel?, TInnerModel?>(ModelNodeName, propertyAccessor)
		);
		return binding;
	}

	/// <summary>
	/// Creates a binding for an <see cref="ICollectionNode"/> using custom getter and setter methods.
	/// </summary>
	/// <param name="getter">The method loading the value from the model.</param>
	/// <param name="setter">The method writing the value to the model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreateCollectionGetterSetterBinding<TItem>(
		Func<TModel, IEnumerable<TItem>> getter,
		Action<TModel, IEnumerable<TItem>> setter
	)
	{
		var binding = new FormNodeBinding<IEnumerable<TItem>>(
			new CollectionNodeBinding<TItem>(),
			new EmbeddedModelGetterSetterBinding<TModel, IEnumerable<TItem>>(ModelNodeName, getter, setter)
		);
		return binding;
	}

	/// <summary>
	/// Creates a binding for an <see cref="ICollectionNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The binding.</returns>
	internal IFormNodeBinding CreateCollectionPropertyBinding<TItem>(
		Expression<Func<TModel, IEnumerable<TItem>>> propertyAccessor
	)
	{
		var binding = new FormNodeBinding<IEnumerable<TItem>>(
			new CollectionNodeBinding<TItem>(),
			new EmbeddedModelPropertyBinding<TModel, IEnumerable<TItem>, IEnumerable<TItem>>(
				ModelNodeName,
				propertyAccessor
			)
		);
		return binding;
	}
}
