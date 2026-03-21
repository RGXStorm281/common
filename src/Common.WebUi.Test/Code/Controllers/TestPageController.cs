namespace RobinEpple.Common.WebUi.Test.Code.Controllers;

using Microsoft.AspNetCore.Mvc;
using RobinEpple.Common.Util;
using RobinEpple.Common.WebUi.Test.Code.Models;

public class TestPageController(ITimeoutCache cache) : Controller
{
	private readonly ITimeoutCache _cache = cache;

	public ActionResult StaticContent()
	{
		var model = new StaticContentPage();
		return View("_Page", model);
	}

	private static string GetOrCreateClientId(HttpContext context)
	{
		const string cookieName = "ClientId";

		if (context.Request.Cookies.TryGetValue(cookieName, out var existingId))
		{
			return existingId;
		}

		var newId = Guid.NewGuid().ToString();

		context.Response.Cookies.Append(
			cookieName,
			newId,
			new CookieOptions
			{
				HttpOnly = true,
				Expires = DateTimeOffset.UtcNow.AddYears(1),
				IsEssential = true,
			}
		);

		return newId;
	}

	public async Task<ActionResult> FormRendering()
	{
		var clientId = GetOrCreateClientId(HttpContext);
		var modelKey = $"{clientId}:{nameof(FormRendering)}";
		if (!_cache.TryGetValue<FormRenderingPage>(modelKey, out var model))
		{
			model = new FormRenderingPage();
			_cache.Cache(modelKey, model, TimeSpan.FromMinutes(5));
		}

		if (!Request.IsHtmxRefresh())
		{
			return View("_Page", model);
		}

		await Request.BindAsync(model.Form);
		return View("_Page", model);
	}
}
