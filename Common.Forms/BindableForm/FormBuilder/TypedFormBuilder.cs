// namespace RobinEpple.HomeSuite.Common.Forms.BindableForm.FormBuilder;
//
// using System.Linq.Expressions;
// using BasicApi;
// using BasicForm.Collections;
// using BasicForm.Fields;
// using BasicForm.Forms;
//
// public class TypedFormBuilder<TModel>
// {
// 	/// <summary>
// 	/// Adds a boolean field to this form.
// 	/// </summary>
// 	/// <inheritdoc cref="BoolField" />
// 	/// <returns>This form, for chaining further actions.</returns>
// 	public Form<TModel> WithBoolField(
// 		Expression<Func<TModel, bool?>> propertyExpression,
// 		string id,
// 		ILabel label,
// 		IExpression<bool>? visibilityCondition = null,
// 		IEnumerable<INodeValidator<BoolField>>? validators = null,
// 		bool? initialValue = null)
// 	{
// 		_nodes.Add(
// 			new BoolField<TModel>(
// 				id,
// 				this,
// 				label,
// 				visibilityCondition,
// 				validators ?? Enumerable.Empty<INodeValidator<BoolField<TModel>>>(),
// 				initialValue,
// 				propertyExpression));
// 		return this;
// 	}
//
// 	/// <summary>
// 	/// Adds a byte field to this form.
// 	/// </summary>
// 	/// <inheritdoc cref="ByteField{TModel}" />
// 	/// <returns>This form, for chaining further actions.</returns>
// 	public Form<TModel> WithByteField(
// 		Expression<Func<TModel, byte[]?>> propertyExpression,
// 		string id,
// 		ILabel label,
// 		IExpression<bool>? visibilityCondition = null,
// 		IEnumerable<INodeValidator<ByteField<TModel>>>? validators = null,
// 		byte[]? initialValue = null)
// 	{
// 		_nodes.Add(
// 			new ByteField<TModel>(
// 				id,
// 				this,
// 				label,
// 				visibilityCondition,
// 				validators ?? Enumerable.Empty<INodeValidator<ByteField<TModel>>>(),
// 				initialValue,
// 				propertyExpression));
// 		return this;
// 	}
//
// 	/// <summary>
// 	/// Adds a double field to this form.
// 	/// </summary>
// 	/// <inheritdoc cref="DoubleField{TModel}" />
// 	/// <returns>This form, for chaining further actions.</returns>
// 	public Form<TModel> WithDoubleField(
// 		Expression<Func<TModel, double?>> propertyExpression,
// 		string id,
// 		ILabel label,
// 		IExpression<bool>? visibilityCondition = null,
// 		IEnumerable<INodeValidator<DoubleField<TModel>>>? validators = null,
// 		double? initialValue = null)
// 	{
// 		_nodes.Add(
// 			new DoubleField<TModel>(
// 				id,
// 				this,
// 				label,
// 				visibilityCondition,
// 				validators ?? Enumerable.Empty<INodeValidator<DoubleField<TModel>>>(),
// 				initialValue,
// 				propertyExpression));
// 		return this;
// 	}
//
// 	/// <summary>
// 	/// Adds an integer field to this form.
// 	/// </summary>
// 	/// <inheritdoc cref="IntField{TModel}" />
// 	/// <returns>This form, for chaining further actions.</returns>
// 	public Form<TModel> WithIntField(
// 		Expression<Func<TModel, int?>> propertyExpression,
// 		string id,
// 		ILabel label,
// 		IExpression<bool>? visibilityCondition = null,
// 		IEnumerable<INodeValidator<IntField<TModel>>>? validators = null,
// 		int? initialValue = null)
// 	{
// 		_nodes.Add(
// 			new IntField<TModel>(
// 				id,
// 				this,
// 				label,
// 				visibilityCondition,
// 				validators ?? Enumerable.Empty<INodeValidator<IntField<TModel>>>(),
// 				initialValue,
// 				propertyExpression));
// 		return this;
// 	}
//
// 	/// <summary>
// 	/// Adds a string field to this form.
// 	/// </summary>
// 	/// <inheritdoc cref="StringField{TModel}" />
// 	/// <returns>This form, for chaining further actions.</returns>
// 	public Form<TModel> WithStringField(
// 		Expression<Func<TModel, string?>> propertyExpression,
// 		string id,
// 		ILabel label,
// 		IExpression<bool>? visibilityCondition = null,
// 		IEnumerable<INodeValidator<StringField<TModel>>>? validators = null,
// 		string? initialValue = null)
// 	{
// 		_nodes.Add(
// 			new StringField<TModel>(
// 				id,
// 				this,
// 				label,
// 				visibilityCondition,
// 				validators ?? Enumerable.Empty<INodeValidator<StringField<TModel>>>(),
// 				initialValue,
// 				propertyExpression));
// 		return this;
// 	}
//
// 	/// <summary>
// 	/// Adds a sub form to this form.
// 	/// </summary>
// 	/// <inheritdoc cref="SubForm" />
// 	/// <returns>This form, for chaining further actions.</returns>
// 	public Form<TModel> WithSubForm<TSubModel>(
// 		Func<TModel, TSubModel> getter,
// 		string id,
// 		Action<SubForm> configure,
// 		IExpression<bool>? visibilityCondition = null,
// 		IEnumerable<INodeValidator<SubForm>>? validators = null,
// 		ILabel? label = null)
// 	{
// 		var subForm = new SubForm(
// 			id,
// 			this,
// 			label,
// 			visibilityCondition,
// 			validators ?? Enumerable.Empty<INodeValidator<SubForm>>(),
// 			getter);
// 		configure(subForm);
// 		_nodes.Add(subForm);
// 		return this;
// 	}
//
// 	/// <summary>
// 	/// Adds a collection of booleans to this form.
// 	/// </summary>
// 	/// <inheritdoc cref="BoolCollection{TModel}" />
// 	/// <returns>This form, for chaining further actions.</returns>
// 	public Form<TModel> WithBoolCollection(
// 		Expression<Func<TModel, IEnumerable<bool>>> propertyExpression,
// 		string id,
// 		ILabel label,
// 		IExpression<bool>? visibilityCondition = null,
// 		IEnumerable<INodeValidator<BoolCollection<TModel>>>? validators = null)
// 	{
// 		_nodes.Add(
// 			new BoolCollection<TModel>(
// 				id,
// 				this,
// 				label,
// 				visibilityCondition,
// 				validators ?? Enumerable.Empty<INodeValidator<BoolCollection<TModel>>>(),
// 				propertyExpression));
// 		return this;
// 	}
//
// 	/// <summary>
// 	/// Adds a collection of byte arrays to this form.
// 	/// </summary>
// 	/// <inheritdoc cref="ByteCollection{TModel}" />
// 	/// <returns>This form, for chaining further actions.</returns>
// 	public Form<TModel> WithByteCollection(
// 		Expression<Func<TModel, IEnumerable<byte[]>>> propertyExpression,
// 		string id,
// 		ILabel label,
// 		IExpression<bool>? visibilityCondition = null,
// 		IEnumerable<INodeValidator<ByteCollection<TModel>>>? validators = null)
// 	{
// 		_nodes.Add(
// 			new ByteCollection<TModel>(
// 				id,
// 				this,
// 				label,
// 				visibilityCondition,
// 				validators ?? Enumerable.Empty<INodeValidator<ByteCollection<TModel>>>(),
// 				propertyExpression));
// 		return this;
// 	}
//
// 	/// <summary>
// 	/// Adds a collection of doubles to this form.
// 	/// </summary>
// 	/// <inheritdoc cref="DoubleCollection{TModel}" />
// 	/// <returns>This form, for chaining further actions.</returns>
// 	public Form<TModel> WithDoubleCollection(
// 		Expression<Func<TModel, IEnumerable<double>>> propertyExpression,
// 		string id,
// 		ILabel label,
// 		IExpression<bool>? visibilityCondition = null,
// 		IEnumerable<INodeValidator<DoubleCollection<TModel>>>? validators = null)
// 	{
// 		_nodes.Add(
// 			new DoubleCollection<TModel>(
// 				id,
// 				this,
// 				label,
// 				visibilityCondition,
// 				validators ?? Enumerable.Empty<INodeValidator<DoubleCollection<TModel>>>(),
// 				propertyExpression));
// 		return this;
// 	}
//
// 	/// <summary>
// 	/// Adds a collection of integers to this form.
// 	/// </summary>
// 	/// <inheritdoc cref="IntCollection{TModel}" />
// 	/// <returns>This form, for chaining further actions.</returns>
// 	public Form<TModel> WithIntCollection(
// 		Expression<Func<TModel, IEnumerable<int>>> propertyExpression,
// 		string id,
// 		ILabel label,
// 		IExpression<bool>? visibilityCondition = null,
// 		IEnumerable<INodeValidator<IntCollection<TModel>>>? validators = null)
// 	{
// 		_nodes.Add(
// 			new IntCollection<TModel>(
// 				id,
// 				this,
// 				label,
// 				visibilityCondition,
// 				validators ?? Enumerable.Empty<INodeValidator<IntCollection<TModel>>>(),
// 				propertyExpression));
// 		return this;
// 	}
//
// 	/// <summary>
// 	/// Adds a collection of strings to this form.
// 	/// </summary>
// 	/// <inheritdoc cref="StringCollection{TModel}" />
// 	/// <returns>This form, for chaining further actions.</returns>
// 	public Form<TModel> WithStringCollection(
// 		Expression<Func<TModel, IEnumerable<string>>> propertyExpression,
// 		string id,
// 		ILabel label,
// 		IExpression<bool>? visibilityCondition = null,
// 		IEnumerable<INodeValidator<StringCollection<TModel>>>? validators = null)
// 	{
// 		_nodes.Add(
// 			new StringCollection<TModel>(
// 				id,
// 				this,
// 				label,
// 				visibilityCondition,
// 				validators ?? Enumerable.Empty<INodeValidator<StringCollection<TModel>>>(),
// 				propertyExpression));
// 		return this;
// 	}
// }