namespace RobinEpple.Common.WebUi.Test.Code.Models;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Html;
using RobinEpple.Common.Forms;
using RobinEpple.Common.Forms.Building;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.SelectLists;
using RobinEpple.Common.Forms.Wrappers.Abstractions;
using static RobinEpple.Common.Forms.Html.FormRendering;
using static RobinEpple.Common.Html.DSL;

public partial class FormRenderingPage : IPageModel
{
	public FormRenderingPage()
	{
		CreateForm();
	}

	public string Title { get; } = "Form Rendering";

	public IForm Form { get; private set; }

	[MemberNotNull(nameof(Form))]
	[WrapFormStructure(nameof(Form))]
	private void CreateForm()
	{
		Form = new FormBuilder("TestForm")
			// Booleans
			.WithBooleanNode("BooleanHidden")
			.WithBooleanNode(
				"BooleanRadio",
				node =>
					node.UseSelectList(
						ISelectListSource<bool?>.ForLabelledValues(
							[(null, "unselected"), (true, "Yes"), (false, "No")]
						),
						validate: true
					)
			)
			.WithBooleanNode(
				"BooleanDropdown",
				node =>
					node.UseSelectList(
						ISelectListSource<bool?>.ForLabelledValues(
							[(null, string.Empty), (true, "Yes"), (false, "No")]
						),
						validate: true
					)
			)
			.WithBooleanNode("BooleanCheckbox", node => node.UseLabel("Custom Checkbox Label"))
			// Files
			.WithFileNode("FileHidden")
			.WithFileNode(
				"FileUpload",
				node => node.UseLabel("Custom File Upload Label").UseFileExtensionValidator(["jpg", "jpeg", "png"])
			)
			// Numbers
			.WithNumberNode("NumberHidden")
			.WithNumberNode(
				"NumberRadio",
				node =>
					node.UseSelectList(
						ISelectListSource<decimal?>.ForLabelledValues([(null, "unselected"), (0, "zero"), (1, "one")]),
						validate: true
					)
			)
			.WithNumberNode(
				"NumberDropdown",
				node =>
					node.UseSelectList(
						ISelectListSource<decimal?>.ForLabelledValues([(null, string.Empty), (0, "zero"), (1, "one")]),
						validate: true
					)
			)
			.WithNumberNode("NumberInput", node => node.UseLabel("Custom Number Input Label"))
			.WithNumberNode(
				"NumberRange",
				node =>
					node.UseSelectList(
							ISelectListSource<decimal?>.ForLabelledValues([(0, "zero"), (1, "one")]),
							validate: true
						)
						.UseLabel("Custom Range Input Label")
			)
			// Texts
			.WithTextNode("TextHidden")
			.WithTextNode(
				"TextRadio",
				node =>
					node.UseSelectList(
						ISelectListSource<string?>.ForLabelledValues(
							[(null, "unselected"), ("a", "I want 'a'"), ("b", "I want 'b'")]
						),
						validate: true
					)
			)
			.WithTextNode(
				"TextDropdown",
				node =>
					node.UseSelectList(
						ISelectListSource<string?>.ForLabelledValues(
							[(null, string.Empty), ("a", "I want 'a'"), ("b", "I want 'b'")]
						),
						validate: true
					)
			)
			.WithTextNode(
				"TextEmail",
				node =>
					node.UseLabel("Email")
						.UseEmailValidator()
						.UseSelectList(
							ISelectListSource<string?>.ForLabelledValues([("sample@mail.de", "Select sample mail")]),
							validate: false
						)
			)
			.WithTextNode("TextPassword", node => node.UseLabel("Password"))
			.WithTextNode("TextSearch", node => node.UseLabel("Search"))
			.WithTextNode(
				"TextPhone",
				node =>
					node.UseLabel("Phone")
						.UsePhoneNumberValidator()
						.UseSelectList(
							ISelectListSource<string?>.ForLabelledValues(
								[("+49 157 12345678", "Select sample phone number")]
							),
							validate: false
						)
			)
			.WithTextNode("TextInput", node => node.UseLabel("Plain Text"))
			.WithTextNode("TextArea", node => node.UseLabel("Multiline Text"))
			.WithTextNode(
				"TextUrl",
				node =>
					node.UseLabel("URL")
						.UseSelectList(
							ISelectListSource<string?>.ForLabelledValues([("www.google.de", "Select sample URL")]),
							validate: false
						)
			)
			// Dates
			.WithTimestampNode("DateHidden")
			.WithTimestampNode(
				"DateRadio",
				node =>
					node.UseSelectList(
						ISelectListSource<DateTime?>.ForLabelledValues(
							[(null, "unselected"), (DateTime.Today, "Today"), (DateTime.Today.AddDays(1), "Tomorrow")]
						),
						validate: true
					)
			)
			.WithTimestampNode(
				"DateDropdown",
				node =>
					node.UseSelectList(
						ISelectListSource<DateTime?>.ForLabelledValues(
							[(null, string.Empty), (DateTime.Today, "Today"), (DateTime.Today.AddDays(1), "Tomorrow")]
						),
						validate: true
					)
			)
			.WithTimestampNode(
				"DateInput",
				node =>
					node.UseLabel("Full Timestamp")
						.UseSelectList(
							ISelectListSource<DateTime?>.ForLabelledValues(
								[(DateTime.Today, "Today"), (DateTime.Today.AddDays(1), "Tomorrow")]
							),
							validate: false
						)
			)
			.WithTimestampNode("DateOnlyTime", node => node.UseLabel("Time"))
			.WithTimestampNode("DateOnlyMonth", node => node.UseLabel("Month"))
			.WithTimestampNode("DateWeek", node => node.UseLabel("Week"))
			.Build();
	}

	public IHtmlContent Render()
	{
		// Update before rendering.
		Form.Update();

		return Div(
			H1("Form Rendering Test Page"),
			P(
				"This page is used to test the rendering of form inputs. It contains various form elements that should be rendered correctly."
			),
			Form(
					H2("Booleans"),
					H3("Hidden boolean"),
					HiddenInput(FormWrapper.BooleanHidden!),
					H3("Tri-State boolean as radio button"),
					RadioButtons(FormWrapper.BooleanRadio!),
					H3("Tri-State boolean as drop-down"),
					DropDown(FormWrapper.BooleanDropdown!),
					H3("Boolean as checkbox"),
					CheckBox(FormWrapper.BooleanCheckbox!),
					H2("Files"),
					H3("Hidden file input"),
					HiddenInput(FormWrapper.FileHidden!),
					H3("File Input"),
					FileInput(FormWrapper.FileUpload!)
				)
				.Name(Form.Name)
				.Attribute("hx-put", "")
				.Attribute("hx-trigger", "change")
				.Attribute("hx-encoding", "multipart/form-data")
				.Attribute("hx-swap", "innerHTML")
				.Attribute("hx-select", "form > *")
		);
	}
}
