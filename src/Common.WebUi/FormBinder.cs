namespace RobinEpple.Common.WebUi;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// A binder instance that uses the registered binding strategies to bind values from an HTTP request to a form model.
/// </summary>
/// <param name="form">The form model to bind values to.</param>
/// <param name="bindingStrategies">
/// Optional custom binding strategies to use.
/// Order matters, strategies will be tried in the order they are provided.
/// If not provided, <see cref="DefaultBindingStrategies"/> will be used.
/// </param>
public class FormBinder(IForm form, IEnumerable<IFormBindingStrategy>? bindingStrategies = null)
{
	private readonly IForm _form = form;
	private readonly IEnumerable<IFormBindingStrategy> _bindingStrategies =
		bindingStrategies ?? DefaultBindingStrategies;

	/// <summary>
	/// Default binding strategies to cover the included node types.
	/// </summary>
	public static IEnumerable<IFormBindingStrategy> DefaultBindingStrategies =>
		[
			new BooleanBindingStrategy(),
			new NumberBindingStrategy(),
			new TimestampBindingStrategy(),
			new TextBindingStrategy(),
		];

	/// <summary>
	/// Reads the form values and documents from the HTTP request and writes them into their corresponding nodes in the form model.
	/// </summary>
	/// <param name="request">The request to read the form values and documents from.</param>
	public async Task BindAsync(HttpRequest request)
	{
		if (!request.HasFormContentType)
		{
			return;
		}

		foreach (var formValue in request.Form)
		{
			LoadValue(formValue.Key, formValue.Value);
		}

		foreach (var formFile in request.Form.Files)
		{
			await LoadDocumentAsync(formFile);
		}
	}

	private void LoadValue(string fieldId, StringValues values)
	{
		var node = _form.FindFirst(node => node.GetId() == fieldId);
		if (node == null)
		{
			// No node matches the path.
			return;
		}

		foreach (var bindingStrategy in _bindingStrategies)
		{
			if (bindingStrategy.TryBind(node, values))
			{
				// Value was successfully bound, stop trying other strategies.
				return;
			}
		}
	}

	private async Task LoadDocumentAsync(IFormFile formFile)
	{
		var node = _form.FindFirst(node => node.GetId() == formFile.Name);

		if (node is not IFileNode fileNode)
		{
			return;
		}

		await using var memoryStream = new MemoryStream();
		await formFile.CopyToAsync(memoryStream);
		var fileBytes = memoryStream.ToArray();

		fileNode.Value = new FileValue(formFile.FileName, fileBytes);
	}
}
