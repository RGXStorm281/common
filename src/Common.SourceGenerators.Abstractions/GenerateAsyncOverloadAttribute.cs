namespace RobinEpple.Common.SourceGenerators.Abstractions;

/// <summary>
/// Decorate a method with this attribute to generate an async overload.<br/>
/// The generator will search for async overloads for all top level function calls and use those where available.<br/>
/// If none are found, returns will be replaced with <see cref="Task.CompletedTask"/> or <see cref="Task.FromResult{TResult}(TResult)"/><br/>
/// Please keep in mind, that this is intended to reduce boilerplate code and improve maintainability <br/>
/// for TRIVIAL async overloads.<br/>
/// It will NOT translate local functions, lambdas, manual parallelization and synchronization<br/>
/// (e.g. with Task.WaitAll()) or other advanced async patterns. Please write dedicated async overloads if <br/>
/// an optimized control flow matters.<br/>
/// The containing class needs to be top-level and partial for the generated code to compile.
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class GenerateAsyncOverloadAttribute : Attribute { }
