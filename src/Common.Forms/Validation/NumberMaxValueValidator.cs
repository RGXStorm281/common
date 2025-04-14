namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// Can only be applied to <see cref="INumberNode">.<br/>
/// Only active on non-<see langword="null"/> values.<br/>
/// Checks the field value against defined maximum value.
/// </summary>
/// <param name="minValue">The expression defining the (inclusive) upper bound the field accepts.</param>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name and {1} for the maximum value.</param>
public class NumberMaxValueValidator(IFormExpression<decimal?> maxValue, string? errorMessageTemplate = null)
	: INodeValidator
{
	public const string ErrorKey = nameof(NumberMaxValueValidator);
	private readonly IFormExpression<decimal?> _maxValue = maxValue;
	private readonly string _errorMessageTemplate = errorMessageTemplate ?? Resources.TheField_AllowsAMaximumValueOf_;

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
