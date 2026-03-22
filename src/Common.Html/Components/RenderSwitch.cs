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

	/// <summary>
	/// A value case that renders the given contents when met.
	/// </summary>
	/// <param name="option">The value to compare with the switch value.</param>
	/// <param name="contents">The contents to render when equal.</param>
	/// <returns>The render component for chaining.</returns>
	public RenderSwitch<TValue> Case(TValue option, params IEnumerable<IHtmlContent> contents)
	{
		_content.ElseIf(CaseMatch(_switchValue, option), contents);
		return this;
	}

	/// <summary>
	/// A type case that renders the given contents when met.
	/// </summary>
	/// <typeparam name="TTypeOption">The condition is met when the given switch value is of the given type.</typeparam>
	/// <param name="contents">The contents to render when the condition is met.</param>
	/// <returns>The render component for chaining.</returns>
	public RenderSwitch<TValue> Case<TTypeOption>(params IEnumerable<IHtmlContent> contents)
		where TTypeOption : TValue
	{
		_content.ElseIf(_switchValue is TTypeOption, contents);
		return this;
	}

	/// <summary>
	/// A type case that renders the given contents when met.
	/// </summary>
	/// <typeparam name="TTypeOption">The condition is met when the given switch value is of the given type.</typeparam>
	/// <param name="render">A rendering function that feeds the type casted item into the rendering pipeline.</param>
	/// <returns>The render component for chaining.</returns>
	public RenderSwitch<TValue> Case<TTypeOption>(Func<TTypeOption, IHtmlContent> render)
		where TTypeOption : TValue
	{
		_content.ElseIf(_switchValue is TTypeOption, Lazy(() => render((TTypeOption)_switchValue!)));
		return this;
	}

	/// <summary>
	/// An unconditioned fallback when all cases are not met.
	/// </summary>
	/// <param name="contents">The contents to render.</param>
	/// <returns>The finished HTML Component, no more chaining is possible after this function is called.</returns>
	public IHtmlContent Default(params IEnumerable<IHtmlContent> contents)
	{
		_content.Else(contents);
		return this;
	}

	/// <summary>
	/// Writes the html text to the writer.
	/// </summary>
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
