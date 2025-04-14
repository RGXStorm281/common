namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// Can only be applied to <see cref="ITextNode">.<br/>
/// Only active on non-<see langword="null"/> values.<br/>
/// Checks the field value against defined maximum length.
/// </summary>
/// <param name="maxLength">The expression defining the (inclusive) lower bound for the content length.</param>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name and {1} for the minimum value.</param>
public class MaxLengthValidator(IFormExpression<decimal?> maxLength, string? errorMessageTemplate = null)
	: INodeValidator
{
	public const string ErrorKey = nameof(MaxLengthValidator);
	private readonly IFormExpression<decimal?> _maxLength = maxLength;
	private readonly string _errorMessageTemplate =
		errorMessageTemplate ?? Resources.TheField_AllowsAMaximumContentLengthOf_;

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
