namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// Can only be applied to <see cref="IFileNode">.<br/>
/// Only active on non-<see langword="null"/> file names.<br/>
/// Checks the provided file name against a whitelist of file name symbols.
/// </summary>
/// <param name="characterWhitelist">The whitelist of symbols that are allowed to occur in a filename.</param>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the invalid characters.</param>
public class AllowedFileNameSymbolValidator(string characterWhitelist, string? errorMessageTemplate = null)
	: INodeValidator
{
	public const string ErrorKey = nameof(AllowedFileNameSymbolValidator);
	private readonly string _characterWhitelist = characterWhitelist;
	private readonly string _errorMessageTemplate =
		errorMessageTemplate ?? Resources.TheFollowingCharactersAreNotAllowedInAFileName_;

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
