namespace RobinEpple.Common.WebUi.Test;

using Microsoft.AspNetCore.Mvc.Infrastructure;
using RobinEpple.Common.Caching;
using RobinEpple.Common.WebUi.Test.Code;

public static class Startup
{
	public record SettingsContainer(TestSettings TestSettings);

	public static SettingsContainer AddSettings(IServiceCollection services, IConfiguration configuration)
	{
		TestSettings testSettings = services.AddSettings<TestSettings>(configuration);
		return new SettingsContainer(testSettings);
	}

	public static void ConfigureServices(
		IServiceCollection services,
		IWebHostEnvironment hostingEnvironment,
		TestSettings testSettings
	)
	{
		AddControllers(services, hostingEnvironment);
		AddSessionAndHttp(services, testSettings);
		services.AddManagedCache(TimeSpan.FromSeconds(30));
	}

	private static void AddControllers(IServiceCollection services, IWebHostEnvironment hostingEnvironment)
	{
		IMvcBuilder mvcBuilder = services
			.AddControllersWithViews()
			.AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

		if (hostingEnvironment.IsDevelopment())
		{
			mvcBuilder.AddRazorRuntimeCompilation();
		}
	}

	private static void AddSessionAndHttp(IServiceCollection services, TestSettings testSettings)
	{
		services.AddSession(options =>
		{
			options.Cookie.Name = $"{testSettings.AppId}_session";
		});
		services.AddHttpContextAccessor();
		services.AddCookiePolicy(options =>
		{
			options.OnAppendCookie = context =>
			{
				context.CookieOptions.Expires = DateTimeOffset.UtcNow.AddMinutes(testSettings.SessionLifetimeInMinutes);
			};
		});
		services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
		services.AddSingleton<HttpClient>();
	}

	public static void ConfigurePipeline(IApplicationBuilder app, IWebHostEnvironment env)
	{
		app.UseExceptionHandler(
			new ExceptionHandlerOptions { ExceptionHandlingPath = "/Home/Error", AllowStatusCode404Response = true }
		);

		if (!env.IsDevelopment())
		{
			app.UseHsts();
		}

		app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");

		app.UseHttpsRedirection();
		app.UseStaticFiles();

		app.UseRouting();
		app.UseSession();

		app.UseAuthentication();
		app.UseAuthorization();

		app.UseEndpoints(Routing.RegisterRoutes);
	}
}
