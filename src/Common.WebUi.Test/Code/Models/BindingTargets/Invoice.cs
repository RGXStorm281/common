namespace RobinEpple.Common.WebUi.Test.Code.Models.BindingTargets;

public class Invoice : IPaymentMethod
{
	public string? Owner { get; set; }
	public string? Iban { get; set; }
}
