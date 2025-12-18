namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

/// <summary>
/// This interface defines an API for a single validation step in the <see cref="IFormNode.Update"/> pipeline.
/// </summary>
public partial interface INodeValidator
{
	/// <summary>
	/// Validates this node.<br/>
	/// Error messages this validator is responsible for are added, <br/>
	/// if the node is not valid or removed otherwise.
	/// </summary>
	/// <param name="node">The node to validate.</param>
	[GenerateAsyncOverload]
	public void Validate(IFormNode node);
}
