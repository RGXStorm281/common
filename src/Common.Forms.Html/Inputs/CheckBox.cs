namespace RobinEpple.Common.Forms.Html.Inputs;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using RobinEpple.Common.Forms.Nodes;
using static RobinEpple.Common.Forms.Html.FormRendering;
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
		var content = InputFieldset(
				_node,
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
						InputLabel(_node)
					)
					.Class("checkbox-group"),
				ValidationErrors(_node)
			)
			.Class("checkbox");
		content.WriteTo(writer, encoder);
	}
}
