namespace RobinEpple.Common.Caching;

using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using RobinEpple.Common.Caching.Abstractions;

/// <summary>
/// A managed cache implementation in form of a hosted service.
/// </summary>
/// <param name="purgeInterval">The time interval in between periodic purges of expired items.</param>
public class ManagedCache(TimeSpan purgeInterval) : IHostedService, IDisposable, IManagedCache
{
	private Timer? _timer;
	private readonly ConcurrentDictionary<string, ManagedCacheEntry> _cache = [];
	private readonly TimeSpan _purgeInterval = purgeInterval;

	#region Purge cycle

	/// <inheritdoc />
	public Task StartAsync(CancellationToken cancellationToken)
	{
		_timer = new Timer(PurgeExpiredCacheEntries, null, TimeSpan.Zero, _purgeInterval);

		return Task.CompletedTask;
	}

	private void PurgeExpiredCacheEntries(object? state)
	{
		var purgeTimestamp = DateTime.UtcNow;
		foreach (var entry in _cache.ToList())
		{
			// Purge if the cached entry hasn't been used for long enough.
			if (entry.Value.LastUtilization.Add(entry.Value.InactivityGracePeriod) < purgeTimestamp)
			{
				_cache.TryRemove(entry.Key, out _);
			}
			// Purge if the cached entry has exceeded it's lifetime and requires to be refreshed.
			else if (
				entry.Value.MaxLifeSpan is { } maxLifeSpan
				&& entry.Value.CachedAt.Add(maxLifeSpan) < purgeTimestamp
			)
			{
				_cache.TryRemove(entry.Key, out _);
			}
		}
	}

	/// <inheritdoc />
	public Task StopAsync(CancellationToken cancellationToken)
	{
		_timer?.Change(Timeout.Infinite, 0);

		return Task.CompletedTask;
	}

	/// <inheritdoc />
	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Internal implementation of dispose, that may be called multiple times
	/// but only once with <paramref name="disposing"/> = <see langword="true"/>.
	/// </summary>
	/// <param name="disposing">Whether the current call should dispose.</param>
	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			_timer?.Dispose();
		}
	}

	#endregion

	#region Model cache

	/// <inheritdoc />
	public bool TryCache<TObject>(
		string key,
		TObject model,
		TimeSpan inactivityGracePeriod,
		TimeSpan? maxLifeSpan = null,
		bool replaceExisting = false
	)
		where TObject : notnull
	{
		var entry = new ManagedCacheEntry(model, inactivityGracePeriod, maxLifeSpan);

		if (replaceExisting)
		{
			_cache.AddOrUpdate(key, _ => entry, (_, _) => entry);
			return true;
		}

		return _cache.TryAdd(key, entry);
	}

	/// <inheritdoc />
	public bool TryGet<TObject>(string key, [NotNullWhen(true)] out TObject? model)
		where TObject : notnull
	{
		model = default;

		if (!_cache.TryGetValue(key, out var entry))
		{
			return false;
		}

		if (entry.CachedObject is not TObject cachedModel)
		{
			return false;
		}

		model = cachedModel;
		entry.LastUtilization = DateTime.UtcNow;
		return true;
	}

	/// <inheritdoc />
	public void Revalidate(string key, TimeSpan? inactivityGracePeriod = null)
	{
		if (_cache.TryGetValue(key, out var entry))
		{
			entry.LastUtilization = DateTime.UtcNow;
		}
	}

	/// <inheritdoc />
	public void Remove(string key) => _cache.TryRemove(key, out _);

	#endregion
}
