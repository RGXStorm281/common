namespace RobinEpple.Common.Forms.Validation;

using System;
using System.Globalization;
using System.Text.RegularExpressions;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;
using RobinEpple.Common.Util;

/// <summary>
/// Can only be applied to <see cref="ITextNode">.<br/>
/// Only active on non-<see langword="null"/> values.<br/>
/// Requires the field value to be a valid email format.
/// </summary>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the invalid value.</param>
public partial class EmailValidator(string? errorMessageTemplate = null) : INodeValidator
{
	public const string ErrorKey = nameof(EmailValidator);
	private readonly string _errorMessageTemplate =
		errorMessageTemplate ?? Resources.TheValue_CouldNotBeRecognizedAsAValidEmailFormat;
	private static Regex _emailFormatRegex = new Regex(
		@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
		RegexOptions.IgnoreCase | RegexOptions.Compiled,
		TimeSpan.FromMilliseconds(250)
	);
	private static Regex _domainRegex = new Regex(@"(@)(.+)$", RegexOptions.Compiled, TimeSpan.FromMilliseconds(200));

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public void Validate(IFormNode node)
	{
		if (node is not ITextNode textNode)
		{
			throw new InvalidOperationException(
				$"A {nameof(EmailValidator)} can only be used on text nodes and not on '{node.GetType().FullName}'."
			);
		}

		if (textNode.Value == null)
		{
			// Do not validate empty, this is the task of the required validation.
			return;
		}

		if (!IsValidEmail(textNode.Value))
		{
			// Invalid.
			textNode.SetValidationError(ErrorKey, _errorMessageTemplate.Format(textNode.Value));
		}
	}

	/// <summary>
	/// Validates Email via regex pattern and domain check as in Reference:<br/>
	/// https://learn.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format
	/// </summary>
	/// <param name="email">The string to check for a valid email address.</param>
	/// <returns><see langword="true"/> if the string is considered to be a valid email address.</returns>
	private static bool IsValidEmail(string email)
	{
		if (string.IsNullOrWhiteSpace(email))
		{
			return false;
		}

		try
		{
			// Normalize the domain
			email = _domainRegex.Replace(email, DomainMapper);
		}
		catch (RegexMatchTimeoutException)
		{
			return false;
		}
		catch (ArgumentException)
		{
			return false;
		}

		try
		{
			return _emailFormatRegex.IsMatch(email);
		}
		catch (RegexMatchTimeoutException)
		{
			return false;
		}
	}

	/// <summary>
	/// Examines the domain part of the email and normalizes it.
	/// </summary>
	/// <param name="match">The domain, extracted via a regex match.</param>
	/// <returns>A normalized domain name, resolved via system API IDN mapping.</returns>
	private static string DomainMapper(Match match)
	{
		// Use IdnMapping class to convert Unicode domain names.
		var idn = new IdnMapping();

		// Pull out and process domain name (throws ArgumentException on invalid)
		string domainName = idn.GetAscii(match.Groups[2].Value);

		return match.Groups[1].Value + domainName;
	}
}
