namespace RobinEpple.Common.WebUi.Test.Code.Models;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Html;
using RobinEpple.Common.Forms;
using RobinEpple.Common.Forms.Building;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.Wrappers.Abstractions;
using RobinEpple.Common.WebUi.Test.Code.Models.FolderModel;
using static RobinEpple.Common.Forms.Html.FormRendering;
using static RobinEpple.Common.Html.DSL;

public partial class RecursiveSamplePage : IPageModel
{
	public RecursiveSamplePage()
	{
		RootFolder = new Folder
		{
			Name = "Root",
			Items =
			[
				new Folder
				{
					Name = "My Notes",
					Items =
					[
						new Note
						{
							Name = "Nested Note",
							Contents =
								"This is a note nested in the predefined folder. Try adding new folders and notes as you like, and watch the recursive structure unfold!",
						},
					],
				},
				new Note { Name = "Hello World", Contents = "Hey, this is a predefined root level Note!" },
			],
		};
		CreateForm();
		Form.LoadFromBinding();
		SelectedItem = FormWrapper.RootFolder.Instance!.Node!;
	}

	public Folder RootFolder { get; set; }
	public IFormNode SelectedItem { get; set; }
	public IForm Form { get; private set; }

	[MemberNotNull(nameof(Form))]
	[WrapFormStructure(nameof(Form))]
	private void CreateForm()
	{
		Form = new FormBuilder("Notes")
			.WithTemplatedSection(
				"RootFolder",
				(section, _) =>
					section
						.UsePropertyBinding(() => RootFolder)
						.UseTemplate(
							"Folder",
							folder =>
								folder
									.UseLabel("Folder")
									.UseEmbeddedModel(() => new Folder(), out var folderModel)
									.WithTextNode(
										"Name",
										name =>
											name.UseLabel("Name")
												.UseRequiredValidator()
												.UseEmbeddedModelPropertyBinding(folderModel, model => model.Name)
									)
									.WithCollectionNode(
										"Items",
										(items, recursiveTemplate) =>
											items
												.UseLabel("Items")
												.UseEmbeddedModelPropertyBinding(folderModel, model => model.Items)
												.UsePreConfiguredTemplate(recursiveTemplate)
												.UseTemplate(
													"Note",
													note =>
														note.UseLabel("Note")
															.UseEmbeddedModel(() => new Note(), out var noteModel)
															.WithTextNode(
																"Name",
																name =>
																	name.UseLabel("Name")
																		.UseRequiredValidator()
																		.UseEmbeddedModelPropertyBinding(
																			noteModel,
																			model => model.Name
																		)
															)
															.WithTextNode(
																"Contents",
																contents =>
																	contents
																		.UseLabel("Contents")
																		.UseEmbeddedModelPropertyBinding(
																			noteModel,
																			model => model.Contents
																		)
															)
												)
									)
						)
			)
			.Build();
	}

	public string Title => "Recursive Folder Structure";

	public IHtmlContent Render()
	{
		return Div(
			H1("Recursive Form Sample"),
			P(
				"Especially on backend or meta-configuration sites, you might need recursive forms. Here are some examples:"
			),
			Ul(
				Li("A survey builder with unlimited sub-question depth."),
				Li(
					"Editing organizational structures, like employees and their subordinates, which in turn may have subordinates again, ..."
				),
				Li("A filter edit dialog which encodes a boolean expression tree."),
				Li("...")
			),
			P(
				"Modelling these structures in C# is easy, and explicit rendering is also pretty straight forward with recursive partial calls."
					+ "But where most form frameworks break is the binding part: Modelling recursive forms, updating them from web requests and binding to a C# model."
					+ "Well, not this one ;)"
			),
			P(
				"Below you can see a simple recursive example in form of a basic notes app. Notes can be organized in arbitrarily deep nested folders."
			),
			Form(Div(Div(RenderColumn(FormWrapper.RootFolder.Instance!)).Class("columns")).Class("scroll-container"))
				.Name(Form.Name)
				.Id("sample-recursive-form")
				.Attribute("hx-put", "")
				.Attribute("hx-trigger", "change")
				.Attribute("hx-encoding", "multipart/form-data")
				.Attribute("hx-swap", "innerHTML")
				.Attribute("hx-select", "form > *")
		);
	}

	private bool IsPartOfPathTo(IFormNode part, IFormNode pathLeaf)
	{
		var current = pathLeaf;

		// Check all parents recursively until root is reached or the part is found.
		while (current != null)
		{
			if (part == current)
			{
				return true;
			}

			current = current.Parent;
		}

		return false;
	}

	private IHtmlContent RenderColumn(IFormWrapper wrapper)
	{
		return wrapper switch
		{
			NotesStruct.RootFolderStruct.FolderStruct folder => RenderFolderColumn(folder),
			NotesStruct.RootFolderStruct.FolderStruct.ItemsStruct.NoteStruct note => RenderNoteColumn(note),
			_ => throw new ArgumentOutOfRangeException($"Unhandled column type {wrapper.GetType().FullName}."),
		};
	}

	private IHtmlContent RenderFolderColumn(NotesStruct.RootFolderStruct.FolderStruct folder)
	{
		var folderId = folder.Node!.GetId();
		var nextColumn = folder.Items.Instances.FirstOrDefault(item => IsPartOfPathTo(item.Node!, SelectedItem));
		return Concat(
			Div(
					H2("Folder"),
					TextInput(folder.Name!),
					Button("Create new folder")
						.Attribute("hx-post", $"CreateFolder?parentId={folderId}")
						.Attribute("hx-target", "#sample-recursive-form")
						.Attribute("hx-swap", "innerHTML")
						.Attribute("hx-select", "form > *")
						.Class("btn"),
					Button("Create new note")
						.Attribute("hx-post", $"CreateNote?parentId={folderId}")
						.Attribute("hx-target", "#sample-recursive-form")
						.Attribute("hx-swap", "innerHTML")
						.Attribute("hx-select", "form > *")
						.Class("btn"),
					RenderEach(
						folder.Items.Instances.OrderByDescending(item =>
							item is NotesStruct.RootFolderStruct.FolderStruct
						),
						item =>
							RenderSwitch(item)
								.Case<NotesStruct.RootFolderStruct.FolderStruct>(subfolder =>
									Div(
											RenderDeleteButton(subfolder.Node!),
											RenderSelectableElement(subfolder.Node!, subfolder.Name?.Value)
										)
										.Class("item folder-item")
								)
								.Case<NotesStruct.RootFolderStruct.FolderStruct.ItemsStruct.NoteStruct>(note =>
									Div(
											RenderDeleteButton(note.Node!),
											RenderSelectableElement(note.Node!, note.Name?.Value)
										)
										.Class("item note-item")
								)
					)
				)
				.Class("folder-column"),
			RenderIf(nextColumn != null, Lazy(() => RenderColumn(nextColumn!)))
		);
	}

	private IHtmlContent RenderSelectableElement(IFormNode node, string? name)
	{
		return Span(name ?? string.Empty)
			.Attribute("hx-post", $"SelectNoteItem?itemId={node.GetId()}")
			.Attribute("hx-target", "#sample-recursive-form")
			.Attribute("hx-swap", "innerHTML")
			.Attribute("hx-select", "form > *");
	}

	private IHtmlContent RenderDeleteButton(IFormNode node)
	{
		return Button("Delete")
			.Attribute("hx-post", $"RemoveNoteItem?itemId={node.GetId()}")
			.Attribute("hx-target", "#sample-recursive-form")
			.Attribute("hx-swap", "innerHTML")
			.Attribute("hx-select", "form > *")
			.Class("btn");
	}

	private IHtmlContent RenderNoteColumn(NotesStruct.RootFolderStruct.FolderStruct.ItemsStruct.NoteStruct note)
	{
		return Div(H2("Note"), TextInput(note.Name!), TextAreaInput(note.Contents!)).Class("note-column");
	}
}
