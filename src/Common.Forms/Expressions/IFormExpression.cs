namespace RobinEpple.Common.Forms.Expressions;

using RobinEpple.Common.Forms.Nodes;

public interface IFormExpression<TValue>
{
	/// <summary>
	/// Evaluates this expression on the given node.
	/// </summary>
	/// <param name="node">The node the expression is evaluated on.</param>
	/// <returns>The result of the evaluation.</returns>
	public TValue EvaluateOn(IFormNode node);

	/// <inheritdoc cref="EvaluateOn"/>
	public Task<TValue> EvaluateOnAsync(IFormNode node);
}
