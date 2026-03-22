namespace RobinEpple.Common.Forms.Html.Inputs;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using RobinEpple.Common.Forms.Nodes;
using static RobinEpple.Common.Html.DSL;

/// <summary>
/// Renders a checkbox for the <paramref name="node"/>.
/// </summary>
/// <param name="node">The node to render the checkbox for.</param>
public class CheckBox(IBooleanNode node) : IHtmlContent
{
	private readonly IBooleanNode _node = node;

	/// <inheritdoc />
	public void WriteTo(TextWriter writer, HtmlEncoder encoder)
	{
		var nodeId = _node.GetId();

		if (!_node.IsVisible)
		{
			return;
		}

		// Fieldset --------------------------------|
		// | Div ---------------------------------| |
		// | | Checkbox                           | |
		// | | Label                              | |
		// | |------------------------------------| |
		// | Error A								|
		// | Error B								|
		// |----------------------------------------|
		var content = Fieldset(
				Div(
						// Fallback hidden input, such that "false" is sent in the request when the checkbox is not selected.
						// By default, browsers will only send selected checkboxes as form value.
						Input().Type("hidden").Name(nodeId).Value(_node.Formatter.Format(false) ?? string.Empty),
						Input()
							.Type("checkbox")
							.Value(_node.Formatter.Format(true) ?? string.Empty)
							.ConfigureIf(_node.Value == true, input => input.Checked("checked"))
							.Name(nodeId)
							.ConfigureIf(_node.IsReadonly, input => input.Disabled("disabled"))
							.Id(nodeId),
						Label(_node.Label).For(nodeId)
					)
					.Class("checkbox-group"),
				RenderEach(_node.ValidationErrorsByKey.Values, error => Span(error).Class("error"))
			)
			.Class("checkbox")
			.Id($"{nodeId}_container");
		content.WriteTo(writer, encoder);
	}
}
