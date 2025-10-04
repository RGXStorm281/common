namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

internal partial class NumberCastIntExpression(IFormExpression<decimal> target) : IFormExpression<int>
{
	private readonly IFormExpression<decimal> _target = target;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public int EvaluateOn(IFormNode node)
	{
		var targetValue = _target.EvaluateOn(node);
		return (int)targetValue;
	}
}
