namespace RobinEpple.Common.Forms;

using RobinEpple.Common.Forms.Binding;
using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Extensions;
using RobinEpple.Common.Forms.Validation;

public interface INodeBuilder<TSpecificNodeBuilder>
{
	/// <summary>
	/// Configures the node to use a label different to the field name.
	/// </summary>
	/// <param name="label">The label of the node.</param>
	/// <returns>The node builder to add further configurations.</returns>
	public TSpecificNodeBuilder UseLabel(string label);

	/// <summary>
	/// Configures the nodes default visibility. Default is true.
	/// </summary>
	/// <param name="isVisible">Whether the node is visible.</param>
	/// <returns>The node builder to add further configurations.</returns>
	public TSpecificNodeBuilder UseDefaultVisibility(bool isVisible);

	/// <summary>
	/// Configures the node to use a visibility condition.<br/>
	/// Only one of them can be used on one node.
	/// </summary>
	/// <param name="condition">The condition. The field is visible, when this evaluates to true.</param>
	/// <returns>The node builder to add further configurations.</returns>
	public TSpecificNodeBuilder UseVisibilityCondition(IFormExpression<bool> condition);

	/// <summary>
	/// Configures the nodes default modifiability. Default is false.
	/// </summary>
	/// <param name="isReadonly">Whether the node is readonly.</param>
	/// <returns>The node builder to add further configurations.</returns>
	public TSpecificNodeBuilder UseDefaultReadonly(bool isReadonly);

	/// <summary>
	/// Configures the node to use a readonly condition.<br/>
	/// Only one of them can be used on one node.
	/// </summary>
	/// <param name="condition">The condition. The field is readonly, when this evaluates to true.</param>
	/// <returns>The node builder to add further configurations.</returns>
	public TSpecificNodeBuilder UseReadonlyCondition(IFormExpression<bool> condition);

	/// <summary>
	/// Configures the node to use a validator.<br/>
	/// Multiple validators can be added and active at the same time.<br/>
	/// The order of <see cref="UseValidator"/> calls determines the order of execution.
	/// </summary>
	/// <param name="validator">The validator.</param>
	/// <returns>The node builder to add further configurations.</returns>
	public TSpecificNodeBuilder UseValidator(INodeValidator validator);

	/// <summary>
	/// Configures the node to use an extension.<br/>
	/// Multiple extensions can be registered and active at the same time.<br/>
	/// The order of <see cref="UseExtension"/> calls determines the order of execution.
	/// </summary>
	/// <param name="extension">The extension.</param>
	/// <returns>The node builder to add further configurations.</returns>
	public TSpecificNodeBuilder UseExtension(IFormNodeExtension extension);

	/// <summary>
	/// Configures the node to bind to a model.<br/>
	/// Only one can be used per node.
	/// </summary>
	/// <param name="binding">The binding to pull and push changes from and to some model.</param>
	/// <returns>The builder to add further configurations.</returns>
	public TSpecificNodeBuilder UseBinding(IFormNodeBinding binding);
}
