namespace RobinEpple.Common.SourceGenerators.Test;

/// <summary>
/// This class contains test methods, who's generated async overloads are compared to some manual reference implementation.
/// </summary>
public partial class AsyncOverloadTestClass
{
	[GenerateAsyncOverload]
	public void EmptyVoidMethod_ShouldReturnCompletedTask() { }
}
