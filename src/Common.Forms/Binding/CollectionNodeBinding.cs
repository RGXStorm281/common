namespace RobinEpple.Common.Forms.Binding;

using System.Diagnostics.CodeAnalysis;
using RobinEpple.Common.Forms.Extensions;
using RobinEpple.Common.Forms.Nodes;

public class CollectionNodeBinding<TItem> : IValueAccessor<IEnumerable<TItem>>
{
	/// <inheritdoc />
	public IEnumerable<TItem> GetValue(IFormNode node)
	{
		if (node is not ICollectionNode collectionNode)
		{
			throw new InvalidOperationException($"A CollectionNodeBinding can only be used on collection nodes.");
		}

		var values = new List<TItem>();
		foreach (var instance in collectionNode.Instances)
		{
			if (TryGetModelFromInstance(instance, out var model))
			{
				values.Add(model);
			}
		}
		return values;
	}

	private bool TryGetModelFromInstance(IForm instance, [NotNullWhen(true)] out TItem? model)
	{
		model = default;
		if (!instance.Tags.TryGetValue(IFormNodeBinding.InstanceModelTagName, out var instanceModel))
		{
			return false;
		}

		if (instanceModel is not TItem value)
		{
			return false;
		}

		model = value;
		return true;
	}

	/// <inheritdoc />
	public void SetValue(IEnumerable<TItem> values, IFormNode node)
	{
		if (node is not ICollectionNode collectionNode)
		{
			throw new InvalidOperationException($"A CollectionNodeBinding can only be used on collection nodes.");
		}

		// Clear the current values.
		collectionNode.Clear();

		// Then instantiate each given value.
		foreach (var value in values)
		{
			InstantiateModel(collectionNode, value);
		}
	}

	private void InstantiateModel(ICollectionNode collectionNode, TItem value)
	{
		// Iterate over all templates.
		foreach (var template in collectionNode.Templates)
		{
			// Check if there is an extension, that is applicable for the given value type.
			foreach (var extension in template.Extensions)
			{
				if (extension is not IValueModelExtension<TItem> valueModel)
				{
					continue;
				}

				if (!valueModel.IsApplicableTo(value))
				{
					continue;
				}

				// Use the first extension that accepts this model type, and quit.
				var instance = collectionNode.Instantiate(template);
				valueModel.LoadValue(instance, value);
				return;
			}
		}
	}
}
