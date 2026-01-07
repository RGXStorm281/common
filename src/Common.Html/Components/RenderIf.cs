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

	public RenderIf(bool conditionMet, params IEnumerable<IHtmlContent> contents)
	{
		if (conditionMet)
		{
			// No lazy evaluation! It is decided at the time of construction that the IF case is chosen.
			_content = Concat(contents);
		}
	}

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

	public void WriteTo(TextWriter writer, HtmlEncoder encoder)
	{
		if (_content != null)
		{
			_content.WriteTo(writer, encoder);
		}
	}
}
