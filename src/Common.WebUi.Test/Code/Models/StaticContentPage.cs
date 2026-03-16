namespace RobinEpple.Common.WebUi.Test.Code.Models;

using Microsoft.AspNetCore.Html;
using static RobinEpple.Common.Html.DSL;

public class StaticContentPage : IPageModel
{
	public string Title { get; } = "Static Content";

	public IHtmlContent Render()
	{
		return Div(
			// Headings and paragraphs
			H1("Static Content Page"),
			P("This is a simple text page used for testing purposes."),
			P("It contains only static content and is used to verify that the basic page rendering works correctly."),
			H2("This is a section heading"),
			H3("This is a subsection heading"),
			H4("This is a sub-subsection heading"),
			H5("This is a paragraph heading with a custom attribute"),
			H6("Who uses H6?"),
			Br(),
			// Decorated text
			P(
				Encode("This paragraph contains some "),
				Strong("strong text"),
				Encode(" and some "),
				Em("emphasized text"),
				Encode(".")
			),
			P("This paragraph has a custom class and attribute.")
				.Class("custom-class")
				.Attribute("data-custom", "custom-value"),
			// Lists and conditions
			H2("This is a list of numbers:"),
			Ul(RenderEach([1, 2, 3, 4, 5, 6, 7, 8, 9, 10], number => Li(number.ToString()))),
			P("The following is rendered conditionally:"),
			RenderIf(true, P("If rendered")).ElseIf(true, P("Else if rendered")).Else(P("Else rendered")),
			// Form items
			P("Now lets render some form items."),
			Div(Input().Type("checkbox").Id("some-checkbox"), Label("I check this checkbox").For("some-checkbox"))
		);
	}
}
