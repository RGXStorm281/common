namespace RobinEpple.Common.Forms.Binding;

using RobinEpple.Common.Forms.Nodes;

public class FormNodeBinding<TValue>(
	IValueAccessor<TValue> nodeValueAccessor,
	IValueAccessor<TValue> modelValueAccessor
) : IFormNodeBinding
{
	private readonly IValueAccessor<TValue> _nodeValueAccessor = nodeValueAccessor;
	private readonly IValueAccessor<TValue> _modelValueAccessor = modelValueAccessor;

	/// <inheritdoc />
	public void LoadFromModel(IFormNode node)
	{
		var value = _modelValueAccessor.GetValue(node);
		_nodeValueAccessor.SetValue(value, node);
	}

	/// <inheritdoc />
	public void WriteToModel(IFormNode node)
	{
		var value = _nodeValueAccessor.GetValue(node);
		_modelValueAccessor.SetValue(value, node);
	}
}
