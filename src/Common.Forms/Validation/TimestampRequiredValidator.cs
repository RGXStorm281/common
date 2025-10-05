namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;
using RobinEpple.Common.Util;

/// <summary>
/// Can only be applied to <see cref="ITextNode">.<br/>
/// Requires the field to have a non <see langword="null"/> value.
/// </summary>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name.</param>
public partial class TimestampRequiredValidator(string? errorMessageTemplate = null) : INodeValidator
{
	public const string ErrorKey = nameof(TimestampRequiredValidator);
	private readonly string _errorMessageTemplate = errorMessageTemplate ?? Resources.TheField_RequiresAnInput;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public void Validate(IFormNode node)
	{
		if (node is not ITimestampNode timestampNode)
		{
			throw new InvalidOperationException(
				$"A {nameof(TimestampRequiredValidator)} can only be used on timestamp nodes and not on '{node.GetType().FullName}'."
			);
		}

		if (timestampNode.Value != null)
		{
			// Valid.
			return;
		}

		// Invalid.
		timestampNode.SetValidationError(ErrorKey, _errorMessageTemplate.Format(timestampNode.Label));
	}
}
