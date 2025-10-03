namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

internal partial class AnyExpression(IFormExpression<IEnumerable<bool>> operands) : IFormExpression<bool>
{
	private readonly IFormExpression<IEnumerable<bool>> _operands = operands;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public bool EvaluateOn(IFormNode node)
	{
		var values = _operands.EvaluateOn(node);
		return values.Any(value => value == true);
	}
}
