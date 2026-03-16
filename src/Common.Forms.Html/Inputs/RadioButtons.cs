namespace RobinEpple.Common.Forms.Html.Inputs;

using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Html.Components;
using static RobinEpple.Common.Html.DSL;

/// <summary>
/// Renders a radio button for each select list item defined in the <paramref name="node"/>.
/// </summary>
/// <typeparam name="TValue">The value type of the node.</typeparam>
/// <param name="node">The node to render the radio buttons for.</param>
public class RadioButtons<TValue>(IValueNode<TValue> node) : IHtmlContent
{
	private readonly IValueNode<TValue> _node = node;

	/// <inheritdoc />
	public void WriteTo(TextWriter writer, HtmlEncoder encoder)
	{
		var content = Fieldset("");
		var fb = new FormBuilder("test").WithBooleanNode("");
	}
}
