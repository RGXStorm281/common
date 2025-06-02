namespace RobinEpple.Common.Forms.Binding;

using System.Diagnostics.CodeAnalysis;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Util;

public class SingleFieldModel<TValue>(string fieldName, TValue emptyValue) : IFormModel
{
	private readonly string _fieldName = fieldName;
	private readonly TValue _emptyValue = emptyValue;

	/// <inheritdoc />
	// Always accept models with the correct type.

	public bool Accepts(object? model) => model == null || TryConvert(model, out _);

	private bool TryConvert(object? model, [NotNullWhen(true)] out TValue? convertedModel)
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
		var targetNode = instance.FindNode(_fieldName);
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
	public void SetInstance(IForm instance, object? model)
	{
		if (!Accepts(model))
		{
			throw new ArgumentException($"The given model is not accepted by this node.");
		}

		var targetNode = instance.FindNode(_fieldName);
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

		if (model == null)
		{
			targetField.Value = _emptyValue;
		}

		if (!TryConvert(model, out var value))
		{
			throw new ArgumentException($"The given model is not accepted by this node.");
		}

		targetField.Value = value;
	}
}
