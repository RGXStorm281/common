namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;
using RobinEpple.Common.Util;

/// <summary>
/// Can only be applied to <see cref="IBooleanNode"/>.<br/>
/// Requires the field to have the value <see langword="true"/>.
/// </summary>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name.</param>
public partial class RequireTrueValidator(string? errorMessageTemplate = null) : INodeValidator
{
	/// <summary>
	/// The key errors from this validator will be registered under.
	/// </summary>
	public const string ErrorKey = nameof(RequireTrueValidator);
	private readonly string _errorMessageTemplate = errorMessageTemplate ?? Resources.TheField_RequiresTheValueTrue;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public void Validate(IFormNode node)
	{
		if (node is not IBooleanNode booleanNode)
		{
			throw new InvalidOperationException(
				$"A {nameof(RequireTrueValidator)} can only be used on boolean fields and not on '{node.GetType().FullName}'."
			);
		}

		if (booleanNode.Value == true)
		{
			// Valid.
			return;
		}

		// Invalid.
		booleanNode.SetValidationError(ErrorKey, _errorMessageTemplate.Format(booleanNode.Label));
	}
}
