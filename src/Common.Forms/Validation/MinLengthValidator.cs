namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// Can only be applied to <see cref="ITextNode">.<br/>
/// Only active on non-<see langword="null"/> values.<br/>
/// Checks the field value against defined minimum length.
/// </summary>
/// <param name="minLength">The expression defining the (inclusive) lower bound for the content length.</param>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name and {1} for the minimum value.</param>
public class MinLengthValidator(IFormExpression<decimal?> minLength, string? errorMessageTemplate = null)
	: INodeValidator
{
	public const string ErrorKey = nameof(FileRequiredValidator);
	private readonly IFormExpression<decimal?> _minLength = minLength;
	private readonly string _errorMessageTemplate =
		errorMessageTemplate ?? Resources.TheField_RequiresAMinimumContentLengthOf_;

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
