namespace RobinEpple.Common.Forms.Html.Inputs;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.Validation;
using static RobinEpple.Common.Html.DSL;

/// <summary>
/// Renders an input label for the <paramref name="node"/>. The input is expected to have the name <see cref="IFormNode.GetId()"/>
/// </summary>
/// <param name="node">The node to render the input label for.</param>
public class InputLabel(IFormNode node) : IHtmlContent
{
	private readonly IFormNode _node = node;

	/// <inheritdoc />
	public void WriteTo(TextWriter writer, HtmlEncoder encoder)
	{
		var nodeId = _node.GetId();

		if (!_node.IsVisible)
		{
			return;
		}

		// |----------------------------------------|
		// | Label       							|
		// |----------------------------------------|
		var content = Label(_node.Label)
			.For(nodeId)
			.Class("input-label")
			.ConfigureIf(HasRequiredValidator(_node), label => label.Class("required"));
		content.WriteTo(writer, encoder);
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
