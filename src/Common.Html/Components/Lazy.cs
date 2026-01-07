namespace RobinEpple.Common.Html.Components;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;

public class Lazy(Func<IHtmlContent> getContent) : IHtmlContent
{
	private readonly Func<IHtmlContent> _getContent = getContent;

	public void WriteTo(TextWriter writer, HtmlEncoder encoder)
	{
		var currentContent = _getContent();
		currentContent.WriteTo(writer, encoder);
	}
}
