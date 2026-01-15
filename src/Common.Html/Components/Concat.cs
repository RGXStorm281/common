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

	/// <inheritdoc cref="Concat"/>
	/// <param name="parts"></param>
	public Concat(params IEnumerable<IHtmlContent> parts)
	{
		_parts = parts;
	}

	/// <summary>
	/// Writes the html text to the writer.
	/// </summary>
	public void WriteTo(TextWriter writer, HtmlEncoder encoder)
	{
		foreach (var part in _parts)
		{
			part.WriteTo(writer, encoder);
		}
	}
}
