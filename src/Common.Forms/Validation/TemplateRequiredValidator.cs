namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Util;

/// <summary>
/// Can only be applied to <see cref="ITemplateNode">.<br/>
/// Requires the section to have an instance.
/// </summary>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name.</param>
public class TemplateRequiredValidator(string? errorMessageTemplate = null) : INodeValidator
{
	public const string ErrorKey = nameof(TemplateRequiredValidator);
	private readonly string _errorMessageTemplate = errorMessageTemplate ?? Resources.TheField_RequiresAnInput;

	/// <inheritdoc />
	public void Validate(IFormNode node)
	{
		if (node is not ITemplateNode templatedSection)
		{
			throw new InvalidOperationException(
				$"A {nameof(TemplateRequiredValidator)} can only be used on templated sections."
			);
		}

		if (templatedSection.Instance != null)
		{
			// Valid.
			return;
		}

		// Invalid.
		templatedSection.SetValidationError(ErrorKey, _errorMessageTemplate.Format(templatedSection.Name));
	}

	/// <inheritdoc />
	public Task ValidateAsync(IFormNode node)
	{
		Validate(node);
		return Task.CompletedTask;
	}
}
