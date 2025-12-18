namespace RobinEpple.Common.Forms.Expressions;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

/// <summary>
/// This interface represents an expression tree evaluating to the specified value type.<br/>
/// The exact value is determined by evaluating the expression on a specific form node.<br/>
/// The node provides the context (which nodes are visible and what state they are in).
/// </summary>
/// <typeparam name="TValue">The value type, this expression evaluates to.</typeparam>
public partial interface IFormExpression<TValue>
{
	/// <summary>
	/// Evaluates this expression on the given node.
	/// </summary>
	/// <param name="node">The node the expression is evaluated on.</param>
	/// <returns>The result of the evaluation.</returns>
	[GenerateAsyncOverload]
	public TValue EvaluateOn(IFormNode node);
}
