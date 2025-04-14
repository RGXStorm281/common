namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// Can only be applied to <see cref="ICollectionNode">.<br/>
/// Checks the collection for having a maximum of <paramref name="minCount"/> instances.
/// </summary>
/// <param name="maxCount">The expression determining the (inclusive) upper bound for the instance count.</param>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for field name and {1} for the maximum count.</param>
public class MaxCountValidator(IFormExpression<decimal?> maxCount, string? errorMessageTemplate = null) : INodeValidator
{
	public const string ErrorKey = nameof(MaxCountValidator);
	private readonly IFormExpression<decimal?> _maxCount = maxCount;
	private readonly string _errorMessageTemplate =
		errorMessageTemplate ?? Resources.TheField_AllowsAMaximumOf_Instances;

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
