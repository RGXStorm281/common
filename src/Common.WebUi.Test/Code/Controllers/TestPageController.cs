using Microsoft.AspNetCore.Mvc;
using RobinEpple.Common.WebUi.Test.Code.Models;

namespace RobinEpple.Common.WebUi.Test.Code.Controllers
{
	public class TestPageController : Controller
	{
		public ActionResult StaticContent()
		{
			var model = new StaticContentPage();
			return View("_Page", model);
		}
	}
}
