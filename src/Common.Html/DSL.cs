namespace RobinEpple.Common.Html;

using Microsoft.AspNetCore.Html;
using RobinEpple.Common.Html.Components;
using RobinEpple.Common.SourceGenerators.Abstractions;

/// <summary>
/// A factory class that allows rendering HTML in a declarative style.
/// </summary>
[StaticFactory(typeof(IHtmlContent))]
public static partial class DSL
{
	/// <summary>
	/// Adds a class to the HTML tag.
	/// </summary>
	/// <typeparam name="TTag">The HTML tag type.</typeparam>
	/// <param name="tag">The tag to add the class to.</param>
	/// <param name="cssClass">The css class to add.</param>
	/// <returns>The HTML tag for chaining.</returns>
	public static TTag Class<TTag>(this TTag tag, string cssClass)
		where TTag : HtmlTag
	{
		tag.Classes.Add(cssClass);
		return tag;
	}

	/// <summary>
	/// Removes a class from the HTML tag.
	/// </summary>
	/// <typeparam name="TTag">The HTML tag type.</typeparam>
	/// <param name="tag">The tag to remove the class from.</param>
	/// <param name="cssClass">The css class to remove.</param>
	/// <returns>The HTML tag for chaining.</returns>
	public static TTag RemoveClass<TTag>(this TTag tag, string cssClass)
		where TTag : HtmlTag
	{
		tag.Classes.Remove(cssClass);
		return tag;
	}

	/// <summary>
	/// Configures an attribute for the HTML tag.
	/// </summary>
	/// <typeparam name="TTag">The HTML tag type.</typeparam>
	/// <param name="tag">The tag configure the attribute for.</param>
	/// <param name="name">The name of the attribute.</param>
	/// <param name="value">The value of the attribute.</param>
	/// <returns>The HTML tag for chaining.</returns>
	public static TTag Attribute<TTag>(this TTag tag, string name, string value)
		where TTag : HtmlTag
	{
		tag.Attributes[name] = value;
		return tag;
	}

	/// <summary>
	/// Calls the configuration function if the condition is met.
	/// </summary>
	/// <typeparam name="TTag">The HTML tag type.</typeparam>
	/// <param name="tag">The tag configure the attribute for.</param>
	/// <param name="conditionMet">A boolean indicating whether the configuration should be applied.</param>
	/// <param name="configure">The configuration function to apply if the condition is met.</param>
	/// <returns>The HTML tag for chaining.</returns>
	public static TTag ConfigureIf<TTag>(this TTag tag, bool conditionMet, Func<TTag, TTag> configure)
		where TTag : HtmlTag
	{
		if (conditionMet)
		{
			tag = configure(tag);
		}
		return tag;
	}

	/// <summary>
	/// Removes an attribute from the HTML tag.
	/// </summary>
	/// <typeparam name="TTag">The HTML tag type.</typeparam>
	/// <param name="tag">The tag remove the attribute from.</param>
	/// <param name="name">The name of the attribute.</param>
	/// <returns>The HTML tag for chaining.</returns>
	public static TTag RemoveAttribute<TTag>(this TTag tag, string name)
		where TTag : HtmlTag
	{
		tag.Attributes.Remove(name);
		return tag;
	}

	/// <summary>
	/// Configures a data-attribute for the HTML tag.
	/// </summary>
	/// <typeparam name="TTag">The HTML tag type.</typeparam>
	/// <param name="tag">The tag configure the attribute for.</param>
	/// <param name="name">The name of the attribute (without data-, the prefix is added internally!).</param>
	/// <param name="value">The value of the attribute.</param>
	/// <returns>The HTML tag for chaining.</returns>
	public static TTag Data<TTag>(this TTag tag, string name, string value)
		where TTag : HtmlTag
	{
		tag.Attributes["data-" + name] = value;
		return tag;
	}

	/// <summary>
	/// Removes a data-attribute from the HTML tag.
	/// </summary>
	/// <typeparam name="TTag">The HTML tag type.</typeparam>
	/// <param name="tag">The tag remove the attribute from.</param>
	/// <param name="name">The name of the attribute (without data-, the prefix is added internally!).</param>
	/// <returns>The HTML tag for chaining.</returns>
	public static TTag RemoveData<TTag>(this TTag tag, string name)
		where TTag : HtmlTag
	{
		tag.Attributes.Remove("data-" + name);
		return tag;
	}
}
