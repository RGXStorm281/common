namespace RobinEpple.Common.Forms.Binding;

using System.Diagnostics.CodeAnalysis;
using RobinEpple.Common.Forms.Extensions;
using RobinEpple.Common.Forms.Nodes;

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

	private bool TryGetModelFromInstance(IForm instance, [NotNullWhen(true)] out TValue? model)
	{
		model = default;
		if (!instance.Tags.TryGetValue(IFormNodeBinding.InstanceModelTagName, out var instanceModel))
		{
			return false;
		}

		if (instanceModel is not TValue value)
		{
			return false;
		}

		model = value;
		return true;
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
		// Iterate over all templates.
		foreach (var template in templateNode.Templates)
		{
			// Check if there is an extension, that is applicable for the given value type.
			foreach (var extension in template.Extensions)
			{
				if (extension is not IValueModelExtension<TValue> embeddedModel)
				{
					continue;
				}

				if (!embeddedModel.IsApplicableTo(value))
				{
					continue;
				}

				// Use the first extension that accepts this model type, and quit.
				var instance = templateNode.Instantiate(template);
				embeddedModel.LoadValue(instance, value);
				return;
			}
		}
	}
}
