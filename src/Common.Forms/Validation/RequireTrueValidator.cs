namespace RobinEpple.Common.Forms.Validation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// Can only be applied to <see cref="IBooleanNode">.<br/>
/// Requires the field to have the value <see langword="true"/>.
/// </summary>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name.</param>
public class RequireTrueValidator(string? errorMessageTemplate = null) : INodeValidator
{
	public const string ErrorKey = nameof(RequireTrueValidator);
	private readonly string _errorMessageTemplate = errorMessageTemplate ?? Resources.TheField_RequiresTheValueTrue;

	/// <inheritdoc />
	public void Validate(IFormNode node)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public Task ValidateAsync(IFormNode node)
	{
		throw new NotImplementedException();
	}
}
