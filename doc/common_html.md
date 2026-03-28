# RobinEpple.Common.Html

([back to readme](../readme.md))

While I am most familiar with Razor syntax and WinForms for UI development, I have had the chance to try out a couple of other UI frameworks in recent years.

Especially SwiftUI stood out to me as easy to read and just intuitive to code. It turns out, what I've been enjoying is called "declarative UI". Razor syntax is surprisingly close, but I always felt like one could do better. Because auto formatters completely fall apart with the C# and HTML syntax mix, and discoverability is just not given in the slightest. So instead of bruting I tried to do it better ;)

The idea is simple: Define an object for each existing HTML tag, that exposes chainable functions to set default attributes. Then collect all tag classes in form of factory methods in a single static class - é voila, import the static class and you got a declarative HTML renderer. Let me show you an example.

```C#
using static RobinEpple.Common.Html.DSL;
// [...]

return Div(
        H1("Hello World!").Class("title"),
        P("This will render HTML.").Class("content")
    )
    .Class("container");
```

You can see, that each tag is just a function call, with the tags' contents being passed as parameters. Configuration can be done by chaining method calls.

This provides perfect discoverability, because after pressing "." you will be presented with all available attributes. To list all HTML tags, just enter member autocompletion on the static class `DSL`. Simple, but effective I think :)

Let me show you a more complex example

```C#
return Div(
        H2(confirmation.Node!.Label),
        Div(
                H3("Terms and conditions").Class("card-header"),
                Div(CheckBox(confirmation.TermsOfService!)).Class("card-body").Class("form-grid")
            )
            .Class("card"),
        Div(
                H3("Summary").Class("card-header"),
                Div(
                        H4("Products"),
                        RenderEach(Model.Products, product => P(Raw(product.PrintSummary()))),
                        H4("Billing Address"),
                        P($@"{Model.BillingAddress.FirstName} {Model.BillingAddress.LastName}"),
                        H4("Accepted Terms of Service"),
                        P(Model.ConfirmedTermsOfService == true ? "Yes" : "No")
                    )
                    .Class("card-body")
            )
            .Class("card"),
        Div(
                RenderIf(previousId != null, Label("Previous").For(previousId!).Class("btn")),
                Button("Check all")
                    .Attribute("hx-post", "CheckAll")
                    .Attribute("hx-target", "#sample-checkout-form")
                    .Attribute("hx-swap", "innerHTML")
                    .Attribute("hx-select", "form > *")
                    .Class("btn primary"),
                RenderIf(nextId != null, Label("Next").For(nextId!).Class("btn primary"))
            )
            .Class("navigation-buttons")
    )
    .Class("confirmation-stage");
```

This is taken from the test project `Common.WebUi.Test`. It shows how the rendering easily accesses properties of the local model. It also shows conditional rendering with `RenderIf(condition, content)` and list rendering with `RenderEach(list, itemRenderer)`. Both control structures are also kept in declarative style. To render optional attributes, every tag exposes the method `.ConfigureIf(condition, tag => ...)`.

A big advantage is, that you can apply all you well established coding styles here: Is the nesting too deep or the function too long? Just extract a part to a local method. Need to reuse a snippet? Just define a parameterized rendering function. Want to design your custom component? Just implement `IHtmlContent` or derive from the `HtmlTag` class.

And as an added bonus: since `IHtmlContent` is actually an ASP.NET interface, all your components and rendering is fully compatible with Razor. Just call the rendering function from a Razor view!

A little warning though. While this looks and feels like a true declarative syntax, it does **not incorporate deferred execution**. So conditions like `previousId != null` will be evaluated in place, and only the result is passed to the function. Similarly, the content `Label("Previous").For(previousId!).Class("btn")` is always instantiated, no matter whether it will be rendered to the HTML or not. So please:

1. be careful with caching the resulting object. It will not re-evaluate conditions by default. You are just caching a fully rendered snapshot.
2. Be careful what you call from your rendering logic, it will be called independent from whether the calling section appears in HTML or not.

You can achieve deferred rendering if you want, but you need to be explicit about it. Every call that takes a lambda instead of an element will only run the lambda when the content is actually rendered. For example the configuration lambda in `.ConfigureIf` is safe. For entire chunks you can use the `Lazy(() => ...)` element to run the lambda only when the component is actually written to HTML.

Before I conclude, let me tell you a little trick: `Lazy` can also be used to escape into a sequential function body, so if you want to trigger some normal C# logic every time the renderer writes to an HTML document, this is the way to do it!

## Documentation comments

I wanted to equip my framework with very good HTML documentation, and in my opinion the best place to find that is the [Mozilla Developer Network (MDN)](https://developer.mozilla.org/de/). So for the tag classes and their attribute methods I used the first paragraph of the respective documentation site in MDN as a doc-comment.

This means, looking up the meaning of tags or attributes is really simple! However, of course I take no credit for this awesome documentation. So please, if you decide to fork or reuse parts of this code in your own work, honor the contributors at MDN and their Creative Commons licensing. See the main readme for details on this project's licensing.
