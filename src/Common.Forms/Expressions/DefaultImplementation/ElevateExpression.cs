namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Nodes;

internal class ElevateExpression<TValue> : IFormExpression<TValue>
{
	private readonly IFormExpression<int> _numberOfScopes;
	private readonly IFormExpression<TValue> _expression;

	public ElevateExpression(IFormExpression<int> numberOfScopes, IFormExpression<TValue> expression)
	{
		_numberOfScopes = numberOfScopes;
		_expression = expression;
	}

	/// <inheritdoc />
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

	/// <inheritdoc />
	public async Task<TValue> EvaluateOnAsync(IFormNode node)
	{
		var numberOfScopesValue = await _numberOfScopes.EvaluateOnAsync(node);
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

		return await _expression.EvaluateOnAsync(targetScope);
	}
}
