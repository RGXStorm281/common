namespace RobinEpple.Common.Forms;

using RobinEpple.Common.Forms.Binding;

public interface ITemplateNodeBuilder : ITemplatedNodeBuilder<ITemplateNodeBuilder>
{
	/// <summary>
	/// Configures the templated section to bind to some property.<br/>
	/// Only one can be used on a single collection.
	/// </summary>
	/// <param name="binding">The binding to pull and push changes from and to some model.</param>
	/// <returns>The template node builder to add further configurations.</returns>
	public ITemplateNodeBuilder UseBinding(ITemplateNodeBinding binding);
}
