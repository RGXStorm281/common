namespace RobinEpple.Common.WebUi.Test.Code.Models.BindingTargets;

public class FanChocolate : IProduct
{
	public string? Flavor { get; set; }
	public int? Amount { get; set; }

	public string PrintSummary() => $"{Amount}x Chocolate {Flavor}";
}
