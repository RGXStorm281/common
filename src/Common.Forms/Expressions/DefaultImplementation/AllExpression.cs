namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

internal class AllExpression(IFormExpression<IEnumerable<bool>> operands) : IFormExpression<bool>
{
	private readonly IFormExpression<IEnumerable<bool>> _operands = operands;

	/// <inheritdoc />
	public bool EvaluateOn(IFormNode node)
	{
		var values = _operands.EvaluateOn(node);
		return values.All(value => value == true);
	}

	/// <inheritdoc />
	public async Task<bool> EvaluateOnAsync(IFormNode node)
	{
		var values = await _operands.EvaluateOnAsync(node);
		return values.All(value => value);
	}
}
