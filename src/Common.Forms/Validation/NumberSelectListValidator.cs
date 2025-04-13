namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.SelectLists;

/// <summary>
/// Can only be applied to <see cref="INumberNode">.<br/>
/// Only active on non-<see langword="null"/> values.<br/>
/// Checks the provided value against the available items in the select list.
/// </summary>
/// <param name="selectListSource">The source to load the select list from.</param>
/// <param name="dependencies">Optional list of dependencies on the form state, that are evaluated and passed to the source to adapt the values accordingly.</param>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the field name.</param>
public class NumberSelectListValidator(
	ISelectListSource<decimal> selectListSource,
	IDictionary<string, IFormExpression<object?>>? dependencies = null,
	string? errorMessageTemplate = null
) : INodeValidator
{
	public const string ErrorKey = nameof(FileRequiredValidator);
	private readonly ISelectListSource<decimal> _selectListSource = selectListSource;
	private readonly IDictionary<string, IFormExpression<object>>? _dependencies = dependencies;
	private readonly string _errorMessageTemplate =
		errorMessageTemplate ?? Resources.TheFollowingCharactersAreNotAllowedInAFileName_;

	/// <inheritdoc />
	public void Validate(IFormNode node)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public Task ValidateAsync(IFormNode node)
	{
		throw new NotImplementedException();
	}
}
