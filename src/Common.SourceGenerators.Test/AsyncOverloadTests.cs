namespace RobinEpple.Common.SourceGenerators.Test;

using Microsoft.CodeAnalysis.CSharp;
using RobinEpple.Common.SourceGenerators.Test.ReferenceImplementation;

[TestClass]
public class AsyncOverloadTests
{
	[DataTestMethod]
	[DataRow(nameof(AsyncOverloadTestClass), nameof(AsyncOverloadTestClass.EmptyVoidMethod_ShouldReturnCompletedTask))]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.BlockBodyWithoutAwaitCalls_ShouldReturnCompletedTask)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.ExpressionBodyWithoutAwaitCalls_ShouldReturnCompletedTask)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AvailableAsyncOverloads_ShouldBeCalledAndAwaitedInBlockBody)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AvailableAsyncOverloads_ShouldBeCalledAndAwaitedInExpressionBody)
	)]
	[DataRow(
		nameof(AsyncOverloadTestAbstractClass),
		nameof(AsyncOverloadTestAbstractClass.AsyncOverload_ShouldWorkOnAbstractMethodStubs)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldWorkOnAbstractMethodStubs)
	)]
	[DataRow(
		nameof(IAsyncOverloadTestInterface),
		nameof(IAsyncOverloadTestInterface.AsyncOverload_ShouldWorkOnInterfaceDeclarations)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldWorkOnInterfaceDeclarations)
	)]
	public void CompareSyntaxTree(string className, string syncMethodName)
	{
		// Define the folders where the generators are located.
		const string sourceGeneratorFolderPath =
			"/workspaces/common/src/Common.SourceGenerators.Test/Generated/RobinEpple.Common.SourceGenerators/RobinEpple.Common.SourceGenerators.AsyncOverloadGenerator";
		const string referenceImplementationFolderPath =
			"/workspaces/common/src/Common.SourceGenerators.Test/ReferenceImplementation/AsyncGenerator";

		// Build the file name.
		var generatedFileName = $"{className}.{syncMethodName}.Async.g.cs";
		var referenceFileName = $"{className}.{syncMethodName}.Async.cs";

		// Combine for the full file paths.
		var generatedFilePath = Path.Combine(sourceGeneratorFolderPath, generatedFileName);
		var referenceFilePath = Path.Combine(referenceImplementationFolderPath, referenceFileName);

		// Check that both files exist and load them.
		if (!File.Exists(generatedFilePath))
		{
			Assert.Fail($"The generated file {generatedFileName} could not be found.");
			return;
		}
		var generatedText = File.ReadAllText(generatedFilePath);
		if (generatedText == null)
		{
			Assert.Fail($"No text contents could be read from file {generatedFileName}.");
			return;
		}

		if (!File.Exists(referenceFilePath))
		{
			Assert.Fail($"The reference file {referenceFileName} could not be found.");
			return;
		}
		var referenceText = File.ReadAllText(referenceFilePath);
		if (referenceText == null)
		{
			Assert.Fail($"No text contents could be read from file {referenceFileName}.");
			return;
		}

		// Parse both files.
		var generatedSyntaxTree = CSharpSyntaxTree.ParseText(generatedText);
		var referenceSyntaxTree = CSharpSyntaxTree.ParseText(referenceText);

		// Compare the syntax tree for equality.
		var treesEqual = generatedSyntaxTree.IsEquivalentTo(referenceSyntaxTree);
		Assert.IsTrue(treesEqual);
	}
}
