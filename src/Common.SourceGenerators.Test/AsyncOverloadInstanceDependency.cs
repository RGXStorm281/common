namespace RobinEpple.Common.SourceGenerators.Test;

public class AsyncOverloadInstanceDependency
{
	public void InstanceCall() { }

	public Task InstanceCallAsync()
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
}
