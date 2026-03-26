# Common.Forms

([back to readme](../../readme.md))

Nice, apparently I convinced you to check out the library documentation! Now I just have to make sure this is actually worth your time ^^.

Let me first explain the motivation for this project, because it might not be immediately obvious why one would write another framework, when there is an abundance of existing frameworks to choose from. To put it simply: I could not find one that would satisfy my absurd requirements -\\\_(シ)\_/-

An input form can quickly become complex modelling task, when the state of a field depends on other fields in the form. For example showing a section only when a checkbox is checked, or choosing one of many options with different sub-forms (e.g. "choose a payment method"). Inter-field rules both for visibility and validation are an absolute must in my opinion, but this is already not expressible in most frameworks.

Let me give you an example: Have you ever thought about how you could realize a boolean expression builder to filter a grid in the web? This is a very complex task for such a small little feature:

1. Boolean expressions are recursive, with potentially endlessly deep nesting.
2. Each node in the expression tree can be a field reference, a binary or unary operator, ... So for each node you have multiple options to choose.
3. And depending on what you choose, the inner form of the node looks different.

Modelling this in C# is easy. Just define a recursive structure with interfaces or inheritance - done. But have you ever seen a form framework that could not only render this, but bind web requests, validate and data bind to the model? Well, congratulations, you found one :)

I hope this example showed you why achieving this task was not easy in the slightest. Or maybe I am just trying to justify the 30.000 lines of code and immense time effort... Nah, don't think about it.

Don't worry, I'll try to explain as simple as possible how this framework is designed, and using it is way easier than designing it!

## Technical documentation

I have decided to put the technical documentation of the inner workings in a separate file [here](common_forms_in_depth.md). Check it out if you want a deeper understanding on how this thing operates. For everyone else who just wants to use it: continue reading :)

## Creating a form
