# Common.Forms

([back to readme](../../readme.md))

Nice, apparently I convinced you to check out the library documentation! Now I just have to make sure this is actually worth your time ^^.

Let me first explain the motivation for this project, because it might not be immediately obvious why one would write another framework, when there is an abundance of existing frameworks to choose from. To put it simply: I could not find one that would satisfy my absurd requirements

-\\\_(シ)\_/-

An input form can quickly become complex modelling task, when the state of a field depends on other fields in the form. For example showing a section only when a checkbox is checked, or choosing one of many options with different sub-forms (e.g. "choose a payment method"). So inter-field rules for both visibility and validation are an absolute must in my opinion, but this is already not expressible in most frameworks.

Let me give you an example: Have you ever thought about how you could realize a boolean expression builder to filter a grid in the web? This is a very complex task for such a small little feature:

1. Boolean expressions are recursive, with potentially endlessly deep nesting.
2. Each node in the expression tree can be a field reference, a binary or unary operator, ... So for each node you have multiple options to choose.
3. And depending on what you choose, the inner structure of the node looks different.

Modelling this in C# is easy. Just define a recursive structure with interfaces or inheritance - done. But have you ever seen a form framework that could not only render a state, but bind web requests, validate the structure and synchronize with a model? Well, congratulations, you found one :)

I hope this example showed you why achieving this task was not easy in the slightest. Or maybe I am just trying to justify the 30.000 lines of code and immense time effort... Nah, don't think about it.

Don't worry, I'll try to explain as simple as possible how this framework can be used. And using it is way easier than designing it, so much is for sure!

## Technical documentation

I have decided to put the technical documentation of the inner workings in a separate file [here](common_forms_in_depth.md). Check it out if you want a deeper understanding on how this thing operates. For everyone else who just wants to use it: continue reading :)

## Creating a form

Lets start at the example of a very basic login form with email and password.

```C#
Form = new FormBuilder("Login")
    .WithTextNode(
        "Email",
        username => username
            .UseLabel("E-Mail")
            .UseEmailValidator()
            .UsePropertyBinding(() => Model.Email)
    )
    .WithTextNode(
        "Password",
        password => password
            .UseLabel("Password")
            .UsePropertyBinding(() => Model.Password)
    )
    .Build();
```

A form is created with a `FormBuilder` instance, which you can add fields to by chaining method calls. Note, how all methods that add a structural node to the form are named "With...", and all methods that configure the current node start with "Use...". That way you can easily filter the autocompletion to what you're looking for.

Node names must start with a letter, and only allow alphanumeric characters plus "\_". This is for compatibility reasons with C# properties and HTML id's. If your node needs a more complex display name, set a dedicated label. Names are required to be unique among neighboring nodes.

The framework contains a bunch of default validators like email, iban, phone number, ... To add them, just call the corresponding method. Model binding for static model structures can simply be done with `UsePropertyBinding` or `UseGetterSetterBinding`.

Congratulations, that is basically all you need to know to build a functional basic form. But wait, how do you use it now?

## Interacting with a form

The underlying structure of the form you've been building is a tree with named nodes. So by default, to access a node you would need to search through that three and find the correct one. In flat structures this can be done by node name, in more complex structures where the name might reappear in different sections you would need the node-id. Ids are basically like a unique path, derived from parent names and joined with "-" to stay compatible with HTML ids.

```C#
var email = Form.Nodes.FirstOrDefault(node => node.Name == "Email");
var password = Form.Nodes.FirstOrDefault(node => node.GetId() == "Login-Password");
```

This way of accessing nodes is awesome for data-driven structures, e.g. if the form structure is built from a definition in a database. But for static structures, accessing nodes by magic strings is screaming to create bugs. Wouldn't it be nice to just access nodes like properties? Precisely that is made possible by a source generator in the package `Common.Forms.Wrappers`. Just include it in your .csproj like the following, to stop the package from propagating through derived projects:

```XML
<PackageReference Include="RobinEpple.Common.Forms.Wrappers"
    PrivateAssets="all"
    ExcludeAssets="runtime" />
```

Now you have access to the `[WrapFormStructure(nameof(Form))]` attribute. To use it, mark your class as `partial`, extract the form building to a dedicated method, and place the attribute above the method. Now you should have immediate access to a new member in your class: `FormWrapper`.

```C#
public partial class LoginPage
{
    // The true form structure.
    public IForm Form { get; private set; }

    // A method for building the structure.
	[MemberNotNull(nameof(Form))]
	[WrapFormStructure(nameof(Form))]
    private void BuildForm()
    {
        Form = new FormBuilder("Login")
            // [... see previous snippet]
            .Build();
    }

    public void Clear()
    {
        // Property access.
        FormWrapper.Email!.Value = null;
        FormWrapper.Password!.Value = null;

        // Restore state.
        Form.Update();
    }
}
```

This wrapper internally does exactly the same name-based access, just that the "magic strings" are derived by the compiler and updated automatically when you change the form definition. Neat, right? And it gets even better: The wrapper generates structs, that you can use in pattern matching to process polymorph structures cleanly!

The method `Clear()` in the example also showcases the call to `Form.Update()` after changing form values. This method runs all internal rules to restore a consistent state. This is intentionally not done on every change to allow efficient bulk updates. But don't forget to call the method when you're done ^^!

Two other methods you might need, are:

```C#
Form.Reset();
Form.SetAllInteracted();
```

I think `Reset()` speaks for itself, it just restores the default state of a form (or the node it is called on). `SetAllInteracted()` sets all `IFieldNode.HasUserInteraction` fields to `true`. This property does not have influence on the semantics of the form (e.g. the node still becomes invalid without user interaction), but it is a way for you to keep track of whether the user has interacted with the node and validation errors should be displayed. Setting all to true is good practice on submit-validation, when the form is fully checked before saving.

## Inter-field rules

To realize inter field rules, this form framework incorporates a custom expression language. It allows you to access field values, compare them among each other and form complex boolean expression trees with transformations, etc. It is exclusive to C# instantiation though, no string parser is contained.

These expressions can be used to define visibility, readonly and validation rules for nodes.

The most important part in an expression is of course accessing values from the form. This is done through calls to `BooleanFieldValue`, `TextFieldValue`, `NumberFieldValue`, ... The values can be transformed using methods like `Not`, `Select` and `Add`, and compared to each other or to a `StaticValue` using methods like `SmallerThan` and `EqualTo`. Finally, conditions can be combined into a boolean expression tree with `And`, `Or`, and so on. Let me show you two examples:

```C#
using static RobinEpple.Common.Forms.Expressions.FormExpression;

// Show section when unchecked.
alternateBillingAddress
    .UseVisibilityCondition(
        BooleanFieldValue("UseDeliveryAddressForBillingAddress").EqualTo(
            StaticValue(false)
        )
    );

// At least one member of the scrum team has to take the roll "product owner".
teamMembers
    .UseExpressionValidator(
        ForEachCollectionItem(
                "TeamMembers",
                TextFieldValue("Role")
                    .Coalesce(StaticValue("Programmer"))
                    .EqualTo(StaticValue("Product Owner"))
            )
            .Any(),
        "You need to specify at least one Product Owner."
    );
```

The first example shows a very basic condition, where a checkbox controls the visibility of a neighboring section. The second one configures an expression validator that checks for the existence of a "Product Owner" role and attaches a validation error to the collection if none is found.

For a full list of available expression components, have a look at [this diagram](common_forms_expressions.svg).

An important concept for inter-field rules is to define which nodes can see each other. This framework uses a scoping based system. Each container (subsection, templated section, collection) creates a scope for the nested items.

All neighboring nodes within a scope can see each other (order is not important), and nodes from higher scopes can search neighboring nested scopes. However by default it is not possible to find nodes in collections or parent scopes. For collections, the expression `ForEachCollectionItem` can be used to access each instance, and an aggregate functions allow to collapse the resulting list into a single value. To access a higher scope, `Elevate(n, expression)` can be used to execute an expression in the scope of the n-th parent.

Expressions can return arbitrary values, but most use-cases require boolean return values.

## Templates and collections

As already touched on in the previous section, polymorphism in forms is realized in two types of nodes: Templated sections for a single instance, and collections for a list of arbitrary length.

Both work, by specifying one or multiple templates to choose from. Since the definitions of these templates can become quite long, I would like to point to the [CheckoutSamplePage](../../src/Common.WebUi.Test/Code/Models/CheckoutSamplePage.cs) for a complete example configuration.

In short: The containers can be added to the form using `WithTemplatedSection` and `WithCollectionNode`. Both offer the method `UseTemplate` in their configuration lambda to add a new template definition. Template names need to be unique.

Regarding inter-field rules: These two containers can still find their neighbors, it is actually their instances that create the new scope and isolate the inner nodes.

For instantiation, the template-object has to be selected from the exposed `Templates` collection and passed to the `Instantiate(template)` method of the respective node. When a form wrapper is used, it will expose quick-access instantiation functions for all defined templates.

To support recursion, scope-opening nodes (subsections, templated sections, collections) pass the parent that defines their _own_ scope as a second parameter to the inner configuration function. This "recursive template" can be added to a templated section or collection with the method `UsePreConfiguredTemplate`. For a full example see the [RecursiveSamplePage](../../src/Common.WebUi.Test/Code/Models/RecursiveSamplePage.cs), since it is a bit tricky to get model binding working with recursion. You need an anchor point with a static Property- or GetterSetterBinding, before you can enter templated binding.

Data binding for templates works with "EmbeddedModels", so a reference to a model instance is stored in the instance node. The embedded model is configured with `.UseEmbeddedModel` and returns an `out`-parameter that can be used to create Property- and GetterSetterBindings within the template, that will be redirected to the appropriate instance model after template instantiation.

When instances are created while loading from a model, the instances in the templates are references to the objects found in the model. Templates are chosen based on model type and an optional filter condition. If a new instance is created through the form, the factory function given as parameter to `.UseEmbeddedModel` is used to create a new and independent model. On writing to the bound property, models are synced to persist as many original objects as possible.

## Request binding

In it's core, the form framework is entirely UI agnostic, so it may be used in WinForms, WPF, Blazor, ASP.NET MVC, etc. However it will only unfold it's full potential, if the model can hold state to orchestrate big, partially rendered forms. Most of the named frameworks implicitly support that, because they push a stateful application or client-server model, except ASP.NET MVC.

Fortunately, I am personally a fan of MVC, so I made sure to add support for that as well ^^. The only two things needed are a way to hand over the form model between requests and to bind the HTTP form collection (including documents) to the form nodes. The model-handover is done with a cache implementation that can be found in `Common.Caching`. The form binding is done via an extension method available in `Common.WebUi`. Combined, a typical MVC method will look like this:

```C#
// Cache instance from dependency injection.
private readonly IManagedCache _cache = cache;

public async Task<IActionResult> CheckoutSample()
{
    // Define a cache key for the model.
    // This should usually be
    // - specific to the user specific, e.g. through a login or a session ID.
    // - specific to the page.
    var clientId = GetOrCreateClientId(HttpContext);
    var modelKey = $"{clientId}:{nameof(CheckoutSample)}";

    // The model is then either found in the cache, or created on cache miss.
    // Model creation will of course be more complex in real world scenarios.
    if (!_cache.TryGet<CheckoutSamplePage>(modelKey, out var model))
    {
        // All cache entries need an inactivity-timeout to expire.
        model = new CheckoutSamplePage();
        _cache.TryCache(modelKey, model, TimeSpan.FromMinutes(5));
    }

    // Differentiate between plain get requests and ones that carry data.
    // Here, HTMX is used to issue requests on change and patch in updated HTML.
    if (!Request.IsHtmxRefresh())
    {
        return View("_Page", model);
    }

    // Bind the form data in the request to the form.
    await Request.BindAsync(model.Form);
    return View("_Page", model);
}
```

The cache can be added to the dependency injection on startup with an extension method. It only requires a time interval for periodic purging of expired entries.

```C#
services.AddManagedCache(TimeSpan.FromSeconds(30));
```

Regarding form binding, the default binding strategies should cover all included rendering options, but you can add custom ones by passing a list of `IFormBindingStrategy` implementations to the optional second parameter of `BindAsync`. Don't forget to include `FormBinder.DefaultBindingStrategies` at the end of your list, if you don't want to override all strategies! And keep in mind that order matters, the first applicable strategy in your list will be used.

## Rendering

Speaking of rendering: As mentioned before, the core structure is UI agnostic. But for HTML rendering I included a couple of default input types in `Common.Forms.Html`, building on my declarative HTML renderer in `Common.Html`. A full list of available input renderers can be found in [this overview](common_forms_html.svg). For a practical example, consult the test project `Common.WebUi.Test`.

## Extensibility

Nearly everything in this framework is decoupled through interfaces. To write your own nodes, validators, expressions or more, just implement the corresponding interface. The form builder exposes open methods for you to enter your own implementations.

Even the wrapper generator is based on attributes. If you write your own nodes, don't forget to annotate the form builder (extension-) methods with the proper attributes to render your nodes into the type structure!

## Dependencies

Since this is supposed to be a base library, it is intentionally low on dependencies. But I included a couple for reliable implementations of the default validators. All libraries in this project use either the MIT or the Apache 2.0 license. For a full list, have a look at the [packages file](../../src/Directory.Packages.props).

For file type validation, the package [Mime](https://github.com/hey-red/Mime) is used. This package builds on the OS library `libmagic` that is usually preinstalled or should at least be available for installation. However, in the DevContainer it struggled to locate the installed library, so I had to help out a little. If you need the file type validation and run into similar problems, maybe this repositories' [startup script](../../post-create.sh) can be a reference for resolving these issues.
