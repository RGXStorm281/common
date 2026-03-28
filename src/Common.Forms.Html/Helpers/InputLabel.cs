namespace RobinEpple.Common.Forms.Html.Helpers;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.Validation;
using RobinEpple.Common.Html.Components;
using static RobinEpple.Common.Html.DSL;

/// <summary>
/// Renders an input label for the node. The input is expected to have the name <see cref="IFormNode.GetId()"/>
/// </summary>
public class InputLabel : Label
{
	/// <summary>
	/// Renders an input label for the <paramref name="node"/>. The input is expected to have the name <see cref="IFormNode.GetId()"/>
	/// </summary>
	/// <param name="node">The node to render the input label for.</param>
	public InputLabel(IFormNode node)
		: base(node.Label)
	{
		var nodeId = node.GetId();
		For(nodeId);
		this.Class("input-label");
		this.ConfigureIf(HasRequiredValidator(node), label => label.Class("required"));
	}

	private bool HasRequiredValidator(IFormNode node)
	{
		switch (node)
		{
			case IBooleanNode typed:
			{
				return typed.NodeValidators.Any(validator => validator is RequireTrueValidator);
			}
			case IFileNode typed:
			{
				return typed.NodeValidators.Any(validator => validator is FileRequiredValidator);
			}
			case ITextNode typed:
			{
				return typed.NodeValidators.Any(validator => validator is TextRequiredValidator);
			}
			case INumberNode typed:
			{
				return typed.NodeValidators.Any(validator => validator is NumberRequiredValidator);
			}
			case ITemplateNode typed:
			{
				return typed.NodeValidators.Any(validator => validator is TemplateRequiredValidator);
			}
			case ITimestampNode typed:
			{
				return typed.NodeValidators.Any(validator => validator is TimestampRequiredValidator);
			}
			case ICollectionNode typed:
			{
				return typed.NodeValidators.Any(validator => validator is MinCountValidator);
			}
			default:
			{
				return false;
			}
		}
	}
}
