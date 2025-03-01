namespace RobinEpple.Common.Forms;

using RobinEpple.Common.Forms.Nodes;

public interface IFormBuilder : INodeBuilder<IFormBuilder>
{
	/// <summary>
	/// A delegate to configure an added form field.
	/// </summary>
	/// <param name="builder">The builder to configure the field.</param>
	public delegate void FieldBuilder(IFieldNodeBuilder builder);

	/// <summary>
	/// A delegate to configure an added form collection.
	/// </summary>
	/// <param name="builder">The builder to configure the collection.</param>
	/// <param name="parentRecursionTemplate">A template that can be used to construct recursive structures by referencing this nodes parent.</param>
	public delegate void CollectionBuilder(ICollectionNodeBuilder builder, IForm parentRecursionTemplate);

	/// <summary>
	/// A delegate to configure an added templated section.
	/// </summary>
	/// <param name="builder">The builder to configure the templated section.</param>
	/// <param name="parentRecursionTemplate">A template that can be used to construct recursive structures by referencing this nodes parent.</param>
	public delegate void TemplatedSectionBuilder(ITemplateNodeBuilder builder, IForm parentRecursionTemplate);

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
	public IFormBuilder WithTextNode(string name, FieldBuilder configure);

	/// <summary>
	/// Adds a <see cref="INumberNode"/> to the form.
	/// </summary>
	/// <param name="name">The name of the node. This should be unique.</param>
	/// <param name="configure">A function to configure the field.</param>
	/// <returns>The form builder for adding more elements or concluding the build process.</returns>
	public IFormBuilder WithNumberNode(string name, FieldBuilder configure);

	/// <summary>
	/// Adds a <see cref="ITimestampNode"/> to the form.
	/// </summary>
	/// <param name="name">The name of the node. This should be unique.</param>
	/// <param name="configure">A function to configure the field.</param>
	/// <returns>The form builder for adding more elements or concluding the build process.</returns>
	public IFormBuilder WithTimestampNode(string name, FieldBuilder configure);

	/// <summary>
	/// Adds a <see cref="IBooleanNode"/> to the form.
	/// </summary>
	/// <param name="name">The name of the node. This should be unique.</param>
	/// <param name="configure">A function to configure the field.</param>
	/// <returns>The form builder for adding more elements or concluding the build process.</returns>
	public IFormBuilder WithBooleanNode(string name, FieldBuilder configure);

	/// <summary>
	/// Adds a <see cref="IFileNode"/> to the form.
	/// </summary>
	/// <param name="name">The name of the node. This should be unique.</param>
	/// <param name="configure">A function to configure the field.</param>
	/// <returns>The form builder for adding more elements or concluding the build process.</returns>
	public IFormBuilder WithFileNode(string name, FieldBuilder configure);

	/// <summary>
	/// Adds a <see cref="ICollectionNode"/> to the form.
	/// </summary>
	/// <param name="name">The name of the node. This should be unique.</param>
	/// <param name="configure">A function to configure the collection.</param>
	/// <returns>The form builder for adding more elements or concluding the build process.</returns>
	public IFormBuilder WithCollectionNode(string name, CollectionBuilder configure);

	/// <summary>
	/// Adds a <see cref="ITemplateNode"/> to the form.
	/// </summary>
	/// <param name="name">The name of the node. This should be unique.</param>
	/// <param name="configure">A function to configure the template section.</param>
	/// <returns>The form builder for adding more elements or concluding the build process.</returns>
	public IFormBuilder WithTemplatedSection(string name, TemplatedSectionBuilder configure);
}
