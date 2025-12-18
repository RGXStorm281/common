namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;
using RobinEpple.Common.Util;

/// <summary>
/// Can only be applied to <see cref="ITextNode">.<br/>
/// Only active on non-<see langword="null"/> values.<br/>
/// Checks the field value against defined minimum length.
/// </summary>
/// <param name="minLength">The expression defining the (inclusive) lower bound for the content length.</param>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name and {1} for the minimum value.</param>
public partial class MinLengthValidator(IFormExpression<decimal?> minLength, string? errorMessageTemplate = null)
	: INodeValidator
{
	public const string ErrorKey = nameof(MinLengthValidator);
	private readonly IFormExpression<decimal?> _minLength = minLength;
	private readonly string _errorMessageTemplate =
		errorMessageTemplate ?? Resources.TheField_RequiresAMinimumContentLengthOf_;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public void Validate(IFormNode node)
	{
		if (node is not ITextNode textNode)
		{
			throw new InvalidOperationException(
				$"A {nameof(MinLengthValidator)} can only be used on text nodes and not on '{node.GetType().FullName}'."
			);
		}

		if (textNode.Value == null)
		{
			// Do not validate empty, this is the task of the required validation.
			return;
		}

		var minLength = _minLength.EvaluateOn(textNode);
		if (textNode.Value.Length < minLength)
		{
			// Invalid.
			textNode.SetValidationError(ErrorKey, _errorMessageTemplate.Format(textNode.Label, minLength));
		}
	}
}
