namespace RobinEpple.Common.Forms.Test.Mocks;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.Validation;

public class ValidMockValidator : INodeValidator
{
	/// <inhertidoc />
	public void Validate(IFormNode node)
	{
		// Do nothing.
	}

	/// <inhertidoc />
	public Task ValidateAsync(IFormNode node)
	{
		// Do nothing.
		return Task.CompletedTask;
	}
}
