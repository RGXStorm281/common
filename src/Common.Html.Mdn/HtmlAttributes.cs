namespace RobinEpple.Common.Html.Mdn;

using System.Text;
using HtmlAgilityPack;

public static class HtmlAttributes
{
	public static IEnumerable<HtmlAttribute> Fetch(string url)
	{
		var web = new HtmlWeb();
		var doc = web.Load(url);
		return ParseFrom(doc, url);
	}

	public static IEnumerable<HtmlAttribute> ParseFrom(HtmlDocument doc, string url)
	{
		var results = new List<HtmlAttribute>();

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

			var deprecatedIcon = dt.SelectSingleNode(".//abbr[contains(@class, 'icon-deprecated')]");
			var isDeprecated = deprecatedIcon != null;

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
			var description = Helper.GetPlainText(p);

			results.Add(new HtmlAttribute(name, description, isDeprecated, url));
		}

		return results;
	}

	public static string Render(HtmlAttribute attribute, string parentClass, bool isGlobalOverride = false)
	{
		var sb = new StringBuilder();
		sb.AppendLine("/// <summary>");
		sb.AppendLine(Helper.PrintMdnDocumentation(attribute.Documentation, attribute.MdnUrl));
		sb.AppendLine("/// </summary>");
		var newKeyword = isGlobalOverride ? " new" : string.Empty;
		if (attribute.IsDeprecated)
		{
			sb.AppendLine("[Obsolete]");
		}
		sb.AppendLine($"public{newKeyword} {parentClass} {Helper.PascalCase(attribute.Name)}(string value)");
		sb.AppendLine("{");
		sb.AppendLine(Helper.Indent($"return this.Attribute(\"{attribute.Name}\", value);"));
		sb.AppendLine("}");
		return sb.ToString();
	}
}
