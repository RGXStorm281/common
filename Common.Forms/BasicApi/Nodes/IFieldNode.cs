using RobinEpple.Common.Forms.BasicApi.DataContainers;

namespace RobinEpple.Common.Forms.BasicApi.Nodes;

/// <summary>
/// The interface for a field, that holds a value of type <typeparamref name="TValue" />.
/// </summary>
/// <typeparam name="TValue">The value type of the field.</typeparam>
public interface IFieldNode<TValue> : IFormNode
{
	/// <summary>
	/// The value of the node.
	/// </summary>
	public TValue Value { get; }

	/// <summary>
	/// Updates the data in the node with the data from the container.
	/// </summary>
	/// <param name="context">The context with the data.</param>
	public void SetData(IFieldDataContainer<TValue> context);

	/// <summary>
	/// Emits the current data from this node as a data container.
	/// </summary>
	/// <returns>The data container.</returns>
	public IFieldDataContainer<TValue> GetData();
}