# Goals and Requirements on the Form Framework

## Base Structure

-   field name (structure)
-   field id (running instance -> differentiation of duplicates in collections)
-   support for basic input types
    -   text (single line / multiline)
    -   number (integer / decimal)
    -   date
    -   boolean
    -   file
-   support for dynamic collections (adding and removing elements)
    -   ideally mixed collections (multiple templates)
-   support for recursive data structures
-   support for readonly fields
-   support externally walking the tree structure
-   support for default values
-   support for field-interdependencies
    -   reference by field NAME
    -   there is no notion of distance or order of fields, since the structure is independent of the rendering
    -   can either search for a unique field (which cannot be inside collections)
    -   or retrieve all instances of a field (which searches through collections)
    -   search does not move up in scope (e.g. search does not leave current collection item)
        -   to move up, search on the root node
    -   no update events, just an external update call that restores consistency after inputs have changed
-   KEEP THE BASE CLEAN
    -   no requirements on underlying data-binding/storage
    -   no requirements on rendering
    -   no requirements on responsive components
-   EXTENSIBILITY
    -   validation
        -   track user interaction (should validate)
        -   track validation state and errors
            -   adding AND REMOVING by validator id
        -   allow plugging in validators - no technology dependency
        -   reset fields to unmodified state
    -   visibility (conditional)
    -   formatter for string conversion (both ways)
    -   binding
        -   dedicated push and pull, no automatic sync
        -   dont want invalid form contents to write to the model
        -   need to be able to reset form to model state
    -   event based extensibility
-   WIDE APPLICABILITY
    -   support for sync and async updates

## Form Builders

-   Flow API

## Expressions

-   evaluates on a single node
-   evaluates to a given type
    Default implementation
-   typed
-   boolean operators
-   default comparison operators
    -   <
    -   <=
    -   =
    -   \>=
    -   \>
-   reference unique field to access value
-   functions
    -   extensible by interface
    -   can receive parameters

## Validators

-   evaluates on a single node
-   adds or removes error messages
-   each error message needs to have a unique identifier, to be able to remove them

## Formatters

-   output value as string
-   input value as string
-   if set, the renderers and input loaders need to consider that (display text input, write input via formatter)

## Binding

-   should do auto cast (e.g. decimal to double), implicit conversion operators and constructor conversion
-   should map between field values and static properties
-   should map between collections and basic type lists (e.g. List<string>)
-   should map between templated sections and collections and object (lists)
    -   discriminator for loading existing objects
    -   factory for each template

## Web Input Loader

-   should read values from default form post

## Field Renderers

-   as little dependencies as necessary
-   css/js only for layouting and functionality
-   styling is task of the wrapping page
-   should support selection lists

## Layout renderers

-   ordering of fields
-   tabs
-   split panels
-   collapsable sections
-   css/js only for layouting and functionality
-   styling is task of the wrapping page

## Web Responsive Panels

-   model should be kept open on server (keyed by user, not session!)
    -   the user should always be able to reset
    -   regular cleanup of old models is necessary
-   all the processing happens on the server
-   the current state is then rendered into html
-   signal button presses and form input changes to the server
-   receive updated html
-   sync to existing html in java script:
    -   remove/add nodes
    -   remove/add attributes
    -   greedy possible thanks to unique field ids
-   animations should be done via css classes
-   no nesting of panels > want to still have page based navigation, not single page app
