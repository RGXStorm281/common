namespace RobinEpple.Common.Forms.Binding;

using RobinEpple.Common.Forms.Nodes;

public class EmbeddedModelGetterSetterBinding<TModel, TValue>(
	string instanceNodeName,
	Func<TModel, TValue> getter,
	Action<TModel, TValue> setter
) : IValueAccessor<TValue>
{
	private readonly string _instanceNodeName = instanceNodeName;

	private readonly Func<TModel, TValue> _getter = getter;
	private readonly Action<TModel, TValue> _setter = setter;

	private TModel GetInstanceModel(IFormNode node)
	{
		// Loop over parents until the target node with the given name is found.
		IFormNode? target = node;
		while (target != null)
		{
			if (target.Name == _instanceNodeName)
			{
				// Target node found.
				break;
			}
			target = target.Parent;
		}

		// If no target is found, no model can be retrieved.
		if (target == null)
		{
			throw new InvalidOperationException(
				$"The parent node with the name '{_instanceNodeName}' could not be found."
			);
		}

		// Try to retrieve and cast the instance model.
		if (!target.Tags.TryGetValue(IFormNodeBinding.InstanceModelTagName, out var instanceModel))
		{
			throw new InvalidOperationException(
				$"The parent node with the name '{_instanceNodeName}' does not contain an instance model."
			);
		}

		if (instanceModel is not TModel typedModel)
		{
			var actualType = instanceModel?.GetType().Name ?? "<NULL>";
			throw new InvalidCastException(
				$"The instance model of type '{actualType}' cannot be cast to the desired type '{typeof(TModel).Name}'."
			);
		}

		return typedModel;
	}

	/// <inheritdoc />
	public TValue GetValue(IFormNode node)
	{
		var instanceModel = GetInstanceModel(node);
		return _getter(instanceModel);
	}

	/// <inheritdoc />
	public void SetValue(TValue value, IFormNode node)
	{
		var instanceModel = GetInstanceModel(node);
		_setter(instanceModel, value);
	}
}
