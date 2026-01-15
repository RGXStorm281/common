namespace RobinEpple.Common.Html.Components;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;

/// <summary>
/// A helper class to print raw, unencoded text.
/// </summary>
public partial class Raw(string text) : IHtmlContent
{
	private string _text { get; } = text;

	/// <summary>
	/// Writes the html text to the writer.
	/// </summary>
	public void WriteTo(TextWriter writer, HtmlEncoder encoder)
	{
		var builder = new HtmlContentBuilder();
		builder.AppendHtml(_text);
		builder.WriteTo(writer, encoder);
	}
}
