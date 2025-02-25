namespace RobinEpple.Common.Forms.BasicApi.DataContainers;

/// <summary>
/// The data container for a complex node.
/// </summary>
public interface IContainerDataContainer : IFormDataContainer
{
	/// <summary>
	/// The data of the children, keyed by the id of the children.
	/// </summary>
	public IDictionary<string, IFormDataContainer> ChildDataContainersByChildId { get; set; }
}
