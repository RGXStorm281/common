namespace RobinEpple.Common.Forms.Html.Inputs;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using RobinEpple.Common.Forms.Nodes;
using static RobinEpple.Common.Forms.Html.FormRendering;
using static RobinEpple.Common.Html.DSL;

/// <summary>
/// Renders a radio button for each select list item defined in the <paramref name="node"/>.
/// </summary>
/// <typeparam name="TValue">The value type of the node.</typeparam>
/// <param name="node">The node to render the radio buttons for.</param>
public class RadioButtons<TValue>(IValueNode<TValue> node) : IHtmlContent
{
	private readonly IValueNode<TValue> _node = node;

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
		if (_node.CurrentSelectListItems == null)
		{
			throw new InvalidOperationException($"Node with id '{nodeId}' does not have select list items defined.");
		}

		// Fieldset --------------------------------|
		// | ( ) Option 1							|
		// | ( ) Option 2							|
		// | Error A								|
		// | Error B								|
		// |----------------------------------------|
		var content = Fieldset(
				InputLabel(_node),
				RenderEach(
					_node.CurrentSelectListItems,
					(item, index) =>
						Div(
								Input()
									.Type("radio")
									.Name(nodeId)
									.Id(GetId(nodeId, index))
									.Value(_node.Formatter.Format(item.Value) ?? string.Empty)
									.ConfigureIf(Equals(item.Value, _node.Value), input => input.Checked("checked"))
									.ConfigureIf(_node.IsReadonly, input => input.Disabled("disabled")),
								Label(item.Label).For(GetId(nodeId, index))
							)
							.Class("radio-option")
							.Id($"{GetId(nodeId, index)}_option")
				),
				ValidationErrors(_node)
			)
			.Class("radio-buttons")
			.Id($"{nodeId}_container");
		content.WriteTo(writer, encoder);
	}
}
