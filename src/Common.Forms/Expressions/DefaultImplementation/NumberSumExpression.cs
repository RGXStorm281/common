namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

internal class NumberSumExpression(IFormExpression<IEnumerable<decimal>> summands) : IFormExpression<decimal>
{
	private readonly IFormExpression<IEnumerable<decimal>> _summands = summands;

	/// <inheritdoc />
	public decimal EvaluateOn(IFormNode node)
	{
		var summandValues = _summands.EvaluateOn(node);
		return summandValues.Sum();
	}

	/// <inheritdoc />
	public async Task<decimal> EvaluateOnAsync(IFormNode node)
	{
		var summandValues = await _summands.EvaluateOnAsync(node);
		return summandValues.Sum();
	}
}
