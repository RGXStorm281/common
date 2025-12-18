namespace RobinEpple.Common.Html.Components;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;

/// <summary>
/// A helper record to concatenate a list of html contents.
/// </summary>
public record class Concat : IHtmlContent
{
	private readonly IEnumerable<IHtmlContent> _parts;

	public Concat(IEnumerable<IHtmlContent> parts)
	{
		_parts = parts;
	}

	public void WriteTo(TextWriter writer, HtmlEncoder encoder)
	{
		foreach (var part in _parts)
		{
			part.WriteTo(writer, encoder);
		}
	}
}
