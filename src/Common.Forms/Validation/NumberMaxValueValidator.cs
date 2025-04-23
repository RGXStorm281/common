namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Util;

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
		if (node is not INumberNode numberNode)
		{
			throw new InvalidOperationException(
				$"A {nameof(NumberMaxValueValidator)} can only be used on number nodes and not on '{node.GetType().FullName}'."
			);
		}

		if (numberNode.Value == null)
		{
			// Do not validate empty, this is the task of the required validation.
			return;
		}

		var maxValue = _maxValue.EvaluateOn(numberNode);
		if (numberNode.Value > maxValue)
		{
			// Invalid.
			numberNode.SetValidationError(ErrorKey, _errorMessageTemplate.Format(numberNode.Label, maxValue));
		}
	}

	/// <inheritdoc />
	public async Task ValidateAsync(IFormNode node)
	{
		if (node is not INumberNode numberNode)
		{
			throw new InvalidOperationException(
				$"A {nameof(NumberMaxValueValidator)} can only be used on number nodes and not on '{node.GetType().FullName}'."
			);
		}

		if (numberNode.Value == null)
		{
			// Do not validate empty, this is the task of the required validation.
			return;
		}

		var maxValue = await _maxValue.EvaluateOnAsync(numberNode);
		if (numberNode.Value > maxValue)
		{
			// Invalid.
			numberNode.SetValidationError(ErrorKey, _errorMessageTemplate.Format(numberNode.Label, maxValue));
		}
	}
}
