namespace RobinEpple.Common.Forms;

using RobinEpple.Common.Forms.Nodes;

public interface ICollectionNodeBuilder : INodeBuilder<ICollectionNodeBuilder>
{
	/// <summary>
	/// A delegate to build the inner form of the template.
	/// </summary>
	/// <param name="builder">The builder.</param>
	public delegate void TemplateBuilder(IFormBuilder builder);

	/// <summary>
	/// Adds a template to this collection, that can be instantiated.
	/// </summary>
	/// <param name="name">The name of the template.</param>
	/// <param name="configure">The function to configure the inner workings of the template.</param>
	/// <returns>The collection builder to add further configurations.</returns>
	public ICollectionNodeBuilder UseTemplate(string name, TemplateBuilder configure);

	/// <summary>
	/// Adds a template to this collection, that can be instantiated.
	/// </summary>
	/// <param name="template">The template, that is configured externally.</param>
	/// <returns>The collection builder to add further configurations.</returns>
	public ICollectionNodeBuilder UsePreconfiguredTemplate(IForm template);
}
