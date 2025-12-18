namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

[StaticFactoryMethodName("CastInt")]
internal partial class NumberCastInt([StaticFactoryThis] IFormExpression<decimal> target) : IFormExpression<int>
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
