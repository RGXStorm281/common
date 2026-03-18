namespace RobinEpple.Common.Forms.Building;

using System.Linq.Expressions;
using RobinEpple.Common.Forms.Binding;
using RobinEpple.Common.Forms.Nodes.DefaultImplementation;
using RobinEpple.Common.Forms.SelectLists;
using RobinEpple.Common.Forms.Validation;

internal class BooleanNodeBuilder : FieldNodeBuilder<IBooleanNodeBuilder, BooleanNode>, IBooleanNodeBuilder
{
	public BooleanNodeBuilder(BooleanNode node)
		: base(node) { }

	/// <inheritdoc />
	public IBooleanNodeBuilder UseDefaultValue(bool? defaultValue)
	{
		Node.Value = defaultValue;
		Node.ReplaceDefaultValue(defaultValue);
		return this;
	}

	/// <inheritdoc />
	public IBooleanNodeBuilder UseGetterSetterBinding(Func<bool?> getter, Action<bool?> setter)
	{
		var binding = new FormNodeBinding<bool?>(
			new ValueNodeBinding<bool?>(),
			new GetterSetterBinding<bool?>(getter, setter)
		);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	public IBooleanNodeBuilder UsePropertyBinding(Expression<Func<bool?>> propertyAccessor)
	{
		var binding = new FormNodeBinding<bool?>(
			new ValueNodeBinding<bool?>(),
			new PropertyBinding<bool?, bool?>(propertyAccessor)
		);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	public IBooleanNodeBuilder UseEmbeddedModelGetterSetterBinding<TModel>(
		EmbeddedModelReference<TModel> modelReference,
		Func<TModel, bool?> getter,
		Action<TModel, bool?> setter
	)
	{
		var binding = modelReference.CreateBooleanGetterSetterBinding(getter, setter);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	public IBooleanNodeBuilder UseEmbeddedModelPropertyBinding<TModel>(
		EmbeddedModelReference<TModel> modelReference,
		Expression<Func<TModel, bool?>> propertyAccessor
	)
	{
		var binding = modelReference.CreateBooleanPropertyBinding(propertyAccessor);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	protected override BooleanNodeBuilder CastThis() => this;

	/// <inheritdoc />
	public IBooleanNodeBuilder UseSelectList(
		ISelectListSource<bool?> source,
		bool validate,
		string? errorMessageTemplate = null
	)
	{
		Node.UseSelectList(source);
		if (validate)
		{
			Node.UseValidator(new SelectListValidator<bool?>(errorMessageTemplate));
		}
		return this;
	}
}
