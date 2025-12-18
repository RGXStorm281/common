namespace RobinEpple.Common.Forms.Validation;

using IbanNet;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;
using RobinEpple.Common.Util;

/// <summary>
/// Can only be applied to <see cref="ITextNode">.<br/>
/// Only active on non-<see langword="null"/> values.<br/>
/// Requires the field value to be a valid IBAN.
/// </summary>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the invalid value.</param>
public partial class IbanValidator(string? errorMessageTemplate = null) : INodeValidator
{
	public const string ErrorKey = nameof(IbanValidator);
	private readonly string _errorMessageTemplate =
		errorMessageTemplate ?? Resources.TheValue_CouldNotBeRecognizedAsAValidIban;
	private readonly IbanParser _parser = new IbanParser(new IbanNet.IbanValidator());

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public void Validate(IFormNode node)
	{
		if (node is not ITextNode textNode)
		{
			throw new InvalidOperationException(
				$"A {nameof(IbanValidator)} can only be used on text nodes and not on '{node.GetType().FullName}'."
			);
		}

		if (textNode.Value == null)
		{
			// Do not validate empty, this is the task of the required validation.
			return;
		}

		if (!IsValidIban(textNode.Value))
		{
			// Invalid.
			textNode.SetValidationError(ErrorKey, _errorMessageTemplate.Format(textNode.Value));
		}
	}

	private bool IsValidIban(string iban)
	{
		// Use IbanParser instead of IbanValidator, because it can handle whitespace characters.
		return _parser.TryParse(iban, out var _);
	}
}
