using RobinEpple.Common.Forms.BasicApi.DataContainers;

namespace RobinEpple.Common.Forms.BasicForm.DataContainers;

/// <summary>
/// The template data container for a basic form.
/// </summary>
public class BasicTemplateDataContainer : BasicContainerDataContainer, ITemplateDataContainer
{
	/// <inheritdoc />
	public string TemplateId { get; set; } = string.Empty;
}