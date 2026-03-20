namespace RobinEpple.Common.SourceGenerators.Abstractions;

/// <summary>
/// Decorate a method or class with this attribute to whitelist an extension methods namespace in async overload generation.<br/>
/// By default the <see cref="GenerateAsyncOverloadAttribute"/> will not consider extension methods, because within a project <br/>
/// Extensions methods from different namespaces can be ambiguous. With this attribute a namespace can be specifically <br/>
/// whitelisted for extension method discovery.<br/>
/// If multiple extension methods are found during source generation, the generator will fail intentionally. In this ambiguous <br/>
/// situation you have to either remove some extension namespaces or implement the overload explicitly to resolve ambiguity.
/// </summary>
/// <param name="extensionNamespace">The namespace to whitelist for extension method detection.</param>
[AttributeUsage(
	AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Struct,
	Inherited = false,
	AllowMultiple = true
)]
public sealed class AsyncOverloadExtensionNamespaceAttribute(string extensionNamespace) : Attribute
{
	/// <summary>
	/// The namespace that is whitelisted for extension method detection.
	/// </summary>
	public string ExtensionNamespace { get; } = extensionNamespace;
}
