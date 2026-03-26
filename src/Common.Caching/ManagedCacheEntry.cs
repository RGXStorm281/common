namespace RobinEpple.Common.Caching;

using System;

internal class ManagedCacheEntry
{
	public ManagedCacheEntry(object cachedObject, TimeSpan inactivityGracePeriod, TimeSpan? maxLifeSpan = null)
	{
		CachedObject = cachedObject;
		InactivityGracePeriod = inactivityGracePeriod;

		var creationTimestamp = DateTime.UtcNow;
		LastUtilization = creationTimestamp;
		CachedAt = creationTimestamp;
		MaxLifeSpan = maxLifeSpan;
	}

	/// <summary>
	/// The object cached in this entry.
	/// </summary>
	public object CachedObject { get; }

	/// <summary>
	/// The minimum time the entry should be cached without being utilized.
	/// </summary>
	public TimeSpan InactivityGracePeriod { get; }

	/// <summary>
	/// The last time this entry has been utilized.
	/// </summary>
	public DateTime LastUtilization { get; set; }

	/// <summary>
	/// Optional maximum lifespan, after that the object is cleared from the cache and needs to be reloaded.
	/// The time span is calculated from the time the object has been cached.
	/// </summary>
	public TimeSpan? MaxLifeSpan { get; }

	/// <summary>
	/// The timestamp when the object was cached.
	/// </summary>
	public DateTime CachedAt { get; set; }
}
