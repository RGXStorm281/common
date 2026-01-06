using System.Diagnostics;
using System.Net;
using Common.WebUi.Test.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Common.WebUi.Test.Controllers;

public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;

	public HomeController(ILogger<HomeController> logger)
	{
		_logger = logger;
	}

	public IActionResult Index()
	{
		return View();
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error(int? statusCode)
	{
		// Determine the request-id.
		var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

		// The exception handler feature can be null, if the method is called for a status code page, e.g. a "true" 404 and not one triggered by an exception.
		var exceptionHandler = HttpContext.Features.Get<IExceptionHandlerFeature>();
		var error = exceptionHandler?.Error;

		// Choose the page to display according to the status code.
		string view = statusCode switch
		{
			(int)HttpStatusCode.NotFound => "NotFound",
			(int)HttpStatusCode.Forbidden => "Forbidden",
			(int)HttpStatusCode.Conflict => "Conflict",
			_ => "Error",
		};

		// Build the view model.
		var viewModel = new ErrorViewModel(requestId, error);
		if (viewModel.ShowRequestId)
		{
			_logger.LogError($"Showing error for request #{requestId}: {viewModel.Error?.Message}");
		}

		return View(view, viewModel);
	}
}
