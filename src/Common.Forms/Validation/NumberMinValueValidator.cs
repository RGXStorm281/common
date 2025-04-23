namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Util;

/// <summary>
/// Can only be applied to <see cref="INumberNode">.<br/>
/// Only active on non-<see langword="null"/> values.<br/>
/// Checks the field value against defined minimum value.
/// </summary>
/// <param name="minValue">The expression defining the (inclusive) lower bound the field accepts.</param>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name and {1} for the minimum value.</param>
public class NumberMinValueValidator(IFormExpression<decimal?> minValue, string? errorMessageTemplate = null)
	: INodeValidator
{
	public const string ErrorKey = nameof(NumberMinValueValidator);
	private readonly IFormExpression<decimal?> _minValue = minValue;
	private readonly string _errorMessageTemplate = errorMessageTemplate ?? Resources.TheField_RequiresAMinimumValueOf_;

	/// <inheritdoc />
	public void Validate(IFormNode node)
	{
		if (node is not INumberNode numberNode)
		{
			throw new InvalidOperationException(
				$"A {nameof(NumberMinValueValidator)} can only be used on number nodes and not on '{node.GetType().FullName}'."
			);
		}

		if (numberNode.Value == null)
		{
			// Do not validate empty, this is the task of the required validation.
			return;
		}

		var minValue = _minValue.EvaluateOn(numberNode);
		if (numberNode.Value < minValue)
		{
			// Invalid.
			numberNode.SetValidationError(ErrorKey, _errorMessageTemplate.Format(numberNode.Label, minValue));
		}
	}

	/// <inheritdoc />
	public async Task ValidateAsync(IFormNode node)
	{
		if (node is not INumberNode numberNode)
		{
			throw new InvalidOperationException(
				$"A {nameof(NumberMinValueValidator)} can only be used on number nodes and not on '{node.GetType().FullName}'."
			);
		}

		if (numberNode.Value == null)
		{
			// Do not validate empty, this is the task of the required validation.
			return;
		}

		var minValue = await _minValue.EvaluateOnAsync(numberNode);
		if (numberNode.Value < minValue)
		{
			// Invalid.
			numberNode.SetValidationError(ErrorKey, _errorMessageTemplate.Format(numberNode.Label, minValue));
		}
	}
}
