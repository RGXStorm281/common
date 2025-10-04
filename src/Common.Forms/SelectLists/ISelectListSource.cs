namespace RobinEpple.Common.Forms.SelectLists;

using RobinEpple.Common.SourceGenerators.Abstractions;

/// <summary>
/// Defines the API for a source, selection options of form fields can be loaded from.
/// </summary>
/// <typeparam name="TValue">The value type of the selection item.</typeparam>
public partial interface ISelectListSource<TValue>
{
	/// <summary>
	/// Loads the list of options available given the current set of <paramref name="dependencies"/>.<br/>
	/// How the dependencies are defined an interpreted is up to the source implementation.
	/// </summary>
	/// <param name="dependencies">The dependency list.</param>
	/// <returns>The list of available selection options.</returns>
	[GenerateAsyncOverload]
	public IEnumerable<ISelectListItem<TValue>> LoadItems(IDictionary<string, object?>? dependencies = null);

	/// <summary>
	/// Creates a source for a static list of values.
	/// </summary>
	/// <param name="values">The list of values.</param>
	/// <returns>The select list source.</returns>
	public static ISelectListSource<TValue> ForValues(IEnumerable<TValue> values)
	{
		var items = values.Select(value => new SelectListItem<TValue>(value?.ToString() ?? string.Empty, value));
		return new StaticSelectListSource<TValue>(items);
	}

	/// <summary>
	/// Creates a source for a static list of values and their labels.
	/// </summary>
	/// <param name="labelledValues">The list of values and their corresponding label.</param>
	/// <returns>The select list source.</returns>
	public static ISelectListSource<TValue> ForLabelledValues(IEnumerable<(TValue Value, string Label)> labelledValues)
	{
		var items = labelledValues.Select(item => new SelectListItem<TValue>(item.Label, item.Value));
		return new StaticSelectListSource<TValue>(items);
	}
}
