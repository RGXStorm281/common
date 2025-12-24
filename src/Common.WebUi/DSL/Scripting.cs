namespace RobinEpple.Common.WebUi.DSL;

using Microsoft.AspNetCore.Html;
using RobinEpple.Common.Html.Components;
using static RobinEpple.Common.Html.DSL;

/// <summary>
/// Provides rendering functions for the html tags defined in
/// https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements#scripting
/// </summary>
public static class Scripting
{
	/// <summary>
	/// Container element to use with either the canvas scripting API or the WebGL API to draw graphics and animations.
	/// </summary>
	public static HtmlTag Canvas(IHtmlContent content) => new HtmlTag("canvas", false, content);

	/// <summary>
	/// Defines a section of HTML to be inserted if a script type on the page is unsupported or if scripting is currently turned off in the browser.
	/// </summary>
	public static HtmlTag NoScript(IHtmlContent content) => new HtmlTag("noscript", false, content);

	/// <summary>
	/// Used to embed executable code or data; this is typically used to embed or refer to JavaScript code. The <script> element can also be used with other languages, such as WebGL's GLSL shader programming language and JSON.
	/// </summary>
	public static HtmlTag Script(string src) => new HtmlTag("script", true).Attribute("src", src);
}
