namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// Can only be applied to <see cref="IFileNode">.<br/>
/// Only active on non-<see langword="null"/> file contents.<br/>
/// Checks the size of the provided file against a maximum file size.
/// </summary>
/// <param name="maxFileSize">The maximum size a file is allowed to be.</param>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name and {1} for the file size.</param>
public class MaxFileSizeValidator(long maxFileSize, string? errorMessageTemplate = null) : INodeValidator
{
	public const string ErrorKey = nameof(TemplateRequiredValidator);
	private readonly long _maxFileSize = maxFileSize;
	private readonly string _errorMessageTemplate =
		errorMessageTemplate ?? Resources.TheFileInput_HasAMaximumFileSizeOf_;

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
