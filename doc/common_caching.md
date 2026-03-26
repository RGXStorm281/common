# RobinEpple.Common.Caching

([back to readme](../readme.md))

This project contains a simple cache implementation, that keeps track of expiry dates for all cached items and removes them in time.

It is implemented in form of a hosted service, such that it can run in the background of any application.
Simply register it on startup with a purge interval (how often expiry timestamps are checked).

```C#
services.AddManagedCache(TimeSpan.FromSeconds(30));
```

After that, any business logic can request the interface `IManagedCache` from the abstractions package (no dependencies) and store typed objects under a string key.

```C#
if (!_cache.TryGet<MyModel>(modelKey, out var model))
{
    model = new MyModel();
    _cache.TryCache(modelKey, model, TimeSpan.FromMinutes(5));
}
```

It is required to enter an inactivity time span, that allows the cache to delete the item when it hasn't been accessed for long enough. Optionally an additional maximum lifetime (not reset on access) can be set when a model is cached.

```C#
_cache.TryCache(modelKey, model, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(60));
```

You should always write the business logic in a way that handles a cache miss though, like in the `TryGet` example. This cache is meant to speed up loading and temporarily store intermediate states, but you should never rely on it for permanent storage, because cached items will be purged eventually.
