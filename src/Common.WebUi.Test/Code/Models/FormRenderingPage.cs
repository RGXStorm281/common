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
			.WithNumberNode(
				"NumberInput",
				node =>
					node.UseSelectList(
							ISelectListSource<decimal?>.ForLabelledValues([(0, "zero"), (1, "one")]),
							validate: true
						)
						.UseLabel("Custom Number Input Label")
			)
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
					node.UseLabel("Date only")
						.UseSelectList(
							ISelectListSource<DateTime?>.ForLabelledValues(
								[
									(null, string.Empty),
									(DateTime.Today, "Today"),
									(DateTime.Today.AddDays(1), "Tomorrow"),
								]
							),
							validate: true
						)
			)
			.WithTimestampNode(
				"TimeInput",
				node =>
					node.UseLabel("Time only")
						.UseSelectList(
							ISelectListSource<DateTime?>.ForLabelledValues(
								[
									(null, string.Empty),
									(DateTime.Today.AddHours(9), "9am"),
									(DateTime.Today.AddHours(21), "9pm"),
								]
							),
							validate: true
						)
			)
			.WithTimestampNode(
				"DateTimeInput",
				node =>
					node.UseLabel("Date and time")
						.UseSelectList(
							ISelectListSource<DateTime?>.ForLabelledValues(
								[
									(null, string.Empty),
									(DateTime.Today.AddHours(9), "Today at 9am"),
									(DateTime.Today.AddHours(21), "Today at 9pm"),
								]
							),
							validate: false
						)
			)
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
					// Booleans
					H2("Booleans"),
					H3("Hidden boolean input"),
					HiddenInput(FormWrapper.BooleanHidden!),
					H3("Tri-State boolean as radio button"),
					RadioButtons(FormWrapper.BooleanRadio!),
					H3("Tri-State boolean as drop-down"),
					DropDown(FormWrapper.BooleanDropdown!),
					H3("Boolean as checkbox"),
					CheckBox(FormWrapper.BooleanCheckbox!),
					// Files
					H2("Files"),
					H3("Hidden file input"),
					HiddenInput(FormWrapper.FileHidden!),
					H3("File Input"),
					FileInput(FormWrapper.FileUpload!),
					// Numbers
					H2("Numbers"),
					H3("Hidden number input"),
					HiddenInput(FormWrapper.NumberHidden!),
					H3("Number radio selection"),
					RadioButtons(FormWrapper.NumberRadio!),
					H3("Number dropdown"),
					DropDown(FormWrapper.NumberDropdown!),
					H3("Number input with data-list"),
					NumberInput(FormWrapper.NumberInput!),
					H3("Range input with data-list"),
					RangeInput(FormWrapper.NumberRange!, min: -1, max: 2),
					// Texts
					H2("Texts"),
					H3("Hidden text input"),
					HiddenInput(FormWrapper.TextHidden!),
					H3("Radio Buttons for text selection"),
					RadioButtons(FormWrapper.TextRadio!),
					H3("Dropdown for text value"),
					DropDown(FormWrapper.TextDropdown!),
					H3("Email input"),
					EmailInput(FormWrapper.TextEmail!),
					H3("Password input"),
					P(
						"This will reset on change, since passwords should not be written to the value attribute in plain text "
							+ "-> HTMX is not suitable for password fields without morphing algorithms."
					),
					PasswordInput(FormWrapper.TextPassword!),
					H3("Search input"),
					SearchInput(FormWrapper.TextSearch!),
					H3("Telephone input"),
					TelephoneInput(FormWrapper.TextPhone!),
					H3("Textbox"),
					TextInput(FormWrapper.TextInput!),
					H3("Text Area"),
					TextAreaInput(FormWrapper.TextArea!),
					H3("Url input"),
					UrlInput(FormWrapper.TextUrl!),
					// Dates
					H2("Dates"),
					H3("Date hidden"),
					HiddenInput(FormWrapper.DateHidden!),
					H3("Radio button date selection"),
					RadioButtons(FormWrapper.DateRadio!),
					H3("Dropdown for dates"),
					DropDown(FormWrapper.DateDropdown!),
					H3("Date input"),
					DateInput(FormWrapper.DateInput!),
					H3("Time input"),
					TimeInput(FormWrapper.TimeInput!),
					H3("DateTimeInput"),
					DateTimeInput(FormWrapper.DateTimeInput!)
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
