namespace RobinEpple.Common.Html.Components;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;

/// <summary>
/// A helper record to html encode text.
/// </summary>
/// <param name="Text"></param>
public record class Encode(string Text) : IHtmlContent
{
	public void WriteTo(TextWriter writer, HtmlEncoder encoder)
	{
		var builder = new HtmlContentBuilder();
		builder.Append(Text);
		builder.WriteTo(writer, encoder);
	}
}
