namespace RobinEpple.Common.SourceGenerators.Test;

using System.Reflection;

[TestClass]
public sealed class StaticFactoryTests
{
	[TestMethod]
	public void FactoryMethods_ShouldInheritDocumentation()
	{
		// The IDE should show some documentation on hover.
		// Testing this automatically is not really possible...
		StaticFactory.MarkedImplementationWithDocumentation("some text");
		Assert.IsTrue(true);
	}

	[TestMethod]
	public void FactoryMethods_ShouldReturnTheCreatedObject()
	{
		var createdObject = StaticFactory.MarkedImplementation();
		Assert.IsTrue(createdObject is MarkedImplementation);
	}

	[TestMethod]
	public void FactoryMethods_ShouldHaveSameParameterTypes()
	{
		// When this compiles it succeeded.
		StaticFactory.MarkedImplementationWithParameters(new DateTime(2025, 09, 25, 21, 21, 00), "some text", 42);
		Assert.IsTrue(true);
	}

	[TestMethod]
	public void FactoryMethods_ShouldBeGeneratedForAllOverloads()
	{
		// When this compiles it succeeded.
		StaticFactory.MarkedImplementationWithOverloads("some text", 10);
		StaticFactory.MarkedImplementationWithOverloads("just some text");
		Assert.IsTrue(true);
	}

	[TestMethod]
	public void FactoryMethods_ShouldOnlyConsiderPublicAndInternalConstructorsAndCopyAccessibility()
	{
		var factoryMethods = typeof(StaticFactory).GetMethods(
			BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly
		);
		var targets = factoryMethods
			.Where(method => method.Name == nameof(StaticFactory.MarkedImplementationWithLimitedAccessibility))
			.ToList();

		Assert.AreEqual(2, targets.Count);
		Assert.IsTrue(targets.Any(method => method.IsPublic));
		Assert.IsTrue(targets.Any(method => method.IsAssembly));
		Assert.IsTrue(!targets.Any(method => method.IsFamily));
		Assert.IsTrue(!targets.Any(method => method.IsPrivate));
	}

	[TestMethod]
	public void FactoryMethods_ShouldHandleGenerics()
	{
		// When this compiles it succeeded.
		StaticFactory.MarkedImplementationWithTypeParams("some text");
		StaticFactory.MarkedImplementationWithTypeParams(true);
		Assert.IsTrue(true);
	}
}
