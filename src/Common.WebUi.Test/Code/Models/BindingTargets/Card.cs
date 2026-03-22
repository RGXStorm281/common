namespace RobinEpple.Common.WebUi.Test.Code.Models.BindingTargets;

public class Card : IPaymentMethod
{
	public string? Owner { get; set; }
	public string? CardNumber { get; set; }
	public DateTime? ExpiryDate { get; set; }
	public decimal? Cvc { get; set; }
}
