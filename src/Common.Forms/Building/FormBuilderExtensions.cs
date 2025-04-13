namespace RobinEpple.Common.Forms.Building;

using RobinEpple.Common.Forms.Validation;

/// <summary>
/// This static class provides additional shortcut methods for common form building use cases.<br/>
/// These rely on the foundation in <see cref="FormBuilder"/> and are NOT unit-tested.
/// </summary>
public static class FormBuilderExtensions
{
	/// <summary>
	/// Requires the field to have the value <see langword="true"/>.
	/// </summary>
	/// <param name="builder">The node builder to append the validator to.</param>
	/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name.</param>
	public static IBooleanNodeBuilder UseRequireTrueValidator(
		this IBooleanNodeBuilder builder,
		string? errorMessageTemplate = null
	) => builder.UseValidator(new RequireTrueValidator(errorMessageTemplate));

	/// <summary>
	/// Requires the field to have a non <see langword="null"/> value.
	/// </summary>
	/// <param name="builder">The node builder to append the validator to.</param>
	/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name.</param>
	public static IFileNodeBuilder UseRequiredValidator(
		this IFileNodeBuilder builder,
		string? errorMessageTemplate = null
	) => builder.UseValidator(new FileRequiredValidator(errorMessageTemplate));

	/// <summary>
	/// Requires the field to have a non <see langword="null"/> value.
	/// </summary>
	/// <param name="builder">The node builder to append the validator to.</param>
	/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name.</param>
	public static INumberNodeBuilder UseRequiredValidator(
		this INumberNodeBuilder builder,
		string? errorMessageTemplate = null
	) => builder.UseValidator(new NumberRequiredValidator(errorMessageTemplate));

	/// <summary>
	/// Requires the field to have a non <see langword="null"/> value.
	/// </summary>
	/// <param name="builder">The node builder to append the validator to.</param>
	/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name.</param>
	/// <param name="acceptWhitespace">Whether empty string or whitespace should be considered a valid value. Default is <see cref="false"/> .</param>
	public static ITextNodeBuilder UseRequiredValidator(
		this ITextNodeBuilder builder,
		string? errorMessageTemplate = null,
		bool acceptWhitespace = false
	) => builder.UseValidator(new TextRequiredValidator(errorMessageTemplate, acceptWhitespace));

	/// <summary>
	/// Requires the field to have a non <see langword="null"/> value.
	/// </summary>
	/// <param name="builder">The node builder to append the validator to.</param>
	/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name.</param>
	public static ITimestampNodeBuilder UseRequiredValidator(
		this ITimestampNodeBuilder builder,
		string? errorMessageTemplate = null
	) => builder.UseValidator(new TimestampRequiredValidator(errorMessageTemplate));

	/// <summary>
	/// Requires the section to have an instance.
	/// </summary>
	/// <param name="builder">The node builder to append the validator to.</param>
	/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name.</param>
	public static ITemplateNodeBuilder UseRequiredValidator(
		this ITemplateNodeBuilder builder,
		string? errorMessageTemplate = null
	) => builder.UseValidator(new TemplateRequiredValidator(errorMessageTemplate));

	/// <summary>
	/// Only active on non-<see langword="null"/> file contents.<br/>
	/// Checks the size of the provided file against a maximum file size.
	/// </summary>
	/// <param name="builder">The node builder to append the validator to.</param>
	/// <param name="maxFileSize">The maximum size a file is allowed to be.</param>
	/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name and {1} for the file size.</param>
	public static IFileNodeBuilder UseMaxFileSizeValidator(
		this IFileNodeBuilder builder,
		long maxFileSize,
		string? errorMessageTemplate = null
	) => builder.UseValidator(new MaxFileSizeValidator(maxFileSize, errorMessageTemplate));

	/// <summary>
	/// Only active on non-<see langword="null"/> file names.<br/>
	/// Checks the provided file name against a whitelist of file name symbols.
	/// </summary>
	/// <param name="builder">The node builder to append the validator to.</param>
	/// <param name="characterWhitelist">The whitelist of symbols that are allowed to occur in a filename.</param>
	/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name.</param>
	public static IFileNodeBuilder UseAllowedFileNameSymbolValidator(
		this IFileNodeBuilder builder,
		string characterWhitelist,
		string? errorMessageTemplate = null
	) => builder.UseValidator(new AllowedFileNameSymbolValidator(characterWhitelist, errorMessageTemplate));

	/// <summary>
	/// Only active on non-<see langword="null"/> file contents.<br/>
	/// Estimates the mime type of a given byte string and checks it against a list of valid extensions.
	/// </summary>
	/// <param name="allowedExtensions">The list of allowed file extensions.</param>
	/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name and {1} for the list of allowed extensions.</param>
	public static IFileNodeBuilder UseFileExtensionValidator(
		this IFileNodeBuilder builder,
		string[] allowedExtensions,
		string? errorMessageTemplate = null
	) => builder.UseValidator(new FileExtensionValidator(allowedExtensions, errorMessageTemplate));
}
