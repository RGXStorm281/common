namespace RobinEpple.Common.Forms.BasicForm.DataContainers;

using RobinEpple.Common.Forms.BasicApi.DataContainers;

/// <summary>
/// The complex data container for a basic form.
/// </summary>
public class BasicContainerDataContainer : IContainerDataContainer
{
	/// <inheritdoc />
	public IDictionary<string, IFormDataContainer> ChildDataContainersByChildId { get; set; } =
		new Dictionary<string, IFormDataContainer>();
}
