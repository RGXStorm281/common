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
	private class TranslationContext(SemanticModel semanticModel, Dictionary<IMethodSymbol, string> awaitableOverloads)
	{
		public SemanticModel SemanticModel { get; } = semanticModel;
		public Dictionary<IMethodSymbol, string> AwaitableOverloads { get; } = awaitableOverloads;
		public bool IsRunningAsync => AwaitableOverloads.Count > 0;
	}

	/// <summary>
	/// Prints out the syntax tree with some rough formatting, replacing every method call that has an awaitable overload available <br/>
	/// with the corresponding async call.
	/// </summary>
	/// <param name="methodDeclaration">The method to translate.</param>
	/// <param name="semanticModel">The semantic model, for semantically identifying methods.</param>
	/// <param name="awaitableOverloads">A dictionary for resolving available async overload names for synchronous methods in the given syntax tree.</param>
	/// <returns>A dictionary, containing the name of the awaitable overload method for each replaceable method invocation in the body.</returns>
	public string TranslateMethodBody(
		MethodDeclarationSyntax methodDeclaration,
		SemanticModel semanticModel,
		IMethodSymbol methodSymbol,
		Dictionary<IMethodSymbol, string> awaitableOverloads
	)
	{
		var context = new TranslationContext(semanticModel, awaitableOverloads);

		// Translate the body if there is one.
		if (methodDeclaration.Body is { } blockBody)
		{
			// Edge case: If the original return type was void, the return statement at the end can be omitted.
			// If the async translation then has no await calls, it needs to return Task.CompletedTask at the end.
			// Therefore we need to add a return statement at the end.
			if (methodSymbol.ReturnsVoid && awaitableOverloads.Count == 0)
			{
				var returnStatement = SyntaxFactory.ReturnStatement();
				blockBody = methodDeclaration.Body.AddStatements(returnStatement);
			}

			return TranslateInternal(blockBody, context);
		}

		if (methodDeclaration.ExpressionBody is { } expressionBody)
		{
			var expression = TranslateInternal(expressionBody.Expression, context);
			return IndentHelper.Indent($"=> {expression};");
		}

		// If there is no body, just close the method signature.
		return IndentHelper.Indent(";");
	}

	/// <summary>
	/// Translates a statement into async.
	/// </summary>
	private string TranslateInternal(StatementSyntax? statement, TranslationContext context)
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
				var expression = TranslateInternal(commonForEachStatementSyntax.Expression, context);
				var body = TranslateInternal(commonForEachStatementSyntax.Statement, context);
				if (commonForEachStatementSyntax.Statement is not BlockSyntax)
				{
					body = IndentHelper.Indent(body);
				}
				if (commonForEachStatementSyntax.AwaitKeyword != null)
				{
					sb.Append("await ");
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
				var condition = TranslateInternal(doStatementSyntax.Condition, context);
				var body = TranslateInternal(doStatementSyntax.Statement, context);
				if (doStatementSyntax.Statement is not BlockSyntax)
				{
					body = IndentHelper.Indent(body);
				}
				sb.AppendLine("do");
				sb.AppendLine(body);
				sb.Append("while (");
				sb.Append(condition);
				sb.AppendLine(")");
				return sb.ToString();
			}
			case ExpressionStatementSyntax expressionStatementSyntax:
			{
				// Translate the expression.
				var expression = TranslateInternal(expressionStatementSyntax.Expression, context);
				sb.Append(expression);
				sb.AppendLine(";");
				return sb.ToString();
			}
			case FixedStatementSyntax fixedStatementSyntax:
			{
				// Print out the "fixed" header and translate the internal statement.
				var translatedStatement = TranslateInternal(fixedStatementSyntax.Statement, context);
				if (fixedStatementSyntax.Statement is not BlockSyntax)
				{
					translatedStatement = IndentHelper.Indent(translatedStatement);
				}
				sb.Append("fixed (");
				sb.Append(Print(fixedStatementSyntax.Declaration));
				sb.AppendLine(")");
				sb.Append(translatedStatement);
				return sb.ToString();
			}
			case ForStatementSyntax forStatementSyntax:
			{
				// Translate the condition and the loop body.
				var condition = TranslateInternal(forStatementSyntax.Condition, context);
				var block = TranslateInternal(forStatementSyntax.Statement, context);
				if (forStatementSyntax.Statement is not BlockSyntax)
				{
					block = IndentHelper.Indent(block);
				}
				var incrementors = forStatementSyntax.Incrementors.Select(incrementor =>
					TranslateInternal(incrementor, context)
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
			case GotoStatementSyntax gotoStatementSyntax:
			{
				// Translate the expression.
				var expression = TranslateInternal(gotoStatementSyntax.Expression, context);
				sb.Append("goto ");
				sb.Append(expression);
				sb.AppendLine(";");
				return sb.ToString();
			}
			case IfStatementSyntax ifStatementSyntax:
			{
				// Translate the condition and it's body.
				var condition = TranslateInternal(ifStatementSyntax.Condition, context);
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
				// Translate the locked expression and the inner block.
				var expression = TranslateInternal(lockStatementSyntax.Expression, context);
				var block = TranslateInternal(lockStatementSyntax.Statement, context);
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
				// Translate each initializer expression.
				var declarations = localDecl
					.Declaration.Variables.Select(variable =>
					{
						var variableBuilder = new StringBuilder();
						variableBuilder.Append(Print(variable.Identifier));
						if (variable.Initializer != null)
						{
							var initializer = TranslateInternal(variable.Initializer.Value, context);
							variableBuilder.Append(" = ");
							variableBuilder.Append(initializer);
						}
						return variableBuilder.ToString();
					})
					.ToList();

				sb.Append(Print(localDecl.Declaration.Type));
				sb.Append(" ");
				if (declarations.Count == 1)
				{
					sb.Append(declarations[0]);
					sb.AppendLine(";");
					return sb.ToString();
				}

				// More than one.
				for (int i = 0; i < declarations.Count; i++)
				{
					var declaration = declarations[i];
					if (i > 0)
					{
						declaration = IndentHelper.Indent(declaration);
					}
					sb.Append(declaration);
					if (i < declarations.Count - 1)
					{
						sb.AppendLine(",");
					}
				}
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
					var expression = TranslateInternal(returnStatementSyntax.Expression, context);

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
						sb.Append("return System.Threading.Tasks.Task.FromResult(");
						sb.Append(expression);
						sb.AppendLine(");");
						return sb.ToString();
					}
				}
			}
			case SwitchStatementSyntax switchStatementSyntax:
			{
				// Translate the target expression and each case body.
				var expression = TranslateInternal(switchStatementSyntax.Expression, context);
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
				var expression = TranslateInternal(throwStatementSyntax.Expression, context);
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
						sb.Append(" (");
						sb.Append(Print(catchBlock.Declaration));
						sb.Append(")");
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
			case UnsafeStatementSyntax unsafeStatementSyntax:
			{
				// Print the keyword and translate the inner block.
				var block = TranslateInternal(unsafeStatementSyntax.Block, context);
				sb.AppendLine("unsafe");
				sb.Append(block);
				return block;
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
				if (usingStatementSyntax.Declaration != null)
				{
					sb.Append(Print(usingStatementSyntax.Declaration));
					sb.Append(" = ");
				}
				if (usingStatementSyntax.Expression != null)
				{
					var expression = TranslateInternal(usingStatementSyntax.Expression, context);
					sb.Append(expression);
				}
				sb.AppendLine(")");
				sb.Append(translatedStatement);
				return sb.ToString();
			}
			case WhileStatementSyntax whileStatementSyntax:
			{
				// Translate the condition and the loop body.
				var condition = TranslateInternal(whileStatementSyntax.Condition, context);
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
				var expression = TranslateInternal(yieldStatementSyntax.Expression, context);
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
			case LocalFunctionStatementSyntax:
			default:
			{
				// Just print out the statement, no translation needed.
				sb.Append(Print(statement));
				sb.AppendLine();
				return sb.ToString();
			}
		}
	}

	/// <summary>
	/// Translates an expression into async.
	/// </summary>
	private string TranslateInternal(ExpressionSyntax? expression, TranslationContext context)
	{
		if (expression == null)
		{
			return string.Empty;
		}

		switch (expression)
		{
			case AssignmentExpressionSyntax assignmentExpressionSyntax:
			{
				// Translate both sides and print the assignment.
				var left = TranslateInternal(assignmentExpressionSyntax.Left, context);
				var right = TranslateInternal(assignmentExpressionSyntax.Right, context);
				var assignment = Print(assignmentExpressionSyntax.OperatorToken);
				return $"{left} {assignment} {right}";
			}
			case BinaryExpressionSyntax binaryExpressionSyntax:
			{
				// Translate both sides and combine with the binary operator.
				var left = TranslateInternal(binaryExpressionSyntax.Left, context);
				var right = TranslateInternal(binaryExpressionSyntax.Right, context);
				return $"{left} {Print(binaryExpressionSyntax.OperatorToken)} {right}";
			}
			case CastExpressionSyntax castExpressionSyntax:
			{
				// Print the cast type and translate the inner expression.
				var innerExpression = TranslateInternal(castExpressionSyntax.Expression, context);
				return $"({Print(castExpressionSyntax.Type)}){innerExpression}";
			}
			case ConditionalAccessExpressionSyntax conditionalAccessExpressionSyntax:
			{
				// Translate the receiver and the access expression.
				var receiver = TranslateInternal(conditionalAccessExpressionSyntax.Expression, context);
				var whenNotNull = TranslateInternal(conditionalAccessExpressionSyntax.WhenNotNull, context);
				return $"{receiver}?.{whenNotNull}";
			}
			case ConditionalExpressionSyntax conditionalExpressionSyntax:
			{
				// Translate the condition, true and false cases and wrap them back in a ternary operator.
				var condition = TranslateInternal(conditionalExpressionSyntax.Condition, context);
				var whenTrue = TranslateInternal(conditionalExpressionSyntax.WhenTrue, context);
				var whenFalse = TranslateInternal(conditionalExpressionSyntax.WhenFalse, context);

				var sb = new StringBuilder();
				sb.AppendLine(condition);
				sb.AppendLine(IndentHelper.Indent("? " + whenTrue));
				sb.Append(IndentHelper.Indent(": " + whenFalse));
				return sb.ToString();
			}
			case InvocationExpressionSyntax invocationExpressionSyntax:
			{
				// Here is the actual magic: Translate into an async call if possible.
				return TryTranslateInvocationToAsync(invocationExpressionSyntax, context);
			}
			case MemberAccessExpressionSyntax memberAccessExpressionSyntax:
			{
				// Translate the receiver and append the member access.
				var receiver = TranslateInternal(memberAccessExpressionSyntax.Expression, context);
				var member = memberAccessExpressionSyntax.Name;
				return $"{receiver}.{member}";
			}
			case ParenthesizedExpressionSyntax parenthesizedExpressionSyntax:
			{
				// Translate the inner expression and put them back in brackets.
				var inner = TranslateInternal(parenthesizedExpressionSyntax.Expression, context);
				return $"({inner})";
			}
			case PostfixUnaryExpressionSyntax postfixUnaryExpressionSyntax:
			{
				// Translate the inner expression and append the operator.
				var inner = TranslateInternal(postfixUnaryExpressionSyntax.Operand, context);
				var unaryOperator = Print(postfixUnaryExpressionSyntax.OperatorToken);
				return $"{inner}{unaryOperator}";
			}
			case PrefixUnaryExpressionSyntax prefixUnaryExpressionSyntax:
			{
				// Translate the inner expression and prepend the operator.
				var inner = TranslateInternal(prefixUnaryExpressionSyntax.Operand, context);
				var unaryOperator = Print(prefixUnaryExpressionSyntax.OperatorToken);
				return $"{unaryOperator}{inner}";
			}
			case SwitchExpressionSyntax switchExpressionSyntax:
			{
				// Translate the condition and each case arm.
				var sb = new StringBuilder();
				var condition = TranslateInternal(switchExpressionSyntax.GoverningExpression, context);
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
					var inner = TranslateInternal(arm.Expression, context);
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
					var inner = TranslateInternal(argument.Expression, context);
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
	private string TryTranslateInvocationToAsync(InvocationExpressionSyntax invocation, TranslationContext context)
	{
		// Get the symbol of the method being called
		var symbolInfo = context.SemanticModel.GetSymbolInfo(invocation);
		var originalMethod = symbolInfo.Symbol as IMethodSymbol;
		if (originalMethod == null)
		{
			// Don't understand this invocation, just print it.
			return Print(invocation);
		}

		// Translate each argument.
		var args = string.Join(
			", ",
			invocation.ArgumentList.Arguments.Select(a => TranslateInternal(a.Expression, context))
		);

		// Rewrite the base call.
		var receiver = TranslateInternal(invocation.Expression, context);

		// Check if an overload exists.
		if (!context.AwaitableOverloads.TryGetValue(originalMethod, out var asyncName))
		{
			// No async overload found — keep original, but call translated arguments.
			return $"{receiver}({args})";
		}

		// Async overload found - replace the method name with the async one and await the call.
		var originalName = originalMethod.Name;

		// Translate the receiver string like obj?. or a StaticClass, if it exists.
		receiver = TrimEnd(receiver, originalName);

		// Wrap the async call in brackets, in case the parent is a chained expression operation (member access, equality operator, ...).
		return $"(await {receiver}{asyncName}({args}))";
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
