namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

internal partial class NumberAverageExpression(IFormExpression<IEnumerable<decimal>> items) : IFormExpression<decimal>
{
	private readonly IFormExpression<IEnumerable<decimal>> _items = items;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public decimal EvaluateOn(IFormNode node)
	{
		var itemValues = _items.EvaluateOn(node);
		return itemValues.Average();
	}
}
