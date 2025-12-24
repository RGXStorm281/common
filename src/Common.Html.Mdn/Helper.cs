namespace RobinEpple.Common.Html.Mdn;

using System.Text;

public class Helper
{
	/// <summary>
	/// Indents each line of the given string by the specified depth.
	/// </summary>
	public static string Indent(string content, int additionalDepth = 1, string indentPattern = "\t")
	{
		var lines = content.Split('\n');
		var indentation = Repeat(indentPattern, additionalDepth);
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

	public static string PascalCase(string attributeName)
	{
		var normalizedName = new StringBuilder();
		var isFirst = true;
		var capitalizeNext = true;

		foreach (var character in attributeName)
		{
			// Always append letters. Capitalize if needed.
			if (char.IsLetter(character))
			{
				if (capitalizeNext)
				{
					normalizedName.Append(char.ToUpper(character));
					capitalizeNext = false;
					isFirst = false;
					continue;
				}

				normalizedName.Append(character);
				isFirst = false;
				continue;
			}

			// Append digits that are not at the start of the name.
			if (char.IsDigit(character) && !isFirst)
			{
				normalizedName.Append(character);
				capitalizeNext = false;
				continue;
			}

			// Skip all other characters and capitalize the next word character (-> PascalCase).
			capitalizeNext = true;
		}

		return normalizedName.ToString();
	}
}
