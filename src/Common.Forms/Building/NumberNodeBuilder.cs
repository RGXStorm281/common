namespace RobinEpple.Common.Forms.Building;

using System.Linq.Expressions;
using RobinEpple.Common.Forms.Binding;
using RobinEpple.Common.Forms.Nodes.DefaultImplementation;
using RobinEpple.Common.Forms.SelectLists;
using RobinEpple.Common.Forms.Validation;

internal class NumberNodeBuilder : FieldNodeBuilder<INumberNodeBuilder, NumberNode>, INumberNodeBuilder
{
	public NumberNodeBuilder(NumberNode node)
		: base(node) { }

	/// <inheritdoc />
	public INumberNodeBuilder UseDefaultValue(decimal? defaultValue)
	{
		Node.Value = defaultValue;
		Node.ReplaceDefaultValue(defaultValue);
		return CastThis();
	}

	/// <inheritdoc />
	public INumberNodeBuilder UseGetterSetterBinding(Func<decimal?> getter, Action<decimal?> setter)
	{
		var binding = new FormNodeBinding<decimal?>(
			new ValueNodeBinding<decimal?>(),
			new GetterSetterBinding<decimal?>(getter, setter)
		);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	public INumberNodeBuilder UsePropertyBinding(Expression<Func<decimal?>> propertyAccessor)
	{
		var binding = new FormNodeBinding<decimal?>(
			new ValueNodeBinding<decimal?>(),
			new PropertyBinding<decimal?, decimal?>(propertyAccessor)
		);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	public INumberNodeBuilder UsePropertyBinding(Expression<Func<double?>> propertyAccessor)
	{
		var binding = new FormNodeBinding<decimal?>(
			new ValueNodeBinding<decimal?>(),
			new PropertyBinding<decimal?, double?>(propertyAccessor)
		);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	public INumberNodeBuilder UsePropertyBinding(Expression<Func<float?>> propertyAccessor)
	{
		var binding = new FormNodeBinding<decimal?>(
			new ValueNodeBinding<decimal?>(),
			new PropertyBinding<decimal?, float?>(propertyAccessor)
		);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	public INumberNodeBuilder UsePropertyBinding(Expression<Func<long?>> propertyAccessor)
	{
		var binding = new FormNodeBinding<decimal?>(
			new ValueNodeBinding<decimal?>(),
			new PropertyBinding<decimal?, long?>(propertyAccessor)
		);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	public INumberNodeBuilder UsePropertyBinding(Expression<Func<int?>> propertyAccessor)
	{
		var binding = new FormNodeBinding<decimal?>(
			new ValueNodeBinding<decimal?>(),
			new PropertyBinding<decimal?, int?>(propertyAccessor)
		);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	public INumberNodeBuilder UseEmbeddedModelGetterSetterBinding<TModel>(
		EmbeddedModelReference<TModel> modelReference,
		Func<TModel, decimal?> getter,
		Action<TModel, decimal?> setter
	)
	{
		var binding = modelReference.CreateNumberGetterSetterBinding(getter, setter);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	public INumberNodeBuilder UseEmbeddedModelPropertyBinding<TModel>(
		EmbeddedModelReference<TModel> modelReference,
		Expression<Func<TModel, decimal?>> propertyAccessor
	)
	{
		var binding = modelReference.CreateNumberPropertyBinding(propertyAccessor);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	public INumberNodeBuilder UseEmbeddedModelPropertyBinding<TModel>(
		EmbeddedModelReference<TModel> modelReference,
		Expression<Func<TModel, double?>> propertyAccessor
	)
	{
		var binding = modelReference.CreateNumberPropertyBinding(propertyAccessor);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	public INumberNodeBuilder UseEmbeddedModelPropertyBinding<TModel>(
		EmbeddedModelReference<TModel> modelReference,
		Expression<Func<TModel, float?>> propertyAccessor
	)
	{
		var binding = modelReference.CreateNumberPropertyBinding(propertyAccessor);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	public INumberNodeBuilder UseEmbeddedModelPropertyBinding<TModel>(
		EmbeddedModelReference<TModel> modelReference,
		Expression<Func<TModel, long?>> propertyAccessor
	)
	{
		var binding = modelReference.CreateNumberPropertyBinding(propertyAccessor);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	public INumberNodeBuilder UseEmbeddedModelPropertyBinding<TModel>(
		EmbeddedModelReference<TModel> modelReference,
		Expression<Func<TModel, int?>> propertyAccessor
	)
	{
		var binding = modelReference.CreateNumberPropertyBinding(propertyAccessor);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	protected override NumberNodeBuilder CastThis() => this;

	/// <inheritdoc />
	public INumberNodeBuilder UseSelectList(
		ISelectListSource<decimal?> source,
		bool validate,
		string? errorMessageTemplate = null
	)
	{
		Node.UseSelectList(source);
		if (validate)
		{
			Node.UseValidator(new SelectListValidator<decimal?>(errorMessageTemplate));
		}
		return this;
	}
}
