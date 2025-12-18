namespace RobinEpple.Common.Forms.Wrappers;

public class InstanceProperty(string name, bool isCollection)
{
	public string Name { get; } = name;
	public bool IsCollection { get; } = isCollection;
}
