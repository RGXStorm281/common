namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

internal class StaticValueExpression<TValue>(TValue value) : IFormExpression<TValue>
{
	private readonly TValue _value = value;

	/// <inheritdoc />
	public TValue EvaluateOn(IFormNode node) => _value;

	/// <inheritdoc />
	public Task<TValue> EvaluateOnAsync(IFormNode node) => Task.FromResult(_value);
}
