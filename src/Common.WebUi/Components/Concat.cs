namespace RobinEpple.Common.WebUi.Components;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;

public record class Concat : IHtmlContent
{
	private readonly IHtmlContent[] _parts;

	public Concat(IHtmlContent[] parts)
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
