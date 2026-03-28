namespace RobinEpple.Common.Forms.Html.TemplatedNodes;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using RobinEpple.Common.Forms.Nodes;
using static RobinEpple.Common.Forms.Html.FormRendering;
using static RobinEpple.Common.Html.DSL;

/// <summary>
/// Renders a select-tag with options for each template defined in the <paramref name="node"/>.
/// </summary>
/// <param name="node">The node to render the drop down for.</param>
public class DropDownForTemplates(ITemplateNode node) : IHtmlContent
{
	private readonly ITemplateNode _node = node;

	/// <inheritdoc />
	public void WriteTo(TextWriter writer, HtmlEncoder encoder)
	{
		var nodeId = _node.GetId();

		if (!_node.IsVisible)
		{
			return;
		}

		// Fieldset --------------------------------|
		// | Label       							|
		// | Select     							|
		// |    Option 1							|
		// |    Option 2							|
		// | Error A								|
		// | Error B								|
		// |----------------------------------------|
		var content = Fieldset(
				InputLabel(_node),
				Select(
						RenderEach(
							_node.Templates,
							(template, index) =>
								Option(template.Label)
									.Value(template.Name)
									.ConfigureIf(
										Equals(_node.Instance?.Name, template.Name),
										input => input.Selected("selected")
									)
						)
					)
					.Name(nodeId)
					.ConfigureIf(_node.IsReadonly, input => input.Disabled("disabled"))
					.Id(nodeId),
				ValidationErrors(_node)
			)
			.Class("drop-down")
			.Id($"{nodeId}_container");

		content.WriteTo(writer, encoder);
	}
}
