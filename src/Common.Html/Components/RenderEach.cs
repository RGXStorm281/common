namespace RobinEpple.Common.Html.Components;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using static RobinEpple.Common.Html.DSL;

/// <summary>
/// Calls the render function for each given item.
/// Attention: This element receives the evaluated list, not the expression. So storing the resulting IHtmlContent will NOT reevaluate the expression as dependencies change!
/// Like most of the rendering framework this element is designed to be constructed, rendered once and then discarded.
/// If you need lazy evaluation at rendering time, use <see cref="Lazy"/> content and capture dependencies into the lambda.
/// </summary>
public class RenderEach<TItem>(IEnumerable<TItem> items, Func<TItem, IHtmlContent> render) : IHtmlContent
{
	private readonly IEnumerable<TItem> _items = items;
	private readonly Func<TItem, IHtmlContent> _render = render;

	public void WriteTo(TextWriter writer, HtmlEncoder encoder)
	{
		var content = Concat(_items.Select(_render));
		content.WriteTo(writer, encoder);
	}
}
