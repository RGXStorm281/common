namespace RobinEpple.Common.Forms.Wrappers.Abstractions;

/// <summary>
/// Tells the form wrapper generator that the marked parameter configures the name of an added node.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
public class NodeNameAttribute : Attribute { }
