namespace RobinEpple.Common.Forms;

using RobinEpple.Common.Forms.Binding;
using RobinEpple.Common.Forms.Nodes;

public interface ITemplateNodeBuilder : INodeBuilder<ITemplateNodeBuilder>
{
	/// <summary>
	/// A delegate to build the inner form of the template.
	/// </summary>
	/// <param name="builder">The builder.</param>
	public delegate void TemplateBuilder(IFormBuilder builder);

	/// <summary>
	/// Configures the templated section to bind to some property.<br/>
	/// Only one can be used on a single collection.
	/// </summary>
	/// <param name="binding">The binding to pull and push changes from and to some model.</param>
	/// <returns>The template node builder to add further configurations.</returns>
	public ITemplateNodeBuilder UseBinding(ITemplateNodeBinding binding);

	/// <summary>
	/// Adds a template to this collection, that can be instantiated.
	/// </summary>
	/// <param name="name">The name of the template.</param>
	/// <param name="configure">The function to configure the inner workings of the template.</param>
	/// <returns>The collection builder to add further configurations.</returns>
	public ITemplateNodeBuilder UseTemplate(string name, TemplateBuilder configure);

	/// <summary>
	/// Adds a template to this collection, that can be instantiated.
	/// </summary>
	/// <param name="template">The template, that is configured externally.</param>
	/// <returns>The collection builder to add further configurations.</returns>
	public ITemplateNodeBuilder UsePreconfiguredTemplate(IForm template);
}
