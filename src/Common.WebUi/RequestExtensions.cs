namespace RobinEpple.Common.WebUi;

using Microsoft.AspNetCore.Http;
using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// Contains extension methods for binding HTTP request values to a form model.
/// </summary>
public static class RequestExtensions
{
	/// <summary>
	/// Checks, whether the request was issued by HTMX.
	/// </summary>
	/// <param name="request">The request to check.</param>
	/// <returns><see langword="true"/>, if the request was issued by HTMX.</returns>
	public static bool IsHtmxRequest(this HttpRequest request) => request.Headers.ContainsKey("HX-Request");

	/// <summary>
	/// Checks, whether the request is an HTMX refresh and not a submit.
	/// </summary>
	/// <param name="request">The request to check.</param>
	/// <returns><see langword="true"/>, if the triggering element does not contain "submit" in its id.</returns>
	public static bool IsHtmxRefresh(this HttpRequest request)
	{
		if (!request.IsHtmxRequest())
		{
			return false;
		}

		if (!request.Headers.TryGetValue("HX-Trigger", out var triggerElementId))
		{
			return true;
		}

		var isSubmit = triggerElementId.ToString().Contains("submit", StringComparison.OrdinalIgnoreCase);
		return !isSubmit;
	}

	/// <summary>
	/// Retrieves all form values and documents from the HTTP request and writes them into their corresponding nodes in the form model.
	/// Other fields stay untouched, and the form is not yet updated.
	/// </summary>
	/// <param name="form">The form to update with the request values.</param>
	/// <param name="request">The HTTP request containing the form values.</param>
	/// <param name="bindingStrategies">
	/// Optional custom binding strategies to use.
	/// Order matters, strategies will be tried in the order they are provided.
	/// If not provided, <see cref="FormBinder.DefaultBindingStrategies"/> will be used.
	/// </param>
	public static async Task BindAsync(
		this HttpRequest request,
		IForm form,
		IEnumerable<IFormBindingStrategy>? bindingStrategies = null
	)
	{
		if (!request.HasFormContentType)
		{
			return;
		}

		var binder = new FormBinder(form, bindingStrategies);
		await binder.BindAsync(request);
	}
}
