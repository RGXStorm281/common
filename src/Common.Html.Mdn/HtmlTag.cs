namespace RobinEpple.Common.Html.Mdn;

public record class HtmlTag(
	string Namespace,
	string ClassName,
	string TagName,
	string Documentation,
	IEnumerable<HtmlAttribute> Attributes
);
