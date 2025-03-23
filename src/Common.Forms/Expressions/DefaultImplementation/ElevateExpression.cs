namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Nodes;

public class ElevateExpression<TValue> : IFormExpression<TValue>
{
	private readonly int _numberOfScopes;
	private readonly IFormExpression<TValue> _expression;

	public ElevateExpression(int numberOfScopes, IFormExpression<TValue> expression)
	{
		if (numberOfScopes < 1)
		{
			throw new InvalidOperationException($"The expression can only elevate a positive amount of scopes.");
		}
		_numberOfScopes = numberOfScopes;
		_expression = expression;
	}

	/// <inheritdoc />
	public TValue EvaluateOn(IFormNode node)
	{
		var targetScope = node.GetScope();
		for (int i = 0; i < _numberOfScopes; i++)
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
		var targetScope = node.GetScope();
		for (int i = 0; i < _numberOfScopes; i++)
		{
			if (targetScope.Parent == null)
			{
				throw new NodeNotFoundException($"There are less parent scopes than specified.");
			}
			try
			{
				targetScope.Parent.GetScope();
			}
			catch (InvalidOperationException)
			{
				throw new NodeNotFoundException($"There are less parent scopes than specified.");
			}
		}

		return await _expression.EvaluateOnAsync(targetScope);
	}
}
