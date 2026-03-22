namespace RobinEpple.Common.Forms.Html.TemplatedNodes;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using RobinEpple.Common.Forms.Nodes;
using static RobinEpple.Common.Html.DSL;

/// <summary>
/// Renders a radio button for each template defined in the <paramref name="node"/>.
/// </summary>
/// <param name="node">The node to render the radio buttons for.</param>
public class RadioButtonsForTemplates(ITemplateNode node) : IHtmlContent
{
	private readonly ITemplateNode _node = node;

	/// <summary>
	/// Computes the ID used for the nth radio button.
	/// </summary>
	/// <param name="nodeId">The id of the node.</param>
	/// <param name="index">The index n of the option.</param>
	/// <returns>The ID of the radio button.</returns>
	private static string GetId(string nodeId, int index) => $"{nodeId}_{index}";

	/// <inheritdoc />
	public void WriteTo(TextWriter writer, HtmlEncoder encoder)
	{
		var nodeId = _node.GetId();

		if (!_node.IsVisible)
		{
			return;
		}

		// Fieldset --------------------------------|
		// | ( ) Option 1							|
		// | ( ) Option 2							|
		// | Error A								|
		// | Error B								|
		// |----------------------------------------|
		var content = Fieldset(
				Label(_node.Label).For(nodeId).Class("input-label"),
				RenderEach(
					_node.Templates,
					(template, index) =>
						Div(
								Input()
									.Type("radio")
									.Name(nodeId)
									.Id(GetId(nodeId, index))
									.Value(template.Name)
									.ConfigureIf(
										Equals(_node.Instance?.Name, template.Name),
										input => input.Checked("checked")
									)
									.ConfigureIf(_node.IsReadonly, input => input.Disabled("disabled")),
								Label(template.Label).For(GetId(nodeId, index))
							)
							.Class("radio-option")
							.Id($"{GetId(nodeId, index)}_option")
				),
				RenderEach(_node.ValidationErrorsByKey.Values, error => Span(error).Class("error"))
			)
			.Class("radio-buttons")
			.Id($"{nodeId}_container");

		content.WriteTo(writer, encoder);
	}
}
