namespace RobinEpple.Common.WebUi.Components;

using Microsoft.AspNetCore.Html;

public record class H1(IHtmlContent Content) : HtmlTag("h1")
{
	protected override void AppendContent(HtmlContentBuilder builder)
	{
		builder.AppendHtml(Content);
	}
}
