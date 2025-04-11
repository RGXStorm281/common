namespace RobinEpple.Common.Forms.Test.Mocks;

using RobinEpple.Common.Forms.Extensions;
using RobinEpple.Common.Forms.Nodes;

public class MockExtension : FormNodeExtensionBase
{
	public bool OnBeforeVisibilityEvaluationHasBeenCalled { get; private set; } = false;
	public bool OnAfterVisibilityEvaluationHasBeenCalled { get; private set; } = false;
	public bool OnBeforeValidationHasBeenCalled { get; private set; } = false;
	public bool OnAfterValidationHasBeenCalled { get; private set; } = false;

	/// <inheritdoc />
	public override void OnBeforeVisibilityEvaluation(IFormNode node)
	{
		OnBeforeVisibilityEvaluationHasBeenCalled = true;
	}

	/// <inheritdoc />
	public override void OnAfterVisibilityEvaluation(IFormNode node)
	{
		OnAfterVisibilityEvaluationHasBeenCalled = true;
	}

	/// <inheritdoc />
	public override void OnBeforeValidation(IFormNode node)
	{
		OnBeforeValidationHasBeenCalled = true;
	}

	/// <inheritdoc />
	public override void OnAfterValidation(IFormNode node)
	{
		OnAfterValidationHasBeenCalled = true;
	}
}
