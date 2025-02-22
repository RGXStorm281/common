namespace RobinEpple.Common.Forms.BasicApi;

using RobinEpple.Common.Forms.BasicApi.Nodes;

public interface IExpression<out TResult> : ICloneable
{
	/// <summary>
	/// Evaluates the expression on the node.
	/// </summary>
	/// <param name="node">The current node.</param>
	/// <param name="context">The evaluation context.</param>
	/// <returns>The result of the expression.</returns>
	TResult EvaluateOn(IFormNode node, StateUpdateContext context);
}
