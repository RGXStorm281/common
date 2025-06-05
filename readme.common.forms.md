# Common.Forms

Hey, you made it to the library documentation! If you haven't set up your environment yet, have a look at the main [readme.md](readme.md), since a working environment will help a lot with making some sense out of this ever growing code pile ( :

This library is all about representing complex forms with an as simple api as possible. While there are a lot of form frameworks around, I found them to be lacking in representing inter-field validation rules and hierarchical model structures. So here we are - hopefully my own solution will handle my absurd requirements ; )

## The requirements

This framework is optimized for the most complex use cases, so it might look very over-engineered for the basic structure of mostly stationary, flat forms. So here is what my framework is designed to handle:

-   Modeling form fields, with all data associated:
    -   Its definition, which consists of the data type, placement in the structure (not necessarily indicating a UI placement!) and rules for validation, loading data from a model and saving changes back to it.
    -   Its state in a running instance, e.g. visibility and validation state, whether user interaction has taken place.
-   Nodes being aware of there context, having clearly defined rules which neighbors are visible and how neighboring states affect the current node.
    -   This allows for cross-field validation (e.g. "this text is required, if that checkbox is checked").
    -   This allows for dynamic visibility of form nodes.
-   Have you noticed the terminology changing from fields to nodes? That is because THE main requirements is to play well with layered, recursive structures, which happens to be what object oriented models represent:
    -   It needs to handle collections (e.g. a list of sub-forms for each passenger of a flight binding to a list of objects in the model)
    -   It needs to handle optional sub-forms (e.g. a "alternative billing address" binding to an optional object in the model)
    -   It needs to handle polymorphism in both cases: Having different templates for different instance types (e.g. a different sub form for each payment option).
-   It should offer a (mostly) readable way of building and configuring a form structure, by
    -   using a flow-builder-api with sensible naming, that enables exploring the options available through autocompletion
    -   assisting with type safety where ever possible
-   It should be extensible where ever possible, to accommodate for edge cases
-   And it should support async operations, in case they are needed by your extensions.

Puh, you might get an idea now, on why this is not trivial. So I'll give you some hints on how you might start looking into this project:

## Concept and structure

I highly recommend having a look at the concept diagrams in "/doc/forms-concept" first.

The highest level view on this library is found in "forms_core_components.drawio". There you can see, that the entire structure evolves around the central tree structure in the the namespace "Nodes". It holds the state of the form, can be queried by expressions and search operations, is validated by validator components and can be extended by hooking into lifecycle or pipeline events. And all is managed by the builder components.

For understanding that tree structure, have a look at "base_structure.drawio". In short: There are nodes that live in a tree structure. Containers define a scope, in which all neighboring nodes can see each other. Nodes can also see the layers beneath, but searching upwards is only possible by traversing through parents and explicitly querying in their scope. This principle for example introduces a separation between neighboring instances in collections. Every node has a name, that is unique in its scope. From these names a unique id for every node can be constructed, by following the path from the root node.

I've already mentioned a pipeline. Contrary to other form frameworks this one holds state and can contain very complex rules. Therefore, updating the state of all nodes on every state modification is not desirable. That's why the form will do nothing, until you call Update() on one of its nodes. With that call, all nodes (in the selected subsection, if not called on the root) will reevaluate their state in their current context. This process follows a very strict pipeline, as documented in "update_pipeline.drawio".

"expressions.drawio", "validators.drawio" and "binding.drawio" also provide a high level view of their respective component, so it might be helpful to have these open while you are looking at the specific segments. In short, expressions query the structure from the outside and are designed to be late-binding. So they can be defined at construction time, but are then executed in the context of a specific node, such that visibility conditions may yield different results for different instances of the same template, etc. Validators and Bindings are basically extensions on a node, that pretty much do what's written in the name. The principle stated in the pipeline also applies here: The form will do nothing without you explicitly calling an action. So binding only happens, when you call the respective method to load from or write to the model.

## Tests

You may have noticed, that this package alone is covered by nearly 300 unit tests. I have been using the test-first development strategy, and that has worked very nicely.

So if you want to explore how to use the framework and interact with the form, it might be the best to just look at the different test cases and start jumping around or stepping through the code from there.

## What's to come

If you wonder how this will all integrate with different UIs later, you may have a look at the rough architecture-sketch in "packages_and_interfaces.drawio". Beware though, that this was only my initial idea and is not fleshed out. At least for the web, the basic idea is to utilize the unique field ids for rendering inputs and loading post values back into the form.

Also you should note, that this framework is meant to hold state on the server. So the state is always updated and rendered into its new shape on the server side. The goal is, to completely eliminate custom JS for interactive form functionality. It may still be used for client side effects like upload progress bars, etc.

## Feedback

I would love to hear all of your thoughts on this project! The praise, but especially the criticism is important for me to not overlook critical issues before I continue with components relying on this library.
