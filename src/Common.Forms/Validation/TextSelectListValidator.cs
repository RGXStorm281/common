namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.SelectLists;
using RobinEpple.Common.SourceGenerators.Abstractions;
using RobinEpple.Common.Util;

/// <summary>
/// Can only be applied to <see cref="ITextNode">.<br/>
/// Only active on non-<see langword="null"/> values.<br/>
/// Checks the field value against the available items in the select list.
/// </summary>
/// <param name="selectListSource">The source to load the select list from.</param>
/// <param name="dependencies">Optional list of dependencies on the form state, that are evaluated and passed to the source to adapt the values accordingly.</param>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the invalid value and {1} for the field name.</param>
public partial class TextSelectListValidator(
	ISelectListSource<string> selectListSource,
	IDictionary<string, IFormExpression<object?>>? dependencies = null,
	string? errorMessageTemplate = null
) : INodeValidator
{
	public const string ErrorKey = nameof(TextSelectListValidator);
	private readonly ISelectListSource<string> _selectListSource = selectListSource;
	private readonly IDictionary<string, IFormExpression<object?>> _dependencies =
		dependencies ?? new Dictionary<string, IFormExpression<object?>>();
	private readonly string _errorMessageTemplate =
		errorMessageTemplate ?? Resources.TheValue_InTheField_IsNotAllowedPleaseSelectOneOfTheProvidedOptions;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public void Validate(IFormNode node)
	{
		if (node is not ITextNode textNode)
		{
			throw new InvalidOperationException(
				$"A {nameof(TextSelectListValidator)} can only be used on text nodes and not on '{node.GetType().FullName}'."
			);
		}

		if (textNode.Value == null)
		{
			// Do not validate empty, this is the task of the required validation.
			return;
		}

		// Evaluate dependencies on the current state of the form.
		var currentDependencyValues = _dependencies.Select(dependency =>
			(Key: dependency.Key, DependencyValue: dependency.Value.EvaluateOn(textNode))
		);

		// Load the select list with the current dependencies.
		var currentSelectOptions = _selectListSource.LoadItems(
			currentDependencyValues.ToDictionary(tuple => tuple.Key, tuple => tuple.DependencyValue)
		);

		// Validate that the current value is in the list.
		if (currentSelectOptions.Any(option => option.Value == textNode.Value))
		{
			// Valid.
			return;
		}

		// Invalid.
		textNode.SetValidationError(ErrorKey, _errorMessageTemplate.Format(textNode.Value, textNode.Label));
	}
}
