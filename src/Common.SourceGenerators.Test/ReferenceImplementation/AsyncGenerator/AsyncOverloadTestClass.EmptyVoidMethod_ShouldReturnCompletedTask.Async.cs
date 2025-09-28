namespace RobinEpple.Common.SourceGenerators.Test;

public partial class AsyncOverloadTestClass
{
	/// <inheritdoc cref="EmptyVoidMethod_ShouldReturnCompletedTask()"/>
	public System.Threading.Tasks.Task EmptyVoidMethod_ShouldReturnCompletedTaskAsync()
	{
		return System.Threading.Tasks.Task.CompletedTask;
	}
}
