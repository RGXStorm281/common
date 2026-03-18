namespace RobinEpple.Common.Html.Components;

using Microsoft.AspNetCore.Html;
using static RobinEpple.Common.Html.DSL;

/// <summary>
/// A simple container to register HTML contents in the order they should be rendered.
/// </summary>
public class SequentialHtmlContentBuilder
{
	private List<IHtmlContent> _sequence = [];

	/// <summary>
	/// Registers another content to render after all previously registered ones.
	/// </summary>
	public void Render(IHtmlContent next)
	{
		_sequence.Add(next);
	}

	internal IHtmlContent GetResult()
	{
		return Concat(_sequence);
	}
}
