namespace RobinEpple.Common.Forms.Wrappers.Abstractions;

/// <summary>
/// Tells the form wrapper generator that the marked method adds a template to the form structure.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class AddsTemplateAttribute : Attribute { }
