namespace RobinEpple.Common.Forms.BasicForm.Collections;

using RobinEpple.Common.Forms.BasicApi;
using RobinEpple.Common.Forms.BasicApi.Nodes;

/// <inheritdoc cref="CollectionBase{TNodeType,TItem}"/>
/// <summary>
/// The collection node for string values.
/// </summary>
internal class StringCollection(
	string id,
	IParentNode parent,
	ILabel? label,
	IExpression<bool>? visibilityCondition,
	IEnumerable<INodeValidator<StringCollection>> validators
) : CollectionBase<StringCollection, string>(id, parent, label, visibilityCondition, validators);
