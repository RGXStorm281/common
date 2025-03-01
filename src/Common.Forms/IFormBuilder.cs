namespace RobinEpple.Common.Forms;

using RobinEpple.Common.Forms.Nodes;

public interface IFormBuilder
{
	/// <summary>
	/// A delegate to configure an added form field.
	/// </summary>
	/// <param name="builder">The builder to configure the field.</param>
	public delegate void FieldBuilder(IFieldNodeBuilder builder);

	/// <summary>
	/// A delegate to configure an added form collection.
	/// </summary>
	/// <param name="builder">The builder to configure the field.</param>
	/// <param name="parentRecursionTemplate">A template that can be used to construct recursive structures by using this nodes parent.</param>
	public delegate void CollectionBuilder(ICollectionNodeBuilder builder, IForm parentRecursionTemplate);

	/// <summary>
	/// Finishes the form building process and returns the finished form structure.
	/// </summary>
	/// <returns>The form.</returns>
	public IForm Build();

	/// <summary>
	/// Adds a <see cref="ITextNode"/> to the form.
	/// </summary>
	/// <param name="name">The name of the node. This should be unique.</param>
	/// <param name="configure">A function to configure the field.</param>
	/// <returns>The form builder for adding more elements or concluding the build process.</returns>
	public IFormBuilder AddTextNode(string name, FieldBuilder configure);

	/// <summary>
	/// Adds a <see cref="INumberNode"/> to the form.
	/// </summary>
	/// <param name="name">The name of the node. This should be unique.</param>
	/// <param name="configure">A function to configure the field.</param>
	/// <returns>The form builder for adding more elements or concluding the build process.</returns>
	public IFormBuilder AddNumberNode(string name, FieldBuilder configure);

	/// <summary>
	/// Adds a <see cref="ITimestampNode"/> to the form.
	/// </summary>
	/// <param name="name">The name of the node. This should be unique.</param>
	/// <param name="configure">A function to configure the field.</param>
	/// <returns>The form builder for adding more elements or concluding the build process.</returns>
	public IFormBuilder AddTimestampNode(string name, FieldBuilder configure);

	/// <summary>
	/// Adds a <see cref="IBooleanNode"/> to the form.
	/// </summary>
	/// <param name="name">The name of the node. This should be unique.</param>
	/// <param name="configure">A function to configure the field.</param>
	/// <returns>The form builder for adding more elements or concluding the build process.</returns>
	public IFormBuilder AddBooleanNode(string name, FieldBuilder configure);

	/// <summary>
	/// Adds a <see cref="IFileNode"/> to the form.
	/// </summary>
	/// <param name="name">The name of the node. This should be unique.</param>
	/// <param name="configure">A function to configure the field.</param>
	/// <returns>The form builder for adding more elements or concluding the build process.</returns>
	public IFormBuilder AddFileNode(string name, FieldBuilder configure);

	/// <summary>
	/// Adds a <see cref="ICollectionNode"/> to the form.
	/// </summary>
	/// <param name="name">The name of the node. This should be unique.</param>
	/// <param name="configure">A function to configure the collection.</param>
	/// <returns>The form builder for adding more elements or concluding the build process.</returns>
	public IFormBuilder AddCollectionNode(string name, CollectionBuilder configure);
}
