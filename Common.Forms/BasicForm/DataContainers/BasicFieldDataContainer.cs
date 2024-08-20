using RobinEpple.Common.Forms.BasicApi.DataContainers;

namespace RobinEpple.Common.Forms.BasicForm.DataContainers;

/// <summary>
/// The field data container for a basic form.
/// </summary>
/// <typeparam name="TValue">The type of value this container holds.</typeparam>
public class BasicFieldDataContainer<TValue> : IFieldDataContainer<TValue>
{
	/// <inheritdoc />
	public TValue? Value { get; set; }
}