namespace RobinEpple.Common.WebUi.Test.Code.Models.BindingTargets;

public class CheckoutModel
{
	public IEnumerable<IProduct> Products { get; set; } = [];
	public DeliveryDetails ContactDetails { get; set; } = new();
	public BillingAddress BillingAddress { get; set; } = new();
	public IPaymentMethod? PaymentMethod { get; set; }
	public bool? ConfirmedTermsOfService { get; set; }
}
