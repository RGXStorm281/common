namespace RobinEpple.Common.Forms.Validation;

using System.Text.RegularExpressions;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;
using RobinEpple.Common.Util;

/// <summary>
/// Can only be applied to <see cref="IFileNode">.<br/>
/// Only active on non-<see langword="null"/> file names.<br/>
/// Checks the provided file name against a whitelist of file name symbols.
/// </summary>
/// <param name="characterWhitelist">The whitelist of symbols that are allowed to occur in a filename.</param>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the invalid characters.</param>
public partial class AllowedFileNameSymbolValidator(string characterWhitelist, string? errorMessageTemplate = null)
	: INodeValidator
{
	public const string ErrorKey = nameof(AllowedFileNameSymbolValidator);
	private readonly Regex _invalidCharacterRegex = new Regex(
		$"[^{Regex.Escape(characterWhitelist)}]",
		RegexOptions.Compiled
	);
	private readonly string _errorMessageTemplate =
		errorMessageTemplate ?? Resources.TheFollowingCharactersAreNotAllowedInAFileName_;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public void Validate(IFormNode node)
	{
		if (node is not IFileNode fileNode)
		{
			throw new InvalidOperationException(
				$"A {nameof(AllowedFileNameSymbolValidator)} can only be used on file nodes and not on '{node.GetType().FullName}'."
			);
		}

		if (fileNode.Value.FileName == null)
		{
			// Do not validate empty, this is the task of the required validation.
			return;
		}

		var invalidCharacterMatches = _invalidCharacterRegex.Matches(fileNode.Value.FileName).ToList();
		if (invalidCharacterMatches.Count == 0)
		{
			// Valid.
			return;
		}

		// Invalid => Collect invalid characters and append error message.
		var invalidCharacters = string.Join(
			string.Empty,
			invalidCharacterMatches.Select(match => match.ToString()).Distinct()
		);
		fileNode.SetValidationError(ErrorKey, _errorMessageTemplate.Format(invalidCharacters));
	}
}
