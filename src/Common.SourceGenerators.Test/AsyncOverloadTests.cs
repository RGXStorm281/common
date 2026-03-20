namespace RobinEpple.Common.SourceGenerators.Test;

using Microsoft.CodeAnalysis.CSharp;

[TestClass]
public class AsyncOverloadTests
{
	[DataTestMethod]
	#region signature and applicability

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
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldCopySignature) + "_day" + "_month" + "_year"
	)]
	[DataRow(
		nameof(AsyncOverloadTestAbstractClass),
		nameof(AsyncOverloadTestAbstractClass.AsyncOverload_ShouldWorkOnAbstractMethodStubs)
			+ "_day"
			+ "_month"
			+ "_year"
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldWorkOnAbstractMethodStubs) + "_day" + "_month" + "_year"
	)]
	[DataRow(
		nameof(IAsyncOverloadTestInterface),
		nameof(IAsyncOverloadTestInterface.AsyncOverload_ShouldWorkOnInterfaceDeclarations)
			+ "_day"
			+ "_month"
			+ "_year"
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldWorkOnInterfaceDeclarations) + "_day" + "_month" + "_year"
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldWorkOnGenericMethods) + "_TItem_item"
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldWorkOnMultipleOverloads) + "_firstParam"
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldWorkOnMultipleOverloads) + "_firstParam" + "_secondParam"
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldCallOtherGeneratedAsyncOverloads)
	)]
	[DataRow(nameof(AsyncOverloadTestClass), nameof(AsyncOverloadTestClass.AsyncOverload_ShouldHandleGenericClasses))]
	#endregion

	#region statement support

	[DataRow(nameof(AsyncOverloadTestClass), nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateCallsInBlocks))]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateCallsInCheckedStatements)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateCallsInForeachStatements)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateCallsInDoWhile)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateCallsInForStatement)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateCallsInIfStatement)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateLabelledStatements)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateOnlyHeadOfLockStatement)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateVariableInitializations)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateReturnStatements)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateSwitchStatement)
	)]
	[DataRow(nameof(AsyncOverloadTestClass), nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateThrow))]
	[DataRow(nameof(AsyncOverloadTestClass), nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateTryCatch))]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateUsingStatement)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateWhileStatement)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateYieldStatement)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldNotTranslateLocalFunctions)
	)]
	#endregion

	#region expression support
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateAssignmentExpression)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateBinaryExpression)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateCastedExpression)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateConditionalAccessExpression)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateConditionalExpression)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateInvocationExpression)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateMemberAccessExpression)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateParenthesizedExpression)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslatePostfixUnaryExpression)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslatePrefixUnaryExpression)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateSwitchExpression)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateTupleExpression)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldNotTranslateAnonymousFunction)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateCollectionExpression)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateElementAccessExpression)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateElementBindingExpression)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateInterpolatedStrings)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateIsPatternExpression)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateMemberBindingExpression)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldNotTranslateQueryExpression)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateRangeExpression)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateThrowExpression)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateWithExpression)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateAnonymousObjectCreation)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateArrayCreationExpression)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateObjectCreationExpression)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldTranslateImplicitArrayCreationExpression)
	)]
	#endregion

	#region extension methods
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldFindExtensionOverloadsWhitelistedOnMethod)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldFindExtensionOverloadsWhitelistedOnClass)
	)]
	[DataRow(
		nameof(AsyncOverloadTestClass),
		nameof(AsyncOverloadTestClass.AsyncOverload_ShouldFindAsyncOverloadsForExtensions)
	)]
	#endregion
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
