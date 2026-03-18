namespace RobinEpple.Common.SourceGenerators.Abstractions;

/// <summary>
/// A class marked with this attribute will get a static factory method generated for each constructor of each implementation of the <see cref="MarkerInterface"/>.
/// The class needs to be partial for the generated code to compile.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class StaticFactoryAttribute : Attribute
{
	/// <summary>
	/// The interface marking all types for that factory methods will be generated.
	/// </summary>
	public Type MarkerInterface { get; }

	/// <inheritdoc cref="StaticFactoryAttribute"/>
	/// <param name="markerInterface">The interface marking all types for that factory methods will be generated.</param>
	public StaticFactoryAttribute(Type markerInterface)
	{
		if (!markerInterface.IsInterface)
		{
			throw new ArgumentException($"The given type needs to be an interface.");
		}
		MarkerInterface = markerInterface;
	}
}
