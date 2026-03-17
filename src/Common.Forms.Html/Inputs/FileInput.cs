namespace RobinEpple.Common.Forms.Html.Inputs;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.Validation;
using static RobinEpple.Common.Html.DSL;

/// <summary>
/// Renders a file input for the <paramref name="node"/>.
/// </summary>
/// <param name="node">The node to render the file input for.</param>
public class FileInput(IFileNode node) : IHtmlContent
{
	private readonly IFileNode _node = node;

	/// <inheritdoc />
	public void WriteTo(TextWriter writer, HtmlEncoder encoder)
	{
		var nodeId = _node.GetId();

		if (!_node.IsVisible)
		{
			return;
		}

		var fileExtensionValidator = _node.NodeValidators.OfType<FileExtensionValidator>().FirstOrDefault();

		// Fieldset --------------------------------|
		// | Div ---------------------------------| |
		// | | Checkbox                           | |
		// | | Label                              | |
		// | |------------------------------------| |
		// | Error A								|
		// | Error B								|
		// |----------------------------------------|
		var content = Fieldset(
				Label(_node.Label).For(nodeId).Class("input-label"),
				RenderIf(
					fileExtensionValidator != null,
					Div(
							RenderEach(
								fileExtensionValidator!.AllowedExtensions,
								allowedExtension => Label(allowedExtension).For(nodeId)
							)
						)
						.Class("allowed-file-extensions")
				),
				Input()
					.Type("file")
					.Name(nodeId)
					.ConfigureIf(_node.IsReadonly, input => input.Disabled("disabled"))
					.ConfigureIf(
						fileExtensionValidator != null,
						input =>
							input.Accept(
								string.Join(",", fileExtensionValidator!.AllowedExtensions.Select(ext => $".{ext}"))
							)
					)
					.Id(nodeId),
				Label(_node.Value.FileName ?? string.Empty)
					.For(nodeId)
					.Class("file-name")
					.ConfigureIf(_node.IsReadonly, input => input.Class("disabled")),
				RenderEach(_node.ValidationErrorsByKey.Values, error => Span(error).Class("error"))
			)
			.Class("file-input")
			.Id($"{nodeId}_container");
		content.WriteTo(writer, encoder);
	}
}
