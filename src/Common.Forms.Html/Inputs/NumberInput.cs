namespace RobinEpple.Common.Forms.Html.Inputs;

using System.Globalization;
using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using RobinEpple.Common.Forms.Nodes;
using static RobinEpple.Common.Forms.Html.FormRendering;
using static RobinEpple.Common.Html.DSL;

/// <summary>
/// Renders a number input for the <paramref name="node"/>.
/// </summary>
/// <param name="node">The node to render the number input for.</param>
public class NumberInput(INumberNode node) : IHtmlContent
{
	private readonly INumberNode _node = node;

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
		var content = InputFieldset(
				_node,
				InputLabel(_node),
				Input()
					.Type("number")
					.Name(nodeId)
					.ConfigureIf(_node.IsReadonly, input => input.Disabled("disabled"))
					.ConfigureIf(_node.CurrentSelectListItems != null, input => input.List($"{nodeId}_list"))
					.Value(_node.Value?.ToString(CultureInfo.InvariantCulture) ?? string.Empty)
					.Id(nodeId),
				RenderIf(_node.CurrentSelectListItems != null, DataItems(_node)),
				ValidationErrors(_node)
			)
			.Class("number-input");
		content.WriteTo(writer, encoder);
	}
}
