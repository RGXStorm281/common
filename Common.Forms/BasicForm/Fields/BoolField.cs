using RobinEpple.Common.Forms.BasicApi;
using RobinEpple.Common.Forms.BasicApi.DataContainers;
using RobinEpple.Common.Forms.BasicApi.Nodes;
using RobinEpple.Common.Forms.BasicForm.DataContainers;

namespace RobinEpple.Common.Forms.BasicForm.Fields;

/// <summary>
/// Represents a field of type <see langword="bool" /> in the form.
/// </summary>
/// <inheritdoc cref="FormFieldBase{TNodeType, TValue}" />
internal class BoolField(
	string id,
	IParentNode parent,
	ILabel? label,
	IExpression<bool>? visibilityCondition,
	IEnumerable<INodeValidator<BoolField>> validators,
	bool? initialValue)
	: FormFieldBase<BoolField, bool?>(
		id,
		parent,
		label,
		visibilityCondition,
		validators,
		initialValue)
{
	public override void SetData(IFieldDataContainer<bool?> context)
	{
		Value = context.Value;
	}

	public override IFieldDataContainer<bool?> GetData()
		=> new BasicFieldDataContainer<bool?> { Value = Value };
}