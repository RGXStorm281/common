namespace RobinEpple.Common.WebUi.Test.Code.Controllers;

using Microsoft.AspNetCore.Mvc;
using RobinEpple.Common.WebUi.Test.Code.Models;

public class TestPageController : Controller
{
	public ActionResult StaticContent()
	{
		var model = new StaticContentPage();
		return View("_Page", model);
	}

	public async Task<ActionResult> FormRendering()
	{
		var model = new FormRenderingPage();

		if (!Request.IsHtmxRefresh())
		{
			return View("_Page", model);
		}

		await Request.BindAsync(model.Form);
		return View("_Page", model);
	}
}
