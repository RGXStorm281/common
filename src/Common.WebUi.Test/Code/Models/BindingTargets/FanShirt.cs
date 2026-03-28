namespace RobinEpple.Common.WebUi.Test.Code.Models.BindingTargets;

public class FanShirt : IProduct
{
	public string? Size { get; set; }
	public string? Design { get; set; }
	public int? Amount { get; set; }

	public string PrintSummary() => $"{Amount} Fan Shirt {Design} ({Size})";
}
