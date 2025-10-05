namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

internal partial class NumberCastDecimalExpression(IFormExpression<int> target) : IFormExpression<decimal>
{
	private readonly IFormExpression<int> _target = target;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public decimal EvaluateOn(IFormNode node)
	{
		var targetValue = _target.EvaluateOn(node);
		return targetValue;
	}
}
