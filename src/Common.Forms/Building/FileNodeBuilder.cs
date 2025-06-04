namespace RobinEpple.Common.Forms.Building;

using System.Linq.Expressions;
using RobinEpple.Common.Forms.Binding;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.Nodes.DefaultImplementation;

internal class FileNodeBuilder : FieldNodeBuilder<IFileNodeBuilder, FileNode>, IFileNodeBuilder
{
	public FileNodeBuilder(FileNode node)
		: base(node) { }

	/// <inheritdoc />
	public IFileNodeBuilder UseDefaultValue(FileValue defaultValue)
	{
		Node.Value = defaultValue;
		Node.ReplaceDefaultValue(defaultValue);
		return CastThis();
	}

	/// <inheritdoc />
	public IFileNodeBuilder UseGetterSetterBinding(Func<FileValue> getter, Action<FileValue> setter)
	{
		var binding = new FormNodeBinding<FileValue>(
			new ValueNodeBinding<FileValue>(),
			new GetterSetterBinding<FileValue>(getter, setter)
		);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	public IFileNodeBuilder UsePropertyBinding(Expression<Func<FileValue>> propertyAccessor)
	{
		var binding = new FormNodeBinding<FileValue>(
			new ValueNodeBinding<FileValue>(),
			new PropertyBinding<FileValue, FileValue>(propertyAccessor)
		);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	public IFileNodeBuilder UseEmbeddedModelGetterSetterBinding<TModel>(
		EmbeddedModelReference<TModel> modelReference,
		Func<TModel, FileValue> getter,
		Action<TModel, FileValue> setter
	)
	{
		var binding = modelReference.CreateFileGetterSetterBinding(getter, setter);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	public IFileNodeBuilder UseEmbeddedModelPropertyBinding<TModel>(
		EmbeddedModelReference<TModel> modelReference,
		Expression<Func<TModel, FileValue>> propertyAccessor
	)
	{
		var binding = modelReference.CreateFilePropertyBinding(propertyAccessor);
		UseBinding(binding);
		return this;
	}

	/// <inheritdoc />
	protected override FileNodeBuilder CastThis() => this;
}
