using RobinEpple.Common.Forms.BasicApi;
using RobinEpple.Common.Forms.BasicApi.Nodes;

namespace RobinEpple.Common.Forms.BasicForm.Collections;

/// <inheritdoc cref="CollectionBase{TNodeType,TItem}"/>
/// <summary>
/// The collection node for byte values.
/// </summary>
internal class ByteCollection(
	string id,
	IParentNode parent,
	ILabel? label,
	IExpression<bool>? visibilityCondition,
	IEnumerable<INodeValidator<ByteCollection>> validators)
	: CollectionBase<ByteCollection, byte[]>(
		id,
		parent,
		label,
		visibilityCondition,
		validators);