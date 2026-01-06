using RobinEpple.Common.WebUi.Test;

// Create builder.
var builder = WebApplication.CreateBuilder(args);

// Setup configuration files.
var rootPath = builder.Environment.ContentRootPath;
var configFolder = Path.Combine(rootPath, "_config");
var appsettingsFile = Path.Combine(configFolder, "appsettings.json");
builder.Configuration.AddJsonFile(appsettingsFile, optional: false, reloadOnChange: false);

// Load settings.
var settings = Startup.AddSettings(builder.Services, builder.Configuration);

// Setup dependency injection.
Startup.ConfigureServices(builder.Services, builder.Environment, settings.TestSettings);

// Configure hosting properties.
builder.WebHost.ConfigureKestrel(options =>
{
	options.Limits.MaxRequestLineSize = 10000;
	options.Limits.MaxRequestBufferSize = 32768;
});

// Configure pipeline.
var app = builder.Build();
Startup.ConfigurePipeline(app, builder.Environment);

// Run application.
app.Run();
