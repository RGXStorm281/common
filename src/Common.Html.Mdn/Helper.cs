namespace RobinEpple.Common.Html.Mdn;

using System.Security;
using System.Text;
using HtmlAgilityPack;

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

	public static string PrintMdnDocumentation(string text, string mdnUrl)
	{
		return PrintDocumentation(text) + PrintMdnReference(mdnUrl);
	}

	public static string PrintDocumentation(string text)
	{
		// Escape <> and other XML characters.
		var escaped = SecurityElement.Escape(text);
		return Indent(escaped, indentPattern: "/// ");
	}

	public static string PrintMdnReference(string mdnUrl)
	{
		var licenseInfo =
			$@"
<br/> <b>This documentation text is derived from MDN and licensed under CC BY-SA 2.5:</b>
<br/> {mdnUrl}
<br/> - by Mozilla Contributors";
		return Indent(licenseInfo, indentPattern: "///");
	}

	public static string PrintMdnLicenseHeader(string mdnUrl)
	{
		var licenseInfo =
			$@"
This file contains documentation text derived from Mozilla Developer Network (MDN) Web Docs.
The article by Mozilla Contributors can be found at:
{mdnUrl}
MDN content is licensed under CC BY-SA 2.5:
https://creativecommons.org/licenses/by-sa/2.5/

All other code in this file is licensed under the Apache License 2.0.
";
		return Indent(licenseInfo, indentPattern: "// ");
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

	public static string GetPlainText(HtmlNode node)
	{
		var sb = new StringBuilder();
		AppendPlainText(node, sb);
		var text = HtmlEntity.DeEntitize(sb.ToString());
		return text.Replace("  ", " ").Replace(" .", ".").Replace(" ,", ",").Trim();
	}

	private static void AppendPlainText(HtmlNode node, StringBuilder sb)
	{
		if (node == null)
		{
			return;
		}

		if (node is HtmlTextNode { Text: { } text })
		{
			if (!string.IsNullOrWhiteSpace(text))
			{
				sb.Append(text);
				sb.Append(' ');
			}
			return;
		}

		// Ignore script/style entirely
		if (node.Name == "script" || node.Name == "style" || node.HasClass("icon-deprecated"))
		{
			return;
		}

		foreach (var child in node.ChildNodes)
		{
			AppendPlainText(child, sb);
		}
	}
}
