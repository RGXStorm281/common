namespace RobinEpple.Common.Forms.SelectLists;

/// <summary>
/// Defines the interface for a single value that can be offered for selection in a form field..
/// </summary>
/// <typeparam name="TValue">The value type of the selection item.</typeparam>
public interface ISelectListItem<TValue>
{
	public string Label { get; }
	public TValue Value { get; }
}
