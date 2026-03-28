namespace RobinEpple.Common.Forms.Html.Inputs;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using RobinEpple.Common.Forms.Nodes;
using static RobinEpple.Common.Html.DSL;

/// <summary>
/// Renders a hidden input for the <paramref name="node"/>.
/// </summary>
/// <typeparam name="TValue">The value type of the node.</typeparam>
/// <param name="node">The node to render the hidden input for.</param>
public class HiddenInput<TValue>(IValueNode<TValue> node) : IHtmlContent
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
		// | Input							        |
		// |----------------------------------------|
		var content = Input()
			.Type("hidden")
			.Name(nodeId)
			.Id(nodeId)
			.Value(_node.Formatter.Format(_node.Value) ?? string.Empty);
		content.WriteTo(writer, encoder);
	}
}
