namespace RobinEpple.Common.Forms.BasicApi;

using RobinEpple.Common.Forms.BasicApi.Nodes;

/// <summary>
/// Interface for validators. Validators are not cloned, so they should not keep any state.
/// </summary>
/// <typeparam name="TNode">The type of the node to validate.</typeparam>
public interface INodeValidator<in TNode>
	where TNode : IFormNode
{
	/// <summary>
	/// Executes the validation on the node.
	/// </summary>
	/// <param name="node">The node to validate.</param>
	/// <param name="context">Context information for the validation.</param>
	public void Validate(TNode node, StateUpdateContext context);
}
