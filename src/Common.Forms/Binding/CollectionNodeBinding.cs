namespace RobinEpple.Common.Forms.Binding;

using System.Diagnostics.CodeAnalysis;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Util;

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

	private bool TryGetModelFromInstance(IForm node, [NotNullWhen(true)] out TItem? model)
	{
		model = default;
		if (node.EmbeddedModel?.GetValue(node) is not { } instance)
		{
			return false;
		}

		return NullableUnwrappingTypeConverter.TryConvert(instance, out model);
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
		// Iterate over all templates and instantiate the first that accepts the model.
		foreach (var template in collectionNode.Templates)
		{
			if (template.EmbeddedModel is not { } model)
			{
				continue;
			}

			if (!model.Accepts(value))
			{
				continue;
			}

			// Use the first extension that accepts this model type, and quit.
			var instance = collectionNode.Instantiate(template);
			model.SetValue(instance, value);
			return;
		}
	}
}
