namespace RobinEpple.Common.Forms.Extensions;

using RobinEpple.Common.Forms.Binding;
using RobinEpple.Common.Forms.Nodes;

public class InstanceModelExtension<TBaseType, TImplementationType>(
	Func<TImplementationType> modelFactory,
	Func<TBaseType, bool> applicabilityPredicate
) : FormNodeExtensionBase, IValueModelExtension<TBaseType>
	where TImplementationType : TBaseType
{
	public InstanceModelExtension(Func<TImplementationType> modelFactory)
		: this(modelFactory, instance => instance is TImplementationType) { }

	private readonly Func<TImplementationType> _createModel = modelFactory;
	private readonly Func<TBaseType, bool> _applicabilityPredicate = applicabilityPredicate;

	/// <inheritdoc />
	public bool IsApplicableTo(TBaseType model) => _applicabilityPredicate(model);

	/// <inheritdoc />
	public void LoadValue(IForm instance, TBaseType model)
	{
		instance.SetTag(IFormNodeBinding.InstanceModelTagName, model);
	}

	/// <inheritdoc />
	public override void OnInitialize(IFormNode node)
	{
		var instanceModel = _createModel();
		node.SetTag(IFormNodeBinding.InstanceModelTagName, instanceModel);
	}
}
