namespace RobinEpple.Common.SourceGenerators.Test;

using RobinEpple.Common.SourceGenerators.Abstractions;

public abstract partial class AsyncOverloadTestAbstractClass
{
	[GenerateAsyncOverload]
	public abstract DateTime AsyncOverload_ShouldWorkOnAbstractMethodStubs(int day, int month, int year);
}
