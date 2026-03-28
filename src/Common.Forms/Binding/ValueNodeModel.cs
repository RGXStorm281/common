namespace RobinEpple.Common.Forms.Binding;

using System.Diagnostics.CodeAnalysis;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Util;

internal class ValueNodeModel<TValue>(string fieldName, TValue emptyValue) : IEmbeddedModel
{
	private readonly string _fieldName = fieldName;
	private readonly TValue _emptyValue = emptyValue;

	/// <inheritdoc />
	// Always accept models with the correct type.
	public bool Accepts(object? value) => value == null || TryConvert(value, out _);

	private bool TryConvert(object? value, [NotNullWhen(true)] out TValue? model)
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
		var targetNode = node.FindFirst(_fieldName);
		if (targetNode == null)
		{
			throw new ArgumentException(
				$"The given form instance does not contain a (unique) node called '{_fieldName}'."
			);
		}

		if (targetNode is not IValueNode<TValue> targetField)
		{
			throw new ArgumentException(
				$"The node '{_fieldName}' does not expose a value of type '{typeof(TValue).Name}'."
			);
		}

		return targetField.Value;
	}

	/// <inheritdoc />
	public void SetValue(IForm node, object? value)
	{
		if (!Accepts(value))
		{
			throw new ArgumentException($"The given model is not accepted by this node.");
		}

		var targetNode = node.FindFirst(_fieldName);
		if (targetNode == null)
		{
			throw new ArgumentException(
				$"The given form instance does not contain a (unique) node called '{_fieldName}'."
			);
		}

		if (targetNode is not IValueNode<TValue> targetField)
		{
			throw new ArgumentException(
				$"The node '{_fieldName}' does not expose a value of type '{typeof(TValue).Name}'."
			);
		}

		if (value == null)
		{
			targetField.Value = _emptyValue;
		}

		if (!TryConvert(value, out var typedValue))
		{
			throw new ArgumentException($"The given model is not accepted by this node.");
		}

		targetField.Value = typedValue;
	}
}
