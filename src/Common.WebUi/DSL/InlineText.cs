namespace RobinEpple.Common.WebUi.DSL;

using Microsoft.AspNetCore.Html;
using RobinEpple.Common.WebUi.Components;
using static RobinEpple.Common.WebUi.DSL.Helper;

/// <summary>
/// Provides rendering functions for the html tags defined in
/// https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements#inline_text_semantics
/// </summary>
public static class InlineText
{
	/// <summary>
	/// Together with its href attribute, creates a hyperlink to web pages, files, email addresses, locations within the current page, or anything else a URL can address.
	/// </summary>
	public static HtmlTag A(string href, IHtmlContent content) =>
		new HtmlTag("a", false, content).Attribute("href", href);

	/// <inheritdoc cref="A(string, IHtmlContent)"/>
	public static HtmlTag A(string href, string text) => A(href, Encode(text));

	/// <summary>
	/// Represents an abbreviation or acronym.
	/// </summary>
	public static HtmlTag Abbr(IHtmlContent content) => new HtmlTag("abbr", false, content);

	/// <inheritdoc cref="Abbr(IHtmlContent)"/>
	public static HtmlTag Abbr(string text) => Abbr(Encode(text));

	/// <summary>
	/// Used to draw the reader's attention to the element's contents, which are not otherwise granted special importance. This was formerly known as the Boldface element, and most browsers still draw the text in boldface. However, you should not use <b> for styling text or granting importance. If you wish to create boldface text, you should use the CSS font-weight property. If you wish to indicate an element is of special importance, you should use the <strong> element.
	/// </summary>
	public static HtmlTag B(IHtmlContent content) => new HtmlTag("b", false, content);

	/// <inheritdoc cref="B(IHtmlContent)"/>
	public static HtmlTag B(string text) => B(Encode(text));

	/// <summary>
	/// Tells the browser's bidirectional algorithm to treat the text it contains in isolation from its surrounding text. It's particularly useful when a website dynamically inserts some text and doesn't know the directionality of the text being inserted.
	/// </summary>
	public static HtmlTag Bdi(IHtmlContent content) => new HtmlTag("bdi", false, content);

	/// <inheritdoc cref="Bdi(IHtmlContent)"/>
	public static HtmlTag Bdi(string text) => Bdi(Encode(text));

	/// <summary>
	/// Overrides the current directionality of text, so that the text within is rendered in a different direction.
	/// </summary>
	public static HtmlTag Bdo(IHtmlContent content) => new HtmlTag("bdo", false, content);

	/// <inheritdoc cref="Bdo(IHtmlContent)"/>
	public static HtmlTag Bdo(string text) => Bdo(Encode(text));

	/// <summary>
	/// Produces a line break in text (carriage-return). It is useful for writing a poem or an address, where the division of lines is significant.
	/// </summary>
	public static HtmlTag Br() => new HtmlTag("br", true);

	/// <summary>
	/// Used to mark up the title of a creative work. The reference may be in an abbreviated form according to context-appropriate conventions related to citation metadata.
	/// </summary>
	public static HtmlTag Cite(IHtmlContent content) => new HtmlTag("cite", true, content);

	/// <inheritdoc cref="Cite(IHtmlContent)"/>
	public static HtmlTag Cite(string text) => Cite(Encode(text));

	/// <summary>
	/// Displays its contents styled in a fashion intended to indicate that the text is a short fragment of computer code. By default, the content text is displayed using the user agent's default monospace font.
	/// </summary>
	public static HtmlTag Code(IHtmlContent content) => new HtmlTag("code", true, content);

	/// <inheritdoc cref="Code(IHtmlContent)"/>
	public static HtmlTag Code(string text) => Code(Encode(text));

	/// <summary>
	/// Links a given piece of content with a machine-readable translation. If the content is time- or date-related, the <time> element must be used.
	/// </summary>
	public static HtmlTag Data(IHtmlContent content) => new HtmlTag("data", true, content);

	/// <inheritdoc cref="Data(IHtmlContent)"/>
	public static HtmlTag Data(string text) => Data(Encode(text));

	/// <summary>
	/// Used to indicate the term being defined within the context of a definition phrase or sentence. The ancestor <p> element, the <dt>/<dd> pairing, or the nearest section ancestor of the <dfn> element, is considered to be the definition of the term.
	/// </summary>
	public static HtmlTag Dfn(IHtmlContent content) => new HtmlTag("dfn", true, content);

	/// <inheritdoc cref="Dfn(IHtmlContent)"/>
	public static HtmlTag Dfn(string text) => Dfn(Encode(text));

	/// <summary>
	/// Marks text that has stress emphasis. The <em> element can be nested, with each nesting level indicating a greater degree of emphasis.
	/// </summary>
	public static HtmlTag Em(IHtmlContent content) => new HtmlTag("em", true, content);

	/// <inheritdoc cref="Em(IHtmlContent)"/>
	public static HtmlTag Em(string text) => Em(Encode(text));

	/// <summary>
	/// Represents a range of text that is set off from the normal text for some reason, such as idiomatic text, technical terms, and taxonomical designations, among others. Historically, these have been presented using italicized type, which is the original source of the <i> naming of this element.
	/// </summary>
	public static HtmlTag I(IHtmlContent content) => new HtmlTag("i", true, content);

	/// <inheritdoc cref="I(IHtmlContent)"/>
	public static HtmlTag I(string text) => I(Encode(text));

	/// <summary>
	/// Represents a span of inline text denoting textual user input from a keyboard, voice input, or any other text entry device. By convention, the user agent defaults to rendering the contents of a <kbd> element using its default monospace font, although this is not mandated by the HTML standard.
	/// </summary>
	public static HtmlTag Kbd(IHtmlContent content) => new HtmlTag("kbd", true, content);

	/// <inheritdoc cref="Kbd(IHtmlContent)"/>
	public static HtmlTag Kbd(string text) => Kbd(Encode(text));

	/// <summary>
	/// Represents text which is marked or highlighted for reference or notation purposes due to the marked passage's relevance in the enclosing context.
	/// </summary>
	public static HtmlTag Mark(IHtmlContent content) => new HtmlTag("mark", true, content);

	/// <inheritdoc cref="Mark(IHtmlContent)"/>
	public static HtmlTag Mark(string text) => Mark(Encode(text));

	/// <summary>
	/// Indicates that the enclosed text is a short inline quotation. Most modern browsers implement this by surrounding the text in quotation marks. This element is intended for short quotations that don't require paragraph breaks; for long quotations use the <blockquote> element.
	/// </summary>
	public static HtmlTag Q(IHtmlContent content) => new HtmlTag("q", true, content);

	/// <inheritdoc cref="Q(IHtmlContent)"/>
	public static HtmlTag Q(string text) => Q(Encode(text));

	/// <summary>
	/// Used to provide fall-back parentheses for browsers that do not support the display of ruby annotations using the <ruby> element. One <rp> element should enclose each of the opening and closing parentheses that wrap the <rt> element that contains the annotation's text.
	/// </summary>
	public static HtmlTag Rp(IHtmlContent content) => new HtmlTag("rp", true, content);

	/// <inheritdoc cref="Rp(IHtmlContent)"/>
	public static HtmlTag Rp(string text) => Rp(Encode(text));

	/// <summary>
	/// Specifies the ruby text component of a ruby annotation, which is used to provide pronunciation, translation, or transliteration information for East Asian typography. The <rt> element must always be contained within a <ruby> element.
	/// </summary>
	public static HtmlTag Rt(IHtmlContent content) => new HtmlTag("rp", true, content);

	/// <inheritdoc cref="Rt(IHtmlContent)"/>
	public static HtmlTag Rt(string text) => Rt(Encode(text));

	/// <summary>
	/// Represents small annotations that are rendered above, below, or next to base text, usually used for showing the pronunciation of East Asian characters. It can also be used for annotating other kinds of text, but this usage is less common.
	/// </summary>
	public static HtmlTag Ruby(IHtmlContent content) => new HtmlTag("ruby", true, content);

	/// <inheritdoc cref="Ruby(IHtmlContent)"/>
	public static HtmlTag Ruby(string text) => Ruby(Encode(text));

	/// <summary>
	/// Renders text with a strikethrough, or a line through it. Use the <s> element to represent things that are no longer relevant or no longer accurate. However, <s> is not appropriate when indicating document edits; for that, use the <del> and <ins> elements, as appropriate.
	/// </summary>
	public static HtmlTag S(IHtmlContent content) => new HtmlTag("s", true, content);

	/// <inheritdoc cref="S(IHtmlContent)"/>
	public static HtmlTag S(string text) => S(Encode(text));

	/// <summary>
	/// Used to enclose inline text which represents sample (or quoted) output from a computer program. Its contents are typically rendered using the browser's default monospaced font (such as Courier or Lucida Console).
	/// </summary>
	public static HtmlTag Samp(IHtmlContent content) => new HtmlTag("samp", true, content);

	/// <inheritdoc cref="Samp(IHtmlContent)"/>
	public static HtmlTag Samp(string text) => Samp(Encode(text));

	/// <summary>
	/// Represents side-comments and small print, like copyright and legal text, independent of its styled presentation. By default, it renders text within it one font size smaller, such as from small to x-small.
	/// </summary>
	public static HtmlTag Small(IHtmlContent content) => new HtmlTag("small", true, content);

	/// <inheritdoc cref="Small(IHtmlContent)"/>
	public static HtmlTag Small(string text) => Small(Encode(text));

	/// <summary>
	/// A generic inline container for phrasing content, which does not inherently represent anything. It can be used to group elements for styling purposes (using the class or id attributes), or because they share attribute values, such as lang. It should be used only when no other semantic element is appropriate. <span> is very much like a div element, but div is a block-level element whereas a <span> is an inline-level element.
	/// </summary>
	public static HtmlTag Span(IHtmlContent content) => new HtmlTag("span", true, content);

	/// <inheritdoc cref="Span(IHtmlContent)"/>
	public static HtmlTag Span(string text) => Span(Encode(text));

	/// <summary>
	/// Indicates that its contents have strong importance, seriousness, or urgency. Browsers typically render the contents in bold type.
	/// </summary>
	public static HtmlTag Strong(IHtmlContent content) => new HtmlTag("strong", true, content);

	/// <inheritdoc cref="Strong(IHtmlContent)"/>
	public static HtmlTag Strong(string text) => Strong(Encode(text));

	/// <summary>
	/// Specifies inline text which should be displayed as subscript for solely typographical reasons. Subscripts are typically rendered with a lowered baseline using smaller text.
	/// </summary>
	public static HtmlTag Sub(IHtmlContent content) => new HtmlTag("strong", true, content);

	/// <inheritdoc cref="Sub(IHtmlContent)"/>
	public static HtmlTag Sub(string text) => Sub(Encode(text));

	/// <summary>
	/// Specifies inline text which is to be displayed as superscript for solely typographical reasons. Superscripts are usually rendered with a raised baseline using smaller text.
	/// </summary>
	public static HtmlTag Sup(IHtmlContent content) => new HtmlTag("sup", true, content);

	/// <inheritdoc cref="Sup(IHtmlContent)"/>
	public static HtmlTag Sup(string text) => Sup(Encode(text));

	/// <summary>
	/// Represents a specific period in time. It may include the datetime attribute to translate dates into machine-readable format, allowing for better search engine results or custom features such as reminders.
	/// </summary>
	public static HtmlTag Time(IHtmlContent content) => new HtmlTag("time", true, content);

	/// <inheritdoc cref="Time(IHtmlContent)"/>
	public static HtmlTag Time(string text) => Time(Encode(text));

	/// <summary>
	/// Represents a span of inline text which should be rendered in a way that indicates that it has a non-textual annotation. This is rendered by default as a single solid underline but may be altered using CSS.
	/// </summary>
	public static HtmlTag U(IHtmlContent content) => new HtmlTag("u", true, content);

	/// <inheritdoc cref="U(IHtmlContent)"/>
	public static HtmlTag U(string text) => U(Encode(text));

	/// <summary>
	/// Represents the name of a variable in a mathematical expression or a programming context. It's typically presented using an italicized version of the current typeface, although that behavior is browser-dependent.
	/// </summary>
	public static HtmlTag Var(IHtmlContent content) => new HtmlTag("var", true, content);

	/// <inheritdoc cref="Var(IHtmlContent)"/>
	public static HtmlTag Var(string text) => Var(Encode(text));

	/// <summary>
	/// Represents a word break opportunity—a position within text where the browser may optionally break a line, though its line-breaking rules would not otherwise create a break at that location.
	/// </summary>
	public static HtmlTag Wrb() => new HtmlTag("wrb", true);
}
