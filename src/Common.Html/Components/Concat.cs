namespace RobinEpple.Common.Html.Components;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;

/// <summary>
/// A helper class to concatenate a list of html contents.
/// </summary>
public partial class Concat : IHtmlContent
{
	private readonly IEnumerable<IHtmlContent> _parts;

	public Concat(params IEnumerable<IHtmlContent> parts)
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
