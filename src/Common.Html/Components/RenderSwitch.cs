namespace RobinEpple.Common.Html.Components;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using static RobinEpple.Common.Html.DSL;

public class RenderSwitch<TValue>(Func<TValue> switchTarget) : IHtmlContent
{
	private readonly Func<TValue> _switchTarget = switchTarget;

	private record SwitchOption(TValue Option, IHtmlContent Content);

	private List<SwitchOption> _switchOptions = [];
	private IHtmlContent? _default = null;

	public RenderSwitch<TValue> Case(TValue option, params IEnumerable<IHtmlContent> contents)
	{
		_switchOptions.Add(new(option, Concat(contents)));
		return this;
	}

	public IHtmlContent Default(params IEnumerable<IHtmlContent> contents)
	{
		_default = Concat(contents);
		return this;
	}

	public void WriteTo(TextWriter writer, HtmlEncoder encoder)
	{
		var currentValue = _switchTarget();
		var activeCase = _switchOptions.FirstOrDefault(option => CaseMatch(option.Option, currentValue));

		if (activeCase != null)
		{
			activeCase.Content.WriteTo(writer, encoder);
			return;
		}

		if (_default != null)
		{
			_default.WriteTo(writer, encoder);
			return;
		}
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
