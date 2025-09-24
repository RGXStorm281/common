namespace RobinEpple.Common.SourceGenerators.Abstractions;

/// <summary>
/// A class marked with this attribute will get a static factory method generated for each constructor of each implementation of the <paramref name="markerInterface"/>.
/// The class needs to be partial for the generated code to work properly.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class StaticFactoryAttribute : Attribute
{
	public Type MarkerInterface { get; }

	/// <param name="markerInterface">The interface the marked type will contain factory methods for.</param>
	public StaticFactoryAttribute(Type markerInterface)
	{
		if (!markerInterface.IsInterface)
		{
			throw new ArgumentException($"The given type needs to be an interface.");
		}
		MarkerInterface = markerInterface;
	}
}
