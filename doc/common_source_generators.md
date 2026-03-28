# RobinEpple.Common.SourceGenerators

([back to readme](../readme.md))

Let's start with a general note on C# source generators. You might be familiar with other types of "source generators", that take some sort of external information as input (a database, a web-API-specification, ...) and output source files into the repository, that are meant to be committed in git and will be compiled like the rest of your code.

C# source generators work differently, and I would rather classify them as a compiler extension. They are called by the compiler process itself, take some resource from your repository as input and feed back a plain source-code string into the next compiler iteration. These resources can be source code, configuration files, etc.

This approach has a couple of advantages:

- The source generator has access to compiler information about your code. It understands the typing context of it's environment and can act accordingly.
- The generated code is always up-to-date. There is no source code living in your repository that needs manual updating, it is updated in every compilation.
- With proper IDE support like in VS Code, the source generator will even run on incremental updates as you type. This gives you immediate access to the generated functionality. It's actually really fun to put the original source and the generated file side by side and watch it update periodically as you type ^^.

You can tell the compiler to output generated source code as files into your project. In VS code you can even debug through the generated files. However, these files are only meant for inspection. They will be overwritten in every compiler-run and need to be excluded from the compilation, otherwise the second compilation will produce conflicts ("type/member already exists"). To output generated files, add these tags to your `.csproj`:

```XML
<PropertyGroup>
    <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
    <CompilerGeneratedFilesOutputPath>./_generated</CompilerGeneratedFilesOutputPath>
</PropertyGroup>

<ItemGroup>
    <None Include="_generated\**\*.cs" />
    <Compile Remove="_generated\**\*.cs" />
</ItemGroup>
```

But of course this approach also has disadvantages that you should be aware of:

- Since source generators run in every compilation and even incrementally as you type, it is important that they run fast. Otherwise they would slow down your IDE massively. For that reason, accessing external sources is a no-go in source generators. Better fetch information from an external source first, transpile it into some config format in your repository and then run the source generator on that local configuration.
- Source generators should run silently in the background. They are not supposed to write logs or print to the console. The only communication method they have is compiler warnings or errors. In that way, they are similar to analyzers.

Especially the second one is important to understand some design decisions in this library: Source generators need to be unobtrusive. For that reason, my generators try to operate non-failing wherever possible. Especially the async overload generator will default to synchronous code if it hits a syntax feature it does not understand, or if it cannot find an async overload.

If you decide to use it, I recommend to reference the library as follows. Setting the asset to private keeps the generator constrained to the current `.csproj`, otherwise you would also inject it to every project referencing yours.

```XML
<PackageReference Include="RobinEpple.Common.SourceGenerators"
    PrivateAssets="all"
    ExcludeAssets="runtime" />
```

## Async overload generator

This generator is probably the most exciting one. I have been annoyed for a while that I need to duplicate code when the same business logic is used by both sync-native platforms (like WinForms) and async-native platforms (like ASP.NET). The `AsyncOverloadGeneratorAttribute` allows to write synchronous code once, and automatically generate an async overload method with identical semantics. Have a look at this example:

```C#
/// <summary>
/// Some documentation.
/// </summary>
[GenerateAsyncOverload]
public bool TryFetchForId(int? id, out MyModel? model)
{
    if (id == null)
    {
        return false;
    }

    model = _repository.LoadForId(id.Value);
    return model != null;
}

// ---------- Generated in a separate file. --------------

/// <summary>
/// Some documentation.
/// </summary>
public async Task<bool> TryFetchForIdAsync(int? id, out MyModel? model)
{
    if (id == null)
    {
        return false;
    }

    model = await _repository.LoadForIdAsync(id.Value);
    return model != null;
}
```

The generator is activated on a synchronous method with the attribute `[GenerateAsyncOverload]`. Please note, that the containing class needs to be marked as `partial`, because the generated async overload will be in a separate syntax tree.

Code snippets that don't contain method calls with async overloads will stay untouched, like the `if` statement in the section above. If no async methods are found at all, the generator still outputs a return type of `Task<>` or `Task`, but defaults to returning `Task.FromResult(...)` or `Task.CompletedTask` instead of running async. This is the "non-destructive" nature of the generator.

Method calls for which an async overload is found are translated to an awaited call like in the example above. If needed, the awaited call is wrapped in brackets.

Additionally it can be used to generate async overloads for body-less signatures like in interfaces and abstract classes.

The detection logic defines "async overloads" equal to what the generator outputs:

- The name needs to be equal plus "Async" appended.
- The return type needs to match the `Task` equivalent.
- Parameter types need to be equal, except the async overload is allowed to have additional optional parameters.

The last exception is important to make common DB-Mapper overloads like from linq2db work, because they tend to add an optional `CancellationToken` parameter. Careful though, the overload will NOT use these additional parameters. Overloads are called with the exact same parameter set as the original synchronous method. There is no semantic translation like for example an "automatic adding of optional parameters" to the async method. This is intentional, because it keeps the behavior of the generator simple and predictable.

The generator can handle most common control structures like `if`, `for`, `foreach`, ... and is able to understand nested and chained method calls and translate them accordingly. By default, it will only search for async overloads in the same type as the original call. Extension methods can be ambiguous in a compilation context, that's why they require explicit opt-in from you. To whitelist extension methods from a specific namespace, add an `AsyncOverloadExtensionNamespaceAttribute` either to the method specifically or to the containing class:

```C#
[AsyncOverloadExtensionNamespace(
    "RobinEpple.Common.SourceGenerators.Test.ClassScopedExtensions")]
internal partial class MyClass
{

    [GenerateAsyncOverload]
    [AsyncOverloadExtensionNamespace(
        "RobinEpple.Common.SourceGenerators.Test.MethodScopedExtensions")]
    public void DoSomething()
    {
        // ...
    }
}
```

Extension methods will be called fully qualified (`Namespace.Class.Method(thisParameter, ...parameters)`), so actually NOT as extension, to omit conflicts with usings etc. If multiple ambiguous candidates are found in the whitelisted namespace(s), the generator will raise a compiler error. This is intentional, as you should implement the method yourself in this case and explicitly choose the method call you want.

The generator does not do semantic translation of advanced concurrency patterns, like launching multiple methods and awaiting them collectively. It is not supposed to do that. If the specific implementation matters, or the async method needs to differ more from the synchronous method than just adding a bunch of `await ...Async`, you should do it yourself.

If you want to know precisely what the generator can and can't do, check out the test-class [here](../src/Common.SourceGenerators.Test/AsyncOverloadTestClass.cs). Everything that is tested in there should work reliably, everything else needs to be treated with caution. As mentioned before, the generator should not produce compile errors, but it might miss an async overloaded call.

To find practical examples, you can have a look through the source code of `Common.Forms`, it is used a lot there.

## Static factory generator

The second generator might be very specific to my personal codestyle. I like to group multiple "strategy" implementations of a marker interface, by representing them as factory methods in a static class. This allows the discovery of available options through IDE member autocompletion. Moreover, this made it easy to design the static-function-based DSLs in `RobinEpple.Common.Html.DSL` and `RobinEpple.Common.Forms.Expressions.FormExpression`.

It is activated with a class attribute like the following:

```C#
[StaticFactory(typeof(IFormExpression<>))]
public static partial class FormExpression
{
}
```

The class needs to be `static partial`. The generator will then collect all implementations of the given marker type, which can be generic as shown in the example. It will find all implementations in the current compilation, but not from included libraries.

For each found implementation, it generates a partial class with factory methods for all constructors. Documentation is inherited, as shown in the example below:

```C#
public static partial class FormExpression
{
	/// <inheritdoc cref="RobinEpple.Common.Forms.Expressions.DefaultImplementation.All.All(RobinEpple.Common.Forms.Expressions.IFormExpression{System.Collections.Generic.IEnumerable{bool}})"/>
	public static RobinEpple.Common.Forms.Expressions.IFormExpression<bool> All(
        this RobinEpple.Common.Forms.Expressions.IFormExpression<System.Collections.Generic.IEnumerable<bool>> operands)
		=> new RobinEpple.Common.Forms.Expressions.DefaultImplementation.All(operands);

}
```

The example also shows that the method can be generated as an extension method, when the `[StaticFactoryThis]` attribute is added to the first constructor parameter. By default the name of the method matches the type name, but you can change that with the class attribute `[StaticFactoryMethodName("Name")]`.
