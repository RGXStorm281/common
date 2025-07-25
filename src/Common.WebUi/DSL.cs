namespace RobinEpple.Common.WebUi;

using Microsoft.AspNetCore.Html;
using RobinEpple.Common.WebUi.Components;

public static class DSL
{
	public static IHtmlContent Raw(string text) => new Raw(text);

	public static IHtmlContent Encode(string text) => new Encode(text);

	public static IHtmlContent H1(IHtmlContent content) => new H1(content);

	public static IHtmlContent H1(string text) => H1(Encode(text));
}
