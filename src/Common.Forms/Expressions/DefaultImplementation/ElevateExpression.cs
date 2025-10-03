namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

internal partial class ElevateExpression<TValue> : IFormExpression<TValue>
{
	private readonly IFormExpression<int> _numberOfScopes;
	private readonly IFormExpression<TValue> _expression;

	public ElevateExpression(IFormExpression<int> numberOfScopes, IFormExpression<TValue> expression)
	{
		_numberOfScopes = numberOfScopes;
		_expression = expression;
	}

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public TValue EvaluateOn(IFormNode node)
	{
		var numberOfScopesValue = _numberOfScopes.EvaluateOn(node);
		if (numberOfScopesValue < 1)
		{
			throw new InvalidOperationException($"The expression can only elevate a positive amount of scopes.");
		}

		var targetScope = node.GetScope();
		for (int i = 0; i < numberOfScopesValue; i++)
		{
			if (targetScope.Parent == null)
			{
				throw new NodeNotFoundException($"There are less parent scopes than specified.");
			}
			try
			{
				targetScope = targetScope.Parent.GetScope();
			}
			catch (InvalidOperationException)
			{
				throw new NodeNotFoundException($"There are less parent scopes than specified.");
			}
		}

		return _expression.EvaluateOn(targetScope);
	}
}
