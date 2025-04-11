namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

internal class NumberModuloExpression(IFormExpression<int> target, IFormExpression<int> field) : IFormExpression<int>
{
	private readonly IFormExpression<int> _target = target;
	private readonly IFormExpression<int> _field = field;

	/// <inheritdoc />
	public int EvaluateOn(IFormNode node)
	{
		var targetValue = _target.EvaluateOn(node);
		var fieldValue = _field.EvaluateOn(node);
		return targetValue % fieldValue;
	}

	/// <inheritdoc />
	public async Task<int> EvaluateOnAsync(IFormNode node)
	{
		var targetValue = await _target.EvaluateOnAsync(node);
		var fieldValue = await _field.EvaluateOnAsync(node);
		return targetValue % fieldValue;
	}
}
