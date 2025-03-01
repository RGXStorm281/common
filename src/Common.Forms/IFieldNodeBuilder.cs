namespace RobinEpple.Common.Forms;

using RobinEpple.Common.Forms.Binding;
using RobinEpple.Common.Forms.Nodes;

public interface IFieldNodeBuilder : INodeBuilder<IFieldNodeBuilder>
{
	/// <summary>
	/// Configures the field to bind to some property.<br/>
	/// Only one can be used on a single field.
	/// </summary>
	/// <param name="binding">The binding to pull and push changes from and to some model.</param>
	/// <returns>The field builder to add further configurations.</returns>
	public IFieldNodeBuilder UseBinding(IFieldBinding binding);

	/// <summary>
	/// Configures the field to use the given formatter for converting between the internal value and string representations.<br/>
	/// Only one can be used on a single field.
	/// </summary>
	/// <param name="formatter">The formatter.</param>
	/// <returns>The field builder to add further configurations.</returns>
	public IFieldNodeBuilder UseFormatter(IValueFormatter formatter);
}
