namespace RobinEpple.Common.Forms.Binding;

using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// Implements a simple binding with getter and setter methods.
/// </summary>
/// <typeparam name="TValue">Type of the value to be used.</typeparam>
/// <param name="getter">The getter function to load a value from the model.</param>
/// <param name="setter">The setter function to write a value to the model.</param>
public class GetterSetterBinding<TValue>(Func<TValue> getter, Action<TValue> setter) : IFormNodeBinding
{
	private readonly Func<TValue> _getter = getter;
	private readonly Action<TValue> _setter = setter;

	/// <inheritdoc />
	public void LoadFromModel(IFormNode node)
	{
		if (node is not IValueNode<TValue> valueNode)
		{
			throw new InvalidOperationException(
				$"The getter setter binding for value type '{typeof(TValue).Name}' can only be used on IValueNodes with the same value type."
			);
		}

		valueNode.Value = _getter();
	}

	/// <inheritdoc />
	public void WriteToModel(IFormNode node)
	{
		if (node is not IValueNode<TValue> valueNode)
		{
			throw new InvalidOperationException(
				$"The getter setter binding for value type '{typeof(TValue).Name}' can only be used on IValueNodes with the same value type."
			);
		}

		_setter(valueNode.Value);
	}
}
