namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Nodes;

public interface INodeValidator
{
	/// <summary>
	/// Validates this node.<br/>
	/// Error messages this validator is responsible for are added, <br/>
	// if the node is not valid or removed otherwise.
	/// </summary>
	/// <param name="node">The node to validate.</param>
	public void Validate(IFormNode node);

	/// <inheritdoc cref="Validate"/>
	public Task ValidateAsync(IFormNode node);
}
