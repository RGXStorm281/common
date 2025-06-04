namespace RobinEpple.Common.Forms.Building;

using System.Linq.Expressions;
using RobinEpple.Common.Forms.Binding;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.Nodes.DefaultImplementation;

internal class CollectionNodeBuilder : NodeBuilder<ICollectionNodeBuilder, CollectionNode>, ICollectionNodeBuilder
{
	public CollectionNodeBuilder(CollectionNode node)
		: base(node) { }

	/// <inheritdoc />
	public ICollectionNodeBuilder UsePreConfiguredTemplate(IForm template)
	{
		Node.UseTemplate(template);
		return CastThis();
	}

	/// <inheritdoc />
	public ICollectionNodeBuilder UseTemplate(
		string name,
		ITemplatedNodeBuilder<ICollectionNodeBuilder>.TemplateBuilder? configure = null
	)
	{
		var builder = new FormBuilder(name, Node);
		configure?.Invoke(builder);
		Node.UseTemplate(builder.Form);
		return CastThis();
	}

	/// <inheritdoc />
	public ICollectionNodeBuilder UseGetterSetterBinding<TItem>(
		Func<IEnumerable<TItem>> getter,
		Action<IEnumerable<TItem>> setter
	)
	{
		var binding = new FormNodeBinding<IEnumerable<TItem>>(
			new CollectionNodeBinding<TItem>(),
			new GetterSetterBinding<IEnumerable<TItem>>(getter, setter)
		);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	public ICollectionNodeBuilder UsePropertyBinding<TItem>(Expression<Func<IEnumerable<TItem>>> propertyAccessor)
	{
		var binding = new FormNodeBinding<IEnumerable<TItem>>(
			new CollectionNodeBinding<TItem>(),
			new PropertyBinding<IEnumerable<TItem>, IEnumerable<TItem>>(propertyAccessor)
		);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	public ICollectionNodeBuilder UseEmbeddedModelGetterSetterBinding<TModel, TItem>(
		EmbeddedModelReference<TModel> modelReference,
		Func<TModel, IEnumerable<TItem>> getter,
		Action<TModel, IEnumerable<TItem>> setter
	)
	{
		var binding = modelReference.CreateCollectionGetterSetterBinding(getter, setter);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	public ICollectionNodeBuilder UseEmbeddedModelPropertyBinding<TModel, TItem>(
		EmbeddedModelReference<TModel> modelReference,
		Expression<Func<TModel, IEnumerable<TItem>>> propertyAccessor
	)
	{
		var binding = modelReference.CreateCollectionPropertyBinding(propertyAccessor);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	protected override CollectionNodeBuilder CastThis() => this;
}
