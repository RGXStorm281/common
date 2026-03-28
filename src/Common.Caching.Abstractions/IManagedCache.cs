namespace RobinEpple.Common.Caching.Abstractions;

using System;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// The abstraction interface for a managed cache implementation,
/// that purges cached items based on inactivity or expiry timers.
/// </summary>
public interface IManagedCache
{
	/// <summary>
	/// Registers an object under the given key.
	/// </summary>
	/// <typeparam name="TObject">The type of the object that is cached.</typeparam>
	/// <param name="key">The key.</param>
	/// <param name="model">The object, that is cached.</param>
	/// <param name="inactivityGracePeriod">The minimum timespan this object should be kept alive without interaction. It might live longer.</param>
	/// <param name="maxLifeSpan">
	/// Optional maximum lifespan, after that the object is cleared from the cache and needs to be reloaded.
	/// The time span is calculated from the time the object has been cached.
	/// </param>
	/// <param name="replaceExisting">If set to <see langword="true"/>, any existing model under the same entry is removed before caching.</param>
	/// <returns><see langword="true"/> if the model was cached successfully.</returns>
	public bool TryCache<TObject>(
		string key,
		TObject model,
		TimeSpan inactivityGracePeriod,
		TimeSpan? maxLifeSpan = null,
		bool replaceExisting = false
	)
		where TObject : notnull;

	/// <summary>
	/// Tries to get an object from the cache. This counts as visit of the cache entry and therefore resets the inactivity timer.
	/// </summary>
	/// <typeparam name="TObject">The expected type of the object requested from the cache.</typeparam>
	/// <param name="key">The key for the object.</param>
	/// <param name="model">The object, if one is registered.</param>
	/// <returns><see langword="true"/>, if a cache entry with matching type was found.</returns>
	public bool TryGet<TObject>(string key, [NotNullWhen(true)] out TObject? model)
		where TObject : notnull;

	/// <summary>
	/// Resets the inactivity timer for the cache entry stored under the given key.
	/// This only affects the usage timeout and does not count as an "update" that would increase the maximum life span.
	/// </summary>
	/// <param name="key">The key for the cache entry.</param>
	/// <param name="inactivityGracePeriod">Optional new inactivity timeout span.</param>
	public void Revalidate(string key, TimeSpan? inactivityGracePeriod = null);

	/// <summary>
	/// Removes any cache entry stored in the given key.
	/// </summary>
	/// <param name="key">The key for the model.</param>
	public void Remove(string key);
}
