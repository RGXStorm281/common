# Technical documentation of RobinEpple.Common.Forms

([back to readme](common_forms.md))

If you want to add your own functionality, or you are interested in the inner workings of the form framework, this page is meant to give you an overview over the design decisions and make your life easier when navigating this ever growing pile of code ;)

## High level overview

![Architecture](common_forms_architecture.svg)

The graphic above shows you an high level overview over the inner structure of a form. The core is defined through a tree structure of form nodes. The leafs are usually input fields, represented by the interface `IFieldNode`, but they may be grouped in containers. These containers include plain sub-forms (`IForm`), templated sections (`ITemplateNode`) and collections (`ICollectionNode`).

Every node contains a configuration which may include visibility or readonly rules, a select list and more. A search implementation defines which nodes can see each other, and this allows expressions to define inter-field rules. Validators operate on a single node, but may use expressions or search to make decisions based on the node's context. And finally, if the predefined internal update pipeline is not enough for you, an extension system allows to hook into events along the update process. All of this is made accessible through a "Flow API" Form-Builder system.

If you want a deeper look into the tree structure, check out [this class diagram](common_forms_nodes.svg).

## Scoping system

I have though long about which nodes in the tree should see each other. The final decision fell on a scope based system. Basically every "Form" instance defines a scope, in that all neighboring nodes can see each other. This means, there is no "order" among neighboring nodes. This decision decouples the form definition from visual rendering. Of course nodes can also find themselves.

Only sub-forms, collections and templated sections define a new scope for each instance. Nodes cannot access nodes from parent scopes by default. But the expression `Elevate(n, expression)` allows to run an inner expression explicitly on the `n`th parent instead of the current node.

## Update Pipeline

An essential part of this form framework is the Update pipeline. The form does not update on property changes, but when `form.Update()` is called. One update cycle restores full consistency in between all form nodes.

![Update Pipeline](common_forms_update_pipeline.svg)

I think the flow diagram above is already very descriptive and shows the events you can hook into with an extension. I want to point out a couple of important details though.

You may notice, that state of the readonly and visibility flag are always updated. But validation is only run for visible nodes. This is intentional, because in my opinion a validation error should appear in the place where the user can do something about it. An invisible node is the opposite that. The framework provides lots of tools to validate across other nodes. Please use these to validate in the right place.

Visibility and readonly are also hierarchical flags, so the values of parent containers may override local results.

Another thing to point out is, that a nodes state should not accumulate over time. Every update clears all validation errors, and you should re-validate when needed.

## Templates and recursion

One of the main features of this form framework is the ability to model polymorph and recursive structures. This is usually done, by saving a structure template without a parent reference. On instantiation, the structure is copied and the parent reference set according to the instance's placement in the tree.

The id's of nodes are computed dynamically from their parent link, so it updates implicitly.

To enable recursion, parent containers pass their own definition to their children. When an instantiation happens, the instance initially is a complete clone of the parent including all data. But a full reset inside the node restores all default values, removes list entries, etc.

## Overview over available feature implementations

Validators, expressions, and data bindings are open interfaces, that you can provide your own implementation for. But if you are looking for a list of what is already included, have a look at these graphics:

- [Validator overview](common_forms_validation.svg)
- [Expressions overview](common_forms_expressions.svg)
- [Data binding overview](common_forms_binding.svg)
