namespace RobinEpple.Common.WebUi.Test.Code.Models;

using Microsoft.AspNetCore.Html;

public interface IPageModel
{
	public string Title { get; }
	public IHtmlContent Render();
}
