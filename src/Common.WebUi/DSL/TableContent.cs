namespace RobinEpple.Common.WebUi.DSL;

using Microsoft.AspNetCore.Html;
using RobinEpple.Common.Html.Components;
using static RobinEpple.Common.WebUi.DSL.Helper;

/// <summary>
/// Provides rendering functions for the html tags defined in
/// https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements#table_content
/// </summary>
public static class TableContent
{
	/// <summary>
	/// Specifies the caption (or title) of a table.
	/// </summary>
	public static HtmlTag Caption(IHtmlContent content) => new HtmlTag("caption", false, content);

	/// <inheritdoc cref="Caption(IHtmlContent)"/>
	public static HtmlTag Caption(string text) => Caption(Encode(text));

	/// <summary>
	/// Defines one or more columns in a column group represented by its implicit or explicit parent <colgroup> element. The <col> element is only valid as a child of a <colgroup> element that has no span attribute defined.
	/// </summary>
	public static HtmlTag Col(int span) => new HtmlTag("col", true).Attribute("span", span.ToString());

	/// <summary>
	/// Defines a group of columns within a table.
	/// </summary>
	public static HtmlTag ColGroup(IHtmlContent content) => new HtmlTag("colgroup", false, content);

	/// <summary>
	/// Represents tabular data—that is, information presented in a two-dimensional table comprised of rows and columns of cells containing data.
	/// </summary>
	public static HtmlTag Table(params IEnumerable<IHtmlContent> content) =>
		new HtmlTag("table", false, Concat(content));

	/// <summary>
	/// Encapsulates a set of table rows (<tr> elements), indicating that they comprise the body of a table's (main) data.
	/// </summary>
	public static HtmlTag TBody(params IEnumerable<IHtmlContent> content) =>
		new HtmlTag("tbody", false, Concat(content));

	/// <summary>
	/// A child of the <tr> element, it defines a cell of a table that contains data.
	/// </summary>
	public static HtmlTag Td(IHtmlContent content) => new HtmlTag("td", false, content);

	/// <summary>
	/// Encapsulates a set of table rows (<tr> elements), indicating that they comprise the foot of a table with information about the table's columns. This is usually a summary of the columns, e.g., a sum of the given numbers in a column.
	/// </summary>
	public static HtmlTag TFoot(params IEnumerable<IHtmlContent> content) =>
		new HtmlTag("tfoot", false, Concat(content));

	/// <summary>
	/// A child of the <tr> element, it defines a cell as the header of a group of table cells. The nature of this group can be explicitly defined by the scope and headers attributes.
	/// </summary>
	public static HtmlTag Th(IHtmlContent content) => new HtmlTag("th", false, content);

	/// <summary>
	/// Encapsulates a set of table rows (<tr> elements), indicating that they comprise the head of a table with information about the table's columns. This is usually in the form of column headers (<th> elements).
	/// </summary>
	public static HtmlTag THead(params IEnumerable<IHtmlContent> content) =>
		new HtmlTag("thead", false, Concat(content));

	/// <summary>
	/// Defines a row of cells in a table. The row's cells can then be established using a mix of <td> (data cell) and <th> (header cell) elements.
	/// </summary>
	public static HtmlTag Tr(IHtmlContent content) => new HtmlTag("tr", false, content);
}
