namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

public class NumberCastDecimalExpression(IFormExpression<int> target) : IFormExpression<decimal>
{
	private readonly IFormExpression<int> _target = target;

	/// <inheritdoc />
	public decimal EvaluateOn(IFormNode node)
	{
		var targetValue = _target.EvaluateOn(node);
		return targetValue;
	}

	/// <inheritdoc />
	public async Task<decimal> EvaluateOnAsync(IFormNode node)
	{
		var targetValue = await _target.EvaluateOnAsync(node);
		return targetValue;
	}
}
