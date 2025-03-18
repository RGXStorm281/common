namespace RobinEpple.Common.Forms.Expressions;

/// <summary>
/// This class provides static factory methods to construct expressions on a form tree.
/// </summary>
public static class FormExpression
{
	# region logical operators

	/// <summary>
	/// Aggregates multiple boolean values with the logical operator "AND".
	/// </summary>
	/// <param name="operands">The list of boolean operands.</param>
	/// <returns>The result is <see langword="true"/> if all operands are <see langword="true"/>.</returns>
	public static IFormExpression<bool> And(params IEnumerable<IFormExpression<bool>> operands) =>
		throw new NotImplementedException();

	/// <summary>
	/// Aggregates multiple boolean values with the logical operator "OR".
	/// </summary>
	/// <param name="operands">The list of boolean operands.</param>
	/// <returns>The result is <see langword="true"/> if at least one of the operands is <see langword="true"/>.</returns>
	public static IFormExpression<bool> Or(params IEnumerable<IFormExpression<bool>> operands) =>
		throw new NotImplementedException();

	# endregion

	# region static values

	/// <summary>
	/// Represents a static value in the form.
	/// </summary>
	/// <param name="value">The value.</param>
	public static IFormExpression<TValue> StaticValue<TValue>(TValue value) => throw new NotImplementedException();

	/// <summary>
	/// Provides a <paramref name="fallbackValue"/> in case the <paramref name="source"/> is <see langword="null"/>.
	/// </summary>
	/// <param name="source">The source to check.</param>
	/// <param name="fallbackValue">The fallback value to use if <paramref name="source"/> is <see langword="null"/>.</param>
	public static IFormExpression<TValue> Coalesce<TValue>(
		this IFormExpression<TValue?> source,
		IFormExpression<TValue> fallbackValue
	) => throw new NotImplementedException();

	/// <summary>
	/// Checks the <paramref name="condition"/> and returns a different value depending on the result.
	/// </summary>
	/// <param name="condition">The condition to check.</param>
	/// <param name="whenTrue">The value to use if the <paramref name="condition"/> evaluates to <see langword="true"/>.</param>
	/// <param name="whenFalse">The value to use if the <paramref name="condition"/> evaluates to <see langword="false"/>.</param>
	public static IFormExpression<TValue> Conditional<TValue>(
		this IFormExpression<bool> condition,
		IFormExpression<TValue> whenTrue,
		IFormExpression<TValue> whenFalse
	) => throw new NotImplementedException();

	# endregion

	# region field access

	/// <summary>
	/// Locates a unique <see cref="RobinEpple.Common.Forms.Nodes.IBooleanNode"/> with the given <paramref name="name"/><br/>
	/// in the current scope (within the direct parent of the target node, if not specified otherwise)<br/>
	/// and returns its value.
	/// </summary>
	/// <param name="name">The name of the node.</param>
	/// <returns>The value of the node, if it is found.</returns>
	public static IFormExpression<bool?> BooleanFieldValue(string name) => throw new NotImplementedException();

	/// <summary>
	/// Locates a unique <see cref="RobinEpple.Common.Forms.Nodes.IFileNode"/> with the given <paramref name="name"/><br/>
	/// in the current scope (within the direct parent of the target node, if not specified otherwise)<br/>
	/// and returns its file name.
	/// </summary>
	/// <param name="name">The name of the node.</param>
	/// <returns>The file name of the node, if it is found.</returns>
	public static IFormExpression<string?> FileFieldFileName(string name) => throw new NotImplementedException();

	/// <summary>
	/// Locates a unique <see cref="RobinEpple.Common.Forms.Nodes.IFileNode"/> with the given <paramref name="name"/><br/>
	/// in the current scope (within the direct parent of the target node, if not specified otherwise)<br/>
	/// and returns its file bytes.
	/// </summary>
	/// <param name="name">The name of the node.</param>
	/// <returns>The file bytes of the node, if it is found.</returns>
	public static IFormExpression<byte[]?> FileFieldFileContent(string name) => throw new NotImplementedException();

	/// <summary>
	/// Locates a unique <see cref="RobinEpple.Common.Forms.Nodes.INumberNode"/> with the given <paramref name="name"/><br/>
	/// in the current scope (within the direct parent of the target node, if not specified otherwise)<br/>
	/// and returns its value.
	/// </summary>
	/// <param name="name">The name of the node.</param>
	/// <returns>The value of the node, if it is found.</returns>
	public static IFormExpression<decimal?> NumberFieldValue(string name) => throw new NotImplementedException();

	/// <summary>
	/// Locates a unique <see cref="RobinEpple.Common.Forms.Nodes.ITextNode"/> with the given <paramref name="name"/><br/>
	/// in the current scope (within the direct parent of the target node, if not specified otherwise)<br/>
	/// and returns its value.
	/// </summary>
	/// <param name="name">The name of the node.</param>
	/// <returns>The value of the node, if it is found.</returns>
	public static IFormExpression<string?> TextFieldValue(string name) => throw new NotImplementedException();

	/// <summary>
	/// Locates a unique <see cref="RobinEpple.Common.Forms.Nodes.ITimestampNode"/> with the given <paramref name="name"/><br/>
	/// in the current scope (within the closest (parent) form, if not specified otherwise)<br/>
	/// and returns its value.
	/// </summary>
	/// <param name="name">The name of the node.</param>
	/// <returns>The value of the node, if it is found.</returns>
	public static IFormExpression<DateTime?> TimestampFieldValue(string name) => throw new NotImplementedException();

	# endregion

	# region change of scope

	/// <summary>
	/// Returns the name of the scope container (the closest (parent) form, if not specified otherwise).<br/>
	/// This can be used to determine what template has been instantiated.
	/// </summary>
	/// <returns>The name of the current scope</returns>
	public static IFormExpression<string> ScopeName() => throw new NotImplementedException();

	/// <summary>
	/// Locates a unique <see cref="RobinEpple.Common.Forms.Nodes.ITemplateNode"/> with the given <paramref name="name"/><br/>
	/// in the current scope (within the closest (parent) form, if not specified otherwise)<br/>
	/// and executes the specified <paramref name="expression"/> in the scope of its instance.
	/// </summary>
	/// <typeparam name="TValue">The value of the inner expression.</typeparam>
	/// <param name="name">The name of the node.</param>
	/// <param name="expression">The expression that is targeted at the instance.</param>
	/// <returns>The value of the <paramref name="expression"/>, given that the node is found and has an instance.</returns>
	public static IFormExpression<TValue?> InSection<TValue>(string name, IFormExpression<TValue> expression) =>
		throw new NotImplementedException();

	/// <summary>
	/// Locates a unique <see cref="RobinEpple.Common.Forms.Nodes.ICollectionNode"/> with the given <paramref name="name"/><br/>
	/// in the current scope (within the closest (parent) form, if not specified otherwise)<br/>
	/// and executes the specified <paramref name="expression"/> in the scope of each of its instances.
	/// </summary>
	/// <typeparam name="TValue">The value of the inner expression.</typeparam>
	/// <param name="name">The name of the node.</param>
	/// <param name="expression">The expression that is targeted at each instance.</param>
	/// <returns>The list of values the <paramref name="expression"/> yields on each instance, given that the node is found.</returns>
	public static IEnumerable<IFormExpression<TValue>> ForEachCollectionItem<TValue>(
		string name,
		IFormExpression<TValue> expression
	) => throw new NotImplementedException();

	/// <summary>
	/// Executes the specified <paramref name="expression"/> in the root scope of the entire form.
	/// </summary>
	/// <typeparam name="TValue">The value of the inner expression.</typeparam>
	/// <param name="expression">The expression that is targeted at the root scope.</param>
	/// <returns>The value of the <paramref name="expression"/>.</returns>
	public static IEnumerable<IFormExpression<TValue>> InRootScope<TValue>(IFormExpression<TValue> expression) =>
		throw new NotImplementedException();

	# endregion
}
