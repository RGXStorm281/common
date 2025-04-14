namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// Can only be applied to <see cref="ITimestampNode">.<br/>
/// Only active on non-<see langword="null"/> values.<br/>
/// Checks the field value against defined minimum value.
/// </summary>
/// <param name="minValue">The expression defining the (inclusive) lower bound the field accepts.</param>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name and {1} for the minimum value.</param>
public class TimestampMinValueValidator(IFormExpression<DateTime?> minValue, string? errorMessageTemplate = null)
	: INodeValidator
{
	public const string ErrorKey = nameof(TimestampMinValueValidator);
	private readonly IFormExpression<DateTime?> _minValue = minValue;
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
