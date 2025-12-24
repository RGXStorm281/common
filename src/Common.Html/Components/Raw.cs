namespace RobinEpple.Common.Html.Components;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;

/// <summary>
/// A helper class to print raw, unencoded text.
/// </summary>
public partial class Raw(string text) : IHtmlContent
{
	public string Text { get; } = text;

	public void WriteTo(TextWriter writer, HtmlEncoder encoder)
	{
		var builder = new HtmlContentBuilder();
		builder.AppendHtml(Text);
		builder.WriteTo(writer, encoder);
	}
}
