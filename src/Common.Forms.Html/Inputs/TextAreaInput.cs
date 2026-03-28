namespace RobinEpple.Common.Forms.Html.Inputs;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using RobinEpple.Common.Forms.Nodes;
using static RobinEpple.Common.Forms.Html.FormRendering;
using static RobinEpple.Common.Html.DSL;

/// <summary>
/// Renders a text area for the <paramref name="node"/>.
/// </summary>
/// <param name="node">The node to render the text area for.</param>
public class TextAreaInput(ITextNode node) : IHtmlContent
{
	private readonly ITextNode _node = node;

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
		// | TextArea      							|
		// | Error A								|
		// | Error B								|
		// |----------------------------------------|
		var content = InputFieldset(
				_node,
				InputLabel(_node),
				Textarea(_node.Formatter.Format(_node.Value) ?? string.Empty)
					.Name(nodeId)
					.ConfigureIf(_node.IsReadonly, input => input.Disabled("disabled"))
					.Id(nodeId),
				ValidationErrors(_node)
			)
			.Class("text-area");
		content.WriteTo(writer, encoder);
	}
}
