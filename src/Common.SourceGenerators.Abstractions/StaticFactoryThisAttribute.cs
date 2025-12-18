namespace RobinEpple.Common.SourceGenerators.Abstractions;

/// <summary>
/// A constructor parameter marked with this attribute will get a "this" reference in a generated static factory method.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
public class StaticFactoryThisAttribute : Attribute { }
