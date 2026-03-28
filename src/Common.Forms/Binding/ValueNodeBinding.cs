namespace RobinEpple.Common.Forms.Binding;

using RobinEpple.Common.Forms.Nodes;

internal class ValueNodeBinding<TValue> : IValueAccessor<TValue>
{
	public TValue GetValue(IFormNode node)
	{
		if (node is not IValueNode<TValue> valueNode)
		{
			throw new InvalidOperationException(
				$"The given node needs to be an IValueNode with the value type '{typeof(TValue).Name}'."
			);
		}

		return valueNode.Value;
	}

	public void SetValue(TValue value, IFormNode node)
	{
		if (node is not IValueNode<TValue> valueNode)
		{
			throw new InvalidOperationException(
				$"The given node needs to be an IValueNode with the value type '{typeof(TValue).Name}' but is of type '{node.GetType().Name}'."
			);
		}

		valueNode.Value = value;
	}
}
