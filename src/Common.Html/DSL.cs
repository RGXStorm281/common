namespace RobinEpple.Common.Html;

using Microsoft.AspNetCore.Html;
using RobinEpple.Common.Html.Components;
using RobinEpple.Common.SourceGenerators.Abstractions;

[StaticFactory(typeof(IHtmlContent))]
public static partial class DSL
{
	public static TTag Class<TTag>(this TTag tag, string cssClass)
		where TTag : HtmlTag
	{
		tag.Classes.Add(cssClass);
		return tag;
	}

	public static TTag RemoveClass<TTag>(this TTag tag, string cssClass)
		where TTag : HtmlTag
	{
		tag.Classes.Remove(cssClass);
		return tag;
	}

	public static TTag Attribute<TTag>(this TTag tag, string name, string value)
		where TTag : HtmlTag
	{
		tag.Attributes[name] = value;
		return tag;
	}

	public static TTag RemoveAttribute<TTag>(this TTag tag, string name)
		where TTag : HtmlTag
	{
		tag.Attributes.Remove(name);
		return tag;
	}

	public static TTag Data<TTag>(this TTag tag, string name, string value)
		where TTag : HtmlTag
	{
		tag.Attributes["data-" + name] = value;
		return tag;
	}

	public static TTag RemoveData<TTag>(this TTag tag, string name)
		where TTag : HtmlTag
	{
		tag.Attributes.Remove("data-" + name);
		return tag;
	}
}
