namespace RobinEpple.Common.Forms.Binding;

using System.Diagnostics.CodeAnalysis;
using RobinEpple.Common.Forms.Extensions;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Util;

public class EmbeddedModel<TModel>(Func<TModel> modelFactory, Func<TModel, bool> applicabilityPredicate)
	: FormNodeExtensionBase,
		IFormModel
{
	public EmbeddedModel(Func<TModel> modelFactory)
		: this(modelFactory, _ => true) { }

	private readonly Func<TModel> _createModel = modelFactory;
	private readonly Func<TModel, bool> _applicabilityPredicate = applicabilityPredicate;

	/// <inheritdoc />
	public bool Accepts(object? model) =>
		TryConvert(model, out var castedModel) && _applicabilityPredicate(castedModel);

	private bool TryConvert(object? model, [NotNullWhen(true)] out TModel? convertedModel)
	{
		convertedModel = default;
		if (model == null)
		{
			return false;
		}
		return NullableUnwrappingTypeConverter.TryConvert(model, out convertedModel);
	}

	/// <inheritdoc />
	public object? GetInstance(IForm instance)
	{
		if (!instance.Tags.TryGetValue(IFormNodeBinding.InstanceModelTagName, out var model))
		{
			throw new ArgumentException($"The form instance does not contain a model.");
		}

		return model;
	}

	/// <inheritdoc />
	public void SetInstance(IForm instance, object? model)
	{
		if (!Accepts(model))
		{
			throw new ArgumentException($"The given model is not accepted by this node.");
		}

		instance.SetTag(IFormNodeBinding.InstanceModelTagName, model);
	}

	/// <inheritdoc />
	public override void OnInitialize(IFormNode node)
	{
		var instanceModel = _createModel();
		node.SetTag(IFormNodeBinding.InstanceModelTagName, instanceModel);
	}
}
