namespace RobinEpple.Common.Forms.BasicApi.DataContainers;

/// <summary>
/// The data container for a collection.
/// </summary>
public interface ICollectionDataContainer : IFormDataContainer
{
	/// <summary>
	/// The data containers for the items in the collection.
	/// </summary>
	public IEnumerable<IFormDataContainer> ItemDataContainers { get; set; }
}