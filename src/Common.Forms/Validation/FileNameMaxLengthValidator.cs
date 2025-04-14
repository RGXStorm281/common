namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// Can only be applied to <see cref="IFileNode">.<br/>
/// Only active on non-<see langword="null"/> file names.<br/>
/// Checks the file name against the defined maximum length.
/// </summary>
/// <param name="maxLength">The expression defining the (inclusive) upper bound for the file name length.</param>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name and {1} for the maximum length.</param>
public class FileNameMaxLengthValidator(IFormExpression<decimal?> maxLength, string? errorMessageTemplate = null)
	: INodeValidator
{
	public const string ErrorKey = nameof(FileNameMaxLengthValidator);
	private readonly IFormExpression<decimal?> _maxLength = maxLength;
	private readonly string _errorMessageTemplate =
		errorMessageTemplate ?? Resources.TheField_AllowsAMaximumFileNameLengthOf_;

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
