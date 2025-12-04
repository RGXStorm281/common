namespace RobinEpple.Common.Forms.Wrappers.Abstractions;

/// <summary>
/// Tells the form wrapper generator that the marked method adds a form node to the form structure.
/// </summary>
/// <param name="nodeType">The (public facing) type of the added form node (e.g. IForm).</param>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class AddsFormNodeAttribute(Type nodeType) : Attribute
{
	public Type NodeType { get; } = nodeType;
}
