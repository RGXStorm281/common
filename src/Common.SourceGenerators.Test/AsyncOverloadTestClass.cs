namespace RobinEpple.Common.SourceGenerators.Test;

using RobinEpple.Common.SourceGenerators.Abstractions;
using RobinEpple.Common.SourceGenerators.Test.ReferenceImplementation;

/// <summary>
/// This class contains test methods, who's generated async overloads are compared to some manual reference implementation.
/// </summary>
public partial class AsyncOverloadTestClass : AsyncOverloadTestAbstractClass, IAsyncOverloadTestInterface
{
	#region signature and applicability

	[GenerateAsyncOverload]
	public void EmptyVoidMethod_ShouldReturnCompletedTask() { }

	[GenerateAsyncOverload]
	public int BlockBodyWithoutAwaitCalls_ShouldReturnCompletedTask()
	{
		return 42;
	}

	[GenerateAsyncOverload]
	public int ExpressionBodyWithoutAwaitCalls_ShouldReturnCompletedTask() => 42;

	private void InternalVoidMethod() { }

	private Task InternalVoidMethodAsync()
	{
		return Task.CompletedTask;
	}

	[GenerateAsyncOverload]
	public void AvailableAsyncOverloads_ShouldBeCalledAndAwaitedInBlockBody()
	{
		var instanceDependency = new AsyncOverloadInstanceDependency();
		InternalVoidMethod();
		AsyncOverloadStaticDependency.StaticCall();
		instanceDependency.InstanceCall();
	}

	[GenerateAsyncOverload]
	public void AvailableAsyncOverloads_ShouldBeCalledAndAwaitedInExpressionBody() => InternalVoidMethod();

	[GenerateAsyncOverload]
	protected static DateTime AsyncOverload_ShouldCopySignature(int day, int month, int year)
	{
		return new DateTime(year, month, day);
	}

	[GenerateAsyncOverload]
	public override DateTime AsyncOverload_ShouldWorkOnAbstractMethodStubs(int day, int month, int year) =>
		throw new NotImplementedException();

	[GenerateAsyncOverload]
	public DateTime AsyncOverload_ShouldWorkOnInterfaceDeclarations(int day, int month, int year) =>
		throw new NotImplementedException();

	#endregion
}
