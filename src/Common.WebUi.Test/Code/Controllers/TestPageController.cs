namespace RobinEpple.Common.WebUi.Test.Code.Controllers;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RobinEpple.Common.Forms.Nodes;
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

	public async Task<ActionResult> CheckoutSample()
	{
		var clientId = GetOrCreateClientId(HttpContext);
		var modelKey = $"{clientId}:{nameof(CheckoutSample)}";
		if (!_cache.TryGetValue<CheckoutSamplePage>(modelKey, out var model))
		{
			model = new CheckoutSamplePage();
			_cache.Cache(modelKey, model, TimeSpan.FromMinutes(5));
		}

		if (!Request.IsHtmxRefresh())
		{
			return View("_Page", model);
		}

		await Request.BindAsync(model.Form);
		return View("_Page", model);
	}

	public ActionResult AddShirt()
	{
		var clientId = GetOrCreateClientId(HttpContext);
		var modelKey = $"{clientId}:{nameof(CheckoutSample)}";
		if (!_cache.TryGetValue<CheckoutSamplePage>(modelKey, out var model))
		{
			model = new CheckoutSamplePage();
			_cache.Cache(modelKey, model, TimeSpan.FromMinutes(5));
		}

		model.FormWrapper.Cart.CartItems.TryInstantiateShirt(out _);
		return View("_Page", model);
	}

	public ActionResult AddChocolate()
	{
		var clientId = GetOrCreateClientId(HttpContext);
		var modelKey = $"{clientId}:{nameof(CheckoutSample)}";
		if (!_cache.TryGetValue<CheckoutSamplePage>(modelKey, out var model))
		{
			model = new CheckoutSamplePage();
			_cache.Cache(modelKey, model, TimeSpan.FromMinutes(5));
		}

		model.FormWrapper.Cart.CartItems.TryInstantiateChocolate(out _);
		return View("_Page", model);
	}

	public async Task<ActionResult> RemoveCartItem(string itemId)
	{
		var clientId = GetOrCreateClientId(HttpContext);
		var modelKey = $"{clientId}:{nameof(CheckoutSample)}";
		if (!_cache.TryGetValue<CheckoutSamplePage>(modelKey, out var model))
		{
			model = new CheckoutSamplePage();
			_cache.Cache(modelKey, model, TimeSpan.FromMinutes(5));
		}

		var item = model.Form!.FindFirst(node => node.GetId() == itemId) as IForm;
		if (item != null)
		{
			await model.FormWrapper.Cart.CartItems.Node!.RemoveItemAsync(item);
		}
		return View("_Page", model);
	}

	public async Task<ActionResult> RecursiveSample()
	{
		var clientId = GetOrCreateClientId(HttpContext);
		var modelKey = $"{clientId}:{nameof(RecursiveSample)}";
		if (!_cache.TryGetValue<RecursiveSamplePage>(modelKey, out var model))
		{
			model = new RecursiveSamplePage();
			_cache.Cache(modelKey, model, TimeSpan.FromMinutes(5));
		}

		if (!Request.IsHtmxRefresh())
		{
			return View("_Page", model);
		}

		await Request.BindAsync(model.Form);
		return View("_Page", model);
	}

	public async Task<ActionResult> SelectNoteItem(string itemId)
	{
		var clientId = GetOrCreateClientId(HttpContext);
		var modelKey = $"{clientId}:{nameof(RecursiveSample)}";
		if (!_cache.TryGetValue<RecursiveSamplePage>(modelKey, out var model))
		{
			model = new RecursiveSamplePage();
			_cache.Cache(modelKey, model, TimeSpan.FromMinutes(5));
		}

		var item = model.Form.FindFirst(node => node.GetId() == itemId) as IForm;
		if (item != null)
		{
			model.SelectedItem = item;
		}
		return View("_Page", model);
	}

	public async Task<ActionResult> CreateFolder(string parentId)
	{
		var clientId = GetOrCreateClientId(HttpContext);
		var modelKey = $"{clientId}:{nameof(RecursiveSample)}";
		if (!_cache.TryGetValue<RecursiveSamplePage>(modelKey, out var model))
		{
			model = new RecursiveSamplePage();
			_cache.Cache(modelKey, model, TimeSpan.FromMinutes(5));
		}

		// Find the parent folder.
		var parentFolder = model.Form.FindFirst(node => node.GetId() == parentId) as IForm;
		if (parentFolder == null)
		{
			return View("_Page", model);
		}

		// Find the collection and its template to instantiate.
		var templateItemCollection = model.FormWrapper.RootFolder.FolderTemplate.Items;
		var itemCollection =
			parentFolder.Nodes.FirstOrDefault(child => child.Name == templateItemCollection.Node!.Name)
			as ICollectionNode;
		if (itemCollection == null)
		{
			return View("_Page", model);
		}

		var folderTemplate = itemCollection.Templates.FirstOrDefault(template =>
			template.Name == templateItemCollection.FolderTemplate.Node!.Name
		);
		if (folderTemplate == null)
		{
			return View("_Page", model);
		}

		// Instantiate and select the new item.
		var newFolder = await itemCollection.InstantiateAsync(folderTemplate);
		model.SelectedItem = newFolder;

		return View("_Page", model);
	}

	public async Task<ActionResult> CreateNote(string parentId)
	{
		var clientId = GetOrCreateClientId(HttpContext);
		var modelKey = $"{clientId}:{nameof(RecursiveSample)}";
		if (!_cache.TryGetValue<RecursiveSamplePage>(modelKey, out var model))
		{
			model = new RecursiveSamplePage();
			_cache.Cache(modelKey, model, TimeSpan.FromMinutes(5));
		}

		// Find the parent folder.
		var parentFolder = model.Form.FindFirst(node => node.GetId() == parentId) as IForm;
		if (parentFolder == null)
		{
			return View("_Page", model);
		}

		// Find the collection and its template to instantiate.
		var templateItemCollection = model.FormWrapper.RootFolder.FolderTemplate.Items;
		var itemCollection =
			parentFolder.Nodes.FirstOrDefault(child => child.Name == templateItemCollection.Node!.Name)
			as ICollectionNode;
		if (itemCollection == null)
		{
			return View("_Page", model);
		}

		var noteTemplate = itemCollection.Templates.FirstOrDefault(template =>
			template.Name == templateItemCollection.NoteTemplate.Node!.Name
		);
		if (noteTemplate == null)
		{
			return View("_Page", model);
		}

		// Instantiate and select the new item.
		var newNote = await itemCollection.InstantiateAsync(noteTemplate);
		model.SelectedItem = newNote;

		return View("_Page", model);
	}

	public async Task<ActionResult> RemoveNoteItem(string itemId)
	{
		var clientId = GetOrCreateClientId(HttpContext);
		var modelKey = $"{clientId}:{nameof(RecursiveSample)}";
		if (!_cache.TryGetValue<RecursiveSamplePage>(modelKey, out var model))
		{
			model = new RecursiveSamplePage();
			_cache.Cache(modelKey, model, TimeSpan.FromMinutes(5));
		}

		var item = model.Form!.FindFirst(node => node.GetId() == itemId) as IForm;
		if (item is { Parent: ICollectionNode collection })
		{
			await collection.RemoveItemAsync(item);
		}
		return View("_Page", model);
	}
}
