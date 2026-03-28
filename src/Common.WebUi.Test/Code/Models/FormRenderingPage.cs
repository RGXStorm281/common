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
					Div(
							H2("Booleans").Class("card-header"),
							Div(
									HiddenInput(FormWrapper.BooleanHidden!),
									RadioButtons(FormWrapper.BooleanRadio!),
									DropDown(FormWrapper.BooleanDropdown!),
									CheckBox(FormWrapper.BooleanCheckbox!)
								)
								.Class("card-body")
								.Class("form-grid striped")
						)
						.Class("card"),
					// Numbers
					Div(
							H2("Numbers").Class("card-header"),
							Div(
									HiddenInput(FormWrapper.NumberHidden!),
									RadioButtons(FormWrapper.NumberRadio!),
									DropDown(FormWrapper.NumberDropdown!),
									NumberInput(FormWrapper.NumberInput!),
									RangeInput(FormWrapper.NumberRange!, min: -1, max: 2)
								)
								.Class("card-body")
								.Class("form-grid striped")
						)
						.Class("card"),
					// Texts
					Div(
							H2("Texts").Class("card-header"),
							Div(
									HiddenInput(FormWrapper.TextHidden!),
									RadioButtons(FormWrapper.TextRadio!),
									DropDown(FormWrapper.TextDropdown!),
									EmailInput(FormWrapper.TextEmail!),
									P(
										"This will reset on change, since passwords should not be written to the value attribute in plain text "
											+ "-> HTMX is not suitable for password fields without morphing algorithms."
									),
									PasswordInput(FormWrapper.TextPassword!),
									SearchInput(FormWrapper.TextSearch!),
									TelephoneInput(FormWrapper.TextPhone!),
									TextInput(FormWrapper.TextInput!),
									TextAreaInput(FormWrapper.TextArea!),
									UrlInput(FormWrapper.TextUrl!)
								)
								.Class("card-body")
								.Class("form-grid striped")
						)
						.Class("card"),
					// Dates
					Div(
							H2("Dates").Class("card-header"),
							Div(
									HiddenInput(FormWrapper.DateHidden!),
									RadioButtons(FormWrapper.DateRadio!),
									DropDown(FormWrapper.DateDropdown!),
									DateInput(FormWrapper.DateInput!),
									TimeInput(FormWrapper.TimeInput!),
									DateTimeInput(FormWrapper.DateTimeInput!)
								)
								.Class("card-body")
								.Class("form-grid striped")
						)
						.Class("card"),
					// Files
					Div(
							H2("Files").Class("card-header"),
							Div(HiddenInput(FormWrapper.FileHidden!), FileInput(FormWrapper.FileUpload!))
								.Class("card-body")
								.Class("form-grid striped")
						)
						.Class("card")
				)
				.Name(Form.Name)
				.Id("sample-rendering-form")
				.Attribute("hx-put", "")
				.Attribute("hx-trigger", "change")
				.Attribute("hx-encoding", "multipart/form-data")
				.Attribute("hx-swap", "innerHTML")
				.Attribute("hx-select", "form > *")
		);
	}
}
