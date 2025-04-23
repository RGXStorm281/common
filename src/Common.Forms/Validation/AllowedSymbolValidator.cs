namespace RobinEpple.Common.Forms.Validation;

using System.Text.RegularExpressions;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Util;

/// <summary>
/// Can only be applied to <see cref="ITextNode">.<br/>
/// Only active on non-<see langword="null"/> values.<br/>
/// Checks the provided value against a whitelist of file name symbols.
/// </summary>
/// <param name="characterWhitelist">The whitelist of symbols that are allowed to occur in the value.</param>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name and {1} for the invalid characters.</param>
public class AllowedSymbolValidator(string characterWhitelist, string? errorMessageTemplate = null) : INodeValidator
{
	public const string ErrorKey = nameof(AllowedSymbolValidator);
	private readonly Regex _invalidCharacterRegex = new Regex($"[^{characterWhitelist}]", RegexOptions.Compiled);
	private readonly string _errorMessageTemplate =
		errorMessageTemplate ?? Resources.TheField_DoesNotAllowTheFollowingCharacters_;

	/// <inheritdoc />
	public void Validate(IFormNode node)
	{
		if (node is not ITextNode textNode)
		{
			throw new InvalidOperationException(
				$"A {nameof(AllowedSymbolValidator)} can only be used on text nodes and not on '{node.GetType().FullName}'."
			);
		}

		if (textNode.Value == null)
		{
			// Do not validate empty, this is the task of the required validation.
			return;
		}

		// Find invalid characters.
		var result = _invalidCharacterRegex.Matches(textNode.Value);
		var invalidCharacterList = result.Select(match => match.ToString()).Distinct().ToList();

		// If there are invalid characters, add the error.
		if (invalidCharacterList.Count > 0)
		{
			var invalidCharacters = string.Join(string.Empty, invalidCharacterList);
			textNode.SetValidationError(ErrorKey, _errorMessageTemplate.Format(textNode.Label, invalidCharacters));
		}
	}

	/// <inheritdoc />
	public Task ValidateAsync(IFormNode node)
	{
		Validate(node);
		return Task.CompletedTask;
	}
}
