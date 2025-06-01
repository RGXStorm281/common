namespace RobinEpple.Common.Forms.Binding;

using RobinEpple.Common.Forms.Nodes;

public interface IValueAccessor<TValue>
{
	/// <summary>
	/// Returns the model value in the context of the <paramref name="node"/>.
	/// </summary>
	/// <param name="node">The bound node as context (e.g. accessing the correct instance model).</param>
	/// <returns>The current value in the model.</returns>
	public TValue GetValue(IFormNode node);

	/// <summary>
	/// Writes the value to the model in the context of the <paramref name="node"/>.
	/// </summary>
	/// <param name="value">The value, that is written to the model.</param>
	/// <param name="node">The bound node as context (e.g. accessing the correct instance model).</param>
	public void SetValue(TValue value, IFormNode node);
}
