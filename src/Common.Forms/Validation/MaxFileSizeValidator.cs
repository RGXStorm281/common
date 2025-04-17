namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Util;

/// <summary>
/// Can only be applied to <see cref="IFileNode">.<br/>
/// Only active on non-<see langword="null"/> file contents.<br/>
/// Checks the size of the provided file against a maximum file size.
/// </summary>
/// <param name="maxFileSizeInByte">The maximum size a file is allowed to be.</param>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name and {1} for the file size.</param>
public class MaxFileSizeValidator(long maxFileSizeInByte, string? errorMessageTemplate = null) : INodeValidator
{
	public const string ErrorKey = nameof(TemplateRequiredValidator);
	private readonly long _maxFileSize = maxFileSizeInByte;
	private readonly string _errorMessageTemplate =
		errorMessageTemplate ?? Resources.TheFileInput_HasAMaximumFileSizeOf_;

	/// <inheritdoc />
	public void Validate(IFormNode node)
	{
		if (node is not IFileNode fileNode)
		{
			throw new InvalidOperationException(
				$"A {nameof(MaxFileSizeValidator)} can only be used on file nodes and not on '{node.GetType().FullName}'."
			);
		}

		if (fileNode.Value.FileContents == null)
		{
			// Do not validate empty, this is the task of the required validation.
			return;
		}

		if (fileNode.Value.FileContents.Length <= _maxFileSize)
		{
			// Valid.
			return;
		}

		// Too big.
		fileNode.SetValidationError(ErrorKey, _errorMessageTemplate.Format(fileNode.Name, GetMaxSizeText()));
	}

	/// <inheritdoc />
	public Task ValidateAsync(IFormNode node)
	{
		Validate(node);
		return Task.CompletedTask;
	}

	private string GetMaxSizeText()
	{
		switch (_maxFileSize)
		{
			case < 1024:
			{
				return $"{_maxFileSize} B";
			}
			case < 1024 * 1024:
			{
				return $"{Math.Round(_maxFileSize / 1024m, 2)} KB";
			}
			case < 1024 * 1024 * 1024:
			{
				return $"{Math.Round(_maxFileSize / (1024 * 1024m), 2)} MB";
			}
			default:
			{
				return $"{Math.Round(_maxFileSize / (1024 * 1024 * 1024m), 2)} GB";
			}
		}
	}
}
