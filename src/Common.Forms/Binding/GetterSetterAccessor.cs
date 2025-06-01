namespace RobinEpple.Common.Forms.Binding;

using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// Implements a simple binding with getter and setter methods.
/// </summary>
/// <typeparam name="TValue">Type of the value to be used.</typeparam>
/// <param name="getter">The getter function to load a value from the model.</param>
/// <param name="setter">The setter function to write a value to the model.</param>
public class GetterSetterAccessor<TValue>(Func<TValue> getter, Action<TValue> setter) : IValueAccessor<TValue>
{
	private readonly Func<TValue> _getter = getter;
	private readonly Action<TValue> _setter = setter;

	/// <inheritdoc />
	public TValue GetValue(IFormNode node) => _getter();

	/// <inheritdoc />
	public void SetValue(TValue value, IFormNode node) => _setter(value);
}
