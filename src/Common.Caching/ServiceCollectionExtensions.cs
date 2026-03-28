namespace RobinEpple.Common.Caching;

using System;
using Microsoft.Extensions.DependencyInjection;
using RobinEpple.Common.Caching.Abstractions;

/// <summary>
/// An extension method to add a managed cache hosted service to a service collection.
/// </summary>
public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Adds a managed cache as a hosted service, accessible from DI as <see cref="IManagedCache"/>.
	/// </summary>
	/// <param name="services">The service collection to add the managed cache to.</param>
	/// <param name="purgeInterval">The periodic time interval in that the cache is checked for and cleared of obsolete entries.</param>
	public static void AddManagedCache(this IServiceCollection services, TimeSpan purgeInterval)
	{
		var managedCache = new ManagedCache(purgeInterval);
		services.AddSingleton(managedCache);
		services.AddHostedService(provider => provider.GetRequiredService<ManagedCache>());
		services.AddTransient<IManagedCache>(provider => provider.GetRequiredService<ManagedCache>());
	}
}
