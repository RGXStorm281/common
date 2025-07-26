namespace RobinEpple.Common.WebUi.Components;

using Microsoft.AspNetCore.Html;
using RobinEpple.Common.WebUi.DSL;

public record class AnchorTag : HtmlTag
{
	public AnchorTag(string target, IHtmlContent content)
		: base("a", false, content, [], new Dictionary<string, string> { { "href", target } }) { }

	public AnchorTag Href(string target) => this.Attribute("href", target);
}
