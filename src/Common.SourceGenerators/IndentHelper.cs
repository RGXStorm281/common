namespace RobinEpple.Common.SourceGenerators;

using System.Text;

internal class IndentHelper
{
	/// <summary>
	/// Indents each line of the given string by the specified depth.
	/// </summary>
	public static string Indent(string content, int additionalDepth = 1)
	{
		var lines = content.Split('\n');
		var indentation = Repeat("\t", additionalDepth);
		var indentedLines = lines.Select(line =>
		{
			if (string.IsNullOrWhiteSpace(line))
			{
				return line;
			}
			return indentation + line;
		});
		return string.Join("\n", indentedLines);
	}

	private static string Repeat(string text, int numberOfTimes)
	{
		var sb = new StringBuilder();
		for (int i = 0; i < numberOfTimes; i++)
		{
			sb.Append(text);
		}
		return sb.ToString();
	}
}
