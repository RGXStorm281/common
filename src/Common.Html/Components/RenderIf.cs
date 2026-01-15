namespace RobinEpple.Common.Html.Components;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using static RobinEpple.Common.Html.DSL;

/// <summary>
/// Renders some content conditionally.
/// Attention: This element receives the evaluated boolean result, not the expression. So storing the resulting IHtmlContent will NOT reevaluate the expression as dependencies change!
/// Like most of the rendering framework this element is designed to be constructed, rendered once and then discarded.
/// If you need lazy evaluation at rendering time, use <see cref="Lazy"/> content and capture dependencies into the lambda.
/// </summary>
public class RenderIf : IHtmlContent
{
	private IHtmlContent? _content = null;

	/// <inheritdoc cref="RenderIf"/>
	/// <param name="conditionMet">The result of the first condition.</param>
	/// <param name="contents">The content to render when the condition is met.</param>
	public RenderIf(bool conditionMet, params IEnumerable<IHtmlContent> contents)
	{
		if (conditionMet)
		{
			// No lazy evaluation! It is decided at the time of construction that the IF case is chosen.
			_content = Concat(contents);
		}
	}

	/// <summary>
	/// Renders a conditioned fallback in case the previous conditions all failed but this one is met.
	/// </summary>
	/// <param name="conditionMet">The result of the fallback condition.</param>
	/// <param name="contents">The content to render when the condition is met.</param>
	/// <returns>The render component for chaining.</returns>
	public RenderIf ElseIf(bool conditionMet, params IEnumerable<IHtmlContent> contents)
	{
		if (_content != null)
		{
			// A prior condition has already been met -> this one is irrelevant.
			return this;
		}
		if (conditionMet)
		{
			// This is the first matched condition -> render the given content.
			_content = Concat(contents);
		}

		// The condition has not been met -> leave the content empty for the possible next condition.
		return this;
	}

	/// <summary>
	/// Renders an unconditioned fallback in case the previous conditions all failed.
	/// </summary>
	/// <param name="contents">The content to render.</param>
	/// <returns>The finished HTML Component, no more chaining is possible after this function is called.</returns>
	public IHtmlContent Else(params IEnumerable<IHtmlContent> contents)
	{
		if (_content != null)
		{
			// A prior condition has already been met -> this one is irrelevant.
			return this;
		}

		// No condition has been met -> render the ELSE case.
		_content = Concat(contents);
		return this;
	}

	/// <summary>
	/// Writes the html text to the writer.
	/// </summary>
	public void WriteTo(TextWriter writer, HtmlEncoder encoder)
	{
		if (_content != null)
		{
			_content.WriteTo(writer, encoder);
		}
	}
}
