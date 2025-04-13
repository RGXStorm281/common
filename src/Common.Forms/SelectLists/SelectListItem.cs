namespace RobinEpple.Common.Forms.SelectLists;

public class SelectListItem<TValue>(string label, TValue value) : ISelectListItem<TValue>
{
	/// <inheritdoc />
	public string Label { get; } = label;

	/// <inheritdoc />
	public TValue Value { get; } = value;
}
