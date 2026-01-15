namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;
using RobinEpple.Common.Util;

/// <summary>
/// Can only be applied to <see cref="ITemplateNode"/>.<br/>
/// Requires the section to have an instance.
/// </summary>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name.</param>
public partial class TemplateRequiredValidator(string? errorMessageTemplate = null) : INodeValidator
{
	/// <summary>
	/// The key errors from this validator will be registered under.
	/// </summary>
	public const string ErrorKey = nameof(TemplateRequiredValidator);
	private readonly string _errorMessageTemplate = errorMessageTemplate ?? Resources.TheField_RequiresAnInput;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public void Validate(IFormNode node)
	{
		if (node is not ITemplateNode templatedSection)
		{
			throw new InvalidOperationException(
				$"A {nameof(TemplateRequiredValidator)} can only be used on templated sections and not on '{node.GetType().FullName}'."
			);
		}

		if (templatedSection.Instance != null)
		{
			// Valid.
			return;
		}

		// Invalid.
		templatedSection.SetValidationError(ErrorKey, _errorMessageTemplate.Format(templatedSection.Label));
	}
}
