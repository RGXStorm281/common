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
	/// This event is run before the node evaluates it's visibility.<br/>
	/// It is always run. <br/>
	/// Changes to the visibility and validation state might be overwritten by the pipeline afterwards.
	/// </summary>
	/// <param name="node">The node that triggered the event.</param>
	public void OnBeforeVisibilityEvaluation(IFormNode node);

	/// <summary>
	/// This event is run after the node has evaluated it's visibility.<br/>
	/// It is always run. <br/>
	/// Changes to the visibility are persistent and decide whether validation is run or not.<br/>
	/// Changes to the validation state might be overwritten by the pipeline afterwards.
	/// </summary>
	/// <param name="node">The node that triggered the event.</param>
	public void OnAfterVisibilityEvaluation(IFormNode node);

	/// <summary>
	/// This event is run before the validation step.<br/>
	/// It is only called if the node is visible and validation is therefore required. <br/>
	/// Changes to the visibility are persistent but do not cancel validation.<br/>
	/// Changes to the validation state might be overwritten by the pipeline afterwards.<br/>
	/// </summary>
	/// <param name="node">The node that triggered the event.</param>
	public void OnBeforeValidation(IFormNode node);

	/// <summary>
	/// This event is run after the validation step.<br/>
	/// It is only called if the node is visible and validation has taken place. <br/>
	/// Changes to the visibility and validation state are persistent.
	/// </summary>
	/// <param name="node">The node that triggered the event.</param>
	public void OnAfterValidation(IFormNode node);
}
