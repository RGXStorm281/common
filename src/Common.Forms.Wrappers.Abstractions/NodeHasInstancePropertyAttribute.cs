namespace RobinEpple.Common.Forms.Wrappers.Abstractions;

/// <summary>
/// Tells the form wrapper generator that the form node defined in this method exposes a property that contains template instances.
/// </summary>
/// <param name="propertyName">The name of the property that contains the instances.</param>
/// <param name="isCollection"><see langword="false"/> if the property contains zero or one instance, <see langword="true"/> for collections of multiple instances.</param>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class NodeHasInstancePropertyAttribute(string propertyName, bool isCollection) : Attribute
{
	/// <summary>
	/// The name of the property that contains the instances.
	/// </summary>
	public string PropertyName { get; } = propertyName;

	/// <summary>
	/// <see langword="false"/> if the property contains zero or one instance, <see langword="true"/> for collections of multiple instances.
	/// </summary>
	public bool IsCollection { get; } = isCollection;
}
