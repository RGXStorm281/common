namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;
using RobinEpple.Common.Util;

/// <summary>
/// Can be applied to any node.<br/>
/// Always evaluates the expression to decide whether the node is valid.
/// </summary>
/// <param name="validCondition">The expression defining when the node is valid. The error message is appended when the expression returns <see langword="false"/>.</param>
/// <param name="errorMessageTemplate">The custom error message. May contain the placeholder {0} for the field name.</param>
public partial class ExpressionValidator(IFormExpression<bool> validCondition, string errorMessageTemplate)
	: INodeValidator
{
	/// <summary>
	/// The key errors from this validator will be registered under.
	/// </summary>
	public const string ErrorKey = nameof(ExpressionValidator);
	private readonly IFormExpression<bool> _validCondition = validCondition;

	private readonly string _errorMessageTemplate = errorMessageTemplate;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public void Validate(IFormNode node)
	{
		// This does not require any specific node type or value state.
		// Every condition needs to be encoded in the condition.
		var valid = _validCondition.EvaluateOn(node);
		if (!valid)
		{
			node.SetValidationError(ErrorKey, _errorMessageTemplate.Format(node.Label));
		}
	}
}
