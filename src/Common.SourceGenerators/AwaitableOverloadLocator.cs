namespace RobinEpple.Common.SourceGenerators;

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

/// <summary>
/// Searches through the syntax tree of a method body and locates method invocations.
/// For each invocation, it is checked if an async overload exists in the target class.
/// </summary>
internal class AwaitableOverloadLocator
{
	private IDictionary<IMethodSymbol, string> _toBeGeneratedAsyncMethodNamesBySyncMethod;

	public AwaitableOverloadLocator(IDictionary<IMethodSymbol, string> toBeGeneratedAsyncMethodNamesBySyncMethod)
	{
		_toBeGeneratedAsyncMethodNamesBySyncMethod = toBeGeneratedAsyncMethodNamesBySyncMethod;
	}

	private class CollectionContext(SemanticModel semanticModel)
	{
		public SemanticModel SemanticModel { get; } = semanticModel;
		public Dictionary<IMethodSymbol, string> CollectedAwaitableOverloads { get; } = [];
	}

	/// <summary>
	/// Searches through the syntax tree of the method body to collect all method calls, that can be replaced by an awaited async overload call.
	/// </summary>
	/// <param name="methodDeclaration">The method to search.</param>
	/// <param name="semanticModel">The semantic model, for resolving target types of methods.</param>
	/// <returns>A dictionary, containing the name of the awaitable overload method for each replaceable method invocation in the body.</returns>
	public Dictionary<IMethodSymbol, string> FindAwaitableOverloadsInMethod(
		MethodDeclarationSyntax methodDeclaration,
		SemanticModel semanticModel
	)
	{
		var context = new CollectionContext(semanticModel);
		if (methodDeclaration.Body != null)
		{
			CollectInternal(methodDeclaration.Body, context);
		}
		if (methodDeclaration.ExpressionBody != null)
		{
			CollectInternal(methodDeclaration.ExpressionBody.Expression, context);
		}
		return context.CollectedAwaitableOverloads;
	}

	private void CollectInternal(StatementSyntax? statement, CollectionContext context)
	{
		switch (statement)
		{
			case BlockSyntax blockSyntax:
			{
				foreach (var innerStatement in blockSyntax.Statements)
				{
					CollectInternal(innerStatement, context);
				}
				break;
			}
			case CheckedStatementSyntax checkedStatementSyntax:
			{
				CollectInternal(checkedStatementSyntax.Block, context);
				break;
			}
			case CommonForEachStatementSyntax commonForEachStatementSyntax:
			{
				CollectInternal(commonForEachStatementSyntax.Expression, context);
				CollectInternal(commonForEachStatementSyntax.Statement, context);
				break;
			}
			case DoStatementSyntax doStatementSyntax:
			{
				CollectInternal(doStatementSyntax.Condition, context);
				CollectInternal(doStatementSyntax.Statement, context);
				break;
			}
			case ExpressionStatementSyntax expressionStatementSyntax:
			{
				CollectInternal(expressionStatementSyntax.Expression, context);
				break;
			}
			case ForStatementSyntax forStatementSyntax:
			{
				CollectInternal(forStatementSyntax.Condition, context);
				CollectInternal(forStatementSyntax.Statement, context);
				foreach (var incrementor in forStatementSyntax.Incrementors)
				{
					CollectInternal(incrementor, context);
				}
				break;
			}
			case IfStatementSyntax ifStatementSyntax:
			{
				CollectInternal(ifStatementSyntax.Condition, context);
				CollectInternal(ifStatementSyntax.Statement, context);
				CollectInternal(ifStatementSyntax.Else?.Statement, context);
				break;
			}
			case LabeledStatementSyntax labeledStatementSyntax:
			{
				CollectInternal(labeledStatementSyntax.Statement, context);
				break;
			}
			case LockStatementSyntax lockStatementSyntax:
			{
				// Translate the locked expression but NOT the inner block.
				// Async calls are not allowed inside lock.
				CollectInternal(lockStatementSyntax.Expression, context);
				break;
			}
			case LocalDeclarationStatementSyntax localDecl:
			{
				foreach (var declaration in localDecl.Declaration.Variables)
				{
					CollectInternal(declaration.Initializer?.Value, context);
				}
				break;
			}
			case ReturnStatementSyntax returnStatementSyntax:
			{
				CollectInternal(returnStatementSyntax.Expression, context);
				break;
			}
			case SwitchStatementSyntax switchStatementSyntax:
			{
				CollectInternal(switchStatementSyntax.Expression, context);
				foreach (var section in switchStatementSyntax.Sections)
				{
					foreach (var innerStatement in section.Statements)
					{
						CollectInternal(innerStatement, context);
					}
				}
				break;
			}
			case ThrowStatementSyntax throwStatementSyntax:
			{
				CollectInternal(throwStatementSyntax.Expression, context);
				break;
			}
			case TryStatementSyntax tryStatementSyntax:
			{
				CollectInternal(tryStatementSyntax.Block, context);
				foreach (var catchBlock in tryStatementSyntax.Catches)
				{
					CollectInternal(catchBlock.Block, context);
				}
				break;
			}
			case UsingStatementSyntax usingStatementSyntax:
			{
				CollectInternal(usingStatementSyntax.Expression, context);
				foreach (var variable in usingStatementSyntax.Declaration?.Variables ?? [])
				{
					CollectInternal(variable.Initializer?.Value, context);
				}
				CollectInternal(usingStatementSyntax.Statement, context);
				break;
			}
			case WhileStatementSyntax whileStatementSyntax:
			{
				CollectInternal(whileStatementSyntax.Condition, context);
				CollectInternal(whileStatementSyntax.Statement, context);
				break;
			}
			case YieldStatementSyntax yieldStatementSyntax:
			{
				CollectInternal(yieldStatementSyntax.Expression, context);
				break;
			}
			case BreakStatementSyntax:
			case ContinueStatementSyntax:
			case EmptyStatementSyntax:
			case FixedStatementSyntax:
			case GotoStatementSyntax:
			case LocalFunctionStatementSyntax:
			case UnsafeStatementSyntax:
			default:
			{
				break;
			}
		}
	}

	private void CollectInternal(ExpressionSyntax? expression, CollectionContext context)
	{
		if (expression == null)
		{
			return;
		}

		switch (expression)
		{
			case AnonymousObjectCreationExpressionSyntax anonymousObjectCreationExpressionSyntax:
			{
				foreach (var member in anonymousObjectCreationExpressionSyntax.Initializers)
				{
					CollectInternal(member.Expression, context);
				}
				break;
			}
			case ArrayCreationExpressionSyntax arrayCreationExpressionSyntax:
			{
				CollectInternal(arrayCreationExpressionSyntax.Initializer, context);
				break;
			}
			case AssignmentExpressionSyntax assignmentExpressionSyntax:
			{
				CollectInternal(assignmentExpressionSyntax.Left, context);
				CollectInternal(assignmentExpressionSyntax.Right, context);
				break;
			}
			case BaseObjectCreationExpressionSyntax baseObjectCreationExpressionSyntax:
			{
				foreach (var argument in baseObjectCreationExpressionSyntax.ArgumentList?.Arguments ?? [])
				{
					CollectInternal(argument.Expression, context);
				}
				if (baseObjectCreationExpressionSyntax.Initializer != null)
				{
					CollectInternal(baseObjectCreationExpressionSyntax.Initializer, context);
				}
				break;
			}
			case BinaryExpressionSyntax binaryExpressionSyntax:
			{
				CollectInternal(binaryExpressionSyntax.Left, context);
				CollectInternal(binaryExpressionSyntax.Right, context);
				break;
			}
			case CastExpressionSyntax castExpressionSyntax:
			{
				CollectInternal(castExpressionSyntax.Expression, context);
				break;
			}
			case CollectionExpressionSyntax collectionExpressionSyntax:
			{
				foreach (var element in collectionExpressionSyntax.Elements)
				{
					switch (element)
					{
						case ExpressionElementSyntax expressionElement:
						{
							CollectInternal(expressionElement.Expression, context);
							break;
						}
						case SpreadElementSyntax spreadElement:
						{
							CollectInternal(spreadElement.Expression, context);
							break;
						}
					}
				}
				break;
			}
			case ConditionalAccessExpressionSyntax conditionalAccessExpressionSyntax:
			{
				CollectInternal(conditionalAccessExpressionSyntax.Expression, context);
				CollectInternal(conditionalAccessExpressionSyntax.WhenNotNull, context);
				break;
			}
			case ConditionalExpressionSyntax conditionalExpressionSyntax:
			{
				CollectInternal(conditionalExpressionSyntax.Condition, context);
				CollectInternal(conditionalExpressionSyntax.WhenTrue, context);
				CollectInternal(conditionalExpressionSyntax.WhenFalse, context);
				break;
			}
			case ElementAccessExpressionSyntax elementAccessExpressionSyntax:
			{
				CollectInternal(elementAccessExpressionSyntax.Expression, context);
				foreach (var arg in elementAccessExpressionSyntax.ArgumentList.Arguments)
				{
					CollectInternal(arg.Expression, context);
				}
				break;
			}
			case ElementBindingExpressionSyntax elementBindingExpressionSyntax:
			{
				foreach (var arg in elementBindingExpressionSyntax.ArgumentList.Arguments)
				{
					CollectInternal(arg.Expression, context);
				}
				break;
			}
			case ImplicitArrayCreationExpressionSyntax implicitArrayCreationExpressionSyntax:
			{
				CollectInternal(implicitArrayCreationExpressionSyntax.Initializer, context);
				break;
			}
			case ImplicitElementAccessSyntax implicitElementAccessSyntax:
			{
				foreach (var argument in implicitElementAccessSyntax.ArgumentList.Arguments)
				{
					CollectInternal(argument.Expression, context);
				}
				break;
			}
			case InitializerExpressionSyntax initializerExpressionSyntax:
			{
				foreach (var propertyInitialization in initializerExpressionSyntax.Expressions)
				{
					CollectInternal(propertyInitialization, context);
				}
				break;
			}
			case InterpolatedStringExpressionSyntax interpolatedStringExpressionSyntax:
			{
				foreach (var interpolation in interpolatedStringExpressionSyntax.Contents.OfType<InterpolationSyntax>())
				{
					CollectInternal(interpolation.Expression, context);
				}
				break;
			}
			case InvocationExpressionSyntax invocationExpressionSyntax:
			{
				foreach (var argument in invocationExpressionSyntax.ArgumentList.Arguments)
				{
					CollectInternal(argument.Expression, context);
				}

				CollectInternal(invocationExpressionSyntax.Expression, context);

				TryAddAsyncOverload(invocationExpressionSyntax, context);
				break;
			}
			case IsPatternExpressionSyntax isPatternExpressionSyntax:
			{
				CollectInternal(isPatternExpressionSyntax.Expression, context);
				break;
			}
			case MemberAccessExpressionSyntax memberAccessExpressionSyntax:
			{
				CollectInternal(memberAccessExpressionSyntax.Expression, context);
				break;
			}
			case ParenthesizedExpressionSyntax parenthesizedExpressionSyntax:
			{
				CollectInternal(parenthesizedExpressionSyntax.Expression, context);
				break;
			}
			case PostfixUnaryExpressionSyntax postfixUnaryExpressionSyntax:
			{
				CollectInternal(postfixUnaryExpressionSyntax.Operand, context);
				break;
			}
			case PrefixUnaryExpressionSyntax prefixUnaryExpressionSyntax:
			{
				CollectInternal(prefixUnaryExpressionSyntax.Operand, context);
				break;
			}
			case RangeExpressionSyntax rangeExpressionSyntax:
			{
				CollectInternal(rangeExpressionSyntax.LeftOperand, context);
				CollectInternal(rangeExpressionSyntax.RightOperand, context);
				break;
			}
			case SwitchExpressionSyntax switchExpressionSyntax:
			{
				CollectInternal(switchExpressionSyntax.GoverningExpression, context);
				foreach (var arm in switchExpressionSyntax.Arms)
				{
					CollectInternal(arm.Expression, context);
				}
				break;
			}
			case ThrowExpressionSyntax throwExpressionSyntax:
			{
				CollectInternal(throwExpressionSyntax.Expression, context);
				break;
			}
			case TupleExpressionSyntax tupleExpressionSyntax:
			{
				foreach (var argument in tupleExpressionSyntax.Arguments)
				{
					CollectInternal(argument.Expression, context);
				}
				break;
			}
			case WithExpressionSyntax withExpressionSyntax:
			{
				CollectInternal(withExpressionSyntax.Expression, context);
				CollectInternal(withExpressionSyntax.Initializer, context);
				break;
			}

			// Default fallback: preserve original text
			case AnonymousFunctionExpressionSyntax:
			case AwaitExpressionSyntax:
			case CheckedExpressionSyntax:
			case DeclarationExpressionSyntax:
			case DefaultExpressionSyntax:
			case ImplicitStackAllocArrayCreationExpressionSyntax:
			case InstanceExpressionSyntax:
			case LiteralExpressionSyntax:
			case MakeRefExpressionSyntax:
			case MemberBindingExpressionSyntax:
			case OmittedArraySizeExpressionSyntax:
			case QueryExpressionSyntax:
			case RefExpressionSyntax:
			case RefTypeExpressionSyntax:
			case RefValueExpressionSyntax:
			case SizeOfExpressionSyntax:
			case StackAllocArrayCreationExpressionSyntax:
			case TypeOfExpressionSyntax:
			case TypeSyntax:
			default:
			{
				break;
			}
		}
	}

	private void TryAddAsyncOverload(InvocationExpressionSyntax invocation, CollectionContext context)
	{
		// Get the symbol of the method being called
		var symbolInfo = context.SemanticModel.GetSymbolInfo(invocation);
		var originalMethod = symbolInfo.Symbol as IMethodSymbol;
		if (originalMethod == null)
		{
			// Don't understand this invocation, skip.
			return;
		}

		// Normalize to generic method definition if applicable
		var methodKey = originalMethod.IsGenericMethod ? originalMethod.OriginalDefinition : originalMethod;

		// If the method has already been resolved, skip.
		if (context.CollectedAwaitableOverloads.ContainsKey(methodKey))
		{
			return;
		}

		// If the method will get a generated async overload, just assume the generation will be successful and the method will exist.
		if (_toBeGeneratedAsyncMethodNamesBySyncMethod.TryGetValue(methodKey, out var toBeGeneratedAsyncMethodName))
		{
			context.CollectedAwaitableOverloads.Add(methodKey, toBeGeneratedAsyncMethodName);
		}

		// Try to find an async overload in the compilation.
		var asyncSymbol = FindAsyncOverload(methodKey, context.SemanticModel);

		if (asyncSymbol != null)
		{
			context.CollectedAwaitableOverloads.Add(methodKey, asyncSymbol.Name);
		}
	}

	/// <summary>
	/// Try to find a matching async overload for an invocation.
	/// </summary>
	/// <param name="invocation">The call expression (e.g. Foo(x, y))</param>
	/// <param name="semanticModel">The semantic model for the syntax tree</param>
	/// <returns>The async method symbol if found, else null</returns>
	public static IMethodSymbol? FindAsyncOverload(IMethodSymbol originalMethod, SemanticModel semanticModel)
	{
		// Get the containing type to search the async overload in.
		var containingType = originalMethod.ContainingType;
		if (containingType == null)
		{
			return null;
		}

		// Async candidate name
		var asyncName = originalMethod.Name + "Async";

		// Check for methods with the correct name in the same type
		var candidates = containingType.GetMembers(asyncName).OfType<IMethodSymbol>();

		foreach (var candidate in candidates)
		{
			// 1. Match parameter types
			if (!ParametersMatch(originalMethod.Parameters, candidate.Parameters))
			{
				continue;
			}

			// 2. Match return type
			if (!ReturnTypesMatch(originalMethod, candidate, semanticModel.Compilation))
			{
				continue;
			}

			return candidate;
		}

		return null;
	}

	private static bool ParametersMatch(
		ImmutableArray<IParameterSymbol> origParams,
		ImmutableArray<IParameterSymbol> candidateParams
	)
	{
		if (origParams.Length != candidateParams.Length)
		{
			return false;
		}

		for (int i = 0; i < origParams.Length; i++)
		{
			if (!SymbolEqualityComparer.Default.Equals(origParams[i].Type, candidateParams[i].Type))
			{
				return false;
			}
		}

		return true;
	}

	private static bool ReturnTypesMatch(IMethodSymbol original, IMethodSymbol candidate, Compilation compilation)
	{
		var taskType = compilation.GetTypeByMetadataName("System.Threading.Tasks.Task");
		var taskOfT = compilation.GetTypeByMetadataName("System.Threading.Tasks.Task`1");

		if (original.ReturnsVoid)
		{
			// Expect Task
			return SymbolEqualityComparer.Default.Equals(candidate.ReturnType, taskType);
		}
		else
		{
			// Expect Task<originalType>
			if (
				candidate.ReturnType is INamedTypeSymbol named
				&& SymbolEqualityComparer.Default.Equals(named.ConstructedFrom, taskOfT)
			)
			{
				var inner = named.TypeArguments.FirstOrDefault();
				return SymbolEqualityComparer.Default.Equals(inner, original.ReturnType);
			}
		}

		return false;
	}
}
