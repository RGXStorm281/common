namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// Can only be applied to <see cref="ITextNode">.<br/>
/// Only active on non-<see langword="null"/> values.<br/>
/// Requires the field value to be a valid email format.
/// </summary>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the invalid value.</param>
public class EmailValidator(string? errorMessageTemplate = null) : INodeValidator
{
	public const string ErrorKey = nameof(EmailValidator);
	private readonly string _errorMessageTemplate =
		errorMessageTemplate ?? Resources.TheValue_CouldNotBeRecognizedAsAValidEmailFormat;

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
