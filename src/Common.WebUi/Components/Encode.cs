namespace RobinEpple.Common.WebUi.Components;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;

public record class Encode(string Text) : IHtmlContent
{
	public void WriteTo(TextWriter writer, HtmlEncoder encoder)
	{
		var builder = new HtmlContentBuilder();
		builder.Append(Text);
		builder.WriteTo(writer, encoder);
	}
}
