namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

internal partial class Conditional<TValue>(
	[StaticFactoryThis] IFormExpression<bool> condition,
	IFormExpression<TValue> whenTrue,
	IFormExpression<TValue> whenFalse
) : IFormExpression<TValue>
{
	private readonly IFormExpression<bool> _condition = condition;
	private readonly IFormExpression<TValue> _whenTrue = whenTrue;
	private readonly IFormExpression<TValue> _whenFalse = whenFalse;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public TValue EvaluateOn(IFormNode node)
	{
		var conditionMet = _condition.EvaluateOn(node);
		if (conditionMet)
		{
			return _whenTrue.EvaluateOn(node);
		}
		else
		{
			return _whenFalse.EvaluateOn(node);
		}
	}
}
