namespace RobinEpple.Common.Forms.Wrappers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

public class StaticFormStructureParser
{
	private class CollectionContext(SemanticModel semanticModel, string name, string type)
	{
		public SemanticModel SemanticModel { get; } = semanticModel;
		public FormWrapperNode Node { get; } = new FormWrapperNode(name, type);
	}

	/// <summary>
	/// Searches through the syntax tree of the method body to map out the form structure produced by static form building calls.
	/// </summary>
	/// <param name="methodDeclaration">The method to search.</param>
	/// <param name="semanticModel">The semantic model, for resolving method information.</param>
	/// <returns>The root node of the form structure.</returns>
	public FormWrapperNode ParseStaticFormStructure(
		MethodDeclarationSyntax methodDeclaration,
		SemanticModel semanticModel
	)
	{
		var context = new CollectionContext(semanticModel, string.Empty, "RobinEpple.Common.Forms.Nodes.IForm");
		if (methodDeclaration.Body != null)
		{
			ParseInternal(methodDeclaration.Body, context);
		}
		if (methodDeclaration.ExpressionBody != null)
		{
			ParseInternal(methodDeclaration.ExpressionBody.Expression, context);
		}
		return context.Node;
	}

	private void ParseInternal(StatementSyntax? statement, CollectionContext context)
	{
		switch (statement)
		{
			case BlockSyntax blockSyntax:
			{
				foreach (var innerStatement in blockSyntax.Statements)
				{
					ParseInternal(innerStatement, context);
				}
				break;
			}
			case CheckedStatementSyntax checkedStatementSyntax:
			{
				ParseInternal(checkedStatementSyntax.Block, context);
				break;
			}
			case CommonForEachStatementSyntax commonForEachStatementSyntax:
			{
				ParseInternal(commonForEachStatementSyntax.Expression, context);
				ParseInternal(commonForEachStatementSyntax.Statement, context);
				break;
			}
			case ExpressionStatementSyntax expressionStatementSyntax:
			{
				ParseInternal(expressionStatementSyntax.Expression, context);
				break;
			}
			case IfStatementSyntax ifStatementSyntax:
			{
				// Parse only the if and else statements, not the condition.
				ParseInternal(ifStatementSyntax.Statement, context);
				ParseInternal(ifStatementSyntax.Else?.Statement, context);
				break;
			}
			case LabeledStatementSyntax labeledStatementSyntax:
			{
				ParseInternal(labeledStatementSyntax.Statement, context);
				break;
			}
			case LockStatementSyntax lockStatementSyntax:
			{
				// Parse only the inner block.
				ParseInternal(lockStatementSyntax.Statement, context);
				break;
			}
			case LocalDeclarationStatementSyntax localDecl:
			{
				foreach (var declaration in localDecl.Declaration.Variables)
				{
					ParseInternal(declaration.Initializer?.Value, context);
				}
				break;
			}
			case ReturnStatementSyntax returnStatementSyntax:
			{
				ParseInternal(returnStatementSyntax.Expression, context);
				break;
			}
			case SwitchStatementSyntax switchStatementSyntax:
			{
				// Parse only the section, not the switch conditions.
				foreach (var section in switchStatementSyntax.Sections)
				{
					foreach (var innerStatement in section.Statements)
					{
						ParseInternal(innerStatement, context);
					}
				}
				break;
			}
			case TryStatementSyntax tryStatementSyntax:
			{
				// Only search through try, not through catch blocks.
				ParseInternal(tryStatementSyntax.Block, context);
				break;
			}
			case UsingStatementSyntax usingStatementSyntax:
			{
				// Do not search the using initializers, only the body expression or statement.
				ParseInternal(usingStatementSyntax.Expression, context);
				ParseInternal(usingStatementSyntax.Statement, context);
				break;
			}
			// Loops are not considered static structure.
			case BreakStatementSyntax:
			case ContinueStatementSyntax:
			case DoStatementSyntax:
			case EmptyStatementSyntax:
			case FixedStatementSyntax:
			case ForStatementSyntax:
			case GotoStatementSyntax:
			case LocalFunctionStatementSyntax:
			case ThrowStatementSyntax:
			case UnsafeStatementSyntax:
			case WhileStatementSyntax:
			case YieldStatementSyntax:
			default:
			{
				break;
			}
		}
	}

	private void ParseInternal(ExpressionSyntax? expression, CollectionContext context)
	{
		if (expression == null)
		{
			return;
		}

		switch (expression)
		{
			case AssignmentExpressionSyntax assignmentExpressionSyntax:
			{
				// Only parse the right side of the assignment. Cannot assign to a flow syntax builder.
				ParseInternal(assignmentExpressionSyntax.Right, context);
				break;
			}
			case CastExpressionSyntax castExpressionSyntax:
			{
				// Might need to cast to a specific builder type.
				ParseInternal(castExpressionSyntax.Expression, context);
				break;
			}
			case ConditionalExpressionSyntax conditionalExpressionSyntax:
			{
				// Conditionals are allowed for optional structure nodes.
				ParseInternal(conditionalExpressionSyntax.Condition, context);
				ParseInternal(conditionalExpressionSyntax.WhenTrue, context);
				ParseInternal(conditionalExpressionSyntax.WhenFalse, context);
				break;
			}
			case InvocationExpressionSyntax invocationExpressionSyntax:
			{
				// First check the inner expression, might be the structure node before this one.
				ParseInternal(invocationExpressionSyntax.Expression, context);

				// Check if this invocation yields a structure node and append it to the context.
				ProcessInvocation(invocationExpressionSyntax, context);
				break;
			}
			case MemberAccessExpressionSyntax memberAccessExpressionSyntax:
			{
				// In case the flow syntax would end with a member access, parse the inner target.
				ParseInternal(memberAccessExpressionSyntax.Expression, context);
				break;
			}
			case ParenthesizedExpressionSyntax parenthesizedExpressionSyntax:
			{
				// Parentheses might be used for conditionals.
				ParseInternal(parenthesizedExpressionSyntax.Expression, context);
				break;
			}
			case SwitchExpressionSyntax switchExpressionSyntax:
			{
				// Only parse the optional arms, the condition should not contain structure building.
				foreach (var arm in switchExpressionSyntax.Arms)
				{
					ParseInternal(arm.Expression, context);
				}
				break;
			}

			// Not needed for the flow syntax.
			case AnonymousFunctionExpressionSyntax:
			case AnonymousObjectCreationExpressionSyntax:
			case ArrayCreationExpressionSyntax:
			case AwaitExpressionSyntax:
			case BaseObjectCreationExpressionSyntax:
			case BinaryExpressionSyntax:
			case CheckedExpressionSyntax:
			case CollectionExpressionSyntax:
			case ConditionalAccessExpressionSyntax:
			case DeclarationExpressionSyntax:
			case DefaultExpressionSyntax:
			case ElementAccessExpressionSyntax:
			case ElementBindingExpressionSyntax:
			case ImplicitArrayCreationExpressionSyntax:
			case ImplicitElementAccessSyntax:
			case ImplicitStackAllocArrayCreationExpressionSyntax:
			case InstanceExpressionSyntax:
			case InitializerExpressionSyntax:
			case InterpolatedStringExpressionSyntax:
			case IsPatternExpressionSyntax:
			case LiteralExpressionSyntax:
			case MakeRefExpressionSyntax:
			case MemberBindingExpressionSyntax:
			case PostfixUnaryExpressionSyntax:
			case PrefixUnaryExpressionSyntax:
			case OmittedArraySizeExpressionSyntax:
			case QueryExpressionSyntax:
			case RangeExpressionSyntax:
			case RefExpressionSyntax:
			case RefTypeExpressionSyntax:
			case RefValueExpressionSyntax:
			case SizeOfExpressionSyntax:
			case StackAllocArrayCreationExpressionSyntax:
			case ThrowExpressionSyntax:
			case TupleExpressionSyntax:
			case TypeOfExpressionSyntax:
			case TypeSyntax:
			case WithExpressionSyntax:
			default:
			{
				break;
			}
		}
	}

	/// <summary>
	/// Check whether the invocation configures an addition to the form structure and potentially add the new structure element to the model.
	/// </summary>
	/// <param name="invocation">The method invocation in the current syntax context.</param>
	/// <param name="semanticModel">The semantic model to look up method definitions.</param>
	/// <param name="context">The context to add the structure elements to.</param>
	private static void ProcessInvocation(InvocationExpressionSyntax invocation, CollectionContext context)
	{
		// 1. Resolve the method being invoked
		var symbolInfo = context.SemanticModel.GetSymbolInfo(invocation);
		var methodSymbol = symbolInfo.Symbol as IMethodSymbol;
		if (methodSymbol == null && symbolInfo.CandidateSymbols.Length == 1)
		{
			methodSymbol = symbolInfo.CandidateSymbols[0] as IMethodSymbol;
		}
		if (methodSymbol == null)
		{
			return;
		}

		// 2. Check for AddsFormNodeAttribute(Type nodeType)
		var methodAttributes = AttributeCollector.CollectAllMethodAttributes(methodSymbol);
		var addsFormNodeAttribute = methodAttributes.FirstOrDefault(attr =>
			attr.AttributeClass?.Name == "AddsFormNodeAttribute"
			|| attr.AttributeClass?.ToDisplayString() == "AddsFormNodeAttribute"
		);
		if (addsFormNodeAttribute == null)
		{
			return;
		}

		// 3. Extract fully qualified nodeType name
		if (addsFormNodeAttribute.ConstructorArguments.Length < 1)
		{
			return;
		}
		var nodeTypeArgument = addsFormNodeAttribute.ConstructorArguments[0];
		if (nodeTypeArgument.Value is not INamedTypeSymbol nodeTypeSymbol)
		{
			return;
		}
		var nodeType = nodeTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

		// 4. Find the parameter that has [NodeName]
		var parameterAttributes = AttributeCollector.CollectAllParameterAttributes(methodSymbol);
		int? nodeNameParameterIndex = null;
		foreach (var kvp in parameterAttributes)
		{
			// This needs to be a loop, because "FirstOrDefault" on a <int,...> key value pair returns <0,...> as default, but 0 is a valid key.
			var index = kvp.Key;
			var attributes = kvp.Value;

			if (
				attributes.Any(attr =>
					attr.AttributeClass?.Name == "NodeNameAttribute"
					|| attr.AttributeClass?.ToDisplayString() == "NodeNameAttribute"
				)
			)
			{
				nodeNameParameterIndex = index;
				break;
			}
		}
		if (nodeNameParameterIndex == null)
		{
			return;
		}
		// Safely get the actual parameter symbol from the original method.
		var nodeNameParameter = methodSymbol.Parameters[nodeNameParameterIndex.Value];
		if (nodeNameParameter == null)
		{
			return;
		}

		// 5. Extract the argument value passed to the NodeName parameter
		var operation = context.SemanticModel.GetOperation(invocation) as IInvocationOperation;
		if (operation == null)
		{
			return;
		}
		var nodeNameArgument = operation.Arguments.FirstOrDefault(a =>
			SymbolEqualityComparer.Default.Equals(a.Parameter, nodeNameParameter)
		);
		if (nodeNameArgument == null)
		{
			return;
		}
		var constant = nodeNameArgument.Value.ConstantValue;
		if (!constant.HasValue || constant.Value is not string nodeName)
		{
			return;
		}

		// 6. Register the node in the context.
		var formNode = new FormWrapperNode(nodeName, nodeType);
		context.Node.Substructure.Add(formNode);
	}
}
