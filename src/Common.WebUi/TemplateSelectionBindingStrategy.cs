namespace RobinEpple.Common.WebUi;

using Microsoft.Extensions.Primitives;
using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// A basic strategy to instantiate the template that matches the selection.
/// </summary>
public class TemplateSelectionBindingStrategy : IFormBindingStrategy
{
	/// <inheritdoc />
	public bool TryBind(IFormNode node, StringValues values)
	{
		if (node is not ITemplateNode templateNode)
		{
			return false;
		}

		var templateName = values.LastOrDefault();
		if (templateNode.Instance?.Name == templateName)
		{
			// Template instance is already correct (also in the null case).
			return true;
		}

		var template = templateNode.Templates.FirstOrDefault(template => template.Name == templateName);
		if (template == null)
		{
			// If no template matches, clear the node.
			templateNode.Clear();
			return true;
		}

		// Otherwise instantiate the template.
		templateNode.Instantiate(template);
		return true;
	}
}
