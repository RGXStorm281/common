namespace RobinEpple.Common.SourceGenerators.Test.ReferenceImplementation;

using RobinEpple.Common.SourceGenerators.Abstractions;

public partial interface IAsyncOverloadTestInterface
{
	[GenerateAsyncOverload]
	public DateTime AsyncOverload_ShouldWorkOnInterfaceDeclarations(int day, int month, int year);
}
