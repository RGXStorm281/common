namespace RobinEpple.Common.Forms.Binding;

using System.Linq.Expressions;
using RobinEpple.Common.Forms.Nodes;

public class PropertyBinding<TFieldValue, TProperty> : IFormNodeBinding
{
	private readonly Func<TFieldValue> _getter;
	private readonly Action<TFieldValue> _setter;

	public PropertyBinding(Expression<Func<TProperty>> propertyAccessor)
	{
		// Compile the getter and setter expressions
		_getter = BuildGetter(propertyAccessor);
		_setter = BuildSetter(propertyAccessor);
	}

	private Func<TFieldValue> BuildGetter(Expression<Func<TProperty>> propertyAccessor)
	{
		try
		{
			var body = Expression.Convert(propertyAccessor.Body, typeof(TFieldValue));
			var lambda = Expression.Lambda<Func<TFieldValue>>(body);
			return lambda.Compile();
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException(
				$"Could not build getter of type '{typeof(TFieldValue).Name}' from property accessor of type '{typeof(TProperty)}'.",
				ex
			);
		}
	}

	public static Action<TFieldValue> BuildSetter(Expression<Func<TProperty>> propertyAccessor)
	{
		try
		{
			if (propertyAccessor.Body is not MemberExpression memberExpr)
			{
				throw new ArgumentException("Expression must be a property access.", nameof(propertyAccessor));
			}

			var targetExpr = memberExpr.Expression;
			if (targetExpr == null)
			{
				throw new ArgumentException("Property expression must have a target instance.");
			}

			var valueParam = Expression.Parameter(typeof(TFieldValue), "value");

			var convertedValue = Expression.Convert(valueParam, typeof(TProperty));
			var assignment = Expression.Assign(memberExpr, convertedValue);

			var lambda = Expression.Lambda<Action<TFieldValue>>(assignment, valueParam);
			return lambda.Compile();
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException(
				$"Could not build setter of type '{typeof(TFieldValue).Name}' from property accessor of type '{typeof(TProperty)}'.",
				ex
			);
		}
	}

	/// <inheritdoc />
	public void LoadFromModel(IFormNode node)
	{
		if (node is not IValueNode<TFieldValue> valueNode)
		{
			throw new InvalidOperationException(
				$"The getter setter binding for value type '{typeof(TFieldValue).Name}' can only be used on IValueNodes with the same value type."
			);
		}

		valueNode.Value = _getter();
	}

	/// <inheritdoc />
	public void WriteToModel(IFormNode node)
	{
		if (node is not IValueNode<TFieldValue> valueNode)
		{
			throw new InvalidOperationException(
				$"The getter setter binding for value type '{typeof(TFieldValue).Name}' can only be used on IValueNodes with the same value type."
			);
		}

		_setter(valueNode.Value);
	}
}
