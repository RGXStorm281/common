namespace RobinEpple.Common.WebUi.DSL;

using Microsoft.AspNetCore.Html;
using RobinEpple.Common.Html.Components;
using static RobinEpple.Common.WebUi.DSL.Helper;

/// <summary>
/// Provides rendering functions for the html tags defined in
/// https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements#content_sectioning
/// </summary>
public static class ContentSectioning
{
	/// <summary>
	/// Indicates that the enclosed HTML provides contact information for a person or people, or for an organization.
	/// </summary>
	public static HtmlTag Address(IHtmlContent content) => new HtmlTag("address", false, content);

	/// <summary>
	/// Represents a self-contained composition in a document, page, application, or site, which is intended to be independently distributable or reusable (e.g., in syndication). Examples include a forum post, a magazine or newspaper article, a blog entry, a product card, a user-submitted comment, an interactive widget or gadget, or any other independent item of content.
	/// </summary>
	public static HtmlTag Article(IHtmlContent content) => new HtmlTag("article", false, content);

	/// <summary>
	/// Represents a portion of a document whose content is only indirectly related to the document's main content. Asides are frequently presented as sidebars or call-out boxes.
	/// </summary>
	public static HtmlTag Aside(IHtmlContent content) => new HtmlTag("aside", false, content);

	/// <summary>
	/// Represents a footer for its nearest ancestor sectioning content or sectioning root element (body). A <footer> typically contains information about the author of the section, copyright data, or links to related documents.
	/// </summary>
	public static HtmlTag Footer(IHtmlContent content) => new HtmlTag("footer", false, content);

	/// <summary>
	/// Represents introductory content, typically a group of introductory or navigational aids. It may contain some heading elements but also a logo, a search form, an author name, and other elements.
	/// </summary>
	public static HtmlTag Header(IHtmlContent content) => new HtmlTag("header", false, content);

	/// <summary>
	/// Represent the first level of section headings.
	/// </summary>
	public static HtmlTag H1(IHtmlContent content) => new HtmlTag("h1", false, content);

	/// <inheritdoc cref="H1(IHtmlContent)"/>
	public static HtmlTag H1(string text) => H1(Encode(text));

	/// <summary>
	/// Represent the second level of section headings.
	/// </summary>
	public static HtmlTag H2(IHtmlContent content) => new HtmlTag("h2", false, content);

	/// <inheritdoc cref="H2(IHtmlContent)"/>
	public static HtmlTag H2(string text) => H2(Encode(text));

	/// <summary>
	/// Represent the third level of section headings.
	/// </summary>
	public static HtmlTag H3(IHtmlContent content) => new HtmlTag("h3", false, content);

	/// <inheritdoc cref="H3(IHtmlContent)"/>
	public static HtmlTag H3(string text) => H3(Encode(text));

	/// <summary>
	/// Represent the fourth level of section headings.
	/// </summary>
	public static HtmlTag H4(IHtmlContent content) => new HtmlTag("h4", false, content);

	/// <inheritdoc cref="H4(IHtmlContent)"/>
	public static HtmlTag H4(string text) => H4(Encode(text));

	/// <summary>
	/// Represent the fifth level of section headings.
	/// </summary>
	public static HtmlTag H5(IHtmlContent content) => new HtmlTag("h5", false, content);

	/// <inheritdoc cref="H5(IHtmlContent)"/>
	public static HtmlTag H5(string text) => H5(Encode(text));

	/// <summary>
	/// Represent the sixth level of section headings.
	/// </summary>
	public static HtmlTag H6(IHtmlContent content) => new HtmlTag("h6", false, content);

	/// <inheritdoc cref="H6(IHtmlContent)"/>
	public static HtmlTag H6(string text) => H6(Encode(text));

	/// <summary>
	/// Represents a heading grouped with any secondary content, such as subheadings, an alternative title, or a tagline.
	/// </summary>
	public static HtmlTag HGroup(IHtmlContent content) => new HtmlTag("hgroup", false, content);

	/// <summary>
	/// Represents the dominant content of the body of a document. The main content area consists of content that is directly related to or expands upon the central topic of a document, or the central functionality of an application.
	/// </summary>
	public static HtmlTag Main(IHtmlContent content) => new HtmlTag("main", false, content);

	/// <summary>
	/// Represents a section of a page whose purpose is to provide navigation links, either within the current document or to other documents. Common examples of navigation sections are menus, tables of contents, and indexes.
	/// </summary>
	public static HtmlTag Nav(IHtmlContent content) => new HtmlTag("nav", false, content);

	/// <summary>
	/// Represents a generic standalone section of a document, which doesn't have a more specific semantic element to represent it. Sections should always have a heading, with very few exceptions.
	/// </summary>
	public static HtmlTag Section(IHtmlContent content) => new HtmlTag("section", false, content);

	/// <summary>
	/// Represents a part that contains a set of form controls or other content related to performing a search or filtering operation.
	/// </summary>
	public static HtmlTag Search(IHtmlContent content) => new HtmlTag("search", false, content);
}
