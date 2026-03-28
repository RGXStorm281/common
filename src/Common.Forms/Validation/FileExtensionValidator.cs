namespace RobinEpple.Common.Forms.Validation;

using HeyRed.Mime;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;
using RobinEpple.Common.Util;

/// <summary>
/// Can only be applied to <see cref="IFileNode"/>.<br/>
/// Only active on non-<see langword="null"/> file contents.<br/>
/// Estimates the mime type of a given byte string and checks it against a list of valid extensions.
/// </summary>
/// <param name="allowedExtensions">The list of allowed file extensions (e.g. "jpg", without ".").</param>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name, {1} for the invalid extension and {2} for the list of allowed extensions.</param>
public partial class FileExtensionValidator(string[] allowedExtensions, string? errorMessageTemplate = null)
	: INodeValidator
{
	/// <summary>
	/// The key errors from this validator will be registered under.
	/// </summary>
	public const string ErrorKey = nameof(TemplateRequiredValidator);

	/// <summary>
	/// The list of allowed file extensions (e.g. "jpg", without ".").
	/// </summary>
	public IReadOnlyList<string> AllowedExtensions { get; } = allowedExtensions;

	private readonly string _errorMessageTemplate =
		errorMessageTemplate ?? Resources.TheFileInput_DoesNotAllowFilesOfType_OnlyAllowsFilesOfTheFollowingTypes_;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public void Validate(IFormNode node)
	{
		if (node is not IFileNode fileNode)
		{
			throw new InvalidOperationException(
				$"A {nameof(FileExtensionValidator)} can only be used on file nodes and not on '{node.GetType().FullName}'."
			);
		}

		if (fileNode.Value.FileContents == null)
		{
			// Do not validate empty, this is the task of the required validation.
			return;
		}

		var guessedExtension = MimeGuesser.GuessExtension(fileNode.Value.FileContents);
		if (!AllowedExtensions.Contains(guessedExtension))
		{
			var allowedExtensionString = string.Join(", ", AllowedExtensions.Select(extension => $".{extension}"));
			fileNode.SetValidationError(
				ErrorKey,
				_errorMessageTemplate.Format(fileNode.Label, $".{guessedExtension}", allowedExtensionString)
			);
		}
	}
}
