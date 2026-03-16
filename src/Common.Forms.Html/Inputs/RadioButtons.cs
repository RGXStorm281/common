namespace RobinEpple.Common.Forms.Html.Inputs;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using RobinEpple.Common.Forms.Nodes;
using static RobinEpple.Common.Html.DSL;

/// <summary>
/// Renders a radio button for each select list item defined in the <paramref name="node"/>.
/// </summary>
/// <typeparam name="TValue">The value type of the node.</typeparam>
/// <param name="node">The node to render the radio buttons for.</param>
public class RadioButtons<TValue>(IValueNode<TValue> node) : IHtmlContent
{
	private readonly IValueNode<TValue> _node = node;

	private string GetId(int index) => $"{_node.Name}_{index}";

	/// <inheritdoc />
	public void WriteTo(TextWriter writer, HtmlEncoder encoder)
	{
		var content = Fieldset(
			RenderEach(
				_node.CurrentSelectListItems ?? [],
				(item, index) =>
					Div(
						Input()
							.Type("radio")
							.Name(_node.GetId())
							.Id(GetId(index))
							.Value(item.Value?.ToString() ?? string.Empty)
							.ConfigureIf(
								Equals(item.Value, _node.Value),
								input => input.Attribute("checked", "checked")
							),
						Label(item.Label).For(GetId(index))
					)
			)
		);
		content.WriteTo(writer, encoder);
	}
}
