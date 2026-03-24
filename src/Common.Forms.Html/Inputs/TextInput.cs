namespace RobinEpple.Common.Forms.Html.Inputs;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using RobinEpple.Common.Forms.Nodes;
using static RobinEpple.Common.Forms.Html.FormRendering;
using static RobinEpple.Common.Html.DSL;

/// <summary>
/// Renders a text input for the <paramref name="node"/>.
/// The formatter is used to convert between text and value representations.
/// </summary>
/// <typeparam name="TValue">The value type of the node.</typeparam>
/// <param name="node">The node to render the text input for.</param>
public class TextInput<TValue>(IValueNode<TValue> node) : IHtmlContent
{
	private readonly IValueNode<TValue> _node = node;

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
					.Type("text")
					.Name(nodeId)
					.ConfigureIf(_node.IsReadonly, input => input.Disabled("disabled"))
					.ConfigureIf(_node.CurrentSelectListItems != null, input => input.List($"{nodeId}_list"))
					.Value(_node.Formatter.Format(_node.Value) ?? string.Empty)
					.Id(nodeId),
				RenderIf(_node.CurrentSelectListItems != null, DataItems(_node)),
				ValidationErrors(_node)
			)
			.Class("text-input");
		content.WriteTo(writer, encoder);
	}
}
