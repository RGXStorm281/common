namespace RobinEpple.Common.Forms.BasicForm.DataContainers;

using RobinEpple.Common.Forms.BasicApi.DataContainers;

/// <summary>
/// The template data container for a basic form.
/// </summary>
public class BasicTemplateDataContainer : BasicContainerDataContainer, ITemplateDataContainer
{
	/// <inheritdoc />
	public string TemplateId { get; set; } = string.Empty;
}
