using RobinEpple.Common.Forms.BasicApi;
using RobinEpple.Common.Forms.BasicApi.DataContainers;
using RobinEpple.Common.Forms.BasicApi.Nodes;

namespace RobinEpple.Common.Forms.BasicForm.Fields;

/// <inheritdoc cref="NodeBase{TNodeType}" />
/// <summary>
/// The abstract base class for all fields in a form.
/// </summary>
/// <typeparam name="TNodeType">The implementation type of this FormFieldBase.</typeparam>
/// <typeparam name="TValue">The type value this field holds.</typeparam>
/// <param name="initialValue">The initial value the form field should possess.</param>
internal abstract class FormFieldBase<TNodeType, TValue>(
	string id,
	IParentNode parent,
	ILabel? label,
	IExpression<bool>? visibilityCondition,
	IEnumerable<INodeValidator<TNodeType>> validators,
	TValue initialValue)
	: NodeBase<TNodeType>(
		  id,
		  parent,
		  label,
		  visibilityCondition,
		  validators),
	  IFieldNode<TValue>
	where TNodeType : IFormNode
{
	private readonly TValue _initialValue = initialValue;

	/// <summary>
	/// The value of the field.
	/// </summary>
	public TValue Value { get; set; } = initialValue;

	/// <inheritdoc />
	public abstract void SetData(IFieldDataContainer<TValue> context);

	/// <inheritdoc />
	public abstract IFieldDataContainer<TValue> GetData();

	/// <inheritdoc />
	public override void ResetState()
	{
		base.ResetState();
		Value = _initialValue;
	}
}