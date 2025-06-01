namespace RobinEpple.Common.Forms;

using RobinEpple.Common.Forms.Nodes;

public interface IFormBuilder : INodeBuilder<IFormBuilder>
{
	/// <summary>
	/// The list of characters a node name is allowed to consist of.
	/// </summary>
	public const string ValidNameCharacters = "ABCDEFGHIKLMNOPQRSTUVWXYZabcdefghiklmnopqrstuvwxyz0123456789_";

	/// <summary>
	/// A delegate to configure an added form field.
	/// </summary>
	/// <param name="builder">The builder to configure the field.</param>
	public delegate void BooleanFieldBuilder(IBooleanNodeBuilder builder);

	/// <summary>
	/// A delegate to configure an added form field.
	/// </summary>
	/// <param name="builder">The builder to configure the field.</param>
	public delegate void FileFieldBuilder(IFileNodeBuilder builder);

	/// <summary>
	/// A delegate to configure an added form field.
	/// </summary>
	/// <param name="builder">The builder to configure the field.</param>
	public delegate void NumberFieldBuilder(INumberNodeBuilder builder);

	/// <summary>
	/// A delegate to configure an added form field.
	/// </summary>
	/// <param name="builder">The builder to configure the field.</param>
	public delegate void TextFieldBuilder(ITextNodeBuilder builder);

	/// <summary>
	/// A delegate to configure an added form field.
	/// </summary>
	/// <param name="builder">The builder to configure the field.</param>
	public delegate void TimestampFieldBuilder(ITimestampNodeBuilder builder);

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
	/// A delegate to configure an added sub form.
	/// </summary>
	/// <param name="builder">The builder to configure the sub form.</param>
	/// <param name="parentRecursionTemplate">A template that can be used to construct recursive structures by referencing this form as parent.</param>
	public delegate void SubFormBuilder(IFormBuilder builder, IForm parentRecursionTemplate);

	/// <summary>
	/// Finishes the form building process and returns the finished form structure.
	/// </summary>
	/// <returns>The form.</returns>
	public IForm Build();

	/// <summary>
	/// Add a node that was configured externally.
	/// </summary>
	/// <param name="node">The node to add.</param>
	/// <returns>The form builder for adding more elements or concluding the build process.</returns>
	public IFormBuilder WithNode(IFormNode node);

	/// <summary>
	/// Adds a sub form for grouping some nodes.
	/// </summary>
	/// <param name="name">The name of the sub form.</param>
	/// <param name="configure">A function to configure the sub form.</param>
	/// <returns>The form builder for adding more elements or concluding the build process.</returns>
	public IFormBuilder WithSubForm(string name, SubFormBuilder? configure = null);

	/// <summary>
	/// Adds a sub form for grouping some nodes.
	/// </summary>
	/// <param name="subForm">The externally configured sub form.</param>
	/// <returns>The form builder for adding more elements or concluding the build process.</returns>
	public IFormBuilder WithPreConfiguredSubForm(IFormNode subForm);

	/// <summary>
	/// Adds a <see cref="ITextNode"/> to the form.
	/// </summary>
	/// <param name="name">The name of the node. This should be unique.</param>
	/// <param name="configure">A function to configure the field.</param>
	/// <returns>The form builder for adding more elements or concluding the build process.</returns>
	public IFormBuilder WithTextNode(string name, TextFieldBuilder? configure = null);

	/// <summary>
	/// Adds a <see cref="INumberNode"/> to the form.
	/// </summary>
	/// <param name="name">The name of the node. This should be unique.</param>
	/// <param name="configure">A function to configure the field.</param>
	/// <returns>The form builder for adding more elements or concluding the build process.</returns>
	public IFormBuilder WithNumberNode(string name, NumberFieldBuilder? configure = null);

	/// <summary>
	/// Adds a <see cref="ITimestampNode"/> to the form.
	/// </summary>
	/// <param name="name">The name of the node. This should be unique.</param>
	/// <param name="configure">A function to configure the field.</param>
	/// <returns>The form builder for adding more elements or concluding the build process.</returns>
	public IFormBuilder WithTimestampNode(string name, TimestampFieldBuilder? configure = null);

	/// <summary>
	/// Adds a <see cref="IBooleanNode"/> to the form.
	/// </summary>
	/// <param name="name">The name of the node. This should be unique.</param>
	/// <param name="configure">A function to configure the field.</param>
	/// <returns>The form builder for adding more elements or concluding the build process.</returns>
	public IFormBuilder WithBooleanNode(string name, BooleanFieldBuilder? configure = null);

	/// <summary>
	/// Adds a <see cref="IFileNode"/> to the form.
	/// </summary>
	/// <param name="name">The name of the node. This should be unique.</param>
	/// <param name="configure">A function to configure the field.</param>
	/// <returns>The form builder for adding more elements or concluding the build process.</returns>
	public IFormBuilder WithFileNode(string name, FileFieldBuilder? configure = null);

	/// <summary>
	/// Adds a <see cref="ICollectionNode"/> to the form.
	/// </summary>
	/// <param name="name">The name of the node. This should be unique.</param>
	/// <param name="configure">A function to configure the collection.</param>
	/// <returns>The form builder for adding more elements or concluding the build process.</returns>
	public IFormBuilder WithCollectionNode(string name, CollectionBuilder? configure = null);

	/// <summary>
	/// Adds a <see cref="ITemplateNode"/> to the form.
	/// </summary>
	/// <param name="name">The name of the node. This should be unique.</param>
	/// <param name="configure">A function to configure the template section.</param>
	/// <returns>The form builder for adding more elements or concluding the build process.</returns>
	public IFormBuilder WithTemplatedSection(string name, TemplatedSectionBuilder? configure = null);
}
