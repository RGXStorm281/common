namespace RobinEpple.Common.SourceGenerators;

using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

/// <summary>
/// Translates a syntax tree into an async counterpart, translating each <br/>
/// method call into an async call when possible.
/// </summary>
public class AsyncTranslator
{
	/// <summary>
	/// Prints out the syntax tree with some rough formatting, replacing every method call that has an awaitable overload available <br/>
	/// with the corresponding async call.
	/// </summary>
	/// <param name="methodDeclaration">The method to translate.</param>
	/// <param name="semanticModel">The semantic model, for semantically identifying methods.</param>
	/// <param name="awaitableOverloads">A dictionary for resolving available async overload names for synchronous methods in the given syntax tree.</param>
	/// <returns>A dictionary, containing the name of the awaitable overload method for each replaceable method invocation in the body.</returns>
	public string TranslateMethodBody(AsyncOverloadGenerationTask context)
	{
		// Translate the body if there is one.
		if (context.MethodDeclaration.Body is { } blockBody)
		{
			var translatedBody = TranslateInternal(blockBody, context);

			// Edge case: If the original return type was void, the return statement at the end can be omitted.
			// If the async translation then has no await calls, it needs to return Task.CompletedTask at the end.
			// Therefore we need to add a return statement at the end.
			if (
				context.MethodSymbol.ReturnsVoid
				&& !context.IsRunningAsync
				&& !LastStatementIsThrowOrReturn(translatedBody)
			)
			{
				// Remove trailing spaces and the closing bracket.
				translatedBody = translatedBody.TrimEnd().TrimEnd('}').TrimEnd();

				// Append the return statement.
				var sb = new StringBuilder(translatedBody);
				sb.AppendLine();
				var returnStatement = TranslateInternal(SyntaxFactory.ReturnStatement(), context);
				sb.Append(IndentHelper.Indent(returnStatement));

				// Close the block again.
				sb.AppendLine("}");
				translatedBody = sb.ToString();
			}

			return translatedBody;
		}

		if (context.MethodDeclaration.ExpressionBody is { } expressionBody)
		{
			var expression = TranslateWithoutParenthesesInternal(expressionBody.Expression, context);
			if (context.AwaitableOverloads.Count == 0 && expressionBody.Expression is not ThrowExpressionSyntax)
			{
				expression =
					$"System.Threading.Tasks.Task.FromResult<{context.MethodSymbol.ReturnType.ToDisplayString()}>({expression})";
			}
			return IndentHelper.Indent($"=> {expression};");
		}

		// If there is no body, just close the method signature.
		return IndentHelper.Indent(";");
	}

	private bool LastStatementIsThrowOrReturn(string block)
	{
		// Split into lines, remove all closing brackets from the end (there might be multiple, e.g. from an "else" statement)
		var lastStatementLine = block
			.Split('\n')
			.Select(line => line.Trim().Trim('}').Trim())
			.Where(line => !string.IsNullOrEmpty(line))
			.LastOrDefault();

		if (lastStatementLine == null)
		{
			return false;
		}

		return lastStatementLine.StartsWith("throw")
			|| lastStatementLine.StartsWith("return")
			|| lastStatementLine.StartsWith("yield");
	}

	/// <summary>
	/// Translates a statement into async.
	/// </summary>
	private string TranslateInternal(StatementSyntax? statement, AsyncOverloadGenerationTask context)
	{
		if (statement == null)
		{
			return string.Empty;
		}
		var sb = new StringBuilder();
		switch (statement)
		{
			case BlockSyntax blockSyntax:
			{
				// Translate each statement in the block and indent them by 1 tab.
				sb.AppendLine("{");
				foreach (var innerStatement in blockSyntax.Statements)
				{
					var translatedStatement = TranslateInternal(innerStatement, context);
					sb.Append(IndentHelper.Indent(translatedStatement));
				}
				sb.AppendLine("}");
				return sb.ToString();
			}
			case CheckedStatementSyntax checkedStatementSyntax:
			{
				// Print the keyword and indent.
				sb.AppendLine("checked");
				var block = TranslateInternal(checkedStatementSyntax.Block, context);
				sb.Append(block);
				return sb.ToString();
			}
			case CommonForEachStatementSyntax commonForEachStatementSyntax:
			{
				// Translate the collection expression and the loop body.
				var expression = TranslateWithoutParenthesesInternal(commonForEachStatementSyntax.Expression, context);
				var body = TranslateInternal(commonForEachStatementSyntax.Statement, context);
				if (commonForEachStatementSyntax.Statement is not BlockSyntax)
				{
					body = IndentHelper.Indent(body);
				}
				if (commonForEachStatementSyntax.AwaitKeyword != null)
				{
					sb.Append(commonForEachStatementSyntax.AwaitKeyword.WithTrailingTrivia().ToFullString());
				}
				sb.Append("foreach (");
				if (commonForEachStatementSyntax is ForEachVariableStatementSyntax forEachVariableStatementSyntax)
				{
					sb.Append("var ");
					sb.Append(Print(forEachVariableStatementSyntax.Variable));
				}
				else if (commonForEachStatementSyntax is ForEachStatementSyntax forEachStatementSyntax)
				{
					sb.Append(Print(forEachStatementSyntax.Type));
					sb.Append(" ");
					sb.Append(Print(forEachStatementSyntax.Identifier));
				}
				sb.Append(" in ");
				sb.Append(expression);
				sb.Append(")");
				sb.AppendLine();

				sb.Append(body);

				return sb.ToString();
			}
			case DoStatementSyntax doStatementSyntax:
			{
				// Translate the condition and the loop body.
				var condition = TranslateWithoutParenthesesInternal(doStatementSyntax.Condition, context);
				var body = TranslateInternal(doStatementSyntax.Statement, context);
				if (doStatementSyntax.Statement is not BlockSyntax)
				{
					body = IndentHelper.Indent(body);
				}
				sb.AppendLine("do");
				sb.AppendLine(body);
				sb.Append("while (");
				sb.Append(condition);
				sb.AppendLine(");");
				return sb.ToString();
			}
			case ExpressionStatementSyntax expressionStatementSyntax:
			{
				// Translate the expression.
				var expression = TranslateWithoutParenthesesInternal(expressionStatementSyntax.Expression, context);
				sb.Append(expression);
				sb.AppendLine(";");
				return sb.ToString();
			}
			case ForStatementSyntax forStatementSyntax:
			{
				// Translate the condition and the loop body.
				var condition = TranslateWithoutParenthesesInternal(forStatementSyntax.Condition, context);
				var block = TranslateInternal(forStatementSyntax.Statement, context);
				if (forStatementSyntax.Statement is not BlockSyntax)
				{
					block = IndentHelper.Indent(block);
				}
				var incrementors = forStatementSyntax.Incrementors.Select(incrementor =>
					TranslateWithoutParenthesesInternal(incrementor, context)
				);
				var incrementorString = string.Join(", ", incrementors);
				sb.Append("for (");
				if (forStatementSyntax.Declaration != null)
				{
					sb.Append(Print(forStatementSyntax.Declaration));
				}
				sb.Append("; ");
				sb.Append(condition);
				sb.Append("; ");
				sb.Append(incrementorString);
				sb.AppendLine(")");
				sb.Append(block);
				return sb.ToString();
			}
			case IfStatementSyntax ifStatementSyntax:
			{
				// Translate the condition and it's body.
				var condition = TranslateWithoutParenthesesInternal(ifStatementSyntax.Condition, context);
				var innerStatement = TranslateInternal(ifStatementSyntax.Statement, context);
				if (ifStatementSyntax.Statement is not BlockSyntax)
				{
					innerStatement = IndentHelper.Indent(innerStatement);
				}
				sb.Append("if (");
				sb.Append(condition);
				sb.AppendLine(")");
				sb.Append(innerStatement);

				// Add else if necessary.
				if (ifStatementSyntax.Else is { } elseClause)
				{
					var elseStatement = TranslateInternal(elseClause.Statement, context);
					if (elseClause.Statement is not IfStatementSyntax)
					{
						sb.AppendLine("else");
					}
					else
					{
						sb.Append("else ");
					}
					sb.Append(elseStatement);
				}
				return sb.ToString();
			}
			case LabeledStatementSyntax labeledStatementSyntax:
			{
				// Print the label and translate the inner statement.
				var innerStatement = TranslateInternal(labeledStatementSyntax.Statement, context);
				sb.Append(Print(labeledStatementSyntax.Identifier));
				sb.AppendLine(":");
				sb.Append(innerStatement);
				return sb.ToString();
			}
			case LockStatementSyntax lockStatementSyntax:
			{
				// Translate the locked expression but NOT the inner block.
				// Async calls are not allowed inside lock.
				var expression = TranslateWithoutParenthesesInternal(lockStatementSyntax.Expression, context);
				var block = Print(lockStatementSyntax.Statement);
				if (lockStatementSyntax.Statement is not BlockSyntax)
				{
					block = IndentHelper.Indent(block);
				}
				sb.Append("lock (");
				sb.Append(expression);
				sb.AppendLine(")");
				sb.Append(block);
				return sb.ToString();
			}
			case LocalDeclarationStatementSyntax localDecl:
			{
				if (localDecl.UsingKeyword != null)
				{
					sb.Append(Print(localDecl.UsingKeyword));
					sb.Append(" ");
				}
				sb.Append(TranslateVariableDeclarationSyntax(localDecl.Declaration, context));
				sb.AppendLine(";");
				return sb.ToString();
			}
			case ReturnStatementSyntax returnStatementSyntax:
			{
				if (returnStatementSyntax.Expression == null)
				{
					// Void return.
					if (context.IsRunningAsync)
					{
						// There are await calls in the method, so just return.
						sb.AppendLine("return;");
						return sb.ToString();
					}
					else
					{
						// There are no await calls. Return completed task.
						sb.AppendLine("return System.Threading.Tasks.Task.CompletedTask;");
						return sb.ToString();
					}
				}
				else
				{
					// Object return.
					var expression = TranslateWithoutParenthesesInternal(returnStatementSyntax.Expression, context);

					if (context.IsRunningAsync)
					{
						// There are await calls in the method, so just return.
						sb.Append("return ");
						sb.Append(expression);
						sb.AppendLine(";");
						return sb.ToString();
					}
					else
					{
						// There are no await calls. Return completed task.
						sb.Append(
							$"return System.Threading.Tasks.Task.FromResult<{context.MethodSymbol.ReturnType.ToDisplayString()}>("
						);
						sb.Append(expression);
						sb.AppendLine(");");
						return sb.ToString();
					}
				}
			}
			case SwitchStatementSyntax switchStatementSyntax:
			{
				// Translate the target expression and each case body.
				var expression = TranslateWithoutParenthesesInternal(switchStatementSyntax.Expression, context);
				sb.Append("switch (");
				sb.Append(expression);
				sb.AppendLine(")");
				sb.AppendLine("{");
				foreach (var section in switchStatementSyntax.Sections)
				{
					foreach (var label in section.Labels)
					{
						sb.AppendLine(IndentHelper.Indent(Print(label)));
					}
					foreach (var innerStatement in section.Statements)
					{
						var translatedStatement = TranslateInternal(innerStatement, context);
						if (innerStatement is not BlockSyntax)
						{
							IndentHelper.Indent(translatedStatement, 2);
						}
						else
						{
							IndentHelper.Indent(translatedStatement, 1);
						}
						sb.AppendLine(translatedStatement);
					}
				}
				sb.AppendLine("}");
				return sb.ToString();
			}
			case ThrowStatementSyntax throwStatementSyntax:
			{
				// Translate the inner expression.
				var expression = TranslateWithoutParenthesesInternal(throwStatementSyntax.Expression, context);
				sb.Append("throw ");
				sb.Append(expression);
				sb.AppendLine(";");
				return sb.ToString();
			}
			case TryStatementSyntax tryStatementSyntax:
			{
				// Translate the inner and each of the catch blocks.
				var translatedTryBlock = TranslateInternal(tryStatementSyntax.Block, context);
				sb.AppendLine("try");
				sb.Append(translatedTryBlock);
				foreach (var catchBlock in tryStatementSyntax.Catches)
				{
					var translatedCatchBlock = TranslateInternal(catchBlock.Block, context);
					sb.Append("catch");
					if (catchBlock.Declaration != null)
					{
						sb.Append(Print(catchBlock.Declaration));
					}
					if (catchBlock.Filter != null)
					{
						sb.Append(" when (");
						sb.Append(Print(catchBlock.Filter));
						sb.Append(")");
					}
					sb.AppendLine();
					sb.Append(translatedCatchBlock);
				}
				return sb.ToString();
			}
			case UsingStatementSyntax usingStatementSyntax:
			{
				// Print the declaration and translate the inner statement.
				var translatedStatement = TranslateInternal(usingStatementSyntax.Statement, context);
				if (usingStatementSyntax.Statement is not BlockSyntax)
				{
					translatedStatement = IndentHelper.Indent(translatedStatement);
				}
				sb.Append("using (");
				if (usingStatementSyntax.Declaration is { } variable)
				{
					sb.Append(TranslateVariableDeclarationSyntax(variable, context));
				}
				if (usingStatementSyntax.Expression != null)
				{
					var expression = TranslateWithoutParenthesesInternal(usingStatementSyntax.Expression, context);
					sb.Append(expression);
				}
				sb.AppendLine(")");
				sb.Append(translatedStatement);
				return sb.ToString();
			}
			case WhileStatementSyntax whileStatementSyntax:
			{
				// Translate the condition and the loop body.
				var condition = TranslateWithoutParenthesesInternal(whileStatementSyntax.Condition, context);
				var block = TranslateInternal(whileStatementSyntax.Statement, context);
				if (whileStatementSyntax.Statement is not BlockSyntax)
				{
					block = IndentHelper.Indent(block);
				}

				sb.Append("while (");
				sb.Append(condition);
				sb.AppendLine(")");
				sb.Append(block);
				return sb.ToString();
			}
			case YieldStatementSyntax yieldStatementSyntax:
			{
				// Translate the expression.
				var expression = TranslateWithoutParenthesesInternal(yieldStatementSyntax.Expression, context);
				sb.Append("yield ");
				if (yieldStatementSyntax.ReturnOrBreakKeyword is { } returnOrBreak)
				{
					sb.Append(returnOrBreak.WithoutTrivia().ToString() + " ");
				}
				sb.Append(expression);
				sb.AppendLine(";");
				return sb.ToString();
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
				// Just print out the statement, no translation needed.
				sb.Append(Print(statement));
				sb.AppendLine();
				return sb.ToString();
			}
		}
	}

	private string TranslateVariableDeclarationSyntax(
		VariableDeclarationSyntax declaration,
		AsyncOverloadGenerationTask context
	)
	{
		// Translate each initializer expression.
		var sb = new StringBuilder();
		var variableDeclarations = declaration
			.Variables.Select(variable =>
			{
				var variableBuilder = new StringBuilder();
				variableBuilder.Append(Print(variable.Identifier));
				if (variable.Initializer != null)
				{
					var initializer = TranslateWithoutParenthesesInternal(variable.Initializer.Value, context);
					variableBuilder.Append(" = ");
					variableBuilder.Append(initializer);
				}
				return variableBuilder.ToString();
			})
			.ToList();

		sb.Append(Print(declaration.Type));
		sb.Append(" ");
		if (variableDeclarations.Count == 1)
		{
			sb.Append(variableDeclarations[0]);
			return sb.ToString();
		}

		// More than one.
		for (int i = 0; i < variableDeclarations.Count; i++)
		{
			var variableDeclaration = variableDeclarations[i];
			if (i > 0)
			{
				variableDeclaration = IndentHelper.Indent(variableDeclaration);
			}
			sb.Append(variableDeclaration);
			if (i < variableDeclarations.Count - 1)
			{
				sb.AppendLine(",");
			}
		}
		return sb.ToString();
	}

	/// <summary>
	/// Translates an expression into async and wraps it in brackets if the top level expression is awaited.
	/// </summary>
	private string TranslateAndParenthesizeInternal(ExpressionSyntax? expression, AsyncOverloadGenerationTask context)
	{
		var translated = TranslateInternal(expression, context, out var isAwaitedMethodCall);
		if (isAwaitedMethodCall)
		{
			return $"({translated})";
		}
		return translated;
	}

	/// <summary>
	/// Translates an expression into async and wraps it in brackets if the top level expression is awaited.
	/// </summary>
	private string TranslateWithoutParenthesesInternal(
		ExpressionSyntax? expression,
		AsyncOverloadGenerationTask context
	)
	{
		return TranslateInternal(expression, context, out _);
	}

	/// <summary>
	/// Translates an expression into async.
	/// </summary>
	private string TranslateInternal(
		ExpressionSyntax? expression,
		AsyncOverloadGenerationTask context,
		out bool isAwaitedMethodCall
	)
	{
		isAwaitedMethodCall = false;
		if (expression == null)
		{
			return string.Empty;
		}

		switch (expression)
		{
			case AssignmentExpressionSyntax assignmentExpressionSyntax:
			{
				// Translate both sides and print the assignment.
				var left = TranslateWithoutParenthesesInternal(assignmentExpressionSyntax.Left, context);
				var right = TranslateWithoutParenthesesInternal(assignmentExpressionSyntax.Right, context);
				var assignment = Print(assignmentExpressionSyntax.OperatorToken);
				return $"{left} {assignment} {right}";
			}
			case BinaryExpressionSyntax binaryExpressionSyntax:
			{
				// Translate both sides and combine with the binary operator.
				var left = TranslateWithoutParenthesesInternal(binaryExpressionSyntax.Left, context);
				var right = TranslateWithoutParenthesesInternal(binaryExpressionSyntax.Right, context);
				return $"{left} {Print(binaryExpressionSyntax.OperatorToken)} {right}";
			}
			case CastExpressionSyntax castExpressionSyntax:
			{
				// Print the cast type and translate the inner expression.
				var innerExpression = TranslateWithoutParenthesesInternal(castExpressionSyntax.Expression, context);
				return $"({Print(castExpressionSyntax.Type)}){innerExpression}";
			}
			case ConditionalAccessExpressionSyntax conditionalAccessExpressionSyntax:
			{
				// Translate the receiver and the access expression.
				var receiver = TranslateAndParenthesizeInternal(conditionalAccessExpressionSyntax.Expression, context);
				var whenNotNull = TranslateWithoutParenthesesInternal(
					conditionalAccessExpressionSyntax.WhenNotNull,
					context
				);
				return $"{receiver}?.{whenNotNull}";
			}
			case ConditionalExpressionSyntax conditionalExpressionSyntax:
			{
				// Translate the condition, true and false cases and wrap them back in a ternary operator.
				var condition = TranslateWithoutParenthesesInternal(conditionalExpressionSyntax.Condition, context);
				var whenTrue = TranslateWithoutParenthesesInternal(conditionalExpressionSyntax.WhenTrue, context);
				var whenFalse = TranslateWithoutParenthesesInternal(conditionalExpressionSyntax.WhenFalse, context);

				var sb = new StringBuilder();
				sb.AppendLine(condition);
				sb.AppendLine(IndentHelper.Indent("? " + whenTrue));
				sb.Append(IndentHelper.Indent(": " + whenFalse));
				return sb.ToString();
			}
			case InvocationExpressionSyntax invocationExpressionSyntax:
			{
				// Here is the actual magic: Translate into an async call if possible.
				return TryTranslateInvocationToAsync(invocationExpressionSyntax, context, out isAwaitedMethodCall);
			}
			case MemberAccessExpressionSyntax memberAccessExpressionSyntax:
			{
				// Translate the receiver and append the member access.
				var receiver = TranslateAndParenthesizeInternal(memberAccessExpressionSyntax.Expression, context);
				var member = memberAccessExpressionSyntax.Name;
				// Wrap the receiver in brackets in case it is an awaited call.
				return $"{receiver}.{member}";
			}
			case ParenthesizedExpressionSyntax parenthesizedExpressionSyntax:
			{
				// Translate the inner expression and put them back in brackets.
				var inner = TranslateWithoutParenthesesInternal(parenthesizedExpressionSyntax.Expression, context);
				return $"({inner})";
			}
			case PostfixUnaryExpressionSyntax postfixUnaryExpressionSyntax:
			{
				// Translate the inner expression and append the operator.
				var inner = TranslateAndParenthesizeInternal(postfixUnaryExpressionSyntax.Operand, context);
				var unaryOperator = Print(postfixUnaryExpressionSyntax.OperatorToken);
				return $"{inner}{unaryOperator}";
			}
			case PrefixUnaryExpressionSyntax prefixUnaryExpressionSyntax:
			{
				// Translate the inner expression and prepend the operator.
				var inner = TranslateAndParenthesizeInternal(prefixUnaryExpressionSyntax.Operand, context);
				var unaryOperator = Print(prefixUnaryExpressionSyntax.OperatorToken);
				return $"{unaryOperator}{inner}";
			}
			case SwitchExpressionSyntax switchExpressionSyntax:
			{
				// Translate the condition and each case arm.
				var sb = new StringBuilder();
				var condition = TranslateWithoutParenthesesInternal(
					switchExpressionSyntax.GoverningExpression,
					context
				);
				sb.AppendLine($"{condition} switch");
				sb.AppendLine("{");
				foreach (var arm in switchExpressionSyntax.Arms)
				{
					var pattern = Print(arm.Pattern);
					if (arm.WhenClause != null)
					{
						pattern = $"{pattern} when {Print(arm.WhenClause)}";
					}
					pattern = IndentHelper.Indent(pattern);
					var inner = TranslateWithoutParenthesesInternal(arm.Expression, context);
					sb.AppendLine($"{pattern} => {inner},");
				}
				sb.AppendLine("}");
				return sb.ToString();
			}
			case TupleExpressionSyntax tupleExpressionSyntax:
			{
				// Translate each tuple argument and wrap them back in brackets.
				var args = tupleExpressionSyntax.Arguments.Select(argument =>
				{
					var inner = TranslateWithoutParenthesesInternal(argument.Expression, context);
					if (argument.NameColon == null)
					{
						return inner;
					}
					return $"{Print(argument.NameColon)} {inner}";
				});

				return $"({string.Join(", ", args)})";
			}

			case AnonymousFunctionExpressionSyntax:
			case AnonymousObjectCreationExpressionSyntax:
			case ArrayCreationExpressionSyntax:
			case AwaitExpressionSyntax:
			case BaseObjectCreationExpressionSyntax:
			case CheckedExpressionSyntax:
			case CollectionExpressionSyntax:
			case DeclarationExpressionSyntax:
			case DefaultExpressionSyntax:
			case ElementAccessExpressionSyntax:
			case ElementBindingExpressionSyntax:
			case ImplicitArrayCreationExpressionSyntax:
			case ImplicitElementAccessSyntax:
			case ImplicitStackAllocArrayCreationExpressionSyntax:
			case InitializerExpressionSyntax:
			case InstanceExpressionSyntax:
			case InterpolatedStringExpressionSyntax:
			case IsPatternExpressionSyntax:
			case LiteralExpressionSyntax:
			case MakeRefExpressionSyntax:
			case MemberBindingExpressionSyntax:
			case OmittedArraySizeExpressionSyntax:
			case QueryExpressionSyntax:
			case RangeExpressionSyntax:
			case RefExpressionSyntax:
			case RefTypeExpressionSyntax:
			case RefValueExpressionSyntax:
			case SizeOfExpressionSyntax:
			case StackAllocArrayCreationExpressionSyntax:
			case ThrowExpressionSyntax:
			case TypeOfExpressionSyntax:
			case TypeSyntax:
			case WithExpressionSyntax:
			default:
			{
				// Default fallback: preserve original text.
				return Print(expression);
			}
		}
	}

	/// <summary>
	/// Looks for an async overload for the invoked method and replaces the call with async if possible.
	/// </summary>
	private string TryTranslateInvocationToAsync(
		InvocationExpressionSyntax invocation,
		AsyncOverloadGenerationTask context,
		out bool isAwaitedMethodCall
	)
	{
		isAwaitedMethodCall = false;

		// Get the symbol of the method being called
		var symbolInfo = context.SemanticModel.GetSymbolInfo(invocation.Expression);
		var originalMethod = symbolInfo.Symbol as IMethodSymbol;
		if (originalMethod == null)
		{
			// Don't understand this invocation, just print it.
			return Print(invocation);
		}

		// Translate each argument.
		var args = string.Join(
			", ",
			invocation.ArgumentList.Arguments.Select(a => TranslateWithoutParenthesesInternal(a.Expression, context))
		);

		// Rewrite the base call.
		var receiver = TranslateWithoutParenthesesInternal(invocation.Expression, context);

		// Normalize to generic method definition if applicable
		var methodKey = originalMethod.IsGenericMethod ? originalMethod.OriginalDefinition : originalMethod;

		// Check if an overload exists.
		if (!context.AwaitableOverloads.TryGetValue(methodKey, out var asyncName))
		{
			// No async overload found — keep original, but call translated arguments.
			return $"{receiver}({args})";
		}

		// Async overload found - replace the method name with the async one and await the call.
		isAwaitedMethodCall = true;
		var originalName = originalMethod.Name;

		// Translate the receiver string like obj?. or a StaticClass, if it exists.
		receiver = TrimEnd(receiver, originalName);

		// Await the async call.
		return $"await {receiver}{asyncName}({args})";
	}

	public static string TrimEnd(string input, string suffix)
	{
		if (input.EndsWith(suffix))
		{
			return input.Substring(0, input.Length - suffix.Length);
		}
		return input;
	}

	private string Print(SyntaxNode node)
	{
		return node.WithoutTrivia().ToFullString();
	}

	private string Print(SyntaxToken token)
	{
		return token.WithoutTrivia().ToFullString();
	}
}
