namespace RobinEpple.Common.WebUi.DSL;

using Microsoft.AspNetCore.Html;
using RobinEpple.Common.Html.Components;
using static RobinEpple.Common.Html.DSL;
using static RobinEpple.Common.WebUi.DSL.EmbeddedContent;

/// <summary>
/// Provides rendering functions for the html tags defined in
/// https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements#image_and_multimedia
/// as well as
/// https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements#svg_and_mathml
/// </summary>
public static class Multimedia
{
	/// <summary>
	/// Defines an area inside an image map that has predefined clickable areas. An image map allows geometric areas on an image to be associated with hyperlink.
	/// </summary>
	public static HtmlTag Area(string shape, string coords, string href, string alt) =>
		new HtmlTag("area", true)
			.Attribute("shape", shape)
			.Attribute("coords", coords)
			.Attribute("href", href)
			.Attribute("alt", alt);

	/// <summary>
	/// Used to embed sound content in documents. It may contain one or more audio sources, represented using the src attribute or the source element: the browser will choose the most suitable one. It can also be the destination for streamed media, using a MediaStream.
	/// </summary>
	public static HtmlTag Audio(
		string src,
		string type,
		bool autoplay = false,
		params IEnumerable<IHtmlContent> additionalContent
	) =>
		new HtmlTag("area", false, Concat(Source(src, type), Concat(additionalContent))).Attribute(
			autoplay ? "autoplay" : "controls",
			"true"
		);

	/// <summary>
	/// Embeds an image into the document.
	/// </summary>
	public static HtmlTag Img(string src) => new HtmlTag("img", true).Attribute("src", src);

	/// <inheritdoc cref="Img(string)"/>
	public static HtmlTag Img(string src, string alt) =>
		new HtmlTag("img", true).Attribute("src", src).Attribute("alt", alt);

	/// <summary>
	/// Used with <area> elements to define an image map (a clickable link area).
	/// </summary>
	public static HtmlTag Map(IHtmlContent content) => new HtmlTag("map", false, content);

	/// <inheritdoc cref="Map(IHtmlContent)"/>
	public static HtmlTag Map(params IEnumerable<IHtmlContent> items) => new HtmlTag("map", false, Concat(items));

	/// <summary>
	/// Used as a child of the media elements, audio and video. It lets you specify timed text tracks (or time-based data), for example to automatically handle subtitles. The tracks are formatted in WebVTT format (.vtt files)—Web Video Text Tracks.
	/// </summary>
	public static HtmlTag Track(bool @default, string kind, string srclang, string src, string label)
	{
		var tag = new HtmlTag("track", true)
			.Attribute("kind", kind)
			.Attribute("srclang", srclang)
			.Attribute("src", src)
			.Attribute("label", label);

		if (@default)
		{
			tag = tag.Attribute("default", "true");
		}

		return tag;
	}

	/// <summary>
	/// Embeds a media player which supports video playback into the document. You can also use <video> for audio content, but the audio element may provide a more appropriate user experience.
	/// </summary>
	public static HtmlTag Video(
		string src,
		string type,
		bool autoplay = false,
		params IEnumerable<IHtmlContent> additionalContent
	) =>
		new HtmlTag("video", false, Concat(Source(src, type), Concat(additionalContent))).Attribute(
			autoplay ? "autoplay" : "controls",
			"true"
		);

	/// <summary>
	/// Container defining a new coordinate system and viewport. It is used as the outermost element of SVG documents, but it can also be used to embed an SVG fragment inside an SVG or HTML document.
	/// </summary>
	public static HtmlTag Svg(IHtmlContent content) => new HtmlTag("svg", false, content);

	/// <summary>
	/// The top-level element in MathML. Every valid MathML instance must be wrapped in it. In addition, you must not nest a second <math> element in another, but you can have an arbitrary number of other child elements in it.
	/// </summary>
	public static HtmlTag Math(IHtmlContent content) => new HtmlTag("math", false, content);
}
