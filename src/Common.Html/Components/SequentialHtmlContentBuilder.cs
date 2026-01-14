namespace RobinEpple.Common.Html.Components;

using Microsoft.AspNetCore.Html;
using static RobinEpple.Common.Html.DSL;

public class SequentialHtmlContentBuilder
{
	private List<IHtmlContent> _sequence = [];

	public void Render(IHtmlContent next)
	{
		_sequence.Add(next);
	}

	public IHtmlContent GetResult()
	{
		return Concat(_sequence);
	}
}
