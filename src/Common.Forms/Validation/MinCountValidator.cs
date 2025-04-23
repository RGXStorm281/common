namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Util;

/// <summary>
/// Can only be applied to <see cref="ICollectionNode">.<br/>
/// Checks the collection for having a minimum of <paramref name="minCount"/> instances.
/// </summary>
/// <param name="minCount">The expression determining the (inclusive) lower bound for the instance count.</param>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for field name and {1} for the minimum count.</param>
public class MinCountValidator(IFormExpression<decimal?> minCount, string? errorMessageTemplate = null) : INodeValidator
{
	public const string ErrorKey = nameof(MinCountValidator);
	private readonly IFormExpression<decimal?> _minCount = minCount;
	private readonly string _errorMessageTemplate =
		errorMessageTemplate ?? Resources.TheField_RequiresAtLeastTheFollowingNumberOfInstances_;

	/// <inheritdoc />
	public void Validate(IFormNode node)
	{
		if (node is not ICollectionNode collectionNode)
		{
			throw new InvalidOperationException(
				$"A {nameof(MinCountValidator)} can only be used on collection nodes and not on '{node.GetType().FullName}'."
			);
		}

		var minCount = _minCount.EvaluateOn(collectionNode);
		if (collectionNode.Instances.Count() < minCount)
		{
			// Invalid.
			collectionNode.SetValidationError(ErrorKey, _errorMessageTemplate.Format(collectionNode.Label, minCount));
		}
	}

	/// <inheritdoc />
	public async Task ValidateAsync(IFormNode node)
	{
		if (node is not ICollectionNode collectionNode)
		{
			throw new InvalidOperationException(
				$"A {nameof(MinCountValidator)} can only be used on collection nodes and not on '{node.GetType().FullName}'."
			);
		}

		var minCount = await _minCount.EvaluateOnAsync(collectionNode);
		if (collectionNode.Instances.Count() < minCount)
		{
			// Invalid.
			collectionNode.SetValidationError(ErrorKey, _errorMessageTemplate.Format(collectionNode.Label, minCount));
		}
	}
}
