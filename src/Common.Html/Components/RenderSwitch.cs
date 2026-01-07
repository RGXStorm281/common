namespace RobinEpple.Common.Html.Components;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using static RobinEpple.Common.Html.DSL;

/// <summary>
/// Renders only the case that matches the given value.
/// Attention: This element receives the evaluated result, not the expression. So storing the resulting IHtmlContent will NOT reevaluate the expression as dependencies change!
/// Like most of the rendering framework this element is designed to be constructed, rendered once and then discarded.
/// If you need lazy evaluation at rendering time, use <see cref="Lazy"/> content and capture dependencies into the lambda.
/// </summary>
public class RenderSwitch<TValue>(TValue switchValue) : IHtmlContent
{
	private readonly TValue _switchValue = switchValue;
	private RenderIf _content = RenderIf(false);

	public RenderSwitch<TValue> Case(TValue option, params IEnumerable<IHtmlContent> contents)
	{
		_content.ElseIf(CaseMatch(_switchValue, option), contents);
		return this;
	}

	public RenderSwitch<TValue> Case<TTypeOption>(params IEnumerable<IHtmlContent> contents)
		where TTypeOption : TValue
	{
		_content.ElseIf(_switchValue is TTypeOption, contents);
		return this;
	}

	public IHtmlContent Default(params IEnumerable<IHtmlContent> contents)
	{
		_content.Else(contents);
		return this;
	}

	public void WriteTo(TextWriter writer, HtmlEncoder encoder)
	{
		_content.WriteTo(writer, encoder);
	}

	private bool CaseMatch(TValue option, TValue currentValue)
	{
		if (option == null)
		{
			return currentValue == null;
		}

		return option.Equals(currentValue);
	}
}
