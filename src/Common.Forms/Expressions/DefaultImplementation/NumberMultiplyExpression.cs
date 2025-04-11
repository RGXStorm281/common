namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

internal class NumberMultiplyExpression(IFormExpression<IEnumerable<decimal>> factors) : IFormExpression<decimal>
{
	private readonly IFormExpression<IEnumerable<decimal>> _factors = factors;

	/// <inheritdoc />
	public decimal EvaluateOn(IFormNode node)
	{
		var factorValues = _factors.EvaluateOn(node);
		return factorValues.Aggregate(1m, (intermediateResult, next) => intermediateResult * next);
	}

	/// <inheritdoc />
	public async Task<decimal> EvaluateOnAsync(IFormNode node)
	{
		var factorValues = await _factors.EvaluateOnAsync(node);
		return factorValues.Aggregate(1m, (intermediateResult, next) => intermediateResult * next);
	}
}
