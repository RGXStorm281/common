namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

internal partial class Throw<TValue>(Func<IFormNode, Exception> exceptionFactory) : IFormExpression<TValue>
{
	private readonly Func<IFormNode, Exception> _exceptionFactory = exceptionFactory;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public TValue EvaluateOn(IFormNode node) => throw _exceptionFactory(node);
}
