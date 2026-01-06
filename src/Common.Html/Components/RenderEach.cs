namespace RobinEpple.Common.Html.Components;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using static RobinEpple.Common.Html.DSL;

public class RenderEach<TItem>(Func<IEnumerable<TItem>> items, Func<TItem, IHtmlContent> render) : IHtmlContent
{
	private readonly Func<IEnumerable<TItem>> _items = items;
	private readonly Func<TItem, IHtmlContent> _render = render;

	public void WriteTo(TextWriter writer, HtmlEncoder encoder)
	{
		var content = Concat(_items().Select(_render));
		content.WriteTo(writer, encoder);
	}
}
