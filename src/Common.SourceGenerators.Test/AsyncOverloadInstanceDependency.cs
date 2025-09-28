namespace RobinEpple.Common.SourceGenerators.Test;

public class AsyncOverloadInstanceDependency
{
	public void InstanceCall() { }

	public Task InstanceCallAsync()
	{
		return Task.CompletedTask;
	}
}
