namespace RobinEpple.Common.Html.Components;

using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;

public class RenderImperative(Action<SequentialHtmlContentBuilder> render) : IHtmlContent
{
	private readonly Action<SequentialHtmlContentBuilder> _configureSequence = render;

	public void WriteTo(TextWriter writer, HtmlEncoder encoder)
	{
		var builder = new SequentialHtmlContentBuilder();
		_configureSequence(builder);
		var content = builder.GetResult();
		content.WriteTo(writer, encoder);
	}
}
