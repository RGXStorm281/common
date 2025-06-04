namespace RobinEpple.Common.Forms.Binding;

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using RobinEpple.Common.Forms.Extensions;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Util;

public class TemplateNodeBinding<TValue>(TValue emptyValue) : IValueAccessor<TValue>
{
	private readonly TValue _emptyValue = emptyValue;

	/// <inheritdoc />
	public TValue GetValue(IFormNode node)
	{
		if (node is not ITemplateNode templateNode)
		{
			throw new InvalidOperationException($"A TemplateNodeBinding can only be used on template nodes.");
		}

		// Extract the value from the instance.
		if (templateNode.Instance == null)
		{
			return _emptyValue;
		}

		if (TryGetModelFromInstance(templateNode.Instance, out var model))
		{
			return model;
		}

		return _emptyValue;
	}

	private bool TryGetModelFromInstance(IForm node, [NotNullWhen(true)] out TValue? model)
	{
		model = default;
		if (node.EmbeddedModel?.GetValue(node) is not { } instance)
		{
			return false;
		}

		return NullableUnwrappingTypeConverter.TryConvert(instance, out model);
	}

	/// <inheritdoc />
	public void SetValue(TValue? value, IFormNode node)
	{
		if (node is not ITemplateNode templateNode)
		{
			throw new InvalidOperationException($"A TemplateNodeBinding can only be used on template nodes.");
		}

		// Clear the current value.
		templateNode.Clear();

		if (value == null)
		{
			// If the given value is null, there is nothing left to do.
			return;
		}

		// Otherwise find the matching template and instantiate it.
		InstantiateModel(templateNode, value);
	}

	private void InstantiateModel(ITemplateNode templateNode, TValue value)
	{
		// Iterate over all templates and instantiate the first that accepts the model.
		foreach (var template in templateNode.Templates)
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
			var instance = templateNode.Instantiate(template);
			model.SetValue(instance, value);
			return;
		}
	}
}
