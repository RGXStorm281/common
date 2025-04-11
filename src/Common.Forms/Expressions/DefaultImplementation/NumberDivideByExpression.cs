namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

internal class NumberDivideByExpression(IFormExpression<decimal> target, IFormExpression<decimal> factor)
	: IFormExpression<decimal>
{
	private readonly IFormExpression<decimal> _target = target;
	private readonly IFormExpression<decimal> _factor = factor;

	/// <inheritdoc />
	public decimal EvaluateOn(IFormNode node)
	{
		var targetValue = _target.EvaluateOn(node);
		var factorValue = _factor.EvaluateOn(node);
		return targetValue / factorValue;
	}

	/// <inheritdoc />
	public async Task<decimal> EvaluateOnAsync(IFormNode node)
	{
		var targetValue = await _target.EvaluateOnAsync(node);
		var factorValue = await _factor.EvaluateOnAsync(node);
		return targetValue / factorValue;
	}
}
