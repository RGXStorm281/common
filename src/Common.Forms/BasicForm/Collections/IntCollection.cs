namespace RobinEpple.Common.Forms.BasicForm.Collections;

using RobinEpple.Common.Forms.BasicApi;
using RobinEpple.Common.Forms.BasicApi.Nodes;

/// <inheritdoc cref="CollectionBase{TNodeType,TItem}"/>
/// <summary>
/// The collection node for integer values.
/// </summary>
internal class IntCollection(
	string id,
	IParentNode parent,
	ILabel? label,
	IExpression<bool>? visibilityCondition,
	IEnumerable<INodeValidator<IntCollection>> validators
) : CollectionBase<IntCollection, int>(id, parent, label, visibilityCondition, validators);
