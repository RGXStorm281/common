namespace RobinEpple.Common.Forms.Binding;

using System.Diagnostics.CodeAnalysis;
using RobinEpple.Common.Forms.Extensions;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Util;

internal class EmbeddedModel<TModel>(Func<TModel> modelFactory, Func<TModel, bool> applicabilityPredicate)
	: FormNodeExtensionBase,
		IEmbeddedModel
{
	public EmbeddedModel(Func<TModel> modelFactory)
		: this(modelFactory, _ => true) { }

	private readonly Func<TModel> _createModel = modelFactory;
	private readonly Func<TModel, bool> _applicabilityPredicate = applicabilityPredicate;

	/// <inheritdoc />
	public bool Accepts(object? value) => TryConvert(value, out var model) && _applicabilityPredicate(model);

	private bool TryConvert(object? value, [NotNullWhen(true)] out TModel? model)
	{
		model = default;
		if (value == null)
		{
			return false;
		}
		return NullableUnwrappingTypeConverter.TryConvert(value, out model);
	}

	/// <inheritdoc />
	public object? GetValue(IForm node)
	{
		if (!node.Tags.TryGetValue(IFormNodeBinding.InstanceModelTagName, out var value))
		{
			throw new ArgumentException($"The form instance does not contain a model.");
		}

		return value;
	}

	/// <inheritdoc />
	public void SetValue(IForm node, object? value)
	{
		if (!Accepts(value))
		{
			throw new ArgumentException($"The given model is not accepted by this node.");
		}

		node.SetTag(IFormNodeBinding.InstanceModelTagName, value);
	}

	/// <inheritdoc />
	public override void OnInitialize(IFormNode node)
	{
		var instanceModel = _createModel();
		node.SetTag(IFormNodeBinding.InstanceModelTagName, instanceModel);
	}
}
