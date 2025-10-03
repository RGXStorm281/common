namespace RobinEpple.Common.SourceGenerators.Test;

using System.Net.Http.Headers;
using System.Runtime.ExceptionServices;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
	internal static DateTime AsyncOverload_ShouldCopySignature(int day, int month, int year)
	{
		return new DateTime(year, month, day);
	}

	[GenerateAsyncOverload]
	public override DateTime AsyncOverload_ShouldWorkOnAbstractMethodStubs(int day, int month, int year) =>
		throw new NotImplementedException();

	[GenerateAsyncOverload]
	public DateTime AsyncOverload_ShouldWorkOnInterfaceDeclarations(int day, int month, int year) =>
		throw new NotImplementedException();

	[GenerateAsyncOverload]
	private void InternalGenerated() { }

	[GenerateAsyncOverload]
	public TItem AsyncOverload_ShouldWorkOnGenericMethods<TItem>(TItem item) => item;

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldWorkOnMultipleOverloads(int firstParam)
	{
		InternalVoidMethod();
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldWorkOnMultipleOverloads(int firstParam, int secondParam)
	{
		InternalVoidMethod();
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldCallOtherGeneratedAsyncOverloads()
	{
		InternalGenerated();
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldHandleGenericClasses()
	{
		var genericInstance = new AsyncOverloadGenericClass<int>();
		genericInstance.SetValue(2);
		var value = genericInstance.GetValue();
	}

	#endregion

	#region statement support

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateCallsInBlocks()
	{
		{
			InternalVoidMethod();
		}
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateCallsInCheckedStatements()
	{
		checked
		{
			InternalVoidMethod();
		}
	}

	private int[] GetArray() => [1, 2, 3];

	private Task<int[]> GetArrayAsync() => Task.FromResult<int[]>([1, 2, 3]);

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateCallsInForeachStatements()
	{
		foreach (var number in GetArray())
		{
			Identity(number);
		}
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateCallsInDoWhile()
	{
		do
		{
			InternalVoidMethod();
		} while (false);
	}

	[GenerateAsyncOverload]
	private int Increment(int i) => i++;

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateCallsInForStatement()
	{
		for (int i = 0; i < 10; Increment(i))
		{
			InternalVoidMethod();
		}
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateCallsInIfStatement()
	{
		if (1 < Increment(2))
		{
			InternalVoidMethod();
		}
		else if (2 > Increment(3))
		{
			GetArray();
		}
		else
		{
			throw new Exception("unexpected result");
		}
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateLabelledStatements()
	{
		Label:
		InternalVoidMethod();

		goto Label;
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateOnlyHeadOfLockStatement()
	{
		lock (GetArray())
		{
			InternalVoidMethod();
		}
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateVariableInitializations()
	{
		int[] customArray = [2, 3, 4],
			methodInitializedArray = GetArray();
	}

	[GenerateAsyncOverload]
	public int[] AsyncOverload_ShouldTranslateReturnStatements()
	{
		return GetArray();
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateSwitchStatement()
	{
		switch (Increment(1))
		{
			case 1:
			{
				break;
			}
			case 2:
			{
				InternalVoidMethod();
				return;
			}
			default:
			{
				return;
			}
		}
	}

	[GenerateAsyncOverload]
	private TItem Identity<TItem>(TItem item) => item;

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateThrow()
	{
		throw Identity(new Exception("test"));
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateTryCatch()
	{
		try
		{
			InternalVoidMethod();
		}
		catch (InvalidOperationException e)
		{
			Identity(e);
		}
		catch
		{
			GetArray();
		}
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateUsingStatement()
	{
		using (var memoryStream = Identity(new MemoryStream()))
		{
			Identity(memoryStream);
		}

		using var memoryStream2 = Identity(new MemoryStream());
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateWhileStatement()
	{
		while (Identity(1) < Increment(1))
		{
			InternalVoidMethod();
			continue;
		}
	}

	[GenerateAsyncOverload]
	public IEnumerable<int> AsyncOverload_ShouldTranslateYieldStatement()
	{
		foreach (var item in GetArray())
		{
			if (item < 2)
			{
				yield return Identity(item);
			}
			else
			{
				yield break;
			}
		}
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldNotTranslateLocalFunctions()
	{
		int Decrement(int i)
		{
			return i--;
		}

		Decrement(2);
	}

	#endregion

	#region expression support

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateAssignmentExpression()
	{
		int assigned;
		var result = assigned = Increment(2);
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateBinaryExpression()
	{
		var result = Identity(true) && Identity(false);
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateCastedExpression()
	{
		var result = (decimal)Identity(4);
	}

	[GenerateAsyncOverload]
	private AsyncOverloadInstanceDependency? GetInstanceUnsure() => null;

	[GenerateAsyncOverload]
	private AsyncOverloadInstanceDependency GetInstanceSure() => new AsyncOverloadInstanceDependency();

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateConditionalAccessExpression()
	{
		var result = GetInstanceUnsure()?.GetOne();
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateConditionalExpression()
	{
		int? result = GetInstanceUnsure() is { } instance ? instance.GetOne() : null;
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateInvocationExpression()
	{
		GetInstanceUnsure();
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateMemberAccessExpression()
	{
		var result = GetInstanceSure().Two;
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateParenthesizedExpression()
	{
		var result = (Identity(1) + Identity(2)) * Identity(3);
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslatePostfixUnaryExpression()
	{
		GetInstanceSure().Counter++;
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslatePrefixUnaryExpression()
	{
		var result = !Identity(true);
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateSwitchExpression()
	{
		var result = Identity(1) switch
		{
			1 => Identity(2),
			_ => Identity(1),
		};
	}

	[GenerateAsyncOverload]
	public (int First, int Second) AsyncOverload_ShouldTranslateTupleExpression()
	{
		return (Identity(1), Identity(2));
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldNotTranslateAnonymousFunction()
	{
		var increment = (int i) => Identity(i) + 1;
		increment(2);
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateCollectionExpression()
	{
		IEnumerable<int> collection = [Identity(1), Identity(2)];
	}

	[GenerateAsyncOverload]
	private int[] GetArraySure() => [1, 2, 3];

	[GenerateAsyncOverload]
	private int[]? GetArrayUnsure() => null;

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateElementAccessExpression()
	{
		var element = GetArraySure()[1];
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateElementBindingExpression()
	{
		var element = GetArrayUnsure()?[1];
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateInitializerExpression()
	{
		var instance = new AsyncOverloadInstanceDependency() { Counter = Identity(3) };
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateInterpolatedStrings()
	{
		var text = $"The number is {Identity(5)}";
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateIsPatternExpression()
	{
		var hasObject = GetInstanceUnsure() is not null;
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateMemberBindingExpression()
	{
		var currentCount = GetInstanceUnsure()?.Counter;
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldNotTranslateQueryExpression()
	{
		var smallerThanTwo = from item in GetArraySure() where item < 2 select item;
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateRangeExpression()
	{
		var firstTwo = GetArraySure()[..Identity(1)];
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateThrowExpression()
	{
		var instance = GetInstanceUnsure() ?? throw Identity(new Exception());
	}

	private record TwoInts(int First, int Second);

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateWithExpression()
	{
		var myStruct = Identity(new TwoInts(1, 2)) with { First = Identity(2) };
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateAnonymousObjectCreation()
	{
		var myObject = new { SomeNumber = Identity(1), SomeText = Identity("Hello World!") };
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateArrayCreationExpression()
	{
		var array = new int[] { Identity(1), Identity(2) };
		var array2 = new int[5];
	}

	private class ClassWithPartialConstructorInit(int number)
	{
		public int Number { get; } = number;
		public string? Text { get; set; }
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateObjectCreationExpression()
	{
		var explicitlyTyped = new ClassWithPartialConstructorInit(Identity(1)) { Text = Identity("some text") };

		ClassWithPartialConstructorInit implicitlyTyped = new(Identity(1)) { Text = Identity("some text") };
	}

	[GenerateAsyncOverload]
	public void AsyncOverload_ShouldTranslateImplicitArrayCreationExpression()
	{
		var array = new[] { Identity(1), Identity(2) };
	}

	#endregion
}
