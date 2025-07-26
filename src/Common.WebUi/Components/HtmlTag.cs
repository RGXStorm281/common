namespace RobinEpple.Common.WebUi.Components;

using System.Collections.Immutable;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;

/// <summary>
/// A record representing an html tag.
/// </summary>
public record class HtmlTag : IHtmlContent
{
	public HtmlTag(string tag, bool selfClosing)
		: this(tag, selfClosing, new HtmlString(string.Empty), [], []) { }

	public HtmlTag(string tag, bool selfClosing, IHtmlContent content)
		: this(tag, selfClosing, content, [], []) { }

	public HtmlTag(
		string tag,
		bool selfClosing,
		IHtmlContent content,
		IEnumerable<string> classes,
		IEnumerable<KeyValuePair<string, string>> attributes
	)
	{
		Tag = tag;
		SelfClosing = selfClosing;
		Content = content;
		Classes = classes.ToImmutableHashSet();
		Attributes = attributes.ToImmutableDictionary();
	}

	/// <summary>
	/// The name of the tag.
	/// </summary>
	public string Tag { get; }

	/// <summary>
	/// Whether the tag is self closing. If set to <see langword="true"/> the <see cref="Content"/> is ignored.
	/// </summary>
	public bool SelfClosing { get; }

	/// <summary>
	/// The content to render within the tag.
	/// </summary>
	public IHtmlContent Content { get; set; }

	/// <summary>
	/// The css classes to render in the tag.
	/// </summary>
	public IImmutableSet<string> Classes { get; set; }

	/// <summary>
	/// Additional attributes to render in the tag.
	/// </summary>
	public IImmutableDictionary<string, string> Attributes { get; set; }

	public void WriteTo(TextWriter writer, HtmlEncoder encoder)
	{
		var builder = new HtmlContentBuilder();

		var classes = Classes.Count > 0 ? $"class=\"{string.Join(" ", Classes)}\"" : string.Empty;
		var attributeList = Attributes.Select(attribute => $"{attribute.Key}=\"{attribute.Value}\"");
		var attributes = string.Join(" ", attributeList);

		if (SelfClosing)
		{
			builder.AppendHtml($"<{Tag} {classes} {attributes} />");
		}
		else
		{
			builder.AppendHtml($"<{Tag} {classes} {attributes}>");
			builder.AppendHtml(Content);
			builder.AppendHtml($"</{Tag}>");
		}
		builder.WriteTo(writer, encoder);
	}
}
