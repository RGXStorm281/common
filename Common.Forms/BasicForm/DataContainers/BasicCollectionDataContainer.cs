namespace RobinEpple.Common.Forms.BasicForm.DataContainers;

using RobinEpple.Common.Forms.BasicApi.DataContainers;

/// <summary>
/// The collection data container for a basic form.
/// </summary>
public class BasicCollectionDataContainer : ICollectionDataContainer
{
	/// <summary>
	/// The mutable data list.
	/// </summary>
	public IList<IFormDataContainer> MutableList { get; set; } = new List<IFormDataContainer>();

	/// <inheritdoc />
	public IEnumerable<IFormDataContainer> ItemDataContainers
	{
		get => MutableList;
		set => MutableList = value.ToList();
	}
}
