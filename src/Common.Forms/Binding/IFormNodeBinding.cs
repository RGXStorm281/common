namespace RobinEpple.Common.Forms.Binding;

using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// Represents a binding, that can load the value of a node and write updates back to the model
/// </summary>
public interface IFormNodeBinding
{
	/// <summary>
	/// Reads a value from the model and loads it into the node.
	/// </summary>
	/// <param name="node">The node to load the value to.</param>
	public void LoadFromModel(IFormNode node);

	/// <summary>
	/// Writes the state of the node back to the bound model.
	/// </summary>
	/// <param name="node">The node to read the value from.</param>
	public void WriteToModel(IFormNode node);

	/// <summary>
	/// The name that is used for tagging nodes with an instance model.
	/// </summary>
	public const string InstanceModelTagName = "_instanceModel";
}
