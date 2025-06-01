namespace RobinEpple.Common.Forms.Building;

using RobinEpple.Common.Forms.Binding;
using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Extensions;
using RobinEpple.Common.Forms.Nodes.DefaultImplementation;
using RobinEpple.Common.Forms.Validation;

internal abstract class NodeBuilder<TSpecificNodeBuilder, TNode> : INodeBuilder<TSpecificNodeBuilder>
	where TNode : NodeBase
{
	protected TNode Node { get; }

	public NodeBuilder(TNode node)
	{
		Node = node;
	}

	/// <summary>
	/// Casts this object into the most detailed form builder type.
	/// </summary>
	/// <returns>The casted builder.</returns>
	protected abstract TSpecificNodeBuilder CastThis();

	/// <inheritdoc/>
	public TSpecificNodeBuilder UseDefaultReadonly(bool isReadonly)
	{
		Node.ReplaceDefaultReadonly(isReadonly);
		return CastThis();
	}

	/// <inheritdoc/>
	public TSpecificNodeBuilder UseDefaultVisibility(bool isVisible)
	{
		Node.ReplaceDefaultVisibility(isVisible);
		return CastThis();
	}

	/// <inheritdoc/>
	public TSpecificNodeBuilder UseVisibilityCondition(IFormExpression<bool> condition)
	{
		Node.UseVisibilityCondition(condition);
		return CastThis();
	}

	/// <inheritdoc/>
	public TSpecificNodeBuilder UseReadonlyCondition(IFormExpression<bool> condition)
	{
		Node.UseReadonlyCondition(condition);
		return CastThis();
	}

	/// <inheritdoc/>
	public TSpecificNodeBuilder UseExtension(IFormNodeExtension extension)
	{
		Node.UseExtension(extension);
		return CastThis();
	}

	/// <inheritdoc/>
	public TSpecificNodeBuilder UseLabel(string label)
	{
		Node.Label = label;
		Node.ReplaceDefaultLabel(label);
		return CastThis();
	}

	/// <inheritdoc/>
	public TSpecificNodeBuilder UseValidator(INodeValidator validator)
	{
		Node.UseValidator(validator);
		return CastThis();
	}

	/// <inheritdoc />
	public TSpecificNodeBuilder UseBinding(IFormNodeBinding binding)
	{
		Node.UseBinding(binding);
		return CastThis();
	}

	/// <inheritdoc/>
	public string GetNodeName() => Node.Name;
}
