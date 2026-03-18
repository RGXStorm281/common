namespace RobinEpple.Common.WebUi;

using Microsoft.Extensions.Primitives;
using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// A basic strategy to bind text values to a text node.
/// </summary>
public class TextBindingStrategy : IFormBindingStrategy
{
	/// <inheritdoc />
	public bool TryBind(IFormNode node, StringValues values)
	{
		if (node is not ITextNode textNode)
		{
			return false;
		}

		var stringValue = values.FirstOrDefault();
		if (string.IsNullOrEmpty(stringValue))
		{
			textNode.Value = null;
			return true;
		}

		textNode.Value = stringValue;
		return true;
	}
}
