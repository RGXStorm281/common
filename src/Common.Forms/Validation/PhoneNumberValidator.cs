namespace RobinEpple.Common.Forms.Validation;

using PhoneNumbers;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Util;

/// <summary>
/// Can only be applied to <see cref="ITextNode">.<br/>
/// Only active on non-<see langword="null"/> values.<br/>
/// Requires the field value to be a valid phone number format.
/// </summary>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the invalid value.</param>
public class PhoneNumberValidator(string? errorMessageTemplate = null, string defaultRegion = "DE") : INodeValidator
{
	public const string ErrorKey = nameof(PhoneNumberValidator);
	private readonly string _errorMessageTemplate =
		errorMessageTemplate ?? Resources.TheValue_CouldNotBeRecognizedAsAValidPhoneNumberFormat;
	private readonly string _defaultRegion = defaultRegion;

	private readonly PhoneNumberUtil _util = PhoneNumberUtil.GetInstance();

	/// <inheritdoc />
	public void Validate(IFormNode node)
	{
		if (node is not ITextNode textNode)
		{
			throw new InvalidOperationException(
				$"A {nameof(PhoneNumberValidator)} can only be used on text nodes and not on '{node.GetType().FullName}'."
			);
		}

		if (textNode.Value == null)
		{
			// Do not validate empty, this is the task of the required validation.
			return;
		}

		if (!IsValidPhoneNumber(textNode.Value))
		{
			// Invalid.
			textNode.SetValidationError(ErrorKey, _errorMessageTemplate.Format(textNode.Value));
		}
	}

	/// <inheritdoc />
	public Task ValidateAsync(IFormNode node)
	{
		Validate(node);
		return Task.CompletedTask;
	}

	private bool IsValidPhoneNumber(string number)
	{
		PhoneNumberMatch[] numbers = _util.FindNumbers(number, _defaultRegion).ToArray();
		return numbers.Length == 1;
	}
}
