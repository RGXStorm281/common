namespace RobinEpple.Common.Forms.Wrappers;

internal class InstanceProperty(string name, bool isCollection)
{
	public string Name { get; } = name;
	public bool IsCollection { get; } = isCollection;
}
