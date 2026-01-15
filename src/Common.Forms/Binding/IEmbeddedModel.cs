namespace RobinEpple.Common.Forms.Binding;

using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// Represents a model that is embedded into the form to bind to.
/// </summary>
public interface IEmbeddedModel
{
	/// <summary>
	/// Checks, whether this node can accept the given value type.
	/// </summary>
	/// <param name="value">The value to check.</param>
	/// <returns><see langword="true"/>, if the value is accepted.</returns>
	public bool Accepts(object? value);

	/// <summary>
	/// Retrieves the model value from the node.
	/// </summary>
	/// <param name="node">The node to get the model from.</param>
	/// <returns>The current model value.</returns>
	public object? GetValue(IForm node);

	/// <summary>
	/// Sets the model value in the node.
	/// </summary>
	/// <param name="node">The node the model is loaded into.</param>
	/// <param name="value">The new model value.</param>
	public void SetValue(IForm node, object? value);
}
