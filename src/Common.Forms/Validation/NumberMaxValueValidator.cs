namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// Can only be applied to <see cref="INumberNode">.<br/>
/// Only active on non-<see langword="null"/> values.<br/>
/// Checks the field value against defined maximum value.
/// </summary>
/// <param name="minValue">The expression defining the (inclusive) upper bound the field accepts.</param>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the invalid value and {1} for the field name.</param>
public class NumberMaxValueValidator(IFormExpression<decimal?> maxValue, string? errorMessageTemplate = null)
	: INodeValidator
{
	public const string ErrorKey = nameof(FileRequiredValidator);
	private readonly IFormExpression<decimal?> _maxValue = maxValue;
	private readonly string _errorMessageTemplate = errorMessageTemplate ?? Resources.TheField_RequiresAMinimumValueOf_;

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
