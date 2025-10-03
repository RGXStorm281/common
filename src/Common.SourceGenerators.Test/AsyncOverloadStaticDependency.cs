namespace RobinEpple.Common.SourceGenerators.Test;

public static class AsyncOverloadStaticDependency
{
	public static void StaticCall() { }

	public static Task StaticCallAsync()
	{
		return Task.CompletedTask;
	}
}
