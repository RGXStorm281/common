namespace RobinEpple.Common.Forms.Binding;

using System.Linq.Expressions;
using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// This factory can be used to create model bindings for instance models in parent nodes.
/// </summary>
/// <typeparam name="TModel">The type of the instance model.</typeparam>
public class InstanceBindingFactory<TModel>
{
	private readonly string _instanceNodeName;

	public InstanceBindingFactory(string instanceNodeName)
	{
		_instanceNodeName = instanceNodeName;
	}

	/// <summary>
	/// Creates a binding for an <see cref="IBooleanNode"/> using custom getter and setter methods.
	/// </summary>
	/// <param name="getter">The method loading the value from the model.</param>
	/// <param name="setter">The method writing the value to the model.</param>
	/// <returns>The binding.</returns>
	public IFormNodeBinding CreateGetterSetterBinding(Func<TModel, bool?> getter, Action<TModel, bool?> setter) =>
		throw new NotImplementedException();

	/// <summary>
	/// Creates a binding for an <see cref="IBooleanNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The binding.</returns>
	public IFormNodeBinding CreatePropertyBinding(Expression<Func<TModel, bool?>> propertyAccessor) =>
		throw new NotImplementedException();

	/// <summary>
	/// Creates a binding for an <see cref="IFileNode"/> using custom getter and setter methods.
	/// </summary>
	/// <param name="getter">The method loading the value from the model.</param>
	/// <param name="setter">The method writing the value to the model.</param>
	/// <returns>The binding.</returns>
	public IFormNodeBinding CreateGetterSetterBinding(
		Func<TModel, FileValue> getter,
		Action<TModel, FileValue> setter
	) => throw new NotImplementedException();

	/// <summary>
	/// Creates a binding for an <see cref="IFileNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The binding.</returns>
	public IFormNodeBinding CreatePropertyBinding(Expression<Func<TModel, FileValue>> propertyAccessor) =>
		throw new NotImplementedException();

	/// <summary>
	/// Creates a binding for an <see cref="INumberNode"/> using custom getter and setter methods.
	/// </summary>
	/// <param name="getter">The method loading the value from the model.</param>
	/// <param name="setter">The method writing the value to the model.</param>
	/// <returns>The binding.</returns>
	public IFormNodeBinding CreateGetterSetterBinding(Func<TModel, decimal?> getter, Action<TModel, decimal?> setter) =>
		throw new NotImplementedException();

	/// <summary>
	/// Creates a binding for an <see cref="INumberNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The binding.</returns>
	public IFormNodeBinding CreatePropertyBinding(Expression<Func<TModel, decimal?>> propertyAccessor) =>
		throw new NotImplementedException();

	/// <summary>
	/// Creates a binding for an <see cref="INumberNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The binding.</returns>
	public IFormNodeBinding CreatePropertyBinding(Expression<Func<TModel, double?>> propertyAccessor) =>
		throw new NotImplementedException();

	/// <summary>
	/// Creates a binding for an <see cref="INumberNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The binding.</returns>
	public IFormNodeBinding CreatePropertyBinding(Expression<Func<TModel, float?>> propertyAccessor) =>
		throw new NotImplementedException();

	/// <summary>
	/// Creates a binding for an <see cref="INumberNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The binding.</returns>
	public IFormNodeBinding CreatePropertyBinding(Expression<Func<TModel, long?>> propertyAccessor) =>
		throw new NotImplementedException();

	/// <summary>
	/// Creates a binding for an <see cref="INumberNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The binding.</returns>
	public IFormNodeBinding CreatePropertyBinding(Expression<Func<TModel, int?>> propertyAccessor) =>
		throw new NotImplementedException();

	/// <summary>
	/// Creates a binding for an <see cref="ITextNode"/> using custom getter and setter methods.
	/// </summary>
	/// <param name="getter">The method loading the value from the model.</param>
	/// <param name="setter">The method writing the value to the model.</param>
	/// <returns>The binding.</returns>
	public IFormNodeBinding CreateGetterSetterBinding(Func<TModel, string?> getter, Action<TModel, string?> setter) =>
		throw new NotImplementedException();

	/// <summary>
	/// Creates a binding for an <see cref="ITextNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The binding.</returns>
	public IFormNodeBinding CreatePropertyBinding(Expression<Func<TModel, string?>> propertyAccessor) =>
		throw new NotImplementedException();

	/// <summary>
	/// Creates a binding for an <see cref="ITimestampNode"/> using custom getter and setter methods.
	/// </summary>
	/// <param name="getter">The method loading the value from the model.</param>
	/// <param name="setter">The method writing the value to the model.</param>
	/// <returns>The binding.</returns>
	public IFormNodeBinding CreateGetterSetterBinding(
		Func<TModel, DateTime?> getter,
		Action<TModel, DateTime?> setter
	) => throw new NotImplementedException();

	/// <summary>
	/// Creates a binding for an <see cref="ITimestampNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The binding.</returns>
	public IFormNodeBinding CreatePropertyBinding(Expression<Func<TModel, DateTime?>> propertyAccessor) =>
		throw new NotImplementedException();

	/// <summary>
	/// Creates a binding for an <see cref="ITemplateNode"/> using custom getter and setter methods.
	/// </summary>
	/// <param name="getter">The method loading the value from the model.</param>
	/// <param name="setter">The method writing the value to the model.</param>
	/// <returns>The binding.</returns>
	public IFormNodeBinding CreateGetterSetterBinding<TInnerModel>(
		Func<TModel, TInnerModel?> getter,
		Action<TModel, TInnerModel?> setter
	) => throw new NotImplementedException();

	/// <summary>
	/// Creates a binding for an <see cref="ITemplateNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The binding.</returns>
	public IFormNodeBinding CreatePropertyBinding<TInnerModel>(
		Expression<Func<TModel, TInnerModel?>> propertyAccessor
	) => throw new NotImplementedException();

	/// <summary>
	/// Creates a binding for an <see cref="ICollectionNode"/> using custom getter and setter methods.
	/// </summary>
	/// <param name="getter">The method loading the value from the model.</param>
	/// <param name="setter">The method writing the value to the model.</param>
	/// <returns>The binding.</returns>
	public IFormNodeBinding CreateGetterSetterBinding<TItem>(
		Func<TModel, IEnumerable<TItem>> getter,
		Action<TModel, IEnumerable<TItem>> setter
	) => throw new NotImplementedException();

	/// <summary>
	/// Creates a binding for an <see cref="ICollectionNode"/> by constructing getter and setter methods from the <paramref name="propertyAccessor"/>.
	/// </summary>
	/// <param name="propertyAccessor">An expression pointing to some property of a given instance model.</param>
	/// <returns>The binding.</returns>
	public IFormNodeBinding CreatePropertyBinding<TItem>(
		Expression<Func<TModel, IEnumerable<TItem>>> propertyAccessor
	) => throw new NotImplementedException();
}
