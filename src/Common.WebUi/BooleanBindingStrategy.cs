namespace RobinEpple.Common.WebUi;

using Microsoft.Extensions.Primitives;
using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// A basic strategy to parse true/false values to a boolean node.
/// </summary>
public class BooleanBindingStrategy : IFormBindingStrategy
{
	/// <inheritdoc />
	public bool TryBind(IFormNode node, StringValues values)
	{
		if (node is not IBooleanNode booleanNode)
		{
			return false;
		}

		var stringValue = values.FirstOrDefault();
		if (string.IsNullOrEmpty(stringValue))
		{
			booleanNode.Value = null;
			return true;
		}

		if (booleanNode.Formatter.Parse(stringValue) is not bool booleanValue)
		{
			return false;
		}

		booleanNode.Value = booleanValue;
		return true;
	}
}
