namespace RobinEpple.Common.Forms.Test.Mocks;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.Validation;

public class InvalidMockValidator : INodeValidator
{
	public bool HasBeenCalled { get; private set; } = false;

	/// <inhertidoc />
	public void Validate(IFormNode node)
	{
		// Always set the error.
		node.SetValidationError(nameof(InvalidMockValidator), "always wrong");
		HasBeenCalled = true;
	}

	/// <inhertidoc />
	public Task ValidateAsync(IFormNode node)
	{
		// Always set the error.
		node.SetValidationError(nameof(InvalidMockValidator), "always wrong");
		HasBeenCalled = true;
		return Task.CompletedTask;
	}
}
