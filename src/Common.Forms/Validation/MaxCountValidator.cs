namespace RobinEpple.Common.Forms.Validation;

using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;
using RobinEpple.Common.Util;

/// <summary>
/// Can only be applied to <see cref="ICollectionNode">.<br/>
/// Checks the collection for having a maximum of <paramref name="minCount"/> instances.
/// </summary>
/// <param name="maxCount">The expression determining the (inclusive) upper bound for the instance count.</param>
/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for field name and {1} for the maximum count.</param>
public partial class MaxCountValidator(IFormExpression<decimal?> maxCount, string? errorMessageTemplate = null)
	: INodeValidator
{
	public const string ErrorKey = nameof(MaxCountValidator);
	private readonly IFormExpression<decimal?> _maxCount = maxCount;
	private readonly string _errorMessageTemplate =
		errorMessageTemplate ?? Resources.TheField_AllowsAMaximumOf_Instances;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public void Validate(IFormNode node)
	{
		if (node is not ICollectionNode collectionNode)
		{
			throw new InvalidOperationException(
				$"A {nameof(MaxCountValidator)} can only be used on collection nodes and not on '{node.GetType().FullName}'."
			);
		}

		var maxCount = _maxCount.EvaluateOn(collectionNode);
		if (collectionNode.Instances.Count() > maxCount)
		{
			// Invalid.
			collectionNode.SetValidationError(ErrorKey, _errorMessageTemplate.Format(collectionNode.Label, maxCount));
		}
	}
}
