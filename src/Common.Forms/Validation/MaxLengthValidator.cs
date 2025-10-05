namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;
using RobinEpple.Common.Util;

/// <summary>
/// Can only be applied to <see cref="ITextNode">.<br/>
/// Only active on non-<see langword="null"/> values.<br/>
/// Checks the field value against defined maximum length.
/// </summary>
/// <param name="maxLength">The expression defining the (inclusive) lower bound for the content length.</param>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name and {1} for the minimum value.</param>
public partial class MaxLengthValidator(IFormExpression<decimal?> maxLength, string? errorMessageTemplate = null)
	: INodeValidator
{
	public const string ErrorKey = nameof(MaxLengthValidator);
	private readonly IFormExpression<decimal?> _maxLength = maxLength;
	private readonly string _errorMessageTemplate =
		errorMessageTemplate ?? Resources.TheField_AllowsAMaximumContentLengthOf_;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public void Validate(IFormNode node)
	{
		if (node is not ITextNode textNode)
		{
			throw new InvalidOperationException(
				$"A {nameof(MaxLengthValidator)} can only be used on text nodes and not on '{node.GetType().FullName}'."
			);
		}

		if (textNode.Value == null)
		{
			// Do not validate empty, this is the task of the required validation.
			return;
		}

		var maxLength = _maxLength.EvaluateOn(textNode);
		if (textNode.Value.Length > maxLength)
		{
			// Invalid.
			textNode.SetValidationError(ErrorKey, _errorMessageTemplate.Format(textNode.Label, maxLength));
		}
	}
}
