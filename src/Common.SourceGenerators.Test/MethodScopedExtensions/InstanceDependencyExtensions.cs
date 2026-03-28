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

	public static Task<List<TItem>> ToListAsync<TItem>(this IEnumerable<TItem> items)
	{
		return Task.FromResult(items.ToList());
	}

	public static Task OverloadedForBaseInExtensionAsync(this AsyncOverloadInstanceDependencyBase instance)
	{
		return Task.CompletedTask;
	}

	public static Task OverloadedForInterfaceInExtensionAsync(this IAsyncOverloadInstanceDependency instance)
	{
		return Task.CompletedTask;
	}

	public static Task OverloadedWithCancellationTokenAsync(
		this AsyncOverloadInstanceDependency instance,
		CancellationToken? cancellationToken = null
	)
	{
		return Task.CompletedTask;
	}
}
