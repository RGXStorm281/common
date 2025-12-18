namespace RobinEpple.Common.WebUi.DSL;

using Microsoft.AspNetCore.Html;
using RobinEpple.Common.Html.Components;

/// <summary>
/// Provides some basic rendering functions and API for working with the
/// HTML content from the DSL defined in this package.
/// </summary>
public static class Helper
{
	public static TTag Class<TTag>(this TTag tag, string cssClass)
		where TTag : HtmlTag => tag with { Classes = tag.Classes.Add(cssClass) };

	public static TTag RemoveClass<TTag>(this TTag tag, string cssClass)
		where TTag : HtmlTag => tag with { Classes = tag.Classes.Remove(cssClass) };

	public static TTag Attribute<TTag>(this TTag tag, string name, string value)
		where TTag : HtmlTag =>
		tag with
		{
			Attributes = tag.Attributes.ContainsKey(name)
				? tag.Attributes.SetItem(name, value)
				: tag.Attributes.Add(name, value),
		};

	public static TTag RemoveAttribute<TTag>(this TTag tag, string name)
		where TTag : HtmlTag => tag with { Attributes = tag.Attributes.Remove(name) };

	public static Raw Raw(string text) => new Raw(text);

	public static Encode Encode(string text) => new Encode(text);

	public static Concat Concat(params IEnumerable<IHtmlContent> parts) => new Concat(parts);
}
