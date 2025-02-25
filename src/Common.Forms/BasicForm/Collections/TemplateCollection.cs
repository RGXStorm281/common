namespace RobinEpple.Common.Forms.BasicForm.Collections;

using RobinEpple.Common.Forms.BasicApi;
using RobinEpple.Common.Forms.BasicApi.DataContainers;
using RobinEpple.Common.Forms.BasicApi.Nodes;
using RobinEpple.Common.Forms.BasicForm.DataContainers;
using RobinEpple.Common.Forms.BasicForm.Forms;

/// <inheritdoc cref="CollectionBase{TNodeType,TItem}" />
/// <summary>
/// The collection node for templated sub-forms.
/// </summary>
internal class TemplateCollection(
	string id,
	IParentNode parent,
	ILabel? label,
	IExpression<bool>? visibilityCondition,
	IEnumerable<INodeValidator<TemplateCollection>> validators,
	IEnumerable<ITemplate> templates
)
	: NodeBase<TemplateCollection>(id, parent, label, visibilityCondition, validators),
		ICollectionNode<IContainerNode>,
		IParentNode
{
	private readonly List<(string TemplateId, IContainerNode Instance)> _templateInstances = [];

	/// <summary>
	/// The templates, that this collection can draw from.
	/// </summary>
	private readonly IDictionary<string, ITemplate> _templatesById = templates.ToDictionary(template => template.Id);

	/// <inheritdoc />
	public IEnumerable<IContainerNode> Values => _templateInstances.Select(instance => instance.Instance);

	/// <inheritdoc />
	public void SetData(ICollectionDataContainer context)
	{
		// Clear list.
		_templateInstances.Clear();

		foreach (var templateDataContainer in context.ItemDataContainers.OfType<ITemplateDataContainer>())
		{
			if (!_templatesById.TryGetValue(templateDataContainer.TemplateId, out var template))
			{
				// Unknown template -> Skip.
				continue;
			}

			// Known template -> Instantiate sub-form.
			var item = template.CreateInstance(this);

			// Set data and add to items.
			item.SetData(templateDataContainer);
			_templateInstances.Add((template.Id, item));
		}
	}

	/// <inheritdoc />
	public ICollectionDataContainer GetData() =>
		new BasicCollectionDataContainer
		{
			MutableList = _templateInstances
				.Select(instance => new BasicTemplateDataContainer
				{
					TemplateId = instance.TemplateId,
					ChildDataContainersByChildId = instance.Instance.GetData().ChildDataContainersByChildId,
				})
				.OfType<IFormDataContainer>()
				.ToList(),
		};

	public IFormNode FindClosestBefore(IFormNode currentNode, Func<IFormNode, bool> predicate) =>
		_templateInstances
			.Select(instance => instance.Instance)
			.OfType<IFormNode>()
			.ToList()
			.FindClosestBefore(currentNode, predicate);

	public IFormNode? FindFirstBackwards(Func<IFormNode, bool> predicate) =>
		_templateInstances
			.Select(instance => instance.Instance)
			.OfType<IFormNode>()
			.ToList()
			.FindFirstBackwards(predicate);
}
