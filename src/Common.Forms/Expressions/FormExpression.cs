namespace RobinEpple.Common.Forms.Expressions;

using RobinEpple.Common.Forms.Expressions.DefaultImplementation;
using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// This class provides static factory methods to construct expressions on a form tree.
/// </summary>
public static class FormExpression
{
	# region utilities

	/// <summary>
	/// Represents a static value in the form.
	/// </summary>
	/// <param name="value">The value.</param>
	public static IFormExpression<TValue> StaticValue<TValue>(TValue value) => new StaticValueExpression<TValue>(value);

	/// <summary>
	/// Throws the given exception when called.
	/// </summary>
	/// <typeparam name="TValue">The value type this represents. Only needed to fit into the expression tree, a value is never produced.</typeparam>
	/// <param name="exceptionFactory">The exception factory to produce the thrown exception in the context of the evaluating node.</param>
	/// <returns>Never returns a value.</returns>
	public static IFormExpression<TValue> Throw<TValue>(Func<IFormNode, Exception> exceptionFactory) =>
		new ThrowExpression<TValue>(exceptionFactory);

	/// <summary>
	/// Provides a <paramref name="fallbackValue"/> in case the <paramref name="source"/> is <see langword="null"/>.
	/// </summary>
	/// <param name="source">The source to check.</param>
	/// <param name="fallbackValue">The fallback value to use if <paramref name="source"/> is <see langword="null"/>.</param>
	public static IFormExpression<TValue> Coalesce<TValue>(
		this IFormExpression<TValue?> source,
		IFormExpression<TValue> fallbackValue
	) => new CoalesceExpression<TValue>(source, fallbackValue);

	/// <inheritdoc cref="Coalesce"/>
	public static IFormExpression<TValue> Coalesce<TValue>(
		this IFormExpression<TValue?> source,
		TValue fallbackValue
	) => source.Coalesce(StaticValue(fallbackValue));

	/// <summary>
	/// Provides a <paramref name="fallbackValue"/> in case the <paramref name="source"/> throws a <see cref="NodeNotFoundException"/>.
	/// </summary>
	/// <param name="source">The source to check.</param>
	/// <param name="fallbackValue">The fallback value to use if <paramref name="source"/> throws.</param>
	public static IFormExpression<TValue> OnNotFound<TValue>(
		this IFormExpression<TValue> source,
		IFormExpression<TValue> fallbackValue
	) => new OnNotFoundExpression<TValue>(source, fallbackValue);

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
	) => new ConditionalExpression<TValue>(condition, whenTrue, whenFalse);

	/// <summary>
	/// Transforms a single <paramref name="source"/> value into the desired type.
	/// </summary>
	/// <typeparam name="TInput">The input type.</typeparam>
	/// <typeparam name="TOutput">The output type.</typeparam>
	/// <param name="source">The source value.</param>
	/// <param name="selector">The selector function.</param>
	/// <returns>The converted value.</returns>
	public static IFormExpression<TOutput> Select<TInput, TOutput>(
		this IFormExpression<TInput> source,
		Func<TInput, TOutput> selector
	) => new TransformExpression<TInput, TOutput>(source, selector);

	/// <summary>
	/// Iterates over the <paramref name="source"/> and applies the <paramref name="selector"/> to each item.
	/// </summary>
	/// <typeparam name="TInput">The input type.</typeparam>
	/// <typeparam name="TOutput">The output type.</typeparam>
	/// <param name="source">The source list.</param>
	/// <param name="selector">The selector function.</param>
	/// <returns>The converted item list.</returns>
	public static IFormExpression<IEnumerable<TOutput>> Select<TInput, TOutput>(
		this IFormExpression<IEnumerable<TInput>> source,
		Func<TInput, TOutput> selector
	) => new SelectExpression<TInput, TOutput>(source, selector);

	/// <summary>
	/// Checks whether the given <paramref name="item"/> is in the specified <paramref name="list"/>.
	/// </summary>
	/// <typeparam name="TElement">The element type of the list.</typeparam>
	/// <param name="list">The list of items to look through.</param>
	/// <param name="item">The item that needs to be matched.</param>
	/// <param name="equalityComparer">Optional equality comparer to define a match.</param>
	/// <returns><see langword="true"/>, if the item is contained. Otherwise <see langword="false"/>.</returns>
	public static IFormExpression<bool> Contains<TElement>(
		this IFormExpression<IEnumerable<TElement>> list,
		IFormExpression<TElement> item,
		IEqualityComparer<TElement>? equalityComparer = null
	) => new ContainsExpression<TElement>(list, item, equalityComparer);

	# endregion

	# region logical operators

	/// <summary>
	/// Inverts the result of the <paramref name="source"/> expression.
	/// </summary>
	/// <param name="source">The source expression.</param>
	/// <returns>The inverted value.</returns>
	public static IFormExpression<bool> Not(IFormExpression<bool> source) => new NotExpression(source);

	/// <summary>
	/// Combines the two values with the logical operator "AND".
	/// </summary>
	/// <param name="left">The left operand.</param>
	/// <param name="right">The right operand.</param>
	/// <returns>The result is <see langword="true"/> if both operands are <see langword="true"/>.</returns>
	public static IFormExpression<bool> And(this IFormExpression<bool> left, IFormExpression<bool> right) =>
		new AndExpression(left, right);

	/// <summary>
	/// Aggregates multiple boolean values with the logical operator "AND".
	/// </summary>
	/// <param name="operands">The list of boolean operands.</param>
	/// <returns>The result is <see langword="true"/> if all operands are <see langword="true"/>.</returns>
	public static IFormExpression<bool> All(this IFormExpression<IEnumerable<bool>> operands) =>
		new AllExpression(operands);

	/// <summary>
	/// Combines the two values with the logical operator "OR".
	/// </summary>
	/// <param name="left">The left operand.</param>
	/// <param name="right">The right operands.</param>
	/// <returns>The result is <see langword="true"/> if at least one of the operands is <see langword="true"/>.</returns>
	public static IFormExpression<bool> Or(this IFormExpression<bool> left, IFormExpression<bool> right) =>
		new OrExpression(left, right);

	/// <summary>
	/// Aggregates multiple boolean values with the logical operator "OR".
	/// </summary>
	/// <param name="operands">The list of boolean operands.</param>
	/// <returns>The result is <see langword="true"/> if at least one of the operands is <see langword="true"/>.</returns>
	public static IFormExpression<bool> Any(this IFormExpression<IEnumerable<bool>> operands) =>
		new AnyExpression(operands);

	# endregion

	# region field access

	/// <summary>
	/// Searches for a node with the given <paramref name="name"/> and node type <typeparamref name="TNode"/>.
	/// </summary>
	/// <typeparam name="TNode">The type of the desired node.</typeparam>
	/// <param name="name">The name of the desired node.</param>
	/// <returns>The unique node in the current scope, if it exists.</returns>
	/// <exception cref="NodeNotFoundException">When the node with the given name does not exist or is not of the desired type.</exception>
	public static IFormExpression<TNode> GetNode<TNode>(string name)
		where TNode : IFormNode => new GetNodeExpression<TNode>(name);

	/// <summary>
	/// Locates a unique <see cref="RobinEpple.Common.Forms.Nodes.IBooleanNode"/> with the given <paramref name="name"/><br/>
	/// in the current scope (within the direct parent of the target node, if not specified otherwise)<br/>
	/// and returns its value.
	/// </summary>
	/// <param name="name">The name of the node.</param>
	/// <returns>The value of the node, if it is found.</returns>
	/// <exception cref="NodeNotFoundException">When the field with the given name does not exist or is not of the desired type.</exception>
	public static IFormExpression<bool?> BooleanFieldValue(string name) =>
		GetNode<IBooleanNode>(name).Select(node => node.Value);

	/// <summary>
	/// Locates a unique <see cref="RobinEpple.Common.Forms.Nodes.IFileNode"/> with the given <paramref name="name"/><br/>
	/// in the current scope (within the direct parent of the target node, if not specified otherwise)<br/>
	/// and returns its file name.
	/// </summary>
	/// <param name="name">The name of the node.</param>
	/// <returns>The file name of the node, if it is found.</returns>
	/// <exception cref="NodeNotFoundException">When the field with the given name does not exist or is not of the desired type.</exception>
	public static IFormExpression<string?> FileFieldFileName(string name) =>
		GetNode<IFileNode>(name).Select(node => node.Value.FileName);

	/// <summary>
	/// Locates a unique <see cref="RobinEpple.Common.Forms.Nodes.IFileNode"/> with the given <paramref name="name"/><br/>
	/// in the current scope (within the direct parent of the target node, if not specified otherwise)<br/>
	/// and returns its file bytes.
	/// </summary>
	/// <param name="name">The name of the node.</param>
	/// <returns>The file bytes of the node, if it is found.</returns>
	/// <exception cref="NodeNotFoundException">When the field with the given name does not exist or is not of the desired type.</exception>
	public static IFormExpression<byte[]?> FileFieldFileContent(string name) =>
		GetNode<IFileNode>(name).Select(node => node.Value.FileContents);

	/// <summary>
	/// Locates a unique <see cref="RobinEpple.Common.Forms.Nodes.INumberNode"/> with the given <paramref name="name"/><br/>
	/// in the current scope (within the direct parent of the target node, if not specified otherwise)<br/>
	/// and returns its value.
	/// </summary>
	/// <param name="name">The name of the node.</param>
	/// <returns>The value of the node, if it is found.</returns>
	/// <exception cref="NodeNotFoundException">When the field with the given name does not exist or is not of the desired type.</exception>
	public static IFormExpression<decimal?> NumberFieldValue(string name) =>
		GetNode<INumberNode>(name).Select(node => node.Value);

	/// <summary>
	/// Locates a unique <see cref="RobinEpple.Common.Forms.Nodes.ITextNode"/> with the given <paramref name="name"/><br/>
	/// in the current scope (within the direct parent of the target node, if not specified otherwise)<br/>
	/// and returns its value.
	/// </summary>
	/// <param name="name">The name of the node.</param>
	/// <returns>The value of the node, if it is found.</returns>
	/// <exception cref="NodeNotFoundException">When the field with the given name does not exist or is not of the desired type.</exception>
	public static IFormExpression<string?> TextFieldValue(string name) =>
		GetNode<ITextNode>(name).Select(node => node.Value);

	/// <summary>
	/// Locates a unique <see cref="RobinEpple.Common.Forms.Nodes.ITimestampNode"/> with the given <paramref name="name"/><br/>
	/// in the current scope (within the closest (parent) form, if not specified otherwise)<br/>
	/// and returns its value.
	/// </summary>
	/// <param name="name">The name of the node.</param>
	/// <returns>The value of the node, if it is found.</returns>
	/// <exception cref="NodeNotFoundException">When the field with the given name does not exist or is not of the desired type.</exception>
	public static IFormExpression<DateTime?> TimestampFieldValue(string name) =>
		GetNode<ITimestampNode>(name).Select(node => node.Value);

	# endregion

	# region change of scope

	/// <summary>
	/// Returns the name of the scope container (the closest (parent) form, if not specified otherwise).<br/>
	/// This can be used to determine what template has been instantiated.
	/// </summary>
	/// <returns>The name of the current scope</returns>
	public static IFormExpression<string> ScopeName() => new ScopeNameExpression();

	/// <summary>
	/// Locates a unique <see cref="RobinEpple.Common.Forms.Nodes.ITemplateNode"/> with the given <paramref name="name"/><br/>
	/// in the current scope (within the closest (parent) form, if not specified otherwise)<br/>
	/// and executes the specified <paramref name="expression"/> in the scope of its instance.
	/// </summary>
	/// <typeparam name="TValue">The value of the inner expression.</typeparam>
	/// <param name="name">The name of the node.</param>
	/// <param name="expression">The expression that is targeted at the instance.</param>
	/// <returns>The value of the <paramref name="expression"/>, given that the node is found and has an instance.</returns>
	/// <exception cref="NodeNotFoundException">When the section does not exist or does not have an instance.</exception>
	public static IFormExpression<TValue> InTemplatedSection<TValue>(string name, IFormExpression<TValue> expression) =>
		new InTemplatedSectionExpression<TValue>(name, expression);

	/// <summary>
	/// Locates a unique <see cref="RobinEpple.Common.Forms.Nodes.ICollectionNode"/> with the given <paramref name="name"/><br/>
	/// in the current scope (within the closest (parent) form, if not specified otherwise)<br/>
	/// and executes the specified <paramref name="expression"/> in the scope of each of its instances.
	/// </summary>
	/// <typeparam name="TValue">The value of the inner expression.</typeparam>
	/// <param name="name">The name of the node.</param>
	/// <param name="expression">The expression that is targeted at each instance.</param>
	/// <returns>The list of values the <paramref name="expression"/> yields on each instance, given that the node is found.</returns>
	public static IFormExpression<IEnumerable<TValue>> ForEachCollectionItem<TValue>(
		string name,
		IFormExpression<TValue> expression
	) => new ForEachCollectionItemExpression<TValue>(name, expression);

	/// <summary>
	/// Executes the specified <paramref name="expression"/> in an elevated scope.
	/// </summary>
	/// <typeparam name="TValue">The value of the inner expression.</typeparam>
	/// <param name="numberOfScopes">The number of scopes to go up. Index starts from 1.</param>
	/// <param name="expression">The expression that is targeted at the elevated scope.</param>
	/// <returns>The value of the <paramref name="expression"/>.</returns>
	/// <exception cref="NodeNotFoundException">When there are less parent scopes than the given <paramref name="numberOfScopes"/> suggests.</exception>
	public static IFormExpression<TValue> Elevate<TValue>(int numberOfScopes, IFormExpression<TValue> expression) =>
		new ElevateExpression<TValue>(numberOfScopes, expression);

	/// <summary>
	/// Executes the specified <paramref name="expression"/> in the root scope of the entire form.
	/// </summary>
	/// <typeparam name="TValue">The value of the inner expression.</typeparam>
	/// <param name="expression">The expression that is targeted at the root scope.</param>
	/// <returns>The value of the <paramref name="expression"/>.</returns>
	public static IFormExpression<TValue> InRootScope<TValue>(IFormExpression<TValue> expression) =>
		throw new NotImplementedException();

	# endregion

	# region comparisons

	/// <summary>
	/// Checks whether the <paramref name="source"/> is smaller than the <paramref name="exclusiveUpperBound"/>.
	/// </summary>
	/// <typeparam name="TComparable">The type of the two values, that are compared.</typeparam>
	/// <param name="source">The source value that is supposed to be smaller.</param>
	/// <param name="exclusiveUpperBound">The exclusive upper bound for the source.</param>
	/// <returns><see langword="true"/> if it is smaller, <see langword="false"/> otherwise.</returns>
	public static IFormExpression<bool> SmallerThan<TComparable>(
		this IFormExpression<TComparable> source,
		IFormExpression<TComparable> exclusiveUpperBound
	)
		where TComparable : IComparable => throw new NotImplementedException();

	/// <summary>
	/// Checks whether the <paramref name="source"/> is smaller or equal to the <paramref name="inclusiveUpperBound"/>.
	/// </summary>
	/// <typeparam name="TComparable">The type of the two values, that are compared.</typeparam>
	/// <param name="source">The source value that is supposed to be smaller or equal.</param>
	/// <param name="inclusiveUpperBound">The inclusive upper bound for the source.</param>
	/// <returns><see langword="true"/> if it is smaller or equal, <see langword="false"/> otherwise.</returns>
	public static IFormExpression<bool> SmallerOrEqual<TComparable>(
		this IFormExpression<TComparable> source,
		IFormExpression<TComparable> inclusiveUpperBound
	)
		where TComparable : IComparable => throw new NotImplementedException();

	/// <summary>
	/// Checks whether the <paramref name="source"/> is equal to the <paramref name="target"/>.
	/// </summary>
	/// <typeparam name="TComparable">The type of the two values, that are compared.</typeparam>
	/// <param name="source">The source value that is supposed to be equal.</param>
	/// <param name="target">The target value, the source is supposed to match.</param>
	/// <returns><see langword="true"/> if the two values are equal, <see langword="false"/> otherwise.</returns>
	public static IFormExpression<bool> EqualTo<TComparable>(
		this IFormExpression<TComparable> source,
		IFormExpression<TComparable> target
	)
		where TComparable : IComparable => throw new NotImplementedException();

	/// <summary>
	/// Checks whether the <paramref name="source"/> is bigger or equal to the <paramref name="inclusiveLowerBound"/>.
	/// </summary>
	/// <typeparam name="TComparable">The type of the two values, that are compared.</typeparam>
	/// <param name="source">The source value that is supposed to be bigger or equal.</param>
	/// <param name="inclusiveLowerBound">The inclusive lower bound for the source.</param>
	/// <returns><see langword="true"/> if the source is bigger or equal, <see langword="false"/> otherwise.</returns>
	public static IFormExpression<bool> BiggerOrEqual<TComparable>(
		this IFormExpression<TComparable> source,
		IFormExpression<TComparable> inclusiveLowerBound
	)
		where TComparable : IComparable => throw new NotImplementedException();

	/// <summary>
	/// Checks whether the <paramref name="source"/> is bigger than the <paramref name="exclusiveLowerBound"/>.
	/// </summary>
	/// <typeparam name="TComparable">The type of the two values, that are compared.</typeparam>
	/// <param name="source">The source value that is supposed to be bigger.</param>
	/// <param name="exclusiveLowerBound">The exclusive lower bound for the source.</param>
	/// <returns><see langword="true"/> if the source is bigger, <see langword="false"/> otherwise.</returns>
	public static IFormExpression<bool> BiggerThan<TComparable>(
		this IFormExpression<TComparable> source,
		IFormExpression<TComparable> exclusiveLowerBound
	)
		where TComparable : IComparable => throw new NotImplementedException();

	/// <summary>
	/// Finds the smallest value in the list.
	/// </summary>
	/// <param name="items">The list of items.</param>
	/// <returns>The smallest value or <see langword="null"/> if the sequence is empty.</returns>
	public static IFormExpression<TComparable?> Min<TComparable>(
		this IFormExpression<IEnumerable<TComparable>> items
	) => throw new NotImplementedException();

	/// <summary>
	/// Finds the biggest value in the list.
	/// </summary>
	/// <param name="items">The list of items.</param>
	/// <returns>The biggest value or <see langword="null"/> if the sequence is empty.</returns>
	public static IFormExpression<TComparable?> Max<TComparable>(
		this IFormExpression<IEnumerable<TComparable>> items
	) => throw new NotImplementedException();

	/// <summary>
	/// Finds the value that sits in the middle of an ordered list.<br/>
	/// If the list has an even number of items, the default bias will pick the smaller item, <br/>
	/// but that can be overwritten with <paramref name="preferBigger"/>.
	/// </summary>
	/// <param name="items">The list of items.</param>
	/// <param name="preferBigger">Defines the bias if there is an even number of <paramref name="items"/>.</param>
	/// <returns>The median or <see langword="null"/> if the sequence is empty.</returns>
	public static IFormExpression<TComparable?> Median<TComparable>(
		this IFormExpression<IEnumerable<TComparable>> items,
		bool preferBigger = false
	) => throw new NotImplementedException();

	# endregion

	# region number calculations

	/// <summary>
	/// Adds the <paramref name="right"/> value onto the <paramref name="left"/> and returns the result.
	/// </summary>
	/// <param name="left">The base number that is added to.</param>
	/// <param name="right">The number that is added.</param>
	/// <returns>The sum both values.</returns>
	public static IFormExpression<decimal> Add(this IFormExpression<decimal> left, IFormExpression<decimal> right) =>
		throw new NotImplementedException();

	/// <summary>
	/// Calculates the sum of all <paramref name="summands"/>.
	/// </summary>
	/// <param name="summands">The list of numbers that are added.</param>
	/// <returns>The sum of all values.</returns>
	public static IFormExpression<decimal> Sum(this IFormExpression<IEnumerable<decimal>> summands) =>
		throw new NotImplementedException();

	/// <summary>
	/// Subtracts the <paramref name="right"/> value from the <paramref name="left"/> and returns the result.
	/// </summary>
	/// <param name="left">The base number that is subtracted from.</param>
	/// <param name="right">The number that is subtracted.</param>
	/// <returns>The remainder.</returns>
	public static IFormExpression<decimal> Subtract(
		this IFormExpression<decimal> left,
		IFormExpression<decimal> right
	) => throw new NotImplementedException();

	/// <summary>
	/// Multiplies the <paramref name="target"/> with the <paramref name="factor"/> and returns the result.
	/// </summary>
	/// <param name="target">The base number that is multiplied.</param>
	/// <param name="factor">The factor.</param>
	/// <returns>The product of all values.</returns>
	public static IFormExpression<decimal> MultiplyBy(
		this IFormExpression<decimal> target,
		IFormExpression<decimal> factor
	) => throw new NotImplementedException();

	/// <summary>
	/// Multiplies the all <paramref name="factors"/> and returns the result.
	/// </summary>
	/// <param name="factors">The list of factors.</param>
	/// <returns>The product of all values.</returns>
	public static IFormExpression<decimal> Multiply(this IFormExpression<IEnumerable<decimal>> factors) =>
		throw new NotImplementedException();

	/// <summary>
	/// Divides the <paramref name="target"/> by the <paramref name="factor"/> and returns the result.
	/// </summary>
	/// <param name="target">The base number that is divided.</param>
	/// <param name="factor">The factor.</param>
	/// <returns>The remainder.</returns>
	public static IFormExpression<decimal> DivideBy(
		this IFormExpression<decimal> target,
		IFormExpression<decimal> factor
	) => throw new NotImplementedException();

	/// <summary>
	/// Strips the <paramref name="target"/> of comma values and returns only the integer part.
	/// </summary>
	/// <param name="target">The number that is converted to an integer.</param>
	/// <returns>The integer part of the target.</returns>
	public static IFormExpression<int> CastInt(this IFormExpression<decimal> target) =>
		throw new NotImplementedException();

	/// <summary>
	/// Converts the integer <paramref name="target"/> into a decimal that can hold comma values.
	/// </summary>
	/// <param name="target">The number that is converted to a decimal.</param>
	/// <returns>The casted number.</returns>
	public static IFormExpression<decimal> CastDecimal(this IFormExpression<int> target) =>
		throw new NotImplementedException();

	/// <summary>
	/// Returns the closest int bigger than the <paramref name="target"/>.<br/>
	/// Keeps the decimal format for further operations.
	/// </summary>
	/// <param name="target">The number that is rounded up.</param>
	/// <returns>The integer ceiling.</returns>
	public static IFormExpression<decimal> Ceil(this IFormExpression<decimal> target) =>
		throw new NotImplementedException();

	/// <summary>
	/// Returns the closest int smaller than the <paramref name="target"/>.<br/>
	/// Keeps the decimal format for further operations.
	/// </summary>
	/// <param name="target">The number that is rounded down.</param>
	/// <returns>The integer floor.</returns>
	public static IFormExpression<decimal> Floor(this IFormExpression<decimal> target) =>
		throw new NotImplementedException();

	/// <summary>
	/// Returns the closest int to the <paramref name="target"/>.<br/>
	/// Keeps the decimal format for further operations.
	/// </summary>
	/// <param name="target">The number that is rounded.</param>
	/// <returns>The rounded value.</returns>
	public static IFormExpression<decimal> Round(this IFormExpression<decimal> target) =>
		throw new NotImplementedException();

	/// <summary>
	/// Divides the <paramref name="target"/> by the <paramref name="field"/> and returns the rest.
	/// </summary>
	/// <param name="target">The base number that may be bigger than the field.</param>
	/// <param name="field">The field, that the number is represented in.</param>
	/// <returns>The division rest.</returns>
	public static IFormExpression<int> Modulo(this IFormExpression<int> target, IFormExpression<int> field) =>
		throw new NotImplementedException();

	/// <summary>
	/// Calculates the average of the given number list.
	/// </summary>
	/// <param name="items">List of numbers.</param>
	/// <returns>The average of the given numbers.</returns>
	public static IFormExpression<decimal> Average(this IFormExpression<IEnumerable<decimal>> items) =>
		throw new NotImplementedException();

	# endregion

	# region date calculations

	/// <summary>
	/// Moves the <paramref name="target"/> date forward in time by the given <paramref name="timeSpan"/>.
	/// </summary>
	/// <param name="target">The original date.</param>
	/// <param name="timeSpan">The time span that the date is moved by.</param>
	/// <returns>The moved date.</returns>
	public static IFormExpression<DateTime> Add(
		this IFormExpression<DateTime> target,
		IFormExpression<TimeSpan> timeSpan
	) => throw new NotImplementedException();

	/// <summary>
	/// Moves the <paramref name="target"/> date backward in time by the given <paramref name="timeSpan"/>.
	/// </summary>
	/// <param name="target">The original date.</param>
	/// <param name="timeSpan">The time span that the date is moved by.</param>
	/// <returns>The moved date.</returns>
	public static IFormExpression<DateTime> Subtract(
		this IFormExpression<DateTime> target,
		IFormExpression<TimeSpan> timeSpan
	) => throw new NotImplementedException();

	/// <summary>
	/// Calculates the difference between the two dates.
	/// </summary>
	/// <param name="start">The start date.</param>
	/// <param name="end">The end date.</param>
	/// <returns>The time span between them.</returns>
	public static IFormExpression<TimeSpan> Difference(
		this IFormExpression<DateTime> start,
		IFormExpression<DateTime> end
	) => throw new NotImplementedException();

	/// <summary>
	/// Multiplies the <paramref name="target"/> with the given <paramref name="factor"/>.
	/// </summary>
	/// <param name="target">The original time span.</param>
	/// <param name="factor">The factor the target is multiplied with.</param>
	/// <returns>The extended time span.</returns>
	public static IFormExpression<TimeSpan> MultiplyBy(
		this IFormExpression<TimeSpan> target,
		IFormExpression<decimal> factor
	) => throw new NotImplementedException();

	/// <summary>
	/// Divides the <paramref name="target"/> by the given <paramref name="factor"/>.
	/// </summary>
	/// <param name="target">The original time span.</param>
	/// <param name="factor">The factor the target is divided by.</param>
	/// <returns>The shortened time span.</returns>
	public static IFormExpression<TimeSpan> DivideBy(
		this IFormExpression<TimeSpan> target,
		IFormExpression<decimal> factor
	) => throw new NotImplementedException();

	# endregion
}
