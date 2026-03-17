namespace RobinEpple.Common.Forms.Html.Inputs;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using RobinEpple.Common.Forms.Nodes;
using static RobinEpple.Common.Html.DSL;

/// <summary>
/// Renders a data-list with options for each select list item defined in the <paramref name="node"/>.
/// The list will be rendered with the ID "{nodeId}_list".
/// </summary>
/// <typeparam name="TValue">The value type of the node.</typeparam>
/// <param name="node">The node to render the data-list for.</param>
public class DataItems<TValue>(IValueNode<TValue> node) : IHtmlContent
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
		if (_node.CurrentSelectListItems == null)
		{
			throw new InvalidOperationException($"Node with id '{nodeId}' does not have select list items defined.");
		}

		// DataList --------------------------------|
		// | Option 1	    						|
		// | Option 2   							|
		// |----------------------------------------|
		var content = Datalist(
				RenderEach(
					_node.CurrentSelectListItems,
					(item, index) => Div(Option(item.Label).Value(_node.Formatter.Format(item.Value) ?? string.Empty))
				)
			)
			.Id($"{nodeId}_list");
		content.WriteTo(writer, encoder);
	}
}
