namespace RobinEpple.Common.WebUi;

using System.Globalization;
using Microsoft.Extensions.Primitives;
using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// A basic strategy to parse numeric values to a number node.
/// </summary>
public class NumberBindingStrategy : IFormBindingStrategy
{
	/// <inheritdoc />
	public bool TryBind(IFormNode node, StringValues values)
	{
		if (node is not INumberNode numberNode)
		{
			return false;
		}

		var stringValue = values.FirstOrDefault();
		if (string.IsNullOrEmpty(stringValue))
		{
			numberNode.Value = null;
			return true;
		}

		if (!decimal.TryParse(stringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var numberValue))
		{
			return false;
		}

		numberNode.Value = numberValue;
		return true;
	}
}
