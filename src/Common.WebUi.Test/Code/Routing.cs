namespace RobinEpple.Common.WebUi.Test.Code;

public class Routing
{
	public static void RegisterRoutes(IEndpointRouteBuilder routes)
	{
		MapLandingPage(routes);
		MapControllerActions(routes);
	}

	private static void MapLandingPage(IEndpointRouteBuilder routes)
	{
		routes.MapControllerRoute(
			name: "landing page",
			pattern: "~/",
			defaults: new { controller = "Home", action = "Index" }
		);
	}

	private static void MapControllerActions(IEndpointRouteBuilder routes) =>
		routes.MapControllerRoute(name: "controller actions", pattern: "~/{controller}/{action}");
}
