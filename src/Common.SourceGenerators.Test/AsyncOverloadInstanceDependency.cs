namespace RobinEpple.Common.SourceGenerators.Test;

public class AsyncOverloadInstanceDependency : AsyncOverloadInstanceDependencyBase, IAsyncOverloadInstanceDependency
{
	public void InstanceCall() { }

	public Task InstanceCallAsync()
	{
		return Task.CompletedTask;
	}

	public void CancellableOverload() { }

	public Task CancellableOverloadAsync(CancellationToken? cancellationToken = null)
	{
		return Task.CompletedTask;
	}

	public int GetOne()
	{
		return 1;
	}

	public Task<int> GetOneAsync()
	{
		return Task.FromResult(1);
	}

	public int Two => 2;
	public int Counter { get; set; } = 0;

	public void OverloadedInMethodScopedExtension() { }

	public void OverloadedInClassScopedExtension() { }

	public void OverloadedForBaseInExtension() { }

	public void OverloadedForInterfaceInExtension() { }

	public void OverloadedWithCancellationToken() { }
}
