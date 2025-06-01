namespace RobinEpple.Common.Forms.Binding;

using System.Linq.Expressions;
using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// Binds a field node to a property in an external model, that can be defined statically at form building time.
/// </summary>
/// <typeparam name="TFieldValue">The value type of the field node.</typeparam>
/// <typeparam name="TProperty">The type of the property to bind to.</typeparam>
public class PropertyAccessor<TFieldValue, TProperty> : IValueAccessor<TFieldValue>
{
	private readonly Func<TFieldValue> _getter;
	private readonly Action<TFieldValue> _setter;

	public PropertyAccessor(Expression<Func<TProperty>> propertyAccessor)
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
	public TFieldValue GetValue(IFormNode node) => _getter();

	/// <inheritdoc />
	public void SetValue(TFieldValue value, IFormNode node) => _setter(value);
}
