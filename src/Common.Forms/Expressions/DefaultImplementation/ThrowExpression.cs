namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

public class ThrowExpression<TValue>(Func<IFormNode, Exception> exceptionFactory) : IFormExpression<TValue>
{
	private readonly Func<IFormNode, Exception> _exceptionFactory = exceptionFactory;

	/// <inheritdoc />
	public TValue EvaluateOn(IFormNode node) => throw _exceptionFactory(node);

	/// <inheritdoc />
	public Task<TValue> EvaluateOnAsync(IFormNode node) => throw _exceptionFactory(node);
}
