namespace RobinEpple.Common.Forms.Building;

using System;
using System.Linq.Expressions;
using RobinEpple.Common.Forms.Binding;
using RobinEpple.Common.Forms.Nodes.DefaultImplementation;
using RobinEpple.Common.Forms.SelectLists;
using RobinEpple.Common.Forms.Validation;

internal class TimestampNodeBuilder : FieldNodeBuilder<ITimestampNodeBuilder, TimestampNode>, ITimestampNodeBuilder
{
	public TimestampNodeBuilder(TimestampNode node)
		: base(node) { }

	/// <inheritdoc />
	public ITimestampNodeBuilder UseDefaultValue(DateTime? defaultValue)
	{
		Node.Value = defaultValue;
		Node.ReplaceDefaultValue(defaultValue);
		return CastThis();
	}

	/// <inheritdoc />
	public ITimestampNodeBuilder UseGetterSetterBinding(Func<DateTime?> getter, Action<DateTime?> setter)
	{
		var binding = new FormNodeBinding<DateTime?>(
			new ValueNodeBinding<DateTime?>(),
			new GetterSetterBinding<DateTime?>(getter, setter)
		);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	public ITimestampNodeBuilder UsePropertyBinding(Expression<Func<DateTime?>> propertyAccessor)
	{
		var binding = new FormNodeBinding<DateTime?>(
			new ValueNodeBinding<DateTime?>(),
			new PropertyBinding<DateTime?, DateTime?>(propertyAccessor)
		);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	public ITimestampNodeBuilder UseEmbeddedModelGetterSetterBinding<TModel>(
		EmbeddedModelReference<TModel> modelReference,
		Func<TModel, DateTime?> getter,
		Action<TModel, DateTime?> setter
	)
	{
		var binding = modelReference.CreateTimestampGetterSetterBinding(getter, setter);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	public ITimestampNodeBuilder UseEmbeddedModelPropertyBinding<TModel>(
		EmbeddedModelReference<TModel> modelReference,
		Expression<Func<TModel, DateTime?>> propertyAccessor
	)
	{
		var binding = modelReference.CreateTimestampPropertyBinding(propertyAccessor);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	protected override TimestampNodeBuilder CastThis() => this;

	/// <inheritdoc />
	public ITimestampNodeBuilder UseSelectList(
		ISelectListSource<DateTime?> source,
		bool validate,
		string? errorMessageTemplate = null
	)
	{
		Node.UseSelectList(source);
		if (validate)
		{
			Node.UseValidator(new SelectListValidator<DateTime?>(errorMessageTemplate));
		}
		return this;
	}

	/// <inheritdoc />
	public ITimestampNodeBuilder UseSelectList(
		IEnumerable<DateTime?> values,
		bool validate,
		string? errorMessageTemplate = null
	) => UseSelectList(ISelectListSource<DateTime?>.ForValues(values), validate, errorMessageTemplate);

	/// <inheritdoc />
	public ITimestampNodeBuilder UseSelectList(
		IEnumerable<(DateTime? Value, string Label)> labelledValues,
		bool validate,
		string? errorMessageTemplate = null
	) => UseSelectList(ISelectListSource<DateTime?>.ForLabelledValues(labelledValues), validate, errorMessageTemplate);
}
