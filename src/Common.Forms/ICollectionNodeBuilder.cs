namespace RobinEpple.Common.Forms;

using RobinEpple.Common.Forms.Binding;
using RobinEpple.Common.Forms.Nodes;

public interface ICollectionNodeBuilder : ITemplatedNodeBuilder<ICollectionNodeBuilder>
{
	/// <summary>
	/// Configures the collection to bind to some property.<br/>
	/// Only one can be used on a single collection.
	/// </summary>
	/// <param name="binding">The binding to pull and push changes from and to some model.</param>
	/// <returns>The collection builder to add further configurations.</returns>
	public ICollectionNodeBuilder UseBinding(ICollectionNodeBinding binding);
}
