namespace RobinEpple.Common.Html.Components;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using static RobinEpple.Common.Html.DSL;

public class RenderIf : IHtmlContent
{
	public RenderIf(Func<bool> condition, params IEnumerable<IHtmlContent> contents)
	{
		_priorityList = [new(condition, Concat(contents))];
	}

	private record Option(Func<bool> Condition, IHtmlContent Content);

	private List<Option> _priorityList;

	public RenderIf ElseIf(Func<bool> condition, params IEnumerable<IHtmlContent> contents)
	{
		_priorityList.Add(new(condition, Concat(contents)));
		return this;
	}

	public IHtmlContent Else(params IEnumerable<IHtmlContent> contents)
	{
		_priorityList.Add(new(() => true, Concat(contents)));
		return this;
	}

	public void WriteTo(TextWriter writer, HtmlEncoder encoder)
	{
		var activeOption = _priorityList.FirstOrDefault(option => option.Condition() == true);
		if (activeOption != null)
		{
			activeOption.Content.WriteTo(writer, encoder);
		}
	}
}
