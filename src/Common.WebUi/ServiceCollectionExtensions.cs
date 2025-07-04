namespace RobinEpple.Common.WebUi;

using Microsoft.Extensions.DependencyInjection;
using RobinEpple.Common.Util;

public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Adds a hosted service implementing <see cref="ITimeoutCache"/> that purges expired entries repeatedly after the given interval time span.
	/// </summary>
	/// <param name="services">The service collection to add the hosted service to.</param>
	/// <param name="purgeInterval">The time between the entry purges.</param>
	public static void AddTimeoutCache(this IServiceCollection services, TimeSpan purgeInterval) =>
		services.AddHostedService(_ => new TimeoutCache(purgeInterval));
}
