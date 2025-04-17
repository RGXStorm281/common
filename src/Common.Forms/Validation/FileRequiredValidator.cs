namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Util;

/// <summary>
/// Can only be applied to <see cref="IFileNode">.<br/>
/// Requires the field to have a non <see langword="null"/> value.
/// </summary>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name.</param>
public class FileRequiredValidator(string? errorMessageTemplate = null) : INodeValidator
{
	public const string ErrorKey = nameof(FileRequiredValidator);
	private readonly string _errorMessageTemplate = errorMessageTemplate ?? Resources.TheField_RequiresAnInput;

	/// <inheritdoc />
	public void Validate(IFormNode node)
	{
		if (node is not IFileNode fileNode)
		{
			throw new InvalidOperationException($"A {nameof(FileRequiredValidator)} can only be used on file nodes.");
		}

		if (!fileNode.Value.FileName.IsNullOrWhiteSpace() && fileNode.Value.FileContents != null)
		{
			// Valid.
			return;
		}

		// Invalid.
		fileNode.SetValidationError(ErrorKey, _errorMessageTemplate.Format(fileNode.Name));
	}

	/// <inheritdoc />
	public Task ValidateAsync(IFormNode node)
	{
		Validate(node);
		return Task.CompletedTask;
	}
}
