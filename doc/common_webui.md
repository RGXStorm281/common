# RobinEpple.Common.WebUi

([back to readme](../readme.md))

This project collects ASP.NET specific helpers. This includes a couple of convenience extensions for handling dependency injection:

- `services.AddAlias<TAlias, TImplementation>()` will register a transient of type `TAlias` that requests the implementation `TImplementation` on every call. It's basically a routing redirect for dependency injection.
- `services.AddSettings<TSettings>(configuration)` will look for an equally named section in the configuration, parse the settings object and register it as singleton in the DI. It also returns the settings object for further use during configuration.

The bigger and more important part is a default binding implementation for my form framework. It exposes an extension method to bind a web request to a form.

```C#
await Request.BindAsync(form)
```

There is a second parameter allows to pass custom binding strategies, so you can add your own implementations of `IFormBindingStrategy`. When omitted, the listed ones in `FormBinder.DefaultBindingStrategies` will be used. These default strategies can handle all input types offered by `Common.Forms.Html`.
