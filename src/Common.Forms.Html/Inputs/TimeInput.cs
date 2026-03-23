namespace RobinEpple.Common.Forms.Html.Inputs;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using RobinEpple.Common.Forms.Nodes;
using static RobinEpple.Common.Forms.Html.FormRendering;
using static RobinEpple.Common.Html.DSL;

/// <summary>
/// Renders a time input for the <paramref name="node"/>.
/// </summary>
/// <param name="node">The node to render the time input for.</param>
public class TimeInput(ITimestampNode node) : IHtmlContent
{
	private readonly ITimestampNode _node = node;

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
		// | Input       							|
		// | DataList   							|
		// | Error A								|
		// | Error B								|
		// |----------------------------------------|
		var content = Fieldset(
				InputLabel(_node),
				Input()
					.Type("time")
					.Name(nodeId)
					.ConfigureIf(_node.IsReadonly, input => input.Disabled("disabled"))
					.ConfigureIf(_node.CurrentSelectListItems != null, input => input.List($"{nodeId}_list"))
					.Value(_node.Value?.ToString("HH:mm") ?? string.Empty)
					.Id(nodeId),
				RenderIf(_node.CurrentSelectListItems != null, DataItems(_node)),
				ValidationErrors(_node)
			)
			.Class("time-input")
			.Id($"{nodeId}_container");
		content.WriteTo(writer, encoder);
	}
}
