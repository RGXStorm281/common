namespace RobinEpple.Common.Forms.Building;

using System.Linq.Expressions;
using RobinEpple.Common.Forms.Binding;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.Nodes.DefaultImplementation;

internal class TemplateNodeBuilder : NodeBuilder<ITemplateNodeBuilder, TemplateNode>, ITemplateNodeBuilder
{
	public TemplateNodeBuilder(TemplateNode node)
		: base(node) { }

	/// <inheritdoc />
	public ITemplateNodeBuilder UsePreConfiguredTemplate(IForm template)
	{
		Node.UseTemplate(template);
		return CastThis();
	}

	/// <inheritdoc />
	public ITemplateNodeBuilder UseTemplate(
		string name,
		ITemplatedNodeBuilder<ITemplateNodeBuilder>.TemplateBuilder? configure = null
	)
	{
		var builder = new FormBuilder(name, Node);
		configure?.Invoke(builder);
		Node.UseTemplate(builder.Form);
		return CastThis();
	}

	/// <inheritdoc />
	public ITemplateNodeBuilder UseGetterSetterBinding<TModel>(
		Func<TModel?> getter,
		Action<TModel?> setter,
		TModel? emptyValue
	)
	{
		var binding = new FormNodeBinding<TModel?>(
			new TemplateNodeBinding<TModel?>(emptyValue),
			new GetterSetterBinding<TModel?>(getter, setter)
		);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	public ITemplateNodeBuilder UseGetterSetterBinding<TModel>(Func<TModel?> getter, Action<TModel?> setter)
		where TModel : class => UseGetterSetterBinding(getter, setter, null);

	/// <inheritdoc />
	public ITemplateNodeBuilder UseGetterSetterBinding<TModel>(Func<TModel?> getter, Action<TModel?> setter)
		where TModel : struct => UseGetterSetterBinding(getter, setter, null);

	/// <inheritdoc />
	public ITemplateNodeBuilder UsePropertyBinding<TModel>(
		Expression<Func<TModel?>> propertyAccessor,
		TModel? emptyValue
	)
	{
		var binding = new FormNodeBinding<TModel?>(
			new TemplateNodeBinding<TModel?>(emptyValue),
			new PropertyBinding<TModel?, TModel?>(propertyAccessor)
		);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	public ITemplateNodeBuilder UsePropertyBinding<TModel>(Expression<Func<TModel?>> propertyAccessor)
		where TModel : class => UsePropertyBinding(propertyAccessor, null);

	/// <inheritdoc />
	public ITemplateNodeBuilder UsePropertyBinding<TModel>(Expression<Func<TModel?>> propertyAccessor)
		where TModel : struct => UsePropertyBinding(propertyAccessor, null);

	/// <inheritdoc />
	public ITemplateNodeBuilder UseEmbeddedModelGetterSetterBinding<TModel, TInnerModel>(
		EmbeddedModelReference<TModel> modelReference,
		Func<TModel, TInnerModel?> getter,
		Action<TModel, TInnerModel?> setter,
		TInnerModel? emptyValue
	)
	{
		var binding = modelReference.CreateTemplateGetterSetterBinding(getter, setter, emptyValue);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	public ITemplateNodeBuilder UseEmbeddedModelGetterSetterBinding<TModel, TInnerModel>(
		EmbeddedModelReference<TModel> modelReference,
		Func<TModel, TInnerModel?> getter,
		Action<TModel, TInnerModel?> setter
	)
		where TInnerModel : class => UseEmbeddedModelGetterSetterBinding(modelReference, getter, setter, null);

	/// <inheritdoc />
	public ITemplateNodeBuilder UseEmbeddedModelGetterSetterBinding<TModel, TInnerModel>(
		EmbeddedModelReference<TModel> modelReference,
		Func<TModel, TInnerModel?> getter,
		Action<TModel, TInnerModel?> setter
	)
		where TInnerModel : struct => UseEmbeddedModelGetterSetterBinding(modelReference, getter, setter, null);

	/// <inheritdoc />
	public ITemplateNodeBuilder UseEmbeddedModelPropertyBinding<TModel, TInnerModel>(
		EmbeddedModelReference<TModel> modelReference,
		Expression<Func<TModel, TInnerModel?>> propertyAccessor,
		TInnerModel? emptyItem
	)
	{
		var binding = modelReference.CreateTemplatePropertyBinding(propertyAccessor, emptyItem);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	public ITemplateNodeBuilder UseEmbeddedModelPropertyBinding<TModel, TInnerModel>(
		EmbeddedModelReference<TModel> modelReference,
		Expression<Func<TModel, TInnerModel?>> propertyAccessor
	)
		where TInnerModel : class => UseEmbeddedModelPropertyBinding(modelReference, propertyAccessor, null);

	/// <inheritdoc />
	public ITemplateNodeBuilder UseEmbeddedModelPropertyBinding<TModel, TInnerModel>(
		EmbeddedModelReference<TModel> modelReference,
		Expression<Func<TModel, TInnerModel?>> propertyAccessor
	)
		where TInnerModel : struct => UseEmbeddedModelPropertyBinding(modelReference, propertyAccessor, null);

	/// <inheritdoc />
	protected override TemplateNodeBuilder CastThis() => this;

	public ITemplateNodeBuilder UseFormatter(IValueFormatter formatter)
	{
		Node.UseFormatter(formatter);
		return this;
	}
}
