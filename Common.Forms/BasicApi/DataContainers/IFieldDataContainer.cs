namespace RobinEpple.Common.Forms.BasicApi.DataContainers;

/// <summary>
/// The data container for a field holding a value of type <typeparamref name="TValue" />.
/// </summary>
/// <typeparam name="TValue">The type of the value stored in this container.</typeparam>
public interface IFieldDataContainer<TValue> : IFormDataContainer
{
	/// <summary>
	/// The value of the field data.
	/// </summary>
	public TValue? Value { get; set; }
}