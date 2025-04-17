namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Util;

/// <summary>
/// Can only be applied to <see cref="ITextNode">.<br/>
/// Requires the field to have a non <see langword="null"/> value.
/// </summary>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name.</param>
/// <param name="acceptWhitespace">Whether empty string or whitespace should be considered a valid value. Default is <see cref="false"/> .</param>
public class TextRequiredValidator(string? errorMessageTemplate = null, bool acceptWhitespace = false) : INodeValidator
{
	public const string ErrorKey = nameof(TextRequiredValidator);
	private readonly string _errorMessageTemplate = errorMessageTemplate ?? Resources.TheField_RequiresAnInput;
	private readonly bool _acceptWhitespace = acceptWhitespace;

	/// <inheritdoc />
	public void Validate(IFormNode node)
	{
		if (node is not ITextNode textNode)
		{
			throw new InvalidOperationException(
				$"A {nameof(TextRequiredValidator)} can only be used on text nodes and not on '{node.GetType().FullName}'."
			);
		}

		if (textNode.Value == null)
		{
			// Invalid.
			textNode.SetValidationError(ErrorKey, _errorMessageTemplate.Format(textNode.Name));
			return;
		}

		// There is at least some text content.

		if (_acceptWhitespace)
		{
			// Every content is valid.
			return;
		}

		// Whitespace is not accepted.

		if (textNode.Value.IsNullOrWhiteSpace())
		{
			// Invalid.
			textNode.SetValidationError(ErrorKey, _errorMessageTemplate.Format(textNode.Name));
		}

		// Some non-whitespace text => valid.
	}

	/// <inheritdoc />
	public Task ValidateAsync(IFormNode node)
	{
		Validate(node);
		return Task.CompletedTask;
	}
}
