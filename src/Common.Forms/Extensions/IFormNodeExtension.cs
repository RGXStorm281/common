namespace RobinEpple.Common.Forms.Extensions;

using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// This interface provides a hook into the form processing pipeline.<br/>
/// For implementation it is advised to use the class <see cref="FormNodeExtensionBase"/><br/>
/// and only override the needed methods.
/// </summary>
public interface IFormNodeExtension
{
	/// <summary>
	/// This event is run after the node has been created.
	/// </summary>
	/// <param name="node">The node that triggered the event.</param>
	public void OnInitialize(IFormNode node);

	/// <inheritdoc cref="OnInitialize"/>
	public Task OnInitializeAsync(IFormNode node);

	/// <summary>
	/// This event is run before the node evaluates it's readonly state.<br/>
	/// It is always run.
	/// </summary>
	/// <param name="node">The node that triggered the event.</param>
	public void OnBeforeReadonlyStateEvaluation(IFormNode node);

	/// <inheritdoc cref="OnBeforeReadonlyStateEvaluation"/>
	public Task OnBeforeReadonlyStateEvaluationAsync(IFormNode node);

	/// <summary>
	/// This event is run after the node has evaluated it's readonly state.<br/>
	/// It is always run.
	/// </summary>
	/// <param name="node">The node that triggered the event.</param>
	public void OnAfterReadonlyStateEvaluation(IFormNode node);

	/// <inheritdoc cref="OnAfterReadonlyStateEvaluation"/>
	public Task OnAfterReadonlyStateEvaluationAsync(IFormNode node);

	/// <summary>
	/// This event is run before the node evaluates it's visibility.<br/>
	/// It is always run.
	/// </summary>
	/// <param name="node">The node that triggered the event.</param>
	public void OnBeforeVisibilityEvaluation(IFormNode node);

	/// <inheritdoc cref="OnBeforeVisibilityEvaluation"/>
	public Task OnBeforeVisibilityEvaluationAsync(IFormNode node);

	/// <summary>
	/// This event is run after the node has evaluated it's visibility.<br/>
	/// It is always run.
	/// </summary>
	/// <param name="node">The node that triggered the event.</param>
	public void OnAfterVisibilityEvaluation(IFormNode node);

	/// <inheritdoc cref="OnAfterVisibilityEvaluation"/>
	public Task OnAfterVisibilityEvaluationAsync(IFormNode node);

	/// <summary>
	/// This event is run before the validation step.<br/>
	/// It is only called if the node is visible and validation is therefore required.
	/// </summary>
	/// <param name="node">The node that triggered the event.</param>
	public void OnBeforeValidation(IFormNode node);

	/// <inheritdoc cref="OnBeforeValidation"/>
	public Task OnBeforeValidationAsync(IFormNode node);

	/// <summary>
	/// This event is run after the validation step.<br/>
	/// It is only called if the node is visible and validation has taken place.
	/// </summary>
	/// <param name="node">The node that triggered the event.</param>
	public void OnAfterValidation(IFormNode node);

	/// <inheritdoc cref="OnAfterValidation"/>
	public Task OnAfterValidationAsync(IFormNode node);
}
