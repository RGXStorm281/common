namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

public class NumberAverageExpression(IFormExpression<IEnumerable<decimal>> items) : IFormExpression<decimal>
{
	private readonly IFormExpression<IEnumerable<decimal>> _items = items;

	/// <inheritdoc />
	public decimal EvaluateOn(IFormNode node)
	{
		var itemValues = _items.EvaluateOn(node);
		return itemValues.Average();
	}

	/// <inheritdoc />
	public async Task<decimal> EvaluateOnAsync(IFormNode node)
	{
		var itemValues = await _items.EvaluateOnAsync(node);
		return itemValues.Average();
	}
}
