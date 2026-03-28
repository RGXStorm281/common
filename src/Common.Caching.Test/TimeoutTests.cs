namespace RobinEpple.Common.Caching;

[TestClass]
public class TimeoutTests
{
	private static readonly TimeSpan _purgeInterval = TimeSpan.FromSeconds(1);
	private static readonly TimeSpan _inactivityGracePeriod = TimeSpan.FromSeconds(5);
	private const string _key1 = "key1";
	private const string _value1 = "value1";

	[TestMethod]
	public void CachedItems_ShouldExpireAfterInactivityGracePeriod()
	{
		// Initialize cache.
		using var cache = new ManagedCache(_purgeInterval);
		cache.StartAsync(CancellationToken.None).Wait();

		// Cache an item.
		cache.TryCache(_key1, _value1, _inactivityGracePeriod);

		// Make sure the item is cached.
		cache.TryGet<string>(_key1, out var cachedValue);
		Assert.AreEqual(_value1, cachedValue, "Cached item should be retrievable immediately after caching.");

		// Wait for the expiry.
		Thread.Sleep((int)_inactivityGracePeriod.Add(_purgeInterval * 2).TotalMilliseconds);

		// Now the item should be removed.
		Assert.IsFalse(cache.TryGet<string>(_key1, out _), "Cached item should expire without interaction.");
	}

	[TestMethod]
	public void CachedItems_ShouldNotExpireWhileInActiveUse()
	{
		// Initialize cache.
		using var cache = new ManagedCache(_purgeInterval);
		cache.StartAsync(CancellationToken.None).Wait();

		// Cache an item.
		cache.TryCache(_key1, _value1, _inactivityGracePeriod);

		// Make sure the item is cached.
		cache.TryGet<string>(_key1, out var cachedValue);
		Assert.AreEqual(_value1, cachedValue, "Cached item should be retrievable immediately after caching.");

		// Wait half the inactivity grace period, then access the item to reset the inactivity timer.
		var halfInactivityPeriod = _inactivityGracePeriod / 2;
		Thread.Sleep((int)halfInactivityPeriod.TotalMilliseconds);
		Assert.IsTrue(
			cache.TryGet<string>(_key1, out _),
			"Cached item should be retrievable after half the inactivity grace period."
		);

		// Wait the rest of the inactivity period and a little extra, the item should still be available.
		Thread.Sleep((int)halfInactivityPeriod.Add(_purgeInterval * 2).TotalMilliseconds / 2);
		Assert.IsTrue(cache.TryGet<string>(_key1, out _), "Inactivity timer should be reset on access.");

		// The second access has revalidated again -> wait full time for the expiry.
		Thread.Sleep((int)_inactivityGracePeriod.Add(_purgeInterval * 2).TotalMilliseconds);

		// Now the item should be removed.
		Assert.IsFalse(cache.TryGet<string>(_key1, out _), "Cached item should expire without interaction.");
	}

	[TestMethod]
	public void CachedItems_ShouldNotExpireWhenRevalidated()
	{
		// Initialize cache.
		using var cache = new ManagedCache(_purgeInterval);
		cache.StartAsync(CancellationToken.None).Wait();

		// Cache an item.
		cache.TryCache(_key1, _value1, _inactivityGracePeriod);

		// Make sure the item is cached.
		cache.TryGet<string>(_key1, out var cachedValue);
		Assert.AreEqual(_value1, cachedValue, "Cached item should be retrievable immediately after caching.");

		// Wait half the inactivity grace period, then access the item to reset the inactivity timer.
		var halfInactivityPeriod = _inactivityGracePeriod / 2;
		Thread.Sleep((int)halfInactivityPeriod.TotalMilliseconds);
		cache.Revalidate(_key1);

		// Wait the rest of the inactivity period and a little extra, the item should still be available.
		Thread.Sleep((int)halfInactivityPeriod.Add(_purgeInterval * 2).TotalMilliseconds / 2);
		Assert.IsTrue(cache.TryGet<string>(_key1, out _), "Inactivity timer should be reset on access.");

		// The second access has revalidated again -> wait full time for the expiry.
		Thread.Sleep((int)_inactivityGracePeriod.Add(_purgeInterval * 2).TotalMilliseconds);

		// Now the item should be removed.
		Assert.IsFalse(cache.TryGet<string>(_key1, out _), "Cached item should expire without interaction.");
	}

	[TestMethod]
	public void CachedItems_ShouldRespectMaxLifespan()
	{
		// Initialize cache.
		using var cache = new ManagedCache(_purgeInterval);
		cache.StartAsync(CancellationToken.None).Wait();

		// Cache an item.
		var guaranteedExpiryTime = DateTime.UtcNow.Add(_inactivityGracePeriod);
		cache.TryCache(_key1, _value1, _inactivityGracePeriod, _inactivityGracePeriod);

		// Continuously access the item to prevent it from expiring due to inactivity, but it should still expire after the max lifespan.
		while (DateTime.UtcNow < guaranteedExpiryTime)
		{
			Assert.IsTrue(cache.TryGet<string>(_key1, out _), "Item should not expire while in active use.");
		}

		// Wait two more cycles of the purge interval to ensure the item is purged.
		Thread.Sleep((int)(_purgeInterval * 2).TotalMilliseconds);

		// Now the item should be removed.
		Assert.IsFalse(cache.TryGet<string>(_key1, out _), "Cached item should expire without interaction.");
	}
}
