namespace RobinEpple.Common.WebUi.Components;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;

public record class Raw(string Text) : IHtmlContent
{
	public void WriteTo(TextWriter writer, HtmlEncoder encoder)
	{
		var builder = new HtmlContentBuilder();
		builder.AppendHtml(Text);
		builder.WriteTo(writer, encoder);
	}
}
