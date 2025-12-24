namespace RobinEpple.Common.Html.Mdn;

using System.Text;
using HtmlAgilityPack;

public static class HtmlAttributes
{
	public static List<HtmlAttribute> Fetch(string url)
	{
		var results = new List<HtmlAttribute>();

		var web = new HtmlWeb();
		var doc = web.Load(url);

		// Find all <dt> elements that have an id attribute
		var dtNodes = doc.DocumentNode.SelectNodes("//dt[@id]");
		if (dtNodes == null)
		{
			return results;
		}

		foreach (var dt in dtNodes)
		{
			var name = dt.GetAttributeValue("id", "").Trim();
			if (string.IsNullOrEmpty(name))
			{
				continue;
			}

			// Find the directly following <dd>
			var dd = dt.SelectSingleNode("following-sibling::dd[1]");
			if (dd == null)
			{
				continue;
			}

			// Get the <p> inside the dd
			var p = dd.SelectSingleNode(".//p");
			if (p == null)
			{
				continue;
			}

			// Remove decorative tags but keep their inner text
			var description = GetPlainText(p);

			results.Add(new HtmlAttribute(name, description));
		}

		return results;
	}

	private static string GetPlainText(HtmlNode node)
	{
		var sb = new StringBuilder();
		AppendPlainText(node, sb);
		var text = HtmlEntity.DeEntitize(sb.ToString());
		return text.Replace("  ", " ").Replace(" .", ".").Trim();
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
		if (node.Name == "script" || node.Name == "style")
		{
			return;
		}

		foreach (var child in node.ChildNodes)
		{
			AppendPlainText(child, sb);
		}
	}

	public static string Render(HtmlAttribute attribute, string parentClass)
	{
		var sb = new StringBuilder();
		sb.AppendLine("/// <summary>");
		sb.AppendLine(Helper.Indent(attribute.Documentation, indentPattern: "/// "));
		sb.AppendLine("/// </summary>");
		sb.AppendLine($"public {parentClass} {Helper.PascalCase(attribute.Name)}(string value)");
		sb.AppendLine("{");
		sb.AppendLine(Helper.Indent($"return this.Attribute(\"{attribute.Name}\", value);"));
		sb.AppendLine("}");
		return sb.ToString();
	}
}
