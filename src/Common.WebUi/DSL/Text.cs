namespace RobinEpple.Common.WebUi.DSL;

using Microsoft.AspNetCore.Html;
using RobinEpple.Common.WebUi.Components;
using static RobinEpple.Common.WebUi.DSL.Helper;

public static class Text
{
	public static HtmlTag H1(IHtmlContent content) => new HtmlTag("h1", false, content);

	public static HtmlTag H1(string text) => H1(Encode(text));

	public static HtmlTag H2(IHtmlContent content) => new HtmlTag("h2", false, content);

	public static HtmlTag H2(string text) => H2(Encode(text));

	public static HtmlTag H3(IHtmlContent content) => new HtmlTag("h3", false, content);

	public static HtmlTag H3(string text) => H3(Encode(text));

	public static HtmlTag H4(IHtmlContent content) => new HtmlTag("h4", false, content);

	public static HtmlTag H4(string text) => H4(Encode(text));

	public static HtmlTag H5(IHtmlContent content) => new HtmlTag("h5", false, content);

	public static HtmlTag H5(string text) => H5(Encode(text));

	public static HtmlTag H6(IHtmlContent content) => new HtmlTag("h6", false, content);

	public static HtmlTag H6(string text) => H6(Encode(text));

	public static HtmlTag Bold(IHtmlContent content) => new HtmlTag("b", false, content);

	public static HtmlTag Bold(string text) => Bold(Encode(text));

	public static HtmlTag Br() => new HtmlTag("br", true);
}
