namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// Can only be applied to <see cref="IFileNode">.<br/>
/// Only active on non-<see langword="null"/> file contents.<br/>
/// Estimates the mime type of a given byte string and checks it against a list of valid extensions.
/// </summary>
/// <param name="allowedExtensions">The list of allowed file extensions.</param>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name and {1} for the list of allowed extensions.</param>
public class FileExtensionValidator(string[] allowedExtensions, string? errorMessageTemplate = null) : INodeValidator
{
	public const string ErrorKey = nameof(TemplateRequiredValidator);
	private readonly string[] _allowedExtensions = allowedExtensions;
	private readonly string _errorMessageTemplate =
		errorMessageTemplate ?? Resources.TheFileInput_OnlyAllowsFilesOfTheFollowingTypes_;

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
