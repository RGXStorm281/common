namespace RobinEpple.Common.Forms.Test;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.Validation;

public class InvalidMockValidator : INodeValidator
{
	/// <inhertidoc />
	public void Validate(IFormNode node)
	{
		// Always set the error.
		node.SetValidationError(nameof(InvalidMockValidator), "always wrong");
	}

	/// <inhertidoc />
	public Task ValidateAsync(IFormNode node)
	{
		// Always set the error.
		node.SetValidationError(nameof(InvalidMockValidator), "always wrong");
		return Task.CompletedTask;
	}
}
