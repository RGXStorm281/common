namespace RobinEpple.Common.Html.Mdn;

using HtmlAgilityPack;

public class VoidElements
{
	public static IEnumerable<string> Fetch(string url)
	{
		var web = new HtmlWeb();
		var doc = web.Load(url);
		return ParseFrom(doc);
	}

	public static IEnumerable<string> ParseFrom(HtmlDocument doc)
	{
		var contentSection = doc.DocumentNode.SelectSingleNode(
			"//section[contains(concat(' ', normalize-space(@class), ' '), ' content-section ')]"
		);
		var voidElementList = contentSection.SelectNodes(".//li");
		return voidElementList.Select(voidElement =>
			Helper.GetPlainText(voidElement).Trim("<> ".ToCharArray()).ToString()
		);
	}
}
