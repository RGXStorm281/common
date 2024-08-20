using RobinEpple.Common.Forms.BasicApi;
using RobinEpple.Common.Forms.BasicApi.DataContainers;
using RobinEpple.Common.Forms.BasicApi.Nodes;
using RobinEpple.Common.Forms.BasicForm.DataContainers;

namespace RobinEpple.Common.Forms.BasicForm.Fields;

/// <summary>
/// Represents a field of type <see langword="byte" />[] in the form.
/// </summary>
/// <inheritdoc cref="FormFieldBase{TNodeType,TValue}" />
internal class ByteField(
	string id,
	IParentNode parent,
	ILabel? label,
	IExpression<bool>? visibilityCondition,
	IEnumerable<INodeValidator<ByteField>> validators,
	byte[]? initialValue)
	: FormFieldBase<ByteField, byte[]?>(
		id,
		parent,
		label,
		visibilityCondition,
		validators,
		initialValue)
{
	public override void SetData(IFieldDataContainer<byte[]?> context)
	{
		Value = context.Value;
	}

	public override IFieldDataContainer<byte[]?> GetData()
		=> new BasicFieldDataContainer<byte[]?> { Value = Value };
}