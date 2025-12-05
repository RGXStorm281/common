namespace RobinEpple.Common.Forms.Wrappers;

using System.Text;
using Microsoft.CodeAnalysis;
using RobinEpple.Common.Forms.Wrappers.Abstractions;

public class FormWrapperNode(string name, string type)
{
	private const string _nodePropertyName = "Node";

	// --------------- name and type of the form node ---------------
	public string Name { get; set; } = name;
	public string NodeType { get; } = type;

	// --------------- collections and templated sections ---------------
	public List<FormWrapperNode> Templates { get; set; } = [];
	public bool HasTemplates => Templates.Any();

	// --------------- form nodes with inner structure ---------------
	public List<FormWrapperNode> Substructure { get; set; } = [];
	public bool HasSubstructure => Substructure.Any();

	// --------------- source generation ---------------
	private string GetNormalizedName()
	{
		var normalizedName = new StringBuilder();
		var isFirst = true;
		var capitalizeNext = true;

		foreach (var character in Name)
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

	public string GetSource(bool nodeIsTemplate)
	{
		if (!nodeIsTemplate && !HasTemplates && !HasSubstructure)
		{
			// simplest case: No inner properties of the wrapper needed
			// -> just print a property to access the node.
			return PrintFormNodeProperty();
		}

		// Otherwise we need to build a wrapper type.
		var source = new StringBuilder();
		var wrapperTypeName = GetNormalizedName() + "Wrapper";

		source.Append(PrintWrapperType(wrapperTypeName, nodeIsTemplate));
		source.AppendLine();
		source.Append(PrintWrapperProperty(wrapperTypeName, nodeIsTemplate));
		return source.ToString();
	}

	private string PrintFormNodeProperty()
	{
		var sb = new StringBuilder();
		sb.Append($"public {NodeType}? {GetNormalizedName()} => ");
		sb.Append(@$"{_nodePropertyName}?.FindFirst(""{Name}"") as {NodeType};");
		sb.AppendLine();
		return sb.ToString();
	}

	public string PrintWrapperType(string wrapperTypeName, bool nodeIsFormWrapper)
	{
		var type = new StringBuilder();
		type.Append($"public class {wrapperTypeName}({NodeType}? node)");
		if (nodeIsFormWrapper)
		{
			type.Append($" : RobinEpple.Common.Forms.IFormWrapper");
		}
		type.AppendLine();
		type.AppendLine("{");
		type.AppendLine(IndentHelper.Indent($"public {NodeType}? {_nodePropertyName} {{ get; }} = node;"));
		foreach (var template in Templates)
		{
			type.AppendLine();
			type.Append(IndentHelper.Indent(template.GetSource(true)));
		}
		foreach (var subNode in Substructure)
		{
			type.AppendLine();
			type.Append(IndentHelper.Indent(subNode.GetSource(false)));
		}
		type.AppendLine("}");
		return type.ToString();
	}

	private string PrintWrapperProperty(string wrapperTypeName, bool nodeIsTemplate)
	{
		var nodeAccessor = @$"{_nodePropertyName}?.FindFirst(""{Name}"") as {NodeType}";
		var propertyName = GetNormalizedName();
		if (nodeIsTemplate)
		{
			nodeAccessor =
				@$"{_nodePropertyName}?.Templates.First(template => template.Name == ""{Name}"") as {NodeType}";
			propertyName += "Template";
		}

		var sb = new StringBuilder();
		sb.Append($"public {wrapperTypeName} {propertyName} => ");
		sb.Append(@$"new {wrapperTypeName}({nodeAccessor});");
		sb.AppendLine();
		return sb.ToString();
	}
}
