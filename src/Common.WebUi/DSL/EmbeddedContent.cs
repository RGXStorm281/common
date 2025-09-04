namespace RobinEpple.Common.WebUi.DSL;

using Microsoft.AspNetCore.Html;
using RobinEpple.Common.WebUi.Components;

/// <summary>
/// Provides rendering functions for the html tags defined in
/// https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements#embedded_content
/// </summary>
public static class EmbeddedContent
{
	/// <summary>
	/// Embeds external content at the specified point in the document. This content is provided by an external application or other source of interactive content such as a browser plug-in.
	/// </summary>
	public static HtmlTag Embed(string src, string type) =>
		new HtmlTag("embed", true).Attribute("src", src).Attribute("type", type);

	/// <summary>
	/// Represents a nested browsing context, like <iframe> but with more native privacy features built in.
	/// </summary>
	public static HtmlTag FencedFrame(IHtmlContent content) => new HtmlTag("fencedframe", false, content);

	/// <summary>
	/// Represents a nested browsing context, embedding another HTML page into the current one.
	/// </summary>
	public static HtmlTag IFrame(string src, IHtmlContent content) =>
		new HtmlTag("iframe", false, content).Attribute("src", src);

	/// <summary>
	/// Contains zero or more <source> elements and one <img> element to offer alternative versions of an image for different display/device scenarios.
	/// </summary>
	public static HtmlTag Picture(IHtmlContent content) => new HtmlTag("picture", false, content);

	/// <summary>
	/// Specifies multiple media resources for the picture, the audio element, or the video element. It is a void element, meaning that it has no content and does not have a closing tag. It is commonly used to offer the same media content in multiple file formats in order to provide compatibility with a broad range of browsers given their differing support for image file formats and media file formats.
	/// </summary>
	public static HtmlTag Source(string src, string type) =>
		new HtmlTag("src", true).Attribute("src", src).Attribute("type", type);
}
