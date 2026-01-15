namespace RobinEpple.Common.Forms.Wrappers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

internal class StaticFormStructureParser
{
	private class ParseContext
	{
		public ParseContext(
			SemanticModel semanticModel,
			MessageLogger logger,
			FormWrapperNode node,
			IEnumerable<AvailableParentNode> availableParentNodes
		)
		{
			SemanticModel = semanticModel;
			Logger = logger;
			Node = node;
			AvailableParentNodes = availableParentNodes.ToList();
		}

		public SemanticModel SemanticModel { get; }
		public MessageLogger Logger { get; }
		public FormWrapperNode Node { get; }
		public List<AvailableParentNode> AvailableParentNodes { get; }
	}

	/// <summary>
	/// Searches through the syntax tree of the method body to map out the form structure produced by static form building calls.
	/// </summary>
	/// <param name="methodDeclaration">The method to search.</param>
	/// <param name="semanticModel">The semantic model, for resolving method information.</param>
	/// <param name="logger">A logger to write errors to.</param>
	/// <param name="declaringType">The declaring type of the method.</param>
	/// <returns>The root node of the form structure.</returns>
	public FormWrapperNode ParseStaticFormStructure(
		MethodDeclarationSyntax methodDeclaration,
		SemanticModel semanticModel,
		MessageLogger logger,
		INamedTypeSymbol declaringType
	)
	{
		if (!TryGetFormName(methodDeclaration, semanticModel, logger, out var formName))
		{
			throw new InvalidOperationException(
				$"The form needs to contain a FormBuilder instantiation with a constant name."
			);
		}
		var rootNode = new FormWrapperNode(
			formName!,
			"RobinEpple.Common.Forms.Nodes.IForm",
			declaringType,
			nodeIsFormWrapper: true
		);
		var context = new ParseContext(semanticModel, logger, rootNode, []);

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

	public static bool TryGetFormName(
		MethodDeclarationSyntax methodDeclaration,
		SemanticModel semanticModel,
		MessageLogger logger,
		out string? formName
	)
	{
		formName = null;

		// Find all object creations in the method
		var objectCreations = methodDeclaration.DescendantNodes().OfType<ObjectCreationExpressionSyntax>().ToList();

		// Filter to only FormBuilder constructions
		var formBuilderCreations = objectCreations
			.Where(o =>
			{
				var typeInfo = semanticModel.GetTypeInfo(o);
				return typeInfo.Type is INamedTypeSymbol named && named.Name == "FormBuilder";
			})
			.ToList();

		// There needs to be at least one.
		if (formBuilderCreations.Count < 1)
		{
			return false;
		}
		// Warn if there are multiple.
		if (formBuilderCreations.Count > 1)
		{
			logger.Warn($"Multiple FormBuilder instantiations found. Only the first one will be processed.");
		}

		var creation = formBuilderCreations[0];

		// Ensure it has at least one argument
		if (creation.ArgumentList == null || creation.ArgumentList.Arguments.Count == 0)
		{
			return false;
		}

		var firstArgument = creation.ArgumentList.Arguments[0];

		// Extract constant string value
		var constantValue = semanticModel.GetConstantValue(firstArgument.Expression);

		if (!constantValue.HasValue || constantValue.Value is not string name)
		{
			return false;
		}

		formName = name;
		return true;
	}

	private static void ParseInternal(StatementSyntax? statement, ParseContext context)
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

	private static void ParseInternal(ExpressionSyntax? expression, ParseContext context)
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
	/// <param name="context">The context to add the structure elements to.</param>
	private static void ProcessInvocation(InvocationExpressionSyntax invocation, ParseContext context)
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

	private static IMethodSymbol? GetMethodSymbol(SyntaxNode syntax, ParseContext context)
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
		ParseContext context
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
		var subNode = new FormWrapperNode(nodeName, nodeType, context.Node.DeclaringType, false, context.Node);
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
		ParseContext context
	)
	{
		// Check for AddsTemplateAttribute
		var methodAttributes = AttributeCollector.CollectAllMethodAttributes(methodSymbol);
		if (!TryGetAttribute(methodAttributes, "AddsTemplateAttribute", out _))
		{
			return;
		}

		// Handle recursive and locally configured templates.
		var parameterAttributes = AttributeCollector.CollectAllParameterAttributes(methodSymbol);
		HandleParentReferenceTemplates(methodSymbol, operation, parameterAttributes, context);
		HandleLocallyConfiguredTemplates(methodSymbol, operation, parameterAttributes, context);
	}

	private static void HandleParentReferenceTemplates(
		IMethodSymbol methodSymbol,
		IInvocationOperation operation,
		IReadOnlyDictionary<int, IReadOnlyList<AttributeData>> parameterAttributes,
		ParseContext context
	)
	{
		// Check for a parameter that may reference a parent.
		if (
			!TryGetParameterForAttribute(
				methodSymbol,
				parameterAttributes,
				"TakesParentNodeReferenceAttribute",
				out var parentNodeReferenceParameter
			)
		)
		{
			return;
		}

		var parentReferenceArgument = operation.Arguments.FirstOrDefault(a =>
			SymbolEqualityComparer.Default.Equals(a.Parameter, parentNodeReferenceParameter)
		);
		if (parentReferenceArgument == null)
		{
			return;
		}

		// Check if the actual argument is a parent reference.
		if (!TryMatchRegisteredParentReference(parentReferenceArgument, context, out var parentReference))
		{
			return;
		}

		// Register if so.
		context.Node.ParentReferenceTemplates.Add(parentReference!.ParentNode);
		return;
	}

	private static void HandleLocallyConfiguredTemplates(
		IMethodSymbol methodSymbol,
		IInvocationOperation operation,
		IReadOnlyDictionary<int, IReadOnlyList<AttributeData>> parameterAttributes,
		ParseContext context
	)
	{
		// Find the parameter that has [NodeName]
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
		var templateNode = new FormWrapperNode(
			nodeName,
			"RobinEpple.Common.Forms.Nodes.IForm",
			context.Node.DeclaringType,
			true,
			context.Node
		);
		context.Node.Templates.Add(templateNode);

		// 5. Handle substructure if provided.
		HandleSubstructureConfiguration(methodSymbol, parameterAttributes, operation, context, templateNode);
	}

	private static bool TryMatchRegisteredParentReference(
		IArgumentOperation argumentOperation,
		ParseContext context,
		out AvailableParentNode? parentNode
	)
	{
		parentNode = null;
		var value = argumentOperation.Value;

		// Strip implicit conversions (very important)
		while (value is IConversionOperation conversion)
		{
			value = conversion.Operand;
		}

		if (value is IParameterReferenceOperation paramRef)
		{
			var usedParameter = paramRef.Parameter;

			parentNode = context.AvailableParentNodes.FirstOrDefault(parent =>
				SymbolEqualityComparer.Default.Equals(parent.ParameterSymbol, usedParameter)
			);
		}

		return parentNode != null;
	}

	private static void HandleSubstructureConfiguration(
		IMethodSymbol methodSymbol,
		IReadOnlyDictionary<int, IReadOnlyList<AttributeData>> parameterAttributes,
		IInvocationOperation operation,
		ParseContext context,
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
		var substructureContext = new ParseContext(
			context.SemanticModel,
			context.Logger,
			newNode,
			context.AvailableParentNodes
		);
		if (
			!TryGetConfigurationBodyAndParentParameter(
				substructureArgument,
				context,
				out var configurationBody,
				out var parentParameter
			)
		)
		{
			return;
		}

		// Register the parent parameter if available.
		if (parentParameter != null)
		{
			substructureContext.AvailableParentNodes.Add(new AvailableParentNode(context.Node, parentParameter));
		}

		// Then process the body.
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

	private static bool TryGetConfigurationBodyAndParentParameter(
		IArgumentOperation substructureArgument,
		ParseContext context,
		out SyntaxNode? configurationBody,
		out IParameterSymbol? parentParameter
	)
	{
		configurationBody = null;
		parentParameter = null;

		IOperation delegateValue = substructureArgument.Value;

		// If the argument is a defined delegate type, check if there is a parameter decorated with [ParentNodeReference].
		int? parentParameterIndex = null;
		if (delegateValue.Type is INamedTypeSymbol { DelegateInvokeMethod: { } methodDefinition })
		{
			var delegateParameterAttributes = AttributeCollector.CollectAllParameterAttributes(methodDefinition);
			parentParameterIndex = GetParameterIndexForAttribute(
				delegateParameterAttributes,
				"ParentNodeReferenceAttribute"
			);
		}

		if (delegateValue is IDelegateCreationOperation delegateCreation)
		{
			delegateValue = delegateCreation.Target;
		}

		// Extract the executable body of the passed delegate.
		switch (delegateValue)
		{
			// lambda or anonymous delegate
			case IAnonymousFunctionOperation lambda:
			{
				// Get the body.
				configurationBody = lambda.Syntax switch
				{
					LambdaExpressionSyntax l => l.Body,
					AnonymousMethodExpressionSyntax a => a.Block,
					_ => null,
				};

				if (configurationBody == null)
				{
					return false;
				}

				// If a parent parameter was defined, map to the parameter of the implementation.
				if (parentParameterIndex != null && lambda.Symbol.Parameters.Length >= parentParameterIndex)
				{
					parentParameter = lambda.Symbol.Parameters[parentParameterIndex.Value];
				}

				// Always return true when a body was found, the parameter is not mandatory.
				return true;
			}

			// method group / local function
			case IMethodReferenceOperation methodGroup:
			{
				var method = methodGroup.Method;

				// Get the body.
				var syntaxRef = method.DeclaringSyntaxReferences.FirstOrDefault();
				if (syntaxRef == null)
				{
					return false;
				}

				configurationBody = syntaxRef.GetSyntax() switch
				{
					MethodDeclarationSyntax m => m.Body ?? (SyntaxNode?)m.ExpressionBody?.Expression,
					LocalFunctionStatementSyntax l => l.Body ?? (SyntaxNode?)l.ExpressionBody?.Expression,
					_ => null,
				};

				if (configurationBody == null)
				{
					return false;
				}

				// If a parent parameter was defined, map to the parameter of the implementation.
				if (parentParameterIndex != null && method.Parameters.Length >= parentParameterIndex)
				{
					parentParameter = method.Parameters[parentParameterIndex.Value];
				}

				// Always return true when a body was found, the parameter is not mandatory.
				return true;
			}
		}

		return false;
	}
}
