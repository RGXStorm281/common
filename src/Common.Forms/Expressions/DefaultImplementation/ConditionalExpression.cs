namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

public class ConditionalExpression<TValue>(
	IFormExpression<bool> condition,
	IFormExpression<TValue> whenTrue,
	IFormExpression<TValue> whenFalse
) : IFormExpression<TValue>
{
	private readonly IFormExpression<bool> _condition = condition;
	private readonly IFormExpression<TValue> _whenTrue = whenTrue;
	private readonly IFormExpression<TValue> _whenFalse = whenFalse;

	/// <inheritdoc />
	public TValue EvaluateOn(IFormNode node)
	{
		var conditionMet = _condition.EvaluateOn(node);
		if (conditionMet)
		{
			return _whenTrue.EvaluateOn(node);
		}
		else
		{
			return _whenFalse.EvaluateOn(node);
		}
	}

	/// <inheritdoc />
	public async Task<TValue> EvaluateOnAsync(IFormNode node)
	{
		var conditionMet = await _condition.EvaluateOnAsync(node);
		if (conditionMet)
		{
			return await _whenTrue.EvaluateOnAsync(node);
		}
		else
		{
			return await _whenFalse.EvaluateOnAsync(node);
		}
	}
}
