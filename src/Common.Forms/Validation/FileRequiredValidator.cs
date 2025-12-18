namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;
using RobinEpple.Common.Util;

/// <summary>
/// Can only be applied to <see cref="IFileNode">.<br/>
/// Requires the field to have a non <see langword="null"/> value.
/// </summary>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name.</param>
public partial class FileRequiredValidator(string? errorMessageTemplate = null) : INodeValidator
{
	public const string ErrorKey = nameof(FileRequiredValidator);
	private readonly string _errorMessageTemplate = errorMessageTemplate ?? Resources.TheField_RequiresAnInput;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public void Validate(IFormNode node)
	{
		if (node is not IFileNode fileNode)
		{
			throw new InvalidOperationException(
				$"A {nameof(FileRequiredValidator)} can only be used on file nodes and not on '{node.GetType().FullName}'."
			);
		}

		if (!fileNode.Value.FileName.IsNullOrWhiteSpace() && fileNode.Value.FileContents != null)
		{
			// Valid.
			return;
		}

		// Invalid.
		fileNode.SetValidationError(ErrorKey, _errorMessageTemplate.Format(fileNode.Label));
	}
}
