namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

internal partial class NumberSumExpression(IFormExpression<IEnumerable<decimal>> summands) : IFormExpression<decimal>
{
	private readonly IFormExpression<IEnumerable<decimal>> _summands = summands;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public decimal EvaluateOn(IFormNode node)
	{
		var summandValues = _summands.EvaluateOn(node);
		return summandValues.Sum();
	}
}
