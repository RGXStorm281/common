namespace RobinEpple.Common.Forms.Wrappers;

using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

public class StaticFormStructureParser
{
	private class CollectionContext
	{
		public CollectionContext(SemanticModel semanticModel, MessageLogger logger, FormWrapperNode node)
		{
			SemanticModel = semanticModel;
			Logger = logger;
			Node = node;
		}

		public CollectionContext(SemanticModel semanticModel, MessageLogger logger, string name, string type)
			: this(semanticModel, logger, new FormWrapperNode(name, type)) { }

		public SemanticModel SemanticModel { get; }
		public MessageLogger Logger { get; }
		public FormWrapperNode Node { get; }
	}

	/// <summary>
	/// Searches through the syntax tree of the method body to map out the form structure produced by static form building calls.
	/// </summary>
	/// <param name="methodDeclaration">The method to search.</param>
	/// <param name="semanticModel">The semantic model, for resolving method information.</param>
	/// <returns>The root node of the form structure.</returns>
	public FormWrapperNode ParseStaticFormStructure(
		MethodDeclarationSyntax methodDeclaration,
		SemanticModel semanticModel,
		MessageLogger logger
	)
	{
		var context = new CollectionContext(semanticModel, logger, string.Empty, "RobinEpple.Common.Forms.Nodes.IForm");
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

	private static void ParseInternal(StatementSyntax? statement, CollectionContext context)
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

	private static void ParseInternal(ExpressionSyntax? expression, CollectionContext context)
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
		var methodSymbol = GetMethodSymbol(invocation, context);
		if (methodSymbol == null)
		{
			return;
		}
		var operation = context.SemanticModel.GetOperation(invocation) as IInvocationOperation;
		if (operation == null)
		{
			return;
		}

		// 2. Handle different structure elements.
		HandleFormNodes(methodSymbol, operation, context);
		HandleTemplates(methodSymbol, operation, context);
	}

	private static IMethodSymbol? GetMethodSymbol(SyntaxNode syntax, CollectionContext context)
	{
		var symbolInfo = context.SemanticModel.GetSymbolInfo(syntax);
		var methodSymbol = symbolInfo.Symbol as IMethodSymbol;
		if (methodSymbol == null && symbolInfo.CandidateSymbols.Length == 1)
		{
			methodSymbol = symbolInfo.CandidateSymbols[0] as IMethodSymbol;
		}
		return methodSymbol;
	}

	private static bool TryGetAttribute(
		IReadOnlyList<AttributeData?> attributes,
		string attributeTypeName,
		out AttributeData? attribute
	)
	{
		attribute = attributes.FirstOrDefault(attr =>
			attr != null
			&& (
				attr.AttributeClass?.Name == attributeTypeName
				|| attr.AttributeClass?.ToDisplayString() == attributeTypeName
			)
		);
		return attribute != null;
	}

	private static IEnumerable<AttributeData> GetAttributes(
		IReadOnlyList<AttributeData?> attributes,
		string attributeTypeName
	)
	{
		var matches = attributes
			.Where(attr =>
				attr != null
				&& (
					attr.AttributeClass?.Name == attributeTypeName
					|| attr.AttributeClass?.ToDisplayString() == attributeTypeName
				)
			)
			.OfType<AttributeData>()
			.ToList();
		return matches;
	}

	private static void HandleFormNodes(
		IMethodSymbol methodSymbol,
		IInvocationOperation operation,
		CollectionContext context
	)
	{
		// 1. Check for AddsFormNodeAttribute(Type nodeType)
		var methodAttributes = AttributeCollector.CollectAllMethodAttributes(methodSymbol);
		if (!TryGetAttribute(methodAttributes, "AddsFormNodeAttribute", out var addsFormNodeAttribute))
		{
			return;
		}

		// 2. Extract fully qualified nodeType name
		if (addsFormNodeAttribute!.ConstructorArguments.Length < 1)
		{
			return;
		}
		var nodeTypeArgument = addsFormNodeAttribute.ConstructorArguments[0];
		if (nodeTypeArgument.Value is not INamedTypeSymbol nodeTypeSymbol)
		{
			return;
		}
		var nodeType = nodeTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

		// 3. Find the parameter that has [NodeName]
		var parameterAttributes = AttributeCollector.CollectAllParameterAttributes(methodSymbol);
		if (
			!TryGetParameterForAttribute(
				methodSymbol,
				parameterAttributes,
				"NodeNameAttribute",
				out var nodeNameParameter
			)
		)
		{
			return;
		}

		// 4. Extract the argument value passed to the NodeName parameter
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

		// 5. Register the node in the context.
		var subNode = new FormWrapperNode(nodeName, nodeType);
		context.Node.Substructure.Add(subNode);

		// 5. Handle substructure if provided.
		HandleSubstructureConfiguration(methodSymbol, parameterAttributes, operation, context, subNode);

		// 6. Handle instance properties if specified.
		var instanceAttributes = GetAttributes(methodAttributes, "NodeHasInstancePropertyAttribute");
		foreach (var instanceAttribute in instanceAttributes)
		{
			// Extract instance property name and whether it is a collection or single instance
			if (instanceAttribute!.ConstructorArguments.Length < 2)
			{
				continue;
			}
			var instancePropertyNameArgument = instanceAttribute.ConstructorArguments[0];
			if (instancePropertyNameArgument.Value is not string instancePropertyName)
			{
				continue;
			}
			var isCollectionArgument = instanceAttribute.ConstructorArguments[1];
			if (isCollectionArgument.Value is not bool isCollection)
			{
				continue;
			}
			subNode.InstanceProperties.Add(new InstanceProperty(instancePropertyName, isCollection));
		}
	}

	private static void HandleTemplates(
		IMethodSymbol methodSymbol,
		IInvocationOperation operation,
		CollectionContext context
	)
	{
		// 1. Check for AddsTemplateAttribute
		var methodAttributes = AttributeCollector.CollectAllMethodAttributes(methodSymbol);
		if (!TryGetAttribute(methodAttributes, "AddsTemplateAttribute", out var addsTemplateAttribute))
		{
			return;
		}

		// 2. Find the parameter that has [NodeName]
		var parameterAttributes = AttributeCollector.CollectAllParameterAttributes(methodSymbol);
		if (
			!TryGetParameterForAttribute(
				methodSymbol,
				parameterAttributes,
				"NodeNameAttribute",
				out var nodeNameParameter
			)
		)
		{
			return;
		}

		// 3. Extract the argument value passed to the NodeName parameter
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

		// 4. Register the node in the context.
		var templateNode = new FormWrapperNode(nodeName, "RobinEpple.Common.Forms.Nodes.IForm");
		context.Node.Templates.Add(templateNode);

		// 5. Handle substructure if provided.
		HandleSubstructureConfiguration(methodSymbol, parameterAttributes, operation, context, templateNode);
	}

	private static void HandleSubstructureConfiguration(
		IMethodSymbol methodSymbol,
		IReadOnlyDictionary<int, IReadOnlyList<AttributeData>> parameterAttributes,
		IInvocationOperation operation,
		CollectionContext context,
		FormWrapperNode newNode
	)
	{
		// Check for [SubstructureConfiguration] parameter and extract the configuration body.
		if (
			!TryGetParameterForAttribute(
				methodSymbol,
				parameterAttributes,
				"SubstructureConfigurationAttribute",
				out var substructureParameter
			)
		)
		{
			return;
		}
		var substructureArgument = operation.Arguments.FirstOrDefault(a =>
			SymbolEqualityComparer.Default.Equals(a.Parameter, substructureParameter)
		);
		if (substructureArgument == null)
		{
			return;
		}

		// A substructure argument was passed. Process it in a new context.
		var substructureContext = new CollectionContext(context.SemanticModel, context.Logger, newNode);
		var substructureExpression = substructureArgument.Value.Syntax as ExpressionSyntax;
		if (substructureExpression == null)
		{
			return;
		}
		var configurationBody = TryGetExecutableBody(substructureExpression, context.SemanticModel);
		switch (configurationBody)
		{
			case StatementSyntax statement:
			{
				ParseInternal(statement, substructureContext);
				break;
			}
			case ExpressionSyntax expression:
			{
				ParseInternal(expression, substructureContext);
				break;
			}
		}
	}

	private static bool TryGetParameterForAttribute(
		IMethodSymbol methodSymbol,
		IReadOnlyDictionary<int, IReadOnlyList<AttributeData>> parameterAttributes,
		string parameterTypeName,
		out IParameterSymbol? parameter
	)
	{
		parameter = null;
		var parameterIndex = GetParameterIndexForAttribute(parameterAttributes, parameterTypeName);
		if (parameterIndex == null)
		{
			return false;
		}
		// Safely get the actual parameter symbol from the original method.
		parameter = methodSymbol.Parameters[parameterIndex.Value];
		return true;
	}

	private static int? GetParameterIndexForAttribute(
		IReadOnlyDictionary<int, IReadOnlyList<AttributeData>> parameterAttributes,
		string attributeTypeName
	)
	{
		// This needs to be a loop, because "FirstOrDefault" on a <int,...> key value pair returns <0,...> as default, but 0 is a valid key.
		foreach (var kvp in parameterAttributes)
		{
			if (TryGetAttribute(kvp.Value, attributeTypeName, out _))
			{
				return kvp.Key;
			}
		}
		return null;
	}

	private static SyntaxNode? TryGetExecutableBody(ExpressionSyntax expression, SemanticModel semanticModel)
	{
		// Case 1: Lambda expressions
		if (expression is LambdaExpressionSyntax lambda)
		{
			return lambda.Body; // Can be BlockSyntax or ExpressionSyntax
		}

		// Case 2: Anonymous delegate: delegate(...) { ... }
		if (expression is AnonymousMethodExpressionSyntax anonymousMethod)
		{
			return anonymousMethod.Block;
		}

		// Case 3: Method group or named method reference (local, instance, static)
		var symbolInfo = semanticModel.GetSymbolInfo(expression);
		var methodSymbol = symbolInfo.Symbol as IMethodSymbol;

		if (methodSymbol == null && symbolInfo.CandidateSymbols.Length == 1)
		{
			methodSymbol = symbolInfo.CandidateSymbols[0] as IMethodSymbol;
		}

		if (methodSymbol == null)
		{
			return null;
		}

		var syntaxRef = methodSymbol.DeclaringSyntaxReferences.FirstOrDefault();
		if (syntaxRef == null)
		{
			return null;
		}

		var methodSyntax = syntaxRef.GetSyntax();

		switch (methodSyntax)
		{
			case MethodDeclarationSyntax method:
				return method.Body ?? (SyntaxNode?)method.ExpressionBody?.Expression;

			case LocalFunctionStatementSyntax localFunction:
				return localFunction.Body ?? (SyntaxNode?)localFunction.ExpressionBody?.Expression;
		}

		return null;
	}
}
