using System.Linq.Expressions;
using System.Reflection;

namespace RobinEpple.Common.Forms.BindableForm;

/// <summary>
/// Wraps the logic to compile getter and setter functions from a property expression.
/// </summary>
/// <typeparam name="TModel">The model type the property is located in.</typeparam>
/// <typeparam name="TProperty">The type of the property this accessor wraps.</typeparam>
/// <param name="propertyExpression">The expression to identify the property.</param>
public class PropertyAccessor<TModel, TProperty>(
	Expression<Func<TModel, TProperty>> propertyExpression)
{
	private readonly Func<TModel, TProperty> _getter = propertyExpression.Compile();
	private readonly Action<TModel, TProperty> _setter = BuildSetter(propertyExpression);

	private static Action<TModel, TProperty> BuildSetter(Expression<Func<TModel, TProperty>> propertyExpression)
	{
		var prop = (PropertyInfo) ((MemberExpression) propertyExpression.Body).Member;
		return (model, value) => prop.SetValue(model, value, null);
	}

	/// <summary>
	/// Gets the value from the wrapped property in the model.
	/// </summary>
	/// <param name="model">The model.</param>
	/// <returns>The current property value.</returns>
	public TProperty Get(TModel model)
		=> _getter(model);

	/// <summary>
	/// Sets the value in the wrapped property in the model.
	/// </summary>
	/// <param name="model">The model.</param>
	/// <param name="value">The new property value.</param>
	public void Set(TModel model, TProperty value)
		=> _setter(model, value);
}