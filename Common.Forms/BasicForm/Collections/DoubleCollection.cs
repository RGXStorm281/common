using RobinEpple.Common.Forms.BasicApi;
using RobinEpple.Common.Forms.BasicApi.Nodes;

namespace RobinEpple.Common.Forms.BasicForm.Collections;

/// <inheritdoc cref="CollectionBase{TNodeType,TItem}"/>
/// <summary>
/// The collection node for double values.
/// </summary>
internal class DoubleCollection(
	string id,
	IParentNode parent,
	ILabel? label,
	IExpression<bool>? visibilityCondition,
	IEnumerable<INodeValidator<DoubleCollection>> validators)
	: CollectionBase<DoubleCollection, double>(
		id,
		parent,
		label,
		visibilityCondition,
		validators);