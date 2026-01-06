namespace RobinEpple.Common.WebUi;

using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using RobinEpple.Common.Util;

public class TimeoutCache : ITimeoutCache, IHostedService
{
	private class CacheEntry(object item, TimeSpan timeout)
	{
		public DateTime LastAccessed { get; set; } = DateTime.UtcNow;

		public object Item { get; } = item;

		public TimeSpan Timeout { get; } = timeout;
	}

	private readonly ConcurrentDictionary<string, CacheEntry> _cache;
	private readonly TimeSpan _purgeInterval;
	private Timer? _purgeTimer;

	public TimeoutCache(TimeSpan purgeInterval)
	{
		_cache = [];
		_purgeInterval = purgeInterval;
	}

	/// <inheritdoc />
	public void Cache(string key, object item, TimeSpan timeout)
	{
		var entry = new CacheEntry(item, timeout);
		_cache.AddOrUpdate(key, entry, (_, _) => entry);
	}

	private void Purge(object? state)
	{
		var purgeTimestamp = DateTime.UtcNow;
		foreach (var key in _cache.Keys)
		{
			if (_cache.TryGetValue(key, out var entry) && entry.LastAccessed.Add(entry.Timeout) < purgeTimestamp)
			{
				_cache.TryRemove(key, out _);
			}
		}
	}

	/// <inheritdoc />
	public Task StartAsync(CancellationToken cancellationToken)
	{
		_purgeTimer = new Timer(Purge, null, TimeSpan.Zero, _purgeInterval);
		return Task.CompletedTask;
	}

	/// <inheritdoc />
	public Task StopAsync(CancellationToken cancellationToken)
	{
		_purgeTimer?.Change(Timeout.Infinite, 0);
		return Task.CompletedTask;
	}

	/// <inheritdoc />
	public bool TryGetValue(string key, [NotNullWhen(true)] out object? item, bool resetTimeout = true)
	{
		if (!_cache.TryGetValue(key, out var entry))
		{
			item = null;
			return false;
		}

		if (resetTimeout)
		{
			entry.LastAccessed = DateTime.UtcNow;
		}

		item = entry.Item;
		return true;
	}

	/// <inheritdoc />
	public bool TryGetValue<TItem>(string key, [NotNullWhen(true)] out TItem? item, bool resetTimeout = true)
		where TItem : class
	{
		if (!TryGetValue(key, out var cachedObject, resetTimeout))
		{
			item = null;
			return false;
		}

		if (cachedObject is not TItem correctType)
		{
			item = null;
			return false;
		}

		item = correctType;
		return true;
	}

	/// <inheritdoc />
	public bool TryGetValue<TItem>(string key, [NotNullWhen(true)] out TItem? item, bool resetTimeout = true)
		where TItem : struct
	{
		if (!TryGetValue(key, out var cachedObject, resetTimeout))
		{
			item = null;
			return false;
		}

		if (cachedObject is not TItem correctType)
		{
			item = null;
			return false;
		}

		item = correctType;
		return true;
	}
}
