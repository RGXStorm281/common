namespace RobinEpple.Common.Html.Components;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;

/// <summary>
/// Evaluates the content builder function every time new at rendering time.
/// </summary>
/// <param name="getContent">The content builder function.</param>
public class Lazy(Func<IHtmlContent> getContent) : IHtmlContent
{
	private readonly Func<IHtmlContent> _getContent = getContent;

	/// <summary>
	/// Writes the html text to the writer.
	/// </summary>
	public void WriteTo(TextWriter writer, HtmlEncoder encoder)
	{
		var currentContent = _getContent();
		currentContent.WriteTo(writer, encoder);
	}
}
