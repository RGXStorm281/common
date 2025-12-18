namespace RobinEpple.Common.WebUi.DSL;

using Microsoft.AspNetCore.Html;
using RobinEpple.Common.Html.Components;

/// <summary>
/// Provides rendering functions for the html tags defined in
/// https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements#forms
/// </summary>
public static class Forms
{
	/// <summary>
	/// An interactive element activated by a user with a mouse, keyboard, finger, voice command, or other assistive technology. Once activated, it performs an action, such as submitting a form or opening a dialog.
	/// </summary>
	public static HtmlTag Button(IHtmlContent content, bool submitButton = false) =>
		new HtmlTag("button", false, content).Attribute("type", submitButton ? "submit" : "button");

	public static HtmlTag DataList(IHtmlContent content, bool submitButton = false) =>
		new HtmlTag("datalist", false, content).Attribute("type", submitButton ? "submit" : "button");
}
