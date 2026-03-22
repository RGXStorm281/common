namespace RobinEpple.Common.WebUi.Test.Code.Models;

using System.Text.RegularExpressions;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.Validation;
using RobinEpple.Common.SourceGenerators.Abstractions;

public partial class ZipCodeValidator : INodeValidator
{
	public const string ErrorKey = nameof(ZipCodeValidator);

	[GenerateAsyncOverload]
	public void Validate(IFormNode node)
	{
		if (node is not ITextNode textNode)
		{
			throw new InvalidOperationException(
				$"The validator {nameof(ZipCodeValidator)} can only be used on text nodes."
			);
		}

		if (string.IsNullOrWhiteSpace(textNode.Value))
		{
			// Do not validate empty, this is the job of the required validator.
			return;
		}

		const string regex = "[0-9]{5}";
		if (!Regex.IsMatch(textNode.Value.Trim(), regex, RegexOptions.Compiled))
		{
			textNode.SetValidationError(ErrorKey, "This field expects a 5 digit ZIP code.");
		}
		else
		{
			textNode.RemoveValidationError(ErrorKey);
		}
	}
}
