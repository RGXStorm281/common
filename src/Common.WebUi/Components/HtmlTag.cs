namespace RobinEpple.Common.WebUi.Components;

using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;

public abstract record class HtmlTag : IHtmlContent
{
	public HtmlTag(string tag)
	{
		_tag = tag;
		_classes = [];
		_attributes = new Dictionary<string, string>();
	}

	public HtmlTag(string tag, IEnumerable<string> classes, IDictionary<string, string> attributes)
	{
		_tag = tag;
		_classes = classes.ToHashSet();
		_attributes = attributes;
	}

	private string _tag { get; }

	private HashSet<string> _classes { get; }

	private IDictionary<string, string> _attributes { get; }

	public void Class(string cssClass) => _classes.Add(cssClass);

	public void RemoveClass(string cssClass) => _classes.Remove(cssClass);

	public void Attribute(string name, string content) => _attributes[name] = content;

	public void RemoveAttribute(string name) => _attributes.Remove(name);

	public void WriteTo(TextWriter writer, HtmlEncoder encoder)
	{
		var builder = new HtmlContentBuilder();

		var classes = _classes.Count > 0 ? $"class=\"{string.Join(" ", _classes)}\"" : string.Empty;
		var attributeList = _attributes.Select(attribute => $"{attribute.Key}=\"{attribute.Value}\"");
		var attributes = string.Join(" ", attributeList);

		builder.AppendHtml($"<{_tag} {classes} {attributes}>");
		AppendContent(builder);
		builder.AppendHtml($"</{_tag}>");

		builder.WriteTo(writer, encoder);
	}

	protected abstract void AppendContent(HtmlContentBuilder builder);
}
