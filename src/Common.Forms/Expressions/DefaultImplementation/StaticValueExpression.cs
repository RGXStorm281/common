namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

internal partial class StaticValueExpression<TValue>(TValue value) : IFormExpression<TValue>
{
	private readonly TValue _value = value;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public TValue EvaluateOn(IFormNode node) => _value;
}
