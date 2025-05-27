namespace RobinEpple.Common.Forms;

using RobinEpple.Common.Forms.Nodes;

public interface IFieldNodeBuilder<TSpecificNodeBuilder> : INodeBuilder<TSpecificNodeBuilder>
{
	/// <summary>
	/// Configures the field to use the given formatter for converting between the internal value and string representations.<br/>
	/// Only one can be used on a single field.
	/// </summary>
	/// <param name="formatter">The formatter.</param>
	/// <returns>The field builder to add further configurations.</returns>
	public TSpecificNodeBuilder UseFormatter(IValueFormatter formatter);
}
