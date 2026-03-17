namespace RobinEpple.Common.Forms.Html.Inputs;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.Validation;
using static RobinEpple.Common.Forms.Html.FormRendering;
using static RobinEpple.Common.Html.DSL;

/// <summary>
/// Renders a range slider for the <paramref name="node"/>.
/// </summary>
/// <param name="node">The node to render the range slider for.</param>
/// <param name="min">The lower bound for the range slider.</param>
/// <param name="max">The upper bound for the range slider.</param>
public class RangeInput(INumberNode node, decimal min, decimal max) : IHtmlContent
{
	private readonly INumberNode _node = node;

	private readonly decimal _min = min;
	private readonly decimal _max = max;

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
				Label(_node.Label).For(nodeId).Class("input-label"),
				Input()
					.Type("range")
					.Min(_node.Formatter.Format(_min) ?? string.Empty)
					.Max(_node.Formatter.Format(_max) ?? string.Empty)
					.Name(nodeId)
					.ConfigureIf(_node.IsReadonly, input => input.Disabled("disabled"))
					.ConfigureIf(_node.CurrentSelectListItems != null, input => input.List($"{nodeId}_list"))
					.Value(_node.Formatter.Format(_node.Value) ?? string.Empty)
					.Id(nodeId),
				RenderIf(_node.CurrentSelectListItems != null, DataItems(_node)),
				RenderEach(_node.ValidationErrorsByKey.Values, error => Span(error).Class("error"))
			)
			.Class("number-input")
			.Id($"{nodeId}_container");
		content.WriteTo(writer, encoder);
	}
}
