using RobinEpple.Common.Forms.BasicApi;
using RobinEpple.Common.Forms.BasicApi.DataContainers;
using RobinEpple.Common.Forms.BasicApi.Nodes;
using RobinEpple.Common.Forms.BasicForm.DataContainers;

namespace RobinEpple.Common.Forms.BasicForm.Fields;

/// <summary>
/// Represents a field of type <see langword="int" /> in the form.
/// </summary>
/// <inheritdoc cref="FormFieldBase{TNodeType,TProperty}" />
internal class IntField(
	string id,
	IParentNode parent,
	ILabel? label,
	IExpression<bool>? visibilityCondition,
	IEnumerable<INodeValidator<IntField>> validators,
	int? initialValue)
	: FormFieldBase<IntField, int?>(
		id,
		parent,
		label,
		visibilityCondition,
		validators,
		initialValue)
{
	public override void SetData(IFieldDataContainer<int?> context)
	{
		Value = context.Value;
	}

	public override IFieldDataContainer<int?> GetData()
		=> new BasicFieldDataContainer<int?> { Value = Value };
}