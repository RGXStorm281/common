namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;
using RobinEpple.Common.Util;

/// <summary>
/// Can only be applied to <see cref="IValueNode{TValue}"/>.<br/>
/// Only active on non-<see langword="null"/> values.<br/>
/// Checks the field value against the available items in the select list.
/// </summary>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the invalid value and {1} for the field name.</param>
public partial class SelectListValidator<TValue>(string? errorMessageTemplate = null) : INodeValidator
{
	/// <summary>
	/// The key errors from this validator will be registered under.
	/// </summary>
	public const string ErrorKey = nameof(SelectListValidator<TValue>);
	private readonly string _errorMessageTemplate =
		errorMessageTemplate ?? Resources.TheValue_InTheField_IsNotAllowedPleaseSelectOneOfTheProvidedOptions;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public void Validate(IFormNode node)
	{
		if (node is not IValueNode<TValue> valueNode)
		{
			throw new InvalidOperationException(
				$"Node {node.GetId()}: A {ErrorKey} can only be used on nodes of type {nameof(IValueNode<TValue>)} and not on '{node.GetType().FullName}'."
			);
		}

		if (valueNode.Value == null)
		{
			// Do not validate empty, this is the task of the required validation.
			return;
		}

		// Load the select list with the current dependencies.
		if (valueNode.CurrentSelectListItems is not { } currentSelectOptions)
		{
			throw new InvalidOperationException(
				$"Node {node.GetId()}: A {ErrorKey} can only be used on nodes with a select list (= list to validate against)."
			);
		}

		// Validate that the current value is in the list.
		if (currentSelectOptions.Any(option => IsOptionMatch(valueNode.Value, option.Value)))
		{
			// Valid.
			return;
		}

		// Invalid.
		valueNode.SetValidationError(ErrorKey, _errorMessageTemplate.Format(valueNode.Value, valueNode.Label));
	}

	private bool IsOptionMatch(TValue nodeValue, TValue optionValue)
	{
		if (nodeValue == null)
		{
			return optionValue == null;
		}

		return nodeValue.Equals(optionValue);
	}
}
