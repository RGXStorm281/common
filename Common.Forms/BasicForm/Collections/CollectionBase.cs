using RobinEpple.Common.Forms.BasicApi;
using RobinEpple.Common.Forms.BasicApi.DataContainers;
using RobinEpple.Common.Forms.BasicApi.Nodes;
using RobinEpple.Common.Forms.BasicForm.DataContainers;

namespace RobinEpple.Common.Forms.BasicForm.Collections;

/// <inheritdoc cref="NodeBase{TNodeImplementationType}" />
/// <summary>
/// The abstract base class for all collections in a form.
/// </summary>
/// <typeparam name="TNodeType">The implementation type of this CollectionBase.</typeparam>
/// <typeparam name="TItem">The type of the items this collection holds.</typeparam>
internal abstract class CollectionBase<TNodeType, TItem>(
	string id,
	IParentNode parent,
	ILabel? label,
	IExpression<bool>? visibilityCondition,
	IEnumerable<INodeValidator<TNodeType>> validators)
	: NodeBase<TNodeType>(
		  id,
		  parent,
		  label,
		  visibilityCondition,
		  validators),
	  ICollectionNode<TItem>
	where TNodeType : IFormNode
{
	/// <summary>
	/// The internal list of items in the collection.
	/// </summary>
	protected List<TItem> Items = [];

	/// <inheritdoc />
	public IEnumerable<TItem> Values
		=> Items;

	/// <inheritdoc />
	public override void ResetState()
	{
		base.ResetState();
		Items.Clear();
	}

	/// <inheritdoc />
	public virtual void SetData(ICollectionDataContainer context)
	{
		// Use field data containers to carry simple form values.
		Items = context.ItemDataContainers
					   .OfType<IFieldDataContainer<TItem>>()
					   .Select(container => container.Value)
					   .OfType<TItem>()
					   .ToList();
	}

	/// <inheritdoc />
	public virtual ICollectionDataContainer GetData()
		=> new BasicCollectionDataContainer
		{
			// Use field data containers to carry simple form values.
			MutableList = Items.Select(item => new BasicFieldDataContainer<TItem> { Value = item })
								  .OfType<IFormDataContainer>()
								  .ToList()
		};
}