namespace RobinEpple.Common.WebUi;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RobinEpple.Common.Util;

/// <summary>
/// Contains convenience methods for adding the services defined in this library to the dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Adds a hosted service implementing <see cref="ITimeoutCache"/> that purges expired entries repeatedly after the given interval time span.
	/// </summary>
	/// <param name="services">The service collection to add the hosted service to.</param>
	/// <param name="purgeInterval">The time between the entry purges.</param>
	public static void AddTimeoutCache(this IServiceCollection services, TimeSpan purgeInterval)
	{
		services.AddSingleton(_ => new TimeoutCache(purgeInterval));
		services.AddAlias<ITimeoutCache, TimeoutCache>();
		services.AddHostedService(provider => provider.GetRequiredService<TimeoutCache>());
	}

	/// <summary>
	/// Adds a type alias for a registered service.
	/// Every time <typeparamref name="TAlias"/> is requested, the registered service for
	/// <typeparamref name="TImplementation"/> will be returned instead.
	/// </summary>
	/// <param name="services">The service collection to register the alias in.</param>
	public static void AddAlias<TAlias, TImplementation>(this IServiceCollection services)
		where TImplementation : TAlias
		where TAlias : class
	{
		services.AddTransient<TAlias>(provider => provider.GetRequiredService<TImplementation>());
	}

	/// <summary>
	/// Reads the settings from the configuration and adds them as singleton to the dependency injection.
	/// </summary>
	/// <typeparam name="TSettings">The type of settings to read from the configuration.</typeparam>
	/// <param name="services">The services to add the settings to.</param>
	/// <param name="configuration">The configuration to read the settings from.</param>
	public static TSettings AddSettings<TSettings>(this IServiceCollection services, IConfiguration configuration)
		where TSettings : class
	{
		var settingsName = typeof(TSettings).Name;
		var section = configuration.GetSection(settingsName);
		var settings = section.Get<TSettings>();
		if (settings == null)
		{
			throw new InvalidOperationException(
				$"Settings '{settingsName}' could not be loaded from the configuration."
			);
		}
		services.AddSingleton(settings);
		return settings;
	}
}
