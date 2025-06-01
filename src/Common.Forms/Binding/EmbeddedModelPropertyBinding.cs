namespace RobinEpple.Common.Forms.Binding;

using System.Linq.Expressions;
using RobinEpple.Common.Forms.Nodes;

public class EmbeddedModelPropertyBinding<TModel, TFieldValue, TProperty> : IValueAccessor<TFieldValue>
{
	private readonly string _instanceNodeName;
	private readonly Func<TModel, TFieldValue> _getter;
	private readonly Action<TModel, TFieldValue> _setter;

	public EmbeddedModelPropertyBinding(string instanceNodeName, Expression<Func<TModel, TProperty>> propertyAccessor)
	{
		_getter = BuildGetter(propertyAccessor);
		_setter = BuildSetter(propertyAccessor);
		_instanceNodeName = instanceNodeName;
	}

	private static Func<TModel, TFieldValue> BuildGetter(Expression<Func<TModel, TProperty>> propertyAccessor)
	{
		try
		{
			var modelParam = propertyAccessor.Parameters[0];

			// Convert TProperty to TFieldValue
			var body = Expression.Convert(propertyAccessor.Body, typeof(TFieldValue));
			var lambda = Expression.Lambda<Func<TModel, TFieldValue>>(body, modelParam);

			return lambda.Compile();
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException(
				$"Could not build getter converting from '{typeof(TProperty).Name}' to '{typeof(TFieldValue).Name}'.",
				ex
			);
		}
	}

	private static Action<TModel, TFieldValue> BuildSetter(Expression<Func<TModel, TProperty>> propertyAccessor)
	{
		try
		{
			if (propertyAccessor.Body is not MemberExpression memberExpr)
			{
				throw new ArgumentException("Expression must be a property access.", nameof(propertyAccessor));
			}

			var modelParam = propertyAccessor.Parameters[0];
			var valueParam = Expression.Parameter(typeof(TFieldValue), "value");

			// Convert TFieldValue to TProperty before assignment
			var convertedValue = Expression.Convert(valueParam, typeof(TProperty));
			var assignment = Expression.Assign(memberExpr, convertedValue);

			var lambda = Expression.Lambda<Action<TModel, TFieldValue>>(assignment, modelParam, valueParam);
			return lambda.Compile();
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException(
				$"Could not build setter converting from '{typeof(TFieldValue).Name}' to '{typeof(TProperty).Name}'.",
				ex
			);
		}
	}

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
	public TFieldValue GetValue(IFormNode node)
	{
		var instanceModel = GetInstanceModel(node);
		return _getter(instanceModel);
	}

	/// <inheritdoc />
	public void SetValue(TFieldValue value, IFormNode node)
	{
		var instanceModel = GetInstanceModel(node);
		_setter(instanceModel, value);
	}
}
