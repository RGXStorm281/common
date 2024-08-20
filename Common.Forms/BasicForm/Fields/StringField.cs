using RobinEpple.Common.Forms.BasicApi;
using RobinEpple.Common.Forms.BasicApi.DataContainers;
using RobinEpple.Common.Forms.BasicApi.Nodes;
using RobinEpple.Common.Forms.BasicForm.DataContainers;

namespace RobinEpple.Common.Forms.BasicForm.Fields;

/// <summary>
/// Represents a field of type <see langword="string" /> in the form.
/// </summary>
/// <inheritdoc cref="FormFieldBase{TNodeType,TValue}" />
internal class StringField(
	string id,
	IParentNode parent,
	ILabel? label,
	IExpression<bool>? visibilityCondition,
	IEnumerable<INodeValidator<StringField>> validators,
	string? initialValue)
	: FormFieldBase<StringField, string?>(
		id,
		parent,
		label,
		visibilityCondition,
		validators,
		initialValue)
{
	public override void SetData(IFieldDataContainer<string?> context)
	{
		Value = context.Value;
	}

	public override IFieldDataContainer<string?> GetData()
		=> new BasicFieldDataContainer<string?> { Value = Value };
}