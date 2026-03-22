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

	private const string? _paymentMethodPaypal = "PayPal";
	private const string? _paymentMethodApplePay = "Apple Pay";
	private const string? _paymentMethodCard = "Card";
	private const string? _paymentMethodInvoice = "Invoice";

	private IEnumerable<string?> _paymentMethods =
	[
		_paymentMethodPaypal,
		_paymentMethodApplePay,
		_paymentMethodCard,
		_paymentMethodInvoice,
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
											BooleanFieldValue("SameBillingAddress").IsEqualTo(StaticValue<bool?>(false))
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
						.WithTextNode(
							"PaymentMethod",
							method =>
								method
									.UseLabel("Payment Method")
									.UseSelectList(_paymentMethods, validate: true)
									.UseRequiredValidator()
						)
						.WithSection(
							"CardDetails",
							(card, _) =>
								card.UseLabel("Card Details")
									.UseVisibilityCondition(
										Elevate(
											StaticValue(1),
											TextFieldValue("PaymentMethod").IsEqualTo(StaticValue(_paymentMethodCard))
										)
									)
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
									.WithNumberNode("Cvc", cvc => cvc.UseLabel("CVC").UseRequiredValidator())
						)
						.WithSection(
							"InvoiceDetails",
							(invoice, _) =>
								invoice
									.UseLabel("Bank Account")
									.UseVisibilityCondition(
										Elevate(
											StaticValue(1),
											TextFieldValue("PaymentMethod")
												.IsEqualTo(StaticValue(_paymentMethodInvoice))
										)
									)
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
										iban => iban.UseLabel("IBAN").UseIbanValidator().UseRequiredValidator()
									)
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
						.Case(
							_cartStage,
							RenderCartStage(FormWrapper.Cart, null, $"{FormWrapper.CurrentPage!.GetId()}_1")
						)
						.Case(_deliveryStage, RenderDeliveryStage(FormWrapper.Delivery))
						.Case(_paymentStage, RenderPaymentStage(FormWrapper.Payment))
						.Case(_confirmationStage, RenderConfirmationStage(FormWrapper.Confirmation))
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
												Div(DropDown(shirt.Size!), NumberInput(shirt.Amount!))
													.Class("cart-item-config"),
												RenderRemoveItemButton(shirt.Node!.GetId())
											)
											.Class("cart-item")
									)
									.Case<CheckoutStruct.CartStruct.CartItemsStruct.ChocolateStruct>(chocolate =>
										Div(
												Span(chocolate.Node!.Label).Class("cart-item-label"),
												Div(DropDown(chocolate.Flavor!), NumberInput(chocolate.Amount!))
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
							.Class("add-button"),
						Button($"Add {cart.CartItems.ChocolateTemplate.Node!.Label}")
							.Attribute("hx-post", "AddChocolate")
							.Attribute("hx-target", "#sample-checkout-form")
							.Attribute("hx-swap", "innerHTML")
							.Attribute("hx-select", "form > *")
							.Class("add-button")
					)
					.Class("cart-actions"),
				Div(
						RenderIf(previousId != null, Label("Previous").For(previousId!).Class("previous-button")),
						RenderIf(nextId != null, Label("Next").For(nextId!).Class("next-button"))
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
			.Class("remove-cart-item");
	}

	private static IHtmlContent RenderDeliveryStage(CheckoutStruct.DeliveryStruct delivery)
	{
		return Div().Class("delivery-stage");
	}

	private static IHtmlContent RenderPaymentStage(CheckoutStruct.PaymentStruct payment)
	{
		return Div().Class("payment-stage");
	}

	private static IHtmlContent RenderConfirmationStage(CheckoutStruct.ConfirmationStruct confirmation)
	{
		return Div().Class("confirmation-stage");
	}
}
