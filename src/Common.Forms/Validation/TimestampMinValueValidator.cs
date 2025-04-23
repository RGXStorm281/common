namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Util;

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
		if (node is not ITimestampNode timestampNode)
		{
			throw new InvalidOperationException(
				$"A {nameof(TimestampMinValueValidator)} can only be used on timestamp nodes and not on '{node.GetType().FullName}'."
			);
		}

		if (timestampNode.Value == null)
		{
			// Do not validate empty, this is the task of the required validation.
			return;
		}

		var minValue = _minValue.EvaluateOn(timestampNode);
		if (timestampNode.Value < minValue)
		{
			// Invalid.
			timestampNode.SetValidationError(ErrorKey, _errorMessageTemplate.Format(timestampNode.Label, minValue));
		}
	}

	/// <inheritdoc />
	public async Task ValidateAsync(IFormNode node)
	{
		if (node is not ITimestampNode timestampNode)
		{
			throw new InvalidOperationException(
				$"A {nameof(TimestampMinValueValidator)} can only be used on timestamp nodes and not on '{node.GetType().FullName}'."
			);
		}

		if (timestampNode.Value == null)
		{
			// Do not validate empty, this is the task of the required validation.
			return;
		}

		var minValue = await _minValue.EvaluateOnAsync(timestampNode);
		if (timestampNode.Value < minValue)
		{
			// Invalid.
			timestampNode.SetValidationError(ErrorKey, _errorMessageTemplate.Format(timestampNode.Label, minValue));
		}
	}
}
