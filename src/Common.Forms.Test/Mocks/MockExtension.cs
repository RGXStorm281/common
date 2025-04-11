namespace RobinEpple.Common.Forms.Test.Mocks;

using RobinEpple.Common.Forms.Extensions;
using RobinEpple.Common.Forms.Nodes;

public class MockExtension : FormNodeExtensionBase
{
	public bool OnBeforeReadonlyStateEvaluationHasBeenCalled { get; private set; } = false;
	public bool? OnBeforeReadonlyStateEvaluationReadonlyValue { get; private set; }
	public bool OnAfterReadonlyStateEvaluationHasBeenCalled { get; private set; } = false;
	public bool? OnAfterReadonlyStateEvaluationReadonlyValue { get; private set; }
	public bool OnBeforeVisibilityEvaluationHasBeenCalled { get; private set; } = false;
	public bool? OnBeforeVisibilityEvaluationVisibilityValue { get; private set; }
	public bool OnAfterVisibilityEvaluationHasBeenCalled { get; private set; } = false;
	public bool? OnAfterVisibilityEvaluationVisibilityValue { get; private set; }
	public bool OnBeforeValidationHasBeenCalled { get; private set; } = false;
	public bool? OnBeforeValidationIsValidValue { get; private set; }
	public bool OnAfterValidationHasBeenCalled { get; private set; } = false;
	public bool? OnAfterValidationIsValidValue { get; private set; }

	/// <inheritdoc />
	public override void OnBeforeReadonlyStateEvaluation(IFormNode node)
	{
		OnBeforeReadonlyStateEvaluationReadonlyValue = node.IsReadonly;
		OnBeforeReadonlyStateEvaluationHasBeenCalled = true;
	}

	/// <inheritdoc />
	public override void OnAfterReadonlyStateEvaluation(IFormNode node)
	{
		OnAfterReadonlyStateEvaluationReadonlyValue = node.IsReadonly;
		OnAfterReadonlyStateEvaluationHasBeenCalled = true;
	}

	/// <inheritdoc />
	public override void OnBeforeVisibilityEvaluation(IFormNode node)
	{
		OnBeforeVisibilityEvaluationVisibilityValue = node.IsVisible;
		OnBeforeVisibilityEvaluationHasBeenCalled = true;
	}

	/// <inheritdoc />
	public override void OnAfterVisibilityEvaluation(IFormNode node)
	{
		OnAfterVisibilityEvaluationVisibilityValue = node.IsVisible;
		OnAfterVisibilityEvaluationHasBeenCalled = true;
	}

	/// <inheritdoc />
	public override void OnBeforeValidation(IFormNode node)
	{
		OnBeforeValidationIsValidValue = node.IsValid;
		OnBeforeValidationHasBeenCalled = true;
	}

	/// <inheritdoc />
	public override void OnAfterValidation(IFormNode node)
	{
		OnAfterValidationIsValidValue = node.IsValid;
		OnAfterValidationHasBeenCalled = true;
	}
}
