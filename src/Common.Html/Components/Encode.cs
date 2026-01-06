namespace RobinEpple.Common.Html.Components;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;

/// <summary>
/// A helper class to html encode text.
/// </summary>
public partial class Encode(string text) : IHtmlContent
{
	public string Text { get; } = text;

	public void WriteTo(TextWriter writer, HtmlEncoder encoder)
	{
		var builder = new HtmlContentBuilder();
		builder.Append(Text);
		builder.WriteTo(writer, encoder);
	}
}
