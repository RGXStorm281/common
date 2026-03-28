namespace RobinEpple.Common.Html.Components;

using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;

/// <summary>
/// Allows the formulation of rendered content in imperative style, where content is appended to the builder in the sequence it should be rendered.
/// </summary>
/// <param name="render">The imperative rendering function.</param>
public class RenderSequential(Action<SequentialHtmlContentBuilder> render) : IHtmlContent
{
	private readonly Action<SequentialHtmlContentBuilder> _configureSequence = render;

	/// <summary>
	/// Writes the html text to the writer.
	/// </summary>
	public void WriteTo(TextWriter writer, HtmlEncoder encoder)
	{
		var builder = new SequentialHtmlContentBuilder();
		_configureSequence(builder);
		var content = builder.GetResult();
		content.WriteTo(writer, encoder);
	}
}
