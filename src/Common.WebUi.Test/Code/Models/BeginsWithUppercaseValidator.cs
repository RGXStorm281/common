namespace RobinEpple.Common.WebUi.Test.Code.Models;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.Validation;
using RobinEpple.Common.SourceGenerators.Abstractions;

public partial class BeginsWithUppercaseValidator() : INodeValidator
{
	public const string ErrorKey = nameof(BeginsWithUppercaseValidator);

	[GenerateAsyncOverload]
	public void Validate(IFormNode node)
	{
		if (node is not ITextNode textNode)
		{
			throw new InvalidOperationException(
				$"The {nameof(BeginsWithUppercaseValidator)} can only be used on text nodes"
			);
		}

		if (string.IsNullOrWhiteSpace(textNode.Value))
		{
			// Do not validate empty, this is the job of the required validator.
			return;
		}

		var firstLetter = textNode.Value.Trim().First();
		if (!char.IsUpper(firstLetter))
		{
			textNode.SetValidationError(
				ErrorKey,
				"The value entered in this field has to start with an uppercase letter"
			);
		}
		else
		{
			textNode.RemoveValidationError(ErrorKey);
		}
	}
}
