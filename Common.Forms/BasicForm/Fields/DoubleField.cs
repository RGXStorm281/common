using RobinEpple.Common.Forms.BasicApi;
using RobinEpple.Common.Forms.BasicApi.DataContainers;
using RobinEpple.Common.Forms.BasicApi.Nodes;
using RobinEpple.Common.Forms.BasicForm.DataContainers;

namespace RobinEpple.Common.Forms.BasicForm.Fields;

/// <summary>
/// Represents a field of type <see langword="double" /> in the form.
/// </summary>
/// <inheritdoc cref="FormFieldBase{TNodeType,TValue}" />
internal class DoubleField(
	string id,
	IParentNode parent,
	ILabel? label,
	IExpression<bool>? visibilityCondition,
	IEnumerable<INodeValidator<DoubleField>> validators,
	double? initialValue)
	: FormFieldBase<DoubleField, double?>(
		id,
		parent,
		label,
		visibilityCondition,
		validators,
		initialValue)
{
	public override void SetData(IFieldDataContainer<double?> context)
	{
		Value = context.Value;
	}

	public override IFieldDataContainer<double?> GetData()
		=> new BasicFieldDataContainer<double?> { Value = Value };
}