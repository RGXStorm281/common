namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Util;

/// <summary>
/// Can be applied to any node.<br/>
/// Always evaluates the expression to decide whether the node is valid.
/// </summary>
/// <param name="checkInvalid">The expression defining when the node is invalid. The error message is appended when the expression returns <see langword="true"/>.</param>
/// <param name="errorMessageTemplate">The custom error message. May contain the placeholder {0} for the field name.</param>
public class ExpressionValidator(IFormExpression<bool> checkInvalid, string errorMessageTemplate) : INodeValidator
{
	public const string ErrorKey = nameof(ExpressionValidator);
	private readonly IFormExpression<bool> _checkInvalid = checkInvalid;

	private readonly string _errorMessageTemplate = errorMessageTemplate;

	/// <inheritdoc />
	public void Validate(IFormNode node)
	{
		// This does not require any specific node type or value state.
		// Every condition needs to be encoded in the condition.
		var invalid = _checkInvalid.EvaluateOn(node);
		if (invalid)
		{
			node.SetValidationError(ErrorKey, _errorMessageTemplate.Format(node.Label));
		}
	}

	/// <inheritdoc />
	public async Task ValidateAsync(IFormNode node)
	{
		// This does not require any specific node type or value state.
		// Every condition needs to be encoded in the condition.
		var invalid = await _checkInvalid.EvaluateOnAsync(node);
		if (invalid)
		{
			node.SetValidationError(ErrorKey, _errorMessageTemplate.Format(node.Label));
		}
	}
}
