namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;
using RobinEpple.Common.Util;

/// <summary>
/// Can only be applied to <see cref="IFileNode"/>.<br/>
/// Only active on non-<see langword="null"/> file names.<br/>
/// Checks the file name against the defined maximum length.
/// </summary>
/// <param name="maxLength">The expression defining the (inclusive) upper bound for the file name length.</param>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name and {1} for the maximum length.</param>
public partial class FileNameMaxLengthValidator(
	IFormExpression<decimal?> maxLength,
	string? errorMessageTemplate = null
) : INodeValidator
{
	/// <summary>
	/// The key errors from this validator will be registered under.
	/// </summary>
	public const string ErrorKey = nameof(FileNameMaxLengthValidator);
	private readonly IFormExpression<decimal?> _maxLength = maxLength;
	private readonly string _errorMessageTemplate =
		errorMessageTemplate ?? Resources.TheField_AllowsAMaximumFileNameLengthOf_;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public void Validate(IFormNode node)
	{
		if (node is not IFileNode fileNode)
		{
			throw new InvalidOperationException(
				$"A {nameof(FileNameMaxLengthValidator)} can only be used on file nodes and not on '{node.GetType().FullName}'."
			);
		}

		if (fileNode.Value.FileName == null)
		{
			// Do not validate empty, this is the task of the required validation.
			return;
		}

		var maxLength = _maxLength.EvaluateOn(fileNode);
		if (fileNode.Value.FileName.Length > maxLength)
		{
			// Invalid.
			fileNode.SetValidationError(ErrorKey, _errorMessageTemplate.Format(fileNode.Label, maxLength));
		}
	}
}
