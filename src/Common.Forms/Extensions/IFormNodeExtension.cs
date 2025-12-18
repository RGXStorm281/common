namespace RobinEpple.Common.Forms.Extensions;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

/// <summary>
/// This interface provides a hook into the form processing pipeline.<br/>
/// For implementation it is advised to use the class <see cref="FormNodeExtensionBase"/><br/>
/// and only override the needed methods.
/// </summary>
public partial interface IFormNodeExtension
{
	/// <summary>
	/// This event is run after the node has been created.
	/// </summary>
	/// <param name="node">The node that triggered the event.</param>
	[GenerateAsyncOverload]
	public void OnInitialize(IFormNode node);

	/// <summary>
	/// This event is run before the node evaluates it's readonly state.<br/>
	/// It is always run.
	/// </summary>
	/// <param name="node">The node that triggered the event.</param>
	[GenerateAsyncOverload]
	public void OnBeforeReadonlyStateEvaluation(IFormNode node);

	/// <summary>
	/// This event is run after the node has evaluated it's readonly state.<br/>
	/// It is always run.
	/// </summary>
	/// <param name="node">The node that triggered the event.</param>
	[GenerateAsyncOverload]
	public void OnAfterReadonlyStateEvaluation(IFormNode node);

	/// <summary>
	/// This event is run before the node evaluates it's visibility.<br/>
	/// It is always run.
	/// </summary>
	/// <param name="node">The node that triggered the event.</param>
	[GenerateAsyncOverload]
	public void OnBeforeVisibilityEvaluation(IFormNode node);

	/// <summary>
	/// This event is run after the node has evaluated it's visibility.<br/>
	/// It is always run.
	/// </summary>
	/// <param name="node">The node that triggered the event.</param>
	[GenerateAsyncOverload]
	public void OnAfterVisibilityEvaluation(IFormNode node);

	/// <summary>
	/// This event is run before the validation step.<br/>
	/// It is only called if the node is visible and validation is therefore required.
	/// </summary>
	/// <param name="node">The node that triggered the event.</param>
	[GenerateAsyncOverload]
	public void OnBeforeValidation(IFormNode node);

	/// <summary>
	/// This event is run after the validation step.<br/>
	/// It is only called if the node is visible and validation has taken place.
	/// </summary>
	/// <param name="node">The node that triggered the event.</param>
	[GenerateAsyncOverload]
	public void OnAfterValidation(IFormNode node);
}
