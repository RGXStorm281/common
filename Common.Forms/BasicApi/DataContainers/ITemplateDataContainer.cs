namespace RobinEpple.Common.Forms.BasicApi.DataContainers;

/// <summary>
/// The data container for an instantiated template.
/// </summary>
public interface ITemplateDataContainer : IContainerDataContainer
{
	/// <summary>
	/// The id of the template this container carries the instance data for.
	/// </summary>
	public string TemplateId { get; set; }
}
