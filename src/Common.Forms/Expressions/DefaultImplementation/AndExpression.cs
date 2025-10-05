namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

internal partial class AndExpression(IFormExpression<bool> left, IFormExpression<bool> right) : IFormExpression<bool>
{
	private readonly IFormExpression<bool> _left = left;
	private readonly IFormExpression<bool> _right = right;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public bool EvaluateOn(IFormNode node)
	{
		var leftValue = _left.EvaluateOn(node);
		var rightValue = _right.EvaluateOn(node);
		return leftValue && rightValue;
	}
}
