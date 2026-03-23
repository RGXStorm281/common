namespace RobinEpple.Common.Forms.Html.Inputs;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using RobinEpple.Common.Forms.Nodes;
using static RobinEpple.Common.Html.DSL;

/// <summary>
/// Renders a list of validation errors for the <paramref name="node"/>, if the node has user interaction.
/// </summary>
/// <param name="node">The node to render the validation errors for.</param>
public class ValidationErrors(IFieldNode node) : IHtmlContent
{
	private readonly IFieldNode _node = node;

	/// <inheritdoc />
	public void WriteTo(TextWriter writer, HtmlEncoder encoder)
	{
		var nodeId = _node.GetId();

		if (!_node.IsVisible || !_node.HasUserInteraction)
		{
			return;
		}

		// |----------------------------------------|
		// | Error A								|
		// | Error B								|
		// |----------------------------------------|
		var content = RenderIf(
			_node.HasUserInteraction,
			RenderEach(_node.ValidationErrorsByKey.Values, error => Span(error).Class("error"))
		);
		content.WriteTo(writer, encoder);
	}
}
