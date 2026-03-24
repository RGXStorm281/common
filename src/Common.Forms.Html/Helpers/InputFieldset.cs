namespace RobinEpple.Common.Forms.Html.Helpers;

using Microsoft.AspNetCore.Html;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Html.Components;
using static RobinEpple.Common.Forms.Html.FormRendering;
using static RobinEpple.Common.Html.DSL;

/// <summary>
/// Renders a fieldset for the node.
/// </summary>
public class InputFieldset : Fieldset
{
	/// <summary>
	/// Renders a fieldset for the <paramref name="node"/>.
	/// </summary>
	/// <param name="node">The node to render the fieldset for.</param>
	/// <param name="contents">The contents of the fieldset.</param>
	public InputFieldset(IFieldNode node, params IEnumerable<IHtmlContent> contents)
		: base(contents)
	{
		var nodeId = node.GetId();
		Id($"{nodeId}_container");
		this.ConfigureIf(ShouldRenderErrorsFor(node), fieldset => fieldset.Class("error"));
		this.ConfigureIf(ShouldRenderValidFor(node), fieldset => fieldset.Class("valid"));
	}
}
