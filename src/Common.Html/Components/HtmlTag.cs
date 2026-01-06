namespace RobinEpple.Common.Html.Components;

using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using static RobinEpple.Common.Html.DSL;

/// <summary>
/// A class representing an html tag.
/// </summary>
public partial class HtmlTag : IHtmlContent
{
	public HtmlTag(string tag, bool selfClosing)
		: this(tag, selfClosing, new HtmlString(string.Empty), [], [], []) { }

	public HtmlTag(string tag, bool selfClosing, IHtmlContent content)
		: this(tag, selfClosing, content, [], [], []) { }

	public HtmlTag(
		string tag,
		bool selfClosing,
		IHtmlContent content,
		IEnumerable<string> classes,
		IEnumerable<KeyValuePair<string, string>> attributes,
		IEnumerable<KeyValuePair<string, string>> styles
	)
	{
		Tag = tag;
		SelfClosing = selfClosing;
		InnerContent = content;
		Classes = classes.ToList();
		Attributes = attributes.ToDictionary();
		Styles = styles.ToDictionary();
	}

	public HtmlTag(string tag, bool selfClosing, params IEnumerable<IHtmlContent> contents)
		: this(tag, selfClosing, Concat(contents), [], [], []) { }

	public HtmlTag(string tag, bool selfClosing, string text)
		: this(tag, selfClosing, Encode(text), [], [], []) { }

	/// <summary>
	/// The name of the tag.
	/// </summary>
	public string Tag { get; }

	/// <summary>
	/// Whether the tag is self closing. If set to <see langword="true"/> the <see cref="InnerContent"/> is ignored.
	/// </summary>
	public bool SelfClosing { get; }

	/// <summary>
	/// The content to render within the tag.
	/// </summary>
	public IHtmlContent InnerContent { get; set; }

	/// <summary>
	/// The css classes to render in the tag.
	/// </summary>
	public IList<string> Classes { get; set; }

	/// <summary>
	/// Contents of the style attribute.
	/// </summary>
	public IDictionary<string, string> Styles { get; set; }

	/// <summary>
	/// Additional attributes to render in the tag.
	/// </summary>
	public IDictionary<string, string> Attributes { get; set; }

	public void WriteTo(TextWriter writer, HtmlEncoder encoder)
	{
		var builder = new HtmlContentBuilder();

		var styles =
			Styles.Count > 0
				? $"style=\"{string.Join(" ", Styles.Select(style => $"{style.Key}: {style.Value};"))}\""
				: string.Empty;
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
			builder.AppendHtml(InnerContent);
			builder.AppendHtml($"</{Tag}>");
		}
		builder.WriteTo(writer, encoder);
	}
}
