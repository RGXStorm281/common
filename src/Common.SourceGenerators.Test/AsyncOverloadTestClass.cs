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
	public void AsyncOverload_ShouldTranslateCallsCallsInForStatement()
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
}
