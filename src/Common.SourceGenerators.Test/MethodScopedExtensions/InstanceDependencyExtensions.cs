namespace RobinEpple.Common.SourceGenerators.Test.MethodScopedExtensions;

public static class InstanceDependencyExtensions
{
	public static Task OverloadedInMethodScopedExtensionAsync(this AsyncOverloadInstanceDependency instance)
	{
		return Task.CompletedTask;
	}

	public static Task ExtensionCallAsync(this AsyncOverloadInstanceDependency instance, int someNumber)
	{
		return Task.CompletedTask;
	}
}
