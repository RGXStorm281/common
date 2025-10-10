namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

internal partial class NumberModulo(IFormExpression<int> target, IFormExpression<int> field) : IFormExpression<int>
{
	private readonly IFormExpression<int> _target = target;
	private readonly IFormExpression<int> _field = field;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public int EvaluateOn(IFormNode node)
	{
		var targetValue = _target.EvaluateOn(node);
		var fieldValue = _field.EvaluateOn(node);
		return targetValue % fieldValue;
	}
}
