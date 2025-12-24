namespace RobinEpple.Common.WebUi.DSL;

using Microsoft.AspNetCore.Html;
using RobinEpple.Common.Html.Components;
using static RobinEpple.Common.Html.DSL;

/// <summary>
/// Provides rendering functions for the html tags defined in
/// https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements#text_content
/// </summary>
public static class TextContent
{
	/// <summary>
	/// Indicates that the enclosed text is an extended quotation. Usually, this is rendered visually by indentation. A URL for the source of the quotation may be given using the cite attribute, while a text representation of the source can be given using the <cite> element.
	/// </summary>
	public static HtmlTag BlockQuote(IHtmlContent content) => new HtmlTag("blockquote", false, content);

	/// <inheritdoc cref="BlockQuote(IHtmlContent)"/>
	public static HtmlTag BlockQuote(string text) => BlockQuote(Encode(text));

	/// <summary>
	/// Provides the description, definition, or value for the preceding term (<dt>) in a description list (<dl>).
	/// </summary>
	public static HtmlTag Dd(IHtmlContent content) => new HtmlTag("dd", false, content);

	/// <inheritdoc cref="Dd(IHtmlContent)"/>
	public static HtmlTag Dd(string text) => Dd(Encode(text));

	/// <summary>
	/// The generic container for flow content. It has no effect on the content or layout until styled in some way using CSS (e.g., styling is directly applied to it, or some kind of layout model like flexbox is applied to its parent element).
	/// </summary>
	public static HtmlTag Div(IHtmlContent content) => new HtmlTag("div", false, content);

	/// <summary>
	/// Renders a <see cref="Div(IHtmlContent)"/> with all items concatenated as content.
	/// </summary>
	public static HtmlTag Div(params IEnumerable<IHtmlContent> items) => Div(Concat(items));

	/// <summary>
	/// Represents a description list. The element encloses a list of groups of terms (specified using the <dt> element) and descriptions (provided by <dd> elements). Common uses for this element are to implement a glossary or to display metadata (a list of key-value pairs).
	/// </summary>
	public static HtmlTag Dl(IHtmlContent content) => new HtmlTag("dl", false, content);

	/// <summary>
	/// Renders a <see cref="Dl(IHtmlContent)"/> with a <see cref="Dt(string)"/> and <see cref="Dd(string)"/> for each element.
	/// </summary>
	public static HtmlTag Dl(params IEnumerable<KeyValuePair<string, string>> items) =>
		Dl(Concat(items.Select(item => Concat(Dt(item.Key), Dd(item.Value)))));

	/// <summary>
	/// Specifies a term in a description or definition list, and as such must be used inside a <dl> element. It is usually followed by a <dd> element; however, multiple <dt> elements in a row indicate several terms that are all defined by the immediate next <dd> element.
	/// </summary>
	public static HtmlTag Dt(IHtmlContent content) => new HtmlTag("dt", false, content);

	/// <inheritdoc cref="Dt(IHtmlContent)"/>
	public static HtmlTag Dt(string text) => Dt(Encode(text));

	/// <summary>
	/// Represents a caption or legend describing the rest of the contents of its parent <figure> element.
	/// </summary>
	public static HtmlTag FigCaption(IHtmlContent content) => new HtmlTag("figcaption", false, content);

	/// <inheritdoc cref="FigCaption(IHtmlContent)"/>
	public static HtmlTag FigCaption(string text) => FigCaption(Encode(text));

	/// <summary>
	/// Represents self-contained content, potentially with an optional caption, which is specified using the <figcaption> element. The figure, its caption, and its contents are referenced as a single unit.
	/// </summary>
	public static HtmlTag Figure(IHtmlContent content) => new HtmlTag("figure", false, content);

	/// <summary>
	/// Renders a <see cref="Figure(IHtmlContent)"/> with a <see cref="FigCaption(string)"/> .
	/// </summary>
	public static HtmlTag Figure(IHtmlContent content, string caption) => Figure(Concat(content, FigCaption(caption)));

	/// <summary>
	/// Represents a thematic break between paragraph-level elements: for example, a change of scene in a story, or a shift of topic within a section.
	/// </summary>
	public static HtmlTag Hr() => new HtmlTag("hr", true);

	/// <summary>
	/// Represents an item in a list. It must be contained in a parent element: an ordered list (<ol>), an unordered list (<ul>), or a menu (<menu>). In menus and unordered lists, list items are usually displayed using bullet points. In ordered lists, they are usually displayed with an ascending counter on the left, such as a number or letter.
	/// </summary>
	public static HtmlTag Li(IHtmlContent content) => new HtmlTag("li", false, content);

	/// <inheritdoc cref="Li(IHtmlContent)"/>
	public static HtmlTag Li(string text) => Li(Encode(text));

	/// <summary>
	/// A semantic alternative to <ul>, but treated by browsers (and exposed through the accessibility tree) as no different than <ul>. It represents an unordered list of items (which are represented by <li> elements).
	/// </summary>
	public static HtmlTag Menu(IHtmlContent content) => new HtmlTag("menu", false, content);

	/// <summary>
	/// Renders an <see cref="Menu(IHtmlContent)"/> with a <see cref="Li(IHtmlContent)"/> enclosing each item.
	/// </summary>
	public static HtmlTag Menu(params IEnumerable<IHtmlContent> items) => Menu(Concat(items.Select(Li)));

	/// <summary>
	/// Renders an <see cref="Menu(IHtmlContent)"/> with a <see cref="Li(IHtmlContent)"/> enclosing each item.
	/// </summary>
	public static HtmlTag Menu(params IEnumerable<string> items) => Menu(Concat(items.Select(Li)));

	/// <summary>
	/// Represents an ordered list of items — typically rendered as a numbered list.
	/// </summary>
	public static HtmlTag Ol(IHtmlContent content) => new HtmlTag("ol", false, content);

	/// <summary>
	/// Renders an <see cref="Ol(IHtmlContent)"/> with a <see cref="Li(IHtmlContent)"/> enclosing each item.
	/// </summary>
	public static HtmlTag Ol(params IEnumerable<IHtmlContent> items) => Ol(Concat(items.Select(Li)));

	/// <summary>
	/// Renders an <see cref="Ol(IHtmlContent)"/> with a <see cref="Li(IHtmlContent)"/> enclosing each item.
	/// </summary>
	public static HtmlTag Ol(params IEnumerable<string> items) => Ol(Concat(items.Select(Li)));

	/// <summary>
	/// Represents a paragraph. Paragraphs are usually represented in visual media as blocks of text separated from adjacent blocks by blank lines and/or first-line indentation, but HTML paragraphs can be any structural grouping of related content, such as images or form fields.
	/// </summary>
	public static HtmlTag P(IHtmlContent content) => new HtmlTag("p", false, content);

	/// <inheritdoc cref="P(IHtmlContent)"/>
	public static HtmlTag P(string text) => P(Encode(text));

	/// <summary>
	/// Represents preformatted text which is to be presented exactly as written in the HTML file. The text is typically rendered using a non-proportional, or monospaced, font. Whitespace inside this element is displayed as written.
	/// </summary>
	public static HtmlTag Pre(IHtmlContent content) => new HtmlTag("p", false, content);

	/// <inheritdoc cref="Pre(IHtmlContent)"/>
	public static HtmlTag Pre(string text) => Pre(Raw(text));

	/// <summary>
	/// Represents an unordered list of items, typically rendered as a bulleted list.
	/// </summary>
	public static HtmlTag Ul(IHtmlContent content) => new HtmlTag("p", false, content);

	/// <summary>
	/// Renders an <see cref="Ul(IHtmlContent)"/> with a <see cref="Li(IHtmlContent)"/> enclosing each item.
	/// </summary>
	public static HtmlTag Ul(params IEnumerable<IHtmlContent> items) => Ul(Concat(items.Select(Li)));

	/// <summary>
	/// Renders an <see cref="Ul(IHtmlContent)"/> with a <see cref="Li(IHtmlContent)"/> enclosing each item.
	/// </summary>
	public static HtmlTag Ul(params IEnumerable<string> items) => Ul(Concat(items.Select(Li)));
}
