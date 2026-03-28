namespace RobinEpple.Common.SourceGenerators.Test.ClassScopedExtensions;

public static class InstanceDependencyExtensions
{
	public static Task OverloadedInClassScopedExtensionAsync(this AsyncOverloadInstanceDependency instance)
	{
		return Task.CompletedTask;
	}

	public static void ExtensionCall(this AsyncOverloadInstanceDependency instance, int someNumber) { }
}
