namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// Can only be applied to <see cref="ITextNode">.<br/>
/// Only active on non-<see langword="null"/> values.<br/>
/// Checks the provided value against a whitelist of file name symbols.
/// </summary>
/// <param name="characterWhitelist">The whitelist of symbols that are allowed to occur in the value.</param>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name and {1} for the invalid characters.</param>
public class AllowedSymbolValidator(string characterWhitelist, string? errorMessageTemplate = null) : INodeValidator
{
	public const string ErrorKey = nameof(FileRequiredValidator);
	private readonly string _characterWhitelist = characterWhitelist;
	private readonly string _errorMessageTemplate =
		errorMessageTemplate ?? Resources.TheField_DoesNotAllowTheFollowingCharacters_;

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
