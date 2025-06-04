namespace RobinEpple.Common.Forms.Building;

using System.Linq.Expressions;
using RobinEpple.Common.Forms.Binding;
using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Extensions;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.SelectLists;
using RobinEpple.Common.Forms.Validation;
using static RobinEpple.Common.Forms.Expressions.FormExpression;

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
	/// <param name="builder">The node builder to append the validator to.</param>
	/// <param name="allowedExtensions">The list of allowed file extensions.</param>
	/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name, {1} for the invalid extension and {2} for the list of allowed extensions.</param>
	public static IFileNodeBuilder UseFileExtensionValidator(
		this IFileNodeBuilder builder,
		string[] allowedExtensions,
		string? errorMessageTemplate = null
	) => builder.UseValidator(new FileExtensionValidator(allowedExtensions, errorMessageTemplate));

	/// <summary>
	/// Only active on non-<see langword="null"/> values.<br/>
	/// Checks the field value against the available items in the select list.
	/// </summary>
	/// <param name="builder">The node builder to append the validator to.</param>
	/// <param name="selectListSource">The source to load the select list from.</param>
	/// <param name="dependencies">Optional list of dependencies on the form state, that are evaluated and passed to the source to adapt the values accordingly.</param>
	/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the invalid value and {1} for the field name.</param>
	public static INumberNodeBuilder UseSelectListValidator(
		this INumberNodeBuilder builder,
		ISelectListSource<decimal> selectListSource,
		IDictionary<string, IFormExpression<object?>>? dependencies = null,
		string? errorMessageTemplate = null
	) => builder.UseValidator(new NumberSelectListValidator(selectListSource, dependencies, errorMessageTemplate));

	/// <summary>
	/// Only active on non-<see langword="null"/> values.<br/>
	/// Checks the field value against the available items in the select list.
	/// </summary>
	/// <param name="builder">The node builder to append the validator to.</param>
	/// <param name="selectListSource">The source to load the select list from.</param>
	/// <param name="dependencies">Optional list of dependencies on the form state, that are evaluated and passed to the source to adapt the values accordingly.</param>
	/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the invalid value and {1} for the field name.</param>
	public static ITextNodeBuilder UseSelectListValidator(
		this ITextNodeBuilder builder,
		ISelectListSource<string> selectListSource,
		IDictionary<string, IFormExpression<object?>>? dependencies = null,
		string? errorMessageTemplate = null
	) => builder.UseValidator(new TextSelectListValidator(selectListSource, dependencies, errorMessageTemplate));

	/// <summary>
	/// Only active on non-<see langword="null"/> values.<br/>
	/// Checks the field value against the available items in the select list.
	/// </summary>
	/// <param name="builder">The node builder to append the validator to.</param>
	/// <param name="selectListSource">The source to load the select list from.</param>
	/// <param name="dependencies">Optional list of dependencies on the form state, that are evaluated and passed to the source to adapt the values accordingly.</param>
	/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the invalid value and {1} for the field name.</param>
	public static ITimestampNodeBuilder UseSelectListValidator(
		this ITimestampNodeBuilder builder,
		ISelectListSource<DateTime> selectListSource,
		IDictionary<string, IFormExpression<object?>>? dependencies = null,
		string? errorMessageTemplate = null
	) => builder.UseValidator(new TimestampSelectListValidator(selectListSource, dependencies, errorMessageTemplate));

	/// <summary>
	/// Only active on non-<see langword="null"/> values.<br/>
	/// Checks the field value against defined minimum value.
	/// </summary>
	/// <param name="builder">The node builder to append the validator to.</param>
	/// <param name="minValue">The expression defining the (inclusive) lower bound the field accepts.</param>
	/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the invalid value and {1} for the field name.</param>
	public static INumberNodeBuilder UseMinValueValidator(
		this INumberNodeBuilder builder,
		IFormExpression<decimal?> minValue,
		string? errorMessageTemplate = null
	) => builder.UseValidator(new NumberMinValueValidator(minValue, errorMessageTemplate));

	/// <inheritdoc cref="UseMinValueValidator(INumberNodeBuilder, IFormExpression{decimal?}, string?)"/>
	public static INumberNodeBuilder UseMinValueValidator(
		this INumberNodeBuilder builder,
		decimal minValue,
		string? errorMessageTemplate = null
	) => builder.UseMinValueValidator(StaticValue<decimal?>(minValue), errorMessageTemplate);

	/// <summary>
	/// Only active on non-<see langword="null"/> values.<br/>
	/// Checks the field value against defined maximum value.
	/// </summary>
	/// <param name="builder">The node builder to append the validator to.</param>
	/// <param name="minValue">The expression defining the (inclusive) upper bound the field accepts.</param>
	/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the invalid value and {1} for the field name.</param>
	public static INumberNodeBuilder UseMaxValueValidator(
		this INumberNodeBuilder builder,
		IFormExpression<decimal?> maxValue,
		string? errorMessageTemplate = null
	) => builder.UseValidator(new NumberMaxValueValidator(maxValue, errorMessageTemplate));

	/// <inheritdoc cref="UseMaxValueValidator(INumberNodeBuilder, IFormExpression{decimal?}, string?)"/>
	public static INumberNodeBuilder UseMaxValueValidator(
		this INumberNodeBuilder builder,
		decimal maxValue,
		string? errorMessageTemplate = null
	) => builder.UseMaxValueValidator(StaticValue<decimal?>(maxValue), errorMessageTemplate);

	/// <summary>
	/// Only active on non-<see langword="null"/> values.<br/>
	/// Checks the field value against defined minimum value.
	/// </summary>
	/// <param name="builder">The node builder to append the validator to.</param>
	/// <param name="minValue">The expression defining the (inclusive) lower bound the field accepts.</param>
	/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the invalid value and {1} for the field name.</param>
	public static ITimestampNodeBuilder UseMinValueValidator(
		this ITimestampNodeBuilder builder,
		IFormExpression<DateTime?> minValue,
		string? errorMessageTemplate = null
	) => builder.UseValidator(new TimestampMinValueValidator(minValue, errorMessageTemplate));

	/// <inheritdoc cref="UseMinValueValidator(ITimestampNodeBuilder, IFormExpression{DateTime?}, string?)"/>
	public static ITimestampNodeBuilder UseMinValueValidator(
		this ITimestampNodeBuilder builder,
		DateTime minValue,
		string? errorMessageTemplate = null
	) => builder.UseMinValueValidator(StaticValue<DateTime?>(minValue), errorMessageTemplate);

	/// <summary>
	/// Only active on non-<see langword="null"/> values.<br/>
	/// Checks the field value against defined maximum value.
	/// </summary>
	/// <param name="builder">The node builder to append the validator to.</param>
	/// <param name="minValue">The expression defining the (inclusive) upper bound the field accepts.</param>
	/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the invalid value and {1} for the field name.</param>
	public static ITimestampNodeBuilder UseMaxValueValidator(
		this ITimestampNodeBuilder builder,
		IFormExpression<DateTime?> maxValue,
		string? errorMessageTemplate = null
	) => builder.UseValidator(new TimestampMaxValueValidator(maxValue, errorMessageTemplate));

	/// <inheritdoc cref="UseMaxValueValidator(ITimestampNodeBuilder, IFormExpression{DateTime?}, string?)"/>
	public static ITimestampNodeBuilder UseMaxValueValidator(
		this ITimestampNodeBuilder builder,
		DateTime maxValue,
		string? errorMessageTemplate = null
	) => builder.UseMaxValueValidator(StaticValue<DateTime?>(maxValue), errorMessageTemplate);

	/// <summary>
	/// Only active on non-<see langword="null"/> values.<br/>
	/// Checks the field value against defined minimum length.
	/// </summary>
	/// <param name="builder">The node builder to append the validator to.</param>
	/// <param name="minLength">The expression defining the (inclusive) lower bound for the content length.</param>
	/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name and {1} for the minimum value.</param>
	public static ITextNodeBuilder UseMinLengthValidator(
		this ITextNodeBuilder builder,
		IFormExpression<decimal?> minLength,
		string? errorMessageTemplate = null
	) => builder.UseValidator(new MinLengthValidator(minLength, errorMessageTemplate));

	/// <inheritdoc cref="UseMinLengthValidator(ITextNodeBuilder, IFormExpression{decimal?}, string?)"/>
	public static ITextNodeBuilder UseMinLengthValidator(
		this ITextNodeBuilder builder,
		int minLength,
		string? errorMessageTemplate = null
	) => builder.UseMinLengthValidator(StaticValue<decimal?>(minLength), errorMessageTemplate);

	/// <summary>
	/// Only active on non-<see langword="null"/> values.<br/>
	/// Checks the field value against defined minimum length.
	/// </summary>
	/// <param name="builder">The node builder to append the validator to.</param>
	/// <param name="maxLength">The expression defining the (inclusive) lower bound for the content length.</param>
	/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name and {1} for the minimum value.</param>
	public static ITextNodeBuilder UseMaxLengthValidator(
		this ITextNodeBuilder builder,
		IFormExpression<decimal?> maxLength,
		string? errorMessageTemplate = null
	) => builder.UseValidator(new MaxLengthValidator(maxLength, errorMessageTemplate));

	/// <inheritdoc cref="UseMaxLengthValidator(ITextNodeBuilder, IFormExpression{decimal?}, string?)"/>
	public static ITextNodeBuilder UseMaxLengthValidator(
		this ITextNodeBuilder builder,
		int maxLength,
		string? errorMessageTemplate = null
	) => builder.UseMaxLengthValidator(StaticValue<decimal?>(maxLength), errorMessageTemplate);

	/// <summary>
	/// Only active on non-<see langword="null"/> values.<br/>
	/// Checks the provided value against a whitelist of file name symbols.
	/// </summary>
	/// <param name="builder">The node builder to append the validator to.</param>
	/// <param name="characterWhitelist">The whitelist of symbols that are allowed to occur in the value.</param>
	/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name and {1} for the invalid characters.</param>
	public static ITextNodeBuilder UseAllowedSymbolValidator(
		this ITextNodeBuilder builder,
		string characterWhitelist,
		string? errorMessageTemplate = null
	) => builder.UseValidator(new AllowedSymbolValidator(characterWhitelist, errorMessageTemplate));

	/// <summary>
	/// Only active on non-<see langword="null"/> values.<br/>
	/// Requires the field value to be a valid email format.
	/// </summary>
	/// <param name="builder">The node builder to append the validator to.</param>
	/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the invalid value.</param>
	public static ITextNodeBuilder UseEmailValidator(
		this ITextNodeBuilder builder,
		string? errorMessageTemplate = null
	) => builder.UseValidator(new EmailValidator(errorMessageTemplate));

	/// <summary>
	/// Only active on non-<see langword="null"/> values.<br/>
	/// Requires the field value to be a valid phone number format.
	/// </summary>
	/// <param name="builder">The node builder to append the validator to.</param>
	/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the invalid value.</param>
	public static ITextNodeBuilder UsePhoneNumberValidator(
		this ITextNodeBuilder builder,
		string? errorMessageTemplate = null
	) => builder.UseValidator(new PhoneNumberValidator(errorMessageTemplate));

	/// <summary>
	/// Only active on non-<see langword="null"/> values.<br/>
	/// Requires the field value to be a valid IBAN.
	/// </summary>
	/// <param name="builder">The node builder to append the validator to.</param>
	/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the invalid value.</param>
	public static ITextNodeBuilder UseIbanValidator(
		this ITextNodeBuilder builder,
		string? errorMessageTemplate = null
	) => builder.UseValidator(new IbanValidator(errorMessageTemplate));

	/// <summary>
	/// Checks the collection for having a minimum of <paramref name="minCount"/> instances.
	/// </summary>
	/// <param name="builder">The node builder to append the validator to.</param>
	/// <param name="minCount">The expression determining the (inclusive) lower bound for the instance count.</param>
	/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for field name and {1} for the minimum count.</param>
	public static ICollectionNodeBuilder UseMinCountValidator(
		this ICollectionNodeBuilder builder,
		IFormExpression<decimal?> minCount,
		string? errorMessageTemplate = null
	) => builder.UseValidator(new MinCountValidator(minCount, errorMessageTemplate));

	/// <inheritdoc cref="UseMinCountValidator(ICollectionNodeBuilder, IFormExpression{decimal?}, string?)"/>
	public static ICollectionNodeBuilder UseMinCountValidator(
		this ICollectionNodeBuilder builder,
		int minCount,
		string? errorMessageTemplate = null
	) => builder.UseMinCountValidator(StaticValue<decimal?>(minCount), errorMessageTemplate);

	/// <summary>
	/// Checks the collection for having a maximum of <paramref name="maxCount"/> instances.
	/// </summary>
	/// <param name="builder">The node builder to append the validator to.</param>
	/// <param name="maxCount">The expression determining the (inclusive) upper bound for the instance count.</param>
	/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for field name and {1} for the maximum count.</param>
	public static ICollectionNodeBuilder UseMaxCountValidator(
		this ICollectionNodeBuilder builder,
		IFormExpression<decimal?> maxCount,
		string? errorMessageTemplate = null
	) => builder.UseValidator(new MaxCountValidator(maxCount, errorMessageTemplate));

	/// <inheritdoc cref="UseMaxCountValidator(ICollectionNodeBuilder, IFormExpression{decimal?}, string?)"/>
	public static ICollectionNodeBuilder UseMaxCountValidator(
		this ICollectionNodeBuilder builder,
		int maxCount,
		string? errorMessageTemplate = null
	) => builder.UseMaxCountValidator(StaticValue<decimal?>(maxCount), errorMessageTemplate);

	/// <summary>
	/// Only active on non-<see langword="null"/> file names.<br/>
	/// Checks the file name against the defined maximum length.
	/// </summary>
	/// <param name="builder">The node builder to append the validator to.</param>
	/// <param name="maxLength">The expression defining the (inclusive) upper bound for the file name length.</param>
	/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name and {1} for the maximum length.</param>
	public static IFileNodeBuilder UseFileNameMaxLengthValidator(
		this IFileNodeBuilder builder,
		IFormExpression<decimal?> maxLength,
		string? errorMessageTemplate = null
	) => builder.UseValidator(new FileNameMaxLengthValidator(maxLength, errorMessageTemplate));

	/// <inheritdoc cref="UseFileNameMaxLengthValidator(IFileNodeBuilder, IFormExpression{decimal?}, string?)"/>
	public static IFileNodeBuilder UseFileNameMaxLengthValidator(
		this IFileNodeBuilder builder,
		int maxLength,
		string? errorMessageTemplate = null
	) => builder.UseFileNameMaxLengthValidator(StaticValue<decimal?>(maxLength), errorMessageTemplate);

	/// <summary>
	/// Always evaluates the expression to decide whether the node is valid.
	/// </summary>
	/// <param name="builder">The node builder to append the validator to.</param>
	/// <param name="checkInvalid">The expression defining when the node is invalid. The error message is appended when the expression returns <see langword="true"/>.</param>
	/// <param name="errorMessageTemplate">The custom error message. May contain the placeholder {0} for the field name.</param>
	public static TNodeBuilder UseExpressionValidator<TNodeBuilder>(
		this TNodeBuilder builder,
		IFormExpression<bool> checkInvalid,
		string errorMessageTemplate
	)
		where TNodeBuilder : INodeBuilder<TNodeBuilder> =>
		builder.UseValidator(new ExpressionValidator(checkInvalid, errorMessageTemplate));

	/// <summary>
	/// Defines, that the model for this form is represented in a single boolean field.
	/// </summary>
	/// <param name="fieldName">The name of the inner field that holds the value.</param>
	/// <returns>The form builder for adding more elements or concluding the build process.</returns>
	public static IFormBuilder UseBooleanSingleFieldModel(this IFormBuilder builder, string fieldName) =>
		builder.UseSingleFieldModel<bool?>(fieldName, null);

	/// <summary>
	/// Defines, that the model for this form is represented in a single file field.
	/// </summary>
	/// <param name="fieldName">The name of the inner field that holds the value.</param>
	/// <returns>The form builder for adding more elements or concluding the build process.</returns>
	public static IFormBuilder UseFileSingleFieldModel(this IFormBuilder builder, string fieldName) =>
		builder.UseSingleFieldModel<FileValue?>(fieldName, null);

	/// <summary>
	/// Defines, that the model for this form is represented in a single number field.
	/// </summary>
	/// <param name="fieldName">The name of the inner field that holds the value.</param>
	/// <returns>The form builder for adding more elements or concluding the build process.</returns>
	public static IFormBuilder UseNumberSingleFieldModel(this IFormBuilder builder, string fieldName) =>
		builder.UseSingleFieldModel<decimal?>(fieldName, null);

	/// <summary>
	/// Defines, that the model for this form is represented in a single text field.
	/// </summary>
	/// <param name="fieldName">The name of the inner field that holds the value.</param>
	/// <returns>The form builder for adding more elements or concluding the build process.</returns>
	public static IFormBuilder UseTextSingleFieldModel(this IFormBuilder builder, string fieldName) =>
		builder.UseSingleFieldModel<string?>(fieldName, null);

	/// <summary>
	/// Defines, that the model for this form is represented in a single timestamp field.
	/// </summary>
	/// <param name="fieldName">The name of the inner field that holds the value.</param>
	/// <returns>The form builder for adding more elements or concluding the build process.</returns>
	public static IFormBuilder UseTimestampSingleFieldModel(this IFormBuilder builder, string fieldName) =>
		builder.UseSingleFieldModel<DateTime?>(fieldName, null);
}
