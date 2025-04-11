namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

internal class NumberCastIntExpression(IFormExpression<decimal> target) : IFormExpression<int>
{
	private readonly IFormExpression<decimal> _target = target;

	/// <inheritdoc />
	public int EvaluateOn(IFormNode node)
	{
		var targetValue = _target.EvaluateOn(node);
		return (int)targetValue;
	}

	/// <inheritdoc />
	public async Task<int> EvaluateOnAsync(IFormNode node)
	{
		var targetValue = await _target.EvaluateOnAsync(node);
		return (int)targetValue;
	}
}
