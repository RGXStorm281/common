using RobinEpple.Common.Forms.BasicApi;
using RobinEpple.Common.Forms.BasicApi.Nodes;

namespace RobinEpple.Common.Forms.BasicForm.Collections;

/// <inheritdoc cref="CollectionBase{TNodeType,TItem}"/>
/// <summary>
/// The collection node for boolean values.
/// </summary>
internal class BoolCollection(
	string id,
	IParentNode parent,
	ILabel? label,
	IExpression<bool>? visibilityCondition,
	IEnumerable<INodeValidator<BoolCollection>> validators)
	: CollectionBase<BoolCollection, bool>(
		id,
		parent,
		label,
		visibilityCondition,
		validators);