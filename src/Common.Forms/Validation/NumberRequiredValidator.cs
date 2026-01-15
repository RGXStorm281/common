namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;
using RobinEpple.Common.Util;

/// <summary>
/// Can only be applied to <see cref="INumberNode"/>.<br/>
/// Requires the field to have a non <see langword="null"/> value.
/// </summary>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name.</param>
public partial class NumberRequiredValidator(string? errorMessageTemplate = null) : INodeValidator
{
	/// <summary>
	/// The key errors from this validator will be registered under.
	/// </summary>
	public const string ErrorKey = nameof(NumberRequiredValidator);
	private readonly string _errorMessageTemplate = errorMessageTemplate ?? Resources.TheField_RequiresAnInput;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public void Validate(IFormNode node)
	{
		if (node is not INumberNode numberNode)
		{
			throw new InvalidOperationException(
				$"A {nameof(NumberRequiredValidator)} can only be used on number nodes and not on '{node.GetType().FullName}'."
			);
		}

		if (numberNode.Value != null)
		{
			// Valid.
			return;
		}

		// Invalid.
		numberNode.SetValidationError(ErrorKey, _errorMessageTemplate.Format(numberNode.Label));
	}
}
