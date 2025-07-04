namespace RobinEpple.Common.Util;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// An interface for a caching service, that takes care of managing cache entries and deleting them after a specified timeout.
/// </summary>
public interface ITimeoutCache
{
	/// <summary>
	/// Adds or overwrites a cache entry.
	/// </summary>
	/// <param name="key">The key the item can later be retrieved with. This needs to be unique across the entire cache!</param>
	/// <param name="item">The item to cache.</param>
	/// <param name="timeout">The timeout, after which the entry will be deleted.</param>
	public void Cache(string key, object item, TimeSpan timeout);

	/// <summary>
	/// Returns the cached item if one is registered under the given key.
	/// </summary>
	/// <param name="key">The key to retrieve the item for.</param>
	/// <param name="item">The item, if one is cached.</param>
	/// <param name="resetTimeout">Optional parameter specifying, whether this access should reset the deletion timer.</param>
	public bool TryGetValue(string key, [NotNullWhen(true)] out object? item, bool resetTimeout = true);

	/// <summary>
	/// Returns the cached item if one is registered under the given key. Additionally the cached item is only returned, if it is of the desired type.
	/// </summary>
	/// <param name="key">The key to retrieve the item for.</param>
	/// <param name="item">The item, if one is cached.</param>
	/// <param name="resetTimeout">Optional parameter specifying, whether this access should reset the deletion timer.</param>
	public bool TryGetValue<TItem>(string key, [NotNullWhen(true)] out TItem? item, bool resetTimeout = true)
		where TItem : class;

	/// <summary>
	/// Returns the cached item if one is registered under the given key. Additionally the cached item is only returned, if it is of the desired type.
	/// </summary>
	/// <param name="key">The key to retrieve the item for.</param>
	/// <param name="item">The item, if one is cached.</param>
	/// <param name="resetTimeout">Optional parameter specifying, whether this access should reset the deletion timer.</param>
	public bool TryGetValue<TItem>(string key, [NotNullWhen(true)] out TItem? item, bool resetTimeout = true)
		where TItem : struct;
}
