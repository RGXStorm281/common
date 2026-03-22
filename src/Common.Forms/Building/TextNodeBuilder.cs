namespace RobinEpple.Common.Forms.Building;

using System.Linq.Expressions;
using RobinEpple.Common.Forms.Binding;
using RobinEpple.Common.Forms.Nodes.DefaultImplementation;
using RobinEpple.Common.Forms.SelectLists;
using RobinEpple.Common.Forms.Validation;

internal class TextNodeBuilder : FieldNodeBuilder<ITextNodeBuilder, TextNode>, ITextNodeBuilder
{
	public TextNodeBuilder(TextNode node)
		: base(node) { }

	/// <inheritdoc />
	public ITextNodeBuilder UseDefaultValue(string? defaultValue)
	{
		Node.Value = defaultValue;
		Node.ReplaceDefaultValue(defaultValue);
		return CastThis();
	}

	/// <inheritdoc />
	public ITextNodeBuilder UseGetterSetterBinding(Func<string?> getter, Action<string?> setter)
	{
		var binding = new FormNodeBinding<string?>(
			new ValueNodeBinding<string?>(),
			new GetterSetterBinding<string?>(getter, setter)
		);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	public ITextNodeBuilder UsePropertyBinding(Expression<Func<string?>> propertyAccessor)
	{
		var binding = new FormNodeBinding<string?>(
			new ValueNodeBinding<string?>(),
			new PropertyBinding<string?, string?>(propertyAccessor)
		);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	public ITextNodeBuilder UseEmbeddedModelGetterSetterBinding<TModel>(
		EmbeddedModelReference<TModel> modelReference,
		Func<TModel, string?> getter,
		Action<TModel, string?> setter
	)
	{
		var binding = modelReference.CreateTextGetterSetterBinding(getter, setter);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	public ITextNodeBuilder UseEmbeddedModelPropertyBinding<TModel>(
		EmbeddedModelReference<TModel> modelReference,
		Expression<Func<TModel, string?>> propertyAccessor
	)
	{
		var binding = modelReference.CreateTextPropertyBinding(propertyAccessor);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	protected override TextNodeBuilder CastThis() => this;

	/// <inheritdoc />
	public ITextNodeBuilder UseSelectList(
		ISelectListSource<string?> source,
		bool validate,
		string? errorMessageTemplate = null
	)
	{
		Node.UseSelectList(source);
		if (validate)
		{
			Node.UseValidator(new SelectListValidator<string?>(errorMessageTemplate));
		}
		return this;
	}

	/// <inheritdoc />
	public ITextNodeBuilder UseSelectList(
		IEnumerable<string?> values,
		bool validate,
		string? errorMessageTemplate = null
	) => UseSelectList(ISelectListSource<string?>.ForValues(values), validate, errorMessageTemplate);

	/// <inheritdoc />
	public ITextNodeBuilder UseSelectList(
		IEnumerable<(string? Value, string Label)> labelledValues,
		bool validate,
		string? errorMessageTemplate = null
	) => UseSelectList(ISelectListSource<string?>.ForLabelledValues(labelledValues), validate, errorMessageTemplate);
}
