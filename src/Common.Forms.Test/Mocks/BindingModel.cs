namespace RobinEpple.Common.Forms.Test.Mocks;

using RobinEpple.Common.Forms.Nodes;

public class BindingModel
{
	public bool? BooleanProperty { get; set; }
	public FileValue FileProperty { get; set; }
	public decimal? DecimalProperty { get; set; }
	public double? DoubleProperty { get; set; }
	public float? FloatProperty { get; set; }
	public long? LongProperty { get; set; }
	public int? IntProperty { get; set; }
	public string? TextProperty { get; set; }
	public DateTime? TimestampProperty { get; set; }
	public IEnumerable<int> CollectionProperty { get; set; } = [];
	public int? SectionProperty { get; set; }
}
