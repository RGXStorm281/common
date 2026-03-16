namespace RobinEpple.Common.WebUi;

using System.Globalization;
using Microsoft.Extensions.Primitives;
using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// A basic strategy to bind timestamp values to a timestamp node.
/// </summary>
public class TimestampBindingStrategy : IFormBindingStrategy
{
	/// <inheritdoc />
	public bool TryBind(IFormNode node, StringValues values)
	{
		if (node is not ITimestampNode timestampNode)
		{
			return false;
		}

		var stringValue = values.FirstOrDefault();
		if (string.IsNullOrEmpty(stringValue))
		{
			timestampNode.Value = null;
			return true;
		}

		if (!DateTime.TryParse(stringValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out var timestampValue))
		{
			return false;
		}

		timestampNode.Value = timestampValue;
		return true;
	}
}
