namespace RobinEpple.Common.Util;

using System.Diagnostics.CodeAnalysis;

public static class CollectionExtensions
{
	#region Enumerables

	/// <summary>
	/// Returns the same collection but with the new item added as first element.
	/// </summary>
	/// <typeparam name="TElement">The type of elements in the collection.</typeparam>
	/// <param name="collection">The collection.</param>
	/// <param name="newItem">The new element.</param>
	/// <returns>The extended collection.</returns>
	public static IEnumerable<TElement> WithPrepended<TElement>(this IEnumerable<TElement> collection, TElement newItem)
	{
		yield return newItem;

		foreach (var element in collection)
		{
			yield return element;
		}
	}

	/// <summary>
	/// Returns the same collection but with the new item added as last element.
	/// </summary>
	/// <typeparam name="TElement">The type of elements in the collection.</typeparam>
	/// <param name="collection">The collection.</param>
	/// <param name="newItem">The new element.</param>
	/// <returns>The extended collection.</returns>
	public static IEnumerable<TElement> WithAppended<TElement>(this IEnumerable<TElement> collection, TElement newItem)
	{
		foreach (var element in collection)
		{
			yield return element;
		}

		yield return newItem;
	}

	/// <summary>
	/// Returns the same collection but with the specified item removed.
	/// </summary>
	/// <typeparam name="TElement">The type of elements in the collection.</typeparam>
	/// <param name="collection">The collection.</param>
	/// <param name="obsoleteItem">The item to remove.</param>
	/// <param name="comparer">Optional comparer to define when two elements are equal.</param>
	/// <returns>The reduced collection.</returns>
	public static IEnumerable<TElement> WithRemoved<TElement>(
		this IList<TElement> collection,
		TElement obsoleteItem,
		IEqualityComparer<TElement>? comparer = null
	)
	{
		foreach (var element in collection)
		{
			// Dedicated comparer.
			if (comparer != null)
			{
				if (comparer.Equals(element, obsoleteItem))
				{
					continue;
				}
				else
				{
					yield return element;
					continue;
				}
			}

			// Use default equals implementation.
			if (obsoleteItem == null && element == null)
			{
				// both are null -> do not return.
				continue;
			}

			if (element == null)
			{
				// unequal -> return this item.
				yield return element;
				continue;
			}

			if (element.Equals(obsoleteItem))
			{
				// equal -> do not return.
				continue;
			}

			// unequal -> return this item.
			yield return element;
		}
	}

	/// <summary>
	/// Returns <see langword="true"/> when none of the items in the collection meet the condition.
	/// </summary>
	/// <typeparam name="TElement">The type of elements in the collection.</typeparam>
	/// <param name="collection">The collection.</param>
	/// <param name="condition">The condition, that the items are tested for.</param>
	/// <returns><see langword="true"/>, when none of the elements meet the condition.</returns>
	public static bool None<TElement>(this IEnumerable<TElement> collection, Func<TElement, bool> condition) =>
		collection.All(item => !condition(item));

	/// <summary>
	/// Immediately aggregates the collection to a list after transforming each element with the selector function.
	/// </summary>
	/// <typeparam name="TSource">The element type in the source collection.</typeparam>
	/// <typeparam name="TResult">The result type of the collection in the new list.</typeparam>
	/// <param name="collection">The source collection.</param>
	/// <param name="selector">The transformation function.</param>
	/// <returns>The new list.</returns>
	[return: NotNullIfNotNull(nameof(collection))]
	public static List<TResult>? ToList<TSource, TResult>(
		this IEnumerable<TSource>? collection,
		Func<TSource, TResult> selector
	) => collection?.Select(selector).ToList();

	/// <summary>
	/// Deep clones the collection by cloning each element.
	/// </summary>
	/// <typeparam name="TElement">The element type in the source collection.</typeparam>
	/// <param name="collection">The source collection.</param>
	/// <returns>The new collection with the cloned elements.</returns>
	[return: NotNullIfNotNull(nameof(collection))]
	public static IEnumerable<TElement>? CloneAll<TElement>(this IEnumerable<TElement>? collection)
		where TElement : ICloneable => collection?.Select(element => (TElement)element.Clone()).ToList();

	#endregion

	#region Dictionaries

	/// <summary>
	/// Similar to <see cref="Enumerable.ToLookup{TSource,TKey}(IEnumerable{TSource},Func{TSource,TKey})"/>, this method groups items with the same key.<br/>
	/// In contrast to the before mentioned method however, the lists-in-a-dictionary structure created by this method can be modified after its creation.
	/// </summary>
	/// <typeparam name="TElement">The type of elements in the source collection.</typeparam>
	/// <typeparam name="TKey">The key type the elements are grouped by.</typeparam>
	/// <param name="collection">The source collection.</param>
	/// <param name="keySelector">The selector function, sourcing the key from each item.</param>
	/// <param name="comparer">An optional comparer for defining the equality of two keys.</param>
	/// <returns>The grouped elements.</returns>
	public static Dictionary<TKey, List<TElement>> ToMultiDict<TElement, TKey>(
		this IEnumerable<TElement> collection,
		Func<TElement, TKey> keySelector,
		IEqualityComparer<TKey>? comparer = null
	)
		where TKey : notnull => collection.ToMultiDict(keySelector, element => element, comparer);

	/// <summary>
	/// Similar to <see cref="Enumerable.ToLookup{TSource,TKey}(IEnumerable{TSource},Func{TSource,TKey})"/>, this method groups items with the same key.<br/>
	/// In contrast to the before mentioned method however, the lists-in-a-dictionary structure created by this method can be modified after its creation.
	/// </summary>
	/// <typeparam name="TElement">The type of elements in the source collection.</typeparam>
	/// <typeparam name="TKey">The key type the elements are grouped by.</typeparam>
	/// <typeparam name="TValue">The value type in the groupings.</typeparam>
	/// <param name="collection">The source collection.</param>
	/// <param name="keySelector">The selector function, sourcing the key from each item.</param>
	/// <param name="valueSelector">A transformation function to translate each item in the source collection into the desired format for the groupings.</param>
	/// <param name="comparer">An optional comparer for defining the equality of two keys.</param>
	/// <returns>The grouped elements.</returns>
	public static Dictionary<TKey, List<TValue>> ToMultiDict<TElement, TKey, TValue>(
		this IEnumerable<TElement> collection,
		Func<TElement, TKey> keySelector,
		Func<TElement, TValue> valueSelector,
		IEqualityComparer<TKey>? comparer = null
	)
		where TKey : notnull =>
		collection
			.ToLookup(keySelector, valueSelector, comparer)
			.ToDictionary(grouping => grouping.Key, grouping => grouping.ToList(), comparer);

	/// <summary>
	/// Only replaces a value in the dictionary, but never creates a new entry.
	/// </summary>
	/// <typeparam name="TKey">The key type of the dictionary.</typeparam>
	/// <typeparam name="TValue">The value type of the dictionary.</typeparam>
	/// <param name="dictionary">The dictionary.</param>
	/// <param name="key">The key, that the item will be replaced for.</param>
	/// <param name="value">The new value.</param>
	/// <returns><see langword="true"/>, when a value got replaced.</returns>
	public static bool TrySetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
	{
		if (dictionary.ContainsKey(key))
		{
			dictionary[key] = value;
			return true;
		}

		return false;
	}

	/// <summary>
	/// If a value is already present for the key, it is returned.<br/>
	/// Otherwise, a new record is created, before returning it.<br/>
	/// Attention: This imitates the method of the <see cref="System.Collections.Concurrent.ConcurrentDictionary{TKey, TValue}"/>, but is NOT threadsafe!!
	/// </summary>
	/// <typeparam name="TKey">The key type of the dictionary.</typeparam>
	/// <typeparam name="TValue">The value type of the dictionary.</typeparam>
	/// <param name="dictionary">The dictionary.</param>
	/// <param name="key">The key type, that the value is searched for.</param>
	/// <param name="createNew">The constructor function, that is called, if a new value needs to be created for the given key for the given key.</param>
	/// <returns>The value for the key.</returns>
	public static TValue GetOrAdd<TKey, TValue>(
		this IDictionary<TKey, TValue> dictionary,
		TKey key,
		Func<TValue> createNew,
		out bool created
	)
	{
		created = false;
		if (!dictionary.ContainsKey(key))
		{
			dictionary[key] = createNew();
			created = true;
		}

		return dictionary[key];
	}

	#endregion
}
