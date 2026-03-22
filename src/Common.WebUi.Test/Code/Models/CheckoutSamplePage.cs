namespace RobinEpple.Common.WebUi.Test.Code.Models;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Html;
using RobinEpple.Common.Forms;
using RobinEpple.Common.Forms.Building;
using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.Wrappers.Abstractions;
using static RobinEpple.Common.Forms.Expressions.FormExpression;
using static RobinEpple.Common.Forms.Html.FormRendering;
using static RobinEpple.Common.Html.DSL;

public partial class CheckoutSamplePage : IPageModel
{
	public CheckoutSamplePage()
	{
		CreateForm();
	}

	public string Title => "Checkout Sample";

	public IForm Form { get; private set; }

	private static readonly decimal? _cartStage = 0;
	private static readonly decimal? _deliveryStage = 1;
	private static readonly decimal? _paymentStage = 2;
	private static readonly decimal? _confirmationStage = 3;

	private IEnumerable<(decimal? Stage, string Name)> _stages =
	[
		(_cartStage, "Shopping Cart"),
		(_deliveryStage, "Delivery"),
		(_paymentStage, "Payment Method"),
		(_confirmationStage, "Confirmation"),
	];

	[MemberNotNull(nameof(Form))]
	[WrapFormStructure(nameof(Form))]
	private void CreateForm()
	{
		Form = new FormBuilder("Checkout")
			// Multi Stage Checkout -> Page selector
			.WithNumberNode(
				"CurrentPage",
				node => node.UseSelectList(_stages, validate: true).UseDefaultValue(_cartStage).UseRequiredValidator()
			)
			// First Stage: Shopping cart -> List of items in cart, in place editing.
			.WithSection(
				"Cart",
				(section, _) =>
					section
						.UseLabel("Shopping Cart")
						.WithCollectionNode(
							"CartItems",
							(collection, _) =>
								collection
									.UseTemplate(
										"Shirt",
										shirt =>
											shirt
												.UseLabel("Fan Shirt")
												.WithTextNode(
													"Size",
													size =>
														size.UseLabel("Size")
															.UseSelectList(["S", "M", "L"], validate: true)
															.UseDefaultValue("M")
															.UseRequiredValidator()
												)
												.WithTextNode(
													"Design",
													size =>
														size.UseLabel("Design")
															.UseSelectList(
																["Kittens", "Trains", "Dragons"],
																validate: true
															)
															.UseDefaultValue("Dragons")
															.UseRequiredValidator()
												)
												.WithNumberNode(
													"Amount",
													amount =>
														amount
															.UseLabel("Amount")
															.UseSelectList(
																Enumerable.Range(1, 20).Select(i => (decimal?)i),
																validate: false
															)
															.UseDefaultValue(1)
															.UseRequiredValidator()
												)
									)
									.UseTemplate(
										"Chocolate",
										chocolate =>
											chocolate
												.UseLabel("Fan Chocolate")
												.WithTextNode(
													"Flavor",
													size =>
														size.UseLabel("Flavor")
															.UseSelectList(["Milk", "Dark", "Orange"], validate: true)
															.UseDefaultValue("Milk")
															.UseRequiredValidator()
												)
												.WithNumberNode(
													"Amount",
													amount =>
														amount
															.UseLabel("Amount")
															.UseSelectList(
																Enumerable.Range(1, 20).Select(i => (decimal?)i),
																validate: false
															)
															.UseDefaultValue(1)
															.UseRequiredValidator()
												)
									)
						)
			)
			// Second stage: Delivery options -> Extensive input format validation.
			.WithSection(
				"Delivery",
				(section, _) =>
					section
						.UseLabel("Delivery Options")
						.WithSection(
							"ContactDetails",
							(contact, _) =>
								contact
									.UseLabel("Contact Details")
									.WithTextNode(
										"FirstName",
										firstName =>
											firstName
												.UseLabel("First Name")
												.UseValidator(new BeginsWithUppercaseValidator())
												.UseRequiredValidator()
									)
									.WithTextNode(
										"LastName",
										firstName =>
											firstName
												.UseLabel("Last Name")
												.UseValidator(new BeginsWithUppercaseValidator())
												.UseRequiredValidator()
									)
									.WithTextNode(
										"PhoneNumber",
										firstName => firstName.UseLabel("Phone Number").UsePhoneNumberValidator()
									)
									.WithTextNode(
										"Email",
										firstName =>
											firstName.UseLabel("E-Mail").UseEmailValidator().UseRequiredValidator()
									)
						)
						.WithSection(
							"ShippingAddress",
							(address, _) =>
								address
									.WithTextNode(
										"Street",
										street => street.UseLabel("Street, Nr.").UseRequiredValidator()
									)
									.WithTextNode(
										"Zip",
										zip =>
											zip.UseLabel("ZIP code")
												.UseValidator(new ZipCodeValidator())
												.UseRequiredValidator()
									)
									.WithTextNode(
										"City",
										zip =>
											zip.UseLabel("City")
												.UseValidator(new BeginsWithUppercaseValidator())
												.UseRequiredValidator()
									)
									.WithTextNode(
										"Country",
										zip =>
											zip.UseLabel("Country")
												.UseDefaultValue("Germany")
												.UseDefaultReadonly(true)
												.UseRequiredValidator()
									)
						)
						.WithTextNode(
							"DeliveryMethod",
							delivery =>
								delivery
									.UseLabel("Delivery Method")
									.UseSelectList(["Default Shipping", "Express Shipping"], validate: true)
									.UseRequiredValidator()
						)
			)
			// Third stage: Payment options -> conditional sub forms.
			.WithSection(
				"Payment",
				(section, _) =>
					section
						.UseLabel("Payment Options")
						.WithBooleanNode(
							"SameBillingAddress",
							sameAddress => sameAddress.UseLabel("Use delivery address as billing address")
						)
						.WithSection(
							"BillingAddress",
							(billingAddress, _) =>
								billingAddress
									.UseLabel("Billing Address")
									.UseVisibilityCondition(
										Elevate(
											StaticValue(1),
											Not(
												BooleanFieldValue("SameBillingAddress")
													.IsEqualTo(StaticValue<bool?>(true))
											)
										)
									)
									.WithTextNode(
										"FirstName",
										firstName =>
											firstName
												.UseLabel("First Name")
												.UseValidator(new BeginsWithUppercaseValidator())
												.UseRequiredValidator()
									)
									.WithTextNode(
										"LastName",
										firstName =>
											firstName
												.UseLabel("Last Name")
												.UseValidator(new BeginsWithUppercaseValidator())
												.UseRequiredValidator()
									)
									.WithTextNode(
										"Street",
										street => street.UseLabel("Street, Nr.").UseRequiredValidator()
									)
									.WithTextNode(
										"Zip",
										zip =>
											zip.UseLabel("ZIP code")
												.UseValidator(new ZipCodeValidator())
												.UseRequiredValidator()
									)
									.WithTextNode(
										"City",
										zip =>
											zip.UseLabel("City")
												.UseValidator(new BeginsWithUppercaseValidator())
												.UseRequiredValidator()
									)
									.WithTextNode(
										"Country",
										zip =>
											zip.UseLabel("Country")
												.UseDefaultValue("Germany")
												.UseDefaultReadonly(true)
												.UseRequiredValidator()
									)
									.WithTextNode(
										"Email",
										firstName =>
											firstName.UseLabel("E-Mail").UseEmailValidator().UseRequiredValidator()
									)
						)
						.WithTemplatedSection(
							"PaymentMethod",
							(method, _) =>
								method
									.UseLabel("Payment Method")
									.UseTemplate("Paypal", paypal => paypal.UseLabel("PayPal"))
									.UseTemplate("ApplePay", applePay => applePay.UseLabel("Apple Pay"))
									.UseTemplate(
										"Card",
										card =>
											card.UseLabel("Card")
												.WithTextNode(
													"Owner",
													owner =>
														owner
															.UseLabel("Owner")
															.UseValidator(new BeginsWithUppercaseValidator())
															.UseRequiredValidator()
												)
												.WithTextNode(
													"CardNumber",
													number => number.UseLabel("Card Number").UseRequiredValidator()
												)
												.WithTimestampNode(
													"Expiry",
													expiry => expiry.UseLabel("Expiry Date").UseRequiredValidator()
												)
												.WithNumberNode(
													"Cvc",
													cvc => cvc.UseLabel("CVC").UseRequiredValidator()
												)
									)
									.UseTemplate(
										"Invoice",
										invoice =>
											invoice
												.UseLabel("Invoice")
												.WithTextNode(
													"Owner",
													owner =>
														owner
															.UseLabel("Owner")
															.UseValidator(new BeginsWithUppercaseValidator())
															.UseRequiredValidator()
												)
												.WithTextNode(
													"Iban",
													iban =>
														iban.UseLabel("IBAN").UseIbanValidator().UseRequiredValidator()
												)
									)
									.UseRequiredValidator()
						)
			)
			// Fourth stage: Confirmation -> terms of service requires checking.
			.WithSection(
				"Confirmation",
				(section, _) =>
					section
						.UseLabel("Confirmation")
						.WithBooleanNode(
							"TermsOfService",
							terms => terms.UseLabel("I accept the terms of service").UseRequireTrueValidator()
						)
			)
			.Build();
	}

	private string GetPageId(decimal? page)
	{
		var pageIndex = FormWrapper
			.CurrentPage?.CurrentSelectListItems?.Index()
			.FirstOrDefault(indexed => indexed.Item.Value == page)
			.Index;

		return $"{FormWrapper.CurrentPage!.GetId()}_{pageIndex}";
	}

	public IHtmlContent Render()
	{
		// Update before rendering.
		Form.Update();

		return Div(
			H1("Sample Checkout page"),
			P("This page a sample to showcase multi-staged forms."),
			Form(
					// Page Selector
					Div(RadioButtons(FormWrapper.CurrentPage!)).Class("page-selector"),
					// Render current page
					RenderSwitch(FormWrapper.CurrentPage!.Value)
						.Case(_cartStage, RenderCartStage(FormWrapper.Cart, null, GetPageId(_deliveryStage)))
						.Case(
							_deliveryStage,
							RenderDeliveryStage(FormWrapper.Delivery, GetPageId(_cartStage), GetPageId(_paymentStage))
						)
						.Case(
							_paymentStage,
							RenderPaymentStage(
								FormWrapper.Payment,
								GetPageId(_deliveryStage),
								GetPageId(_confirmationStage)
							)
						)
						.Case(
							_confirmationStage,
							RenderConfirmationStage(FormWrapper.Confirmation, GetPageId(_paymentStage), null)
						)
				)
				.Name(Form.Name)
				.Id("sample-checkout-form")
				.Attribute("hx-put", "")
				.Attribute("hx-trigger", "change")
				.Attribute("hx-encoding", "multipart/form-data")
				.Attribute("hx-swap", "innerHTML")
				.Attribute("hx-select", "form > *")
		);
	}

	private static IHtmlContent RenderCartStage(CheckoutStruct.CartStruct cart, string? previousId, string? nextId)
	{
		return Div(
				H2(cart.Node!.Label),
				Div(
						RenderEach(
							cart.CartItems.Instances,
							item =>
								RenderSwitch(item)
									.Case<CheckoutStruct.CartStruct.CartItemsStruct.ShirtStruct>(shirt =>
										Div(
												Span(shirt.Node!.Label).Class("cart-item-label"),
												Div(
														DropDown(shirt.Size!),
														DropDown(shirt.Design!),
														NumberInput(shirt.Amount!)
													)
													.Class("form-grid")
													.Class("cart-item-config"),
												RenderRemoveItemButton(shirt.Node!.GetId())
											)
											.Class("cart-item")
									)
									.Case<CheckoutStruct.CartStruct.CartItemsStruct.ChocolateStruct>(chocolate =>
										Div(
												Span(chocolate.Node!.Label).Class("cart-item-label"),
												Div(DropDown(chocolate.Flavor!), NumberInput(chocolate.Amount!))
													.Class("form-grid")
													.Class("cart-item-config"),
												RenderRemoveItemButton(chocolate.Node!.GetId())
											)
											.Class("cart-item")
									)
						)
					)
					.Class("cart-item-list"),
				Div(
						Button($"Add {cart.CartItems.ShirtTemplate.Node!.Label}")
							.Attribute("hx-post", "AddShirt")
							.Attribute("hx-target", "#sample-checkout-form")
							.Attribute("hx-swap", "innerHTML")
							.Attribute("hx-select", "form > *")
							.Class("btn"),
						Button($"Add {cart.CartItems.ChocolateTemplate.Node!.Label}")
							.Attribute("hx-post", "AddChocolate")
							.Attribute("hx-target", "#sample-checkout-form")
							.Attribute("hx-swap", "innerHTML")
							.Attribute("hx-select", "form > *")
							.Class("btn")
					)
					.Class("cart-actions"),
				Div(
						RenderIf(previousId != null, Label("Previous").For(previousId!).Class("btn")),
						RenderIf(nextId != null, Label("Next").For(nextId!).Class("btn primary"))
					)
					.Class("navigation-buttons")
			)
			.Class("cart-stage");
	}

	private static IHtmlContent RenderRemoveItemButton(string itemNodeId)
	{
		return Button("Remove from cart")
			.Attribute("hx-post", $"RemoveCartItem?itemId={itemNodeId}")
			.Attribute("hx-target", "#sample-checkout-form")
			.Attribute("hx-swap", "innerHTML")
			.Attribute("hx-select", "form > *")
			.Class("btn danger");
	}

	private static IHtmlContent RenderDeliveryStage(
		CheckoutStruct.DeliveryStruct delivery,
		string? previousId,
		string? nextId
	)
	{
		return Div(
				H2(delivery.Node!.Label),
				Div(
						H3(delivery.ContactDetails.Node!.Label).Class("card-header"),
						Div(
								TextInput(delivery.ContactDetails.FirstName!),
								TextInput(delivery.ContactDetails.LastName!),
								TextInput(delivery.ContactDetails.PhoneNumber!),
								TextInput(delivery.ContactDetails.Email!)
							)
							.Class("card-body")
							.Class("form-grid striped")
					)
					.Class("card"),
				Div(
						H3(delivery.ShippingAddress.Node!.Label).Class("card-header"),
						Div(
								TextInput(delivery.ShippingAddress.Street!),
								TextInput(delivery.ShippingAddress.Zip!),
								TextInput(delivery.ShippingAddress.City!),
								TextInput(delivery.ShippingAddress.Country!)
							)
							.Class("card-body")
							.Class("form-grid striped")
					)
					.Class("card"),
				Div(
						H3("Delivery Service").Class("card-header"),
						Div(RadioButtons(delivery.DeliveryMethod!)).Class("card-body").Class("form-grid striped")
					)
					.Class("card"),
				Div(
						RenderIf(previousId != null, Label("Previous").For(previousId!).Class("btn")),
						RenderIf(nextId != null, Label("Next").For(nextId!).Class("btn primary"))
					)
					.Class("navigation-buttons")
			)
			.Class("delivery-stage");
	}

	private static IHtmlContent RenderPaymentStage(
		CheckoutStruct.PaymentStruct payment,
		string? previousId,
		string? nextId
	)
	{
		return Div(
				H2(payment.Node!.Label),
				Div(
						H3(payment.BillingAddress.Node!.Label).Class("card-header"),
						Div(
								CheckBox(payment.SameBillingAddress!),
								TextInput(payment.BillingAddress.FirstName!),
								TextInput(payment.BillingAddress.LastName!),
								TextInput(payment.BillingAddress.Street!),
								TextInput(payment.BillingAddress.Zip!),
								TextInput(payment.BillingAddress.City!),
								TextInput(payment.BillingAddress.Country!),
								TextInput(payment.BillingAddress.Email!)
							)
							.Class("card-body")
							.Class("form-grid")
					)
					.Class("card"),
				Div(
						H3(payment.PaymentMethod.Node!.Label).Class("card-header"),
						Div(
								RadioButtonsForTemplates(payment.PaymentMethod.Node!),
								RenderSwitch(payment.PaymentMethod.Instance)
									.Case<CheckoutStruct.PaymentStruct.PaymentMethodStruct.CardStruct>(card =>
										Concat(
											TextInput(card.Owner!),
											TextInput(card.CardNumber!),
											DateInput(card.Expiry!),
											NumberInput(card.Cvc!)
										)
									)
									.Case<CheckoutStruct.PaymentStruct.PaymentMethodStruct.InvoiceStruct>(invoice =>
										Concat(TextInput(invoice.Owner!), TextInput(invoice.Iban!))
									)
							)
							.Class("card-body")
							.Class("form-grid")
					)
					.Class("card"),
				Div(
						RenderIf(previousId != null, Label("Previous").For(previousId!).Class("btn")),
						RenderIf(nextId != null, Label("Next").For(nextId!).Class("btn primary"))
					)
					.Class("navigation-buttons")
			)
			.Class("payment-stage");
	}

	private static IHtmlContent RenderConfirmationStage(
		CheckoutStruct.ConfirmationStruct confirmation,
		string? previousId,
		string? nextId
	)
	{
		return Div(
				H2(confirmation.Node!.Label),
				Div(
						H3("Terms and conditions").Class("card-header"),
						Div(CheckBox(confirmation.TermsOfService!)).Class("card-body").Class("form-grid")
					)
					.Class("card"),
				Div(
						RenderIf(previousId != null, Label("Previous").For(previousId!).Class("btn")),
						RenderIf(nextId != null, Label("Next").For(nextId!).Class("btn primary"))
					)
					.Class("navigation-buttons")
			)
			.Class("confirmation-stage");
	}
}
