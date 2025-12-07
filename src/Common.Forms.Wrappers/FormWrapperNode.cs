namespace RobinEpple.Common.Forms.Wrappers;

using System.Text;
using Microsoft.CodeAnalysis;
using RobinEpple.Common.Forms.Wrappers.Abstractions;

public class FormWrapperNode(
	string name,
	string type,
	INamedTypeSymbol declaringType,
	bool nodeIsFormWrapper,
	FormWrapperNode? parentNode = null
)
{
	private const string _nodePropertyName = "Node";
	private readonly bool _nodeIsFormWrapper = nodeIsFormWrapper;

	// --------------- name and type of the form node ---------------


	public string Name { get; set; } = name;
	public string NodeType { get; } = type;
	public INamedTypeSymbol DeclaringType { get; } = declaringType;
	public FormWrapperNode? ParentNode { get; } = parentNode;

	// --------------- collections and templated sections ---------------

	public List<FormWrapperNode> Templates { get; set; } = [];
	public bool HasTemplates => Templates.Any();
	public List<InstanceProperty> InstanceProperties { get; } = [];
	public bool HasInstanceProperties => InstanceProperties.Any();

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

	public string GetTypeName() => GetNormalizedName() + "Struct";

	public string GetFullyQualifiedTypeName()
	{
		if (ParentNode != null)
		{
			return $"{ParentNode.GetFullyQualifiedTypeName()}.{GetTypeName()}";
		}

		return $"{DeclaringType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}.{GetTypeName()}";
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

		source.Append(PrintWrapperType());
		source.AppendLine();
		source.Append(PrintWrapperProperty(nodeIsTemplate));
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

	public string PrintWrapperType()
	{
		var type = new StringBuilder();
		type.Append($"public struct {GetTypeName()}({NodeType}? node)");
		if (_nodeIsFormWrapper)
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
			if (HasInstanceProperties)
			{
				type.AppendLine();
				type.Append(IndentHelper.Indent(PrintInstantiationFunction(template)));
			}
		}

		if (HasInstanceProperties)
		{
			type.AppendLine();
			type.Append(IndentHelper.Indent(PrintInstanceWrapperFunction()));

			foreach (var instanceProperty in InstanceProperties)
			{
				type.AppendLine();
				type.Append(IndentHelper.Indent(PrintInstanceProperty(instanceProperty)));
			}
		}

		foreach (var subNode in Substructure)
		{
			type.AppendLine();
			type.Append(IndentHelper.Indent(subNode.GetSource(false)));
		}
		type.AppendLine("}");
		return type.ToString();
	}

	private string PrintInstanceWrapperFunction()
	{
		var sb = new StringBuilder();
		sb.AppendLine(
			"private RobinEpple.Common.Forms.IFormWrapper? WrapInstance(RobinEpple.Common.Forms.Nodes.IForm? instance)"
		);
		sb.AppendLine("{");
		sb.AppendLine(IndentHelper.Indent("switch(instance?.Name)"));
		sb.AppendLine(IndentHelper.Indent("{"));
		foreach (var template in Templates)
		{
			sb.AppendLine(IndentHelper.Indent(@$"case ""{template.Name}"":", 2));
			sb.AppendLine(IndentHelper.Indent("{", 2));
			sb.AppendLine(IndentHelper.Indent($"return new {template.GetTypeName()}(instance);", 3));
			sb.AppendLine(IndentHelper.Indent("}", 2));
		}
		sb.AppendLine(IndentHelper.Indent("default:", 2));
		sb.AppendLine(IndentHelper.Indent("{", 2));
		sb.AppendLine(IndentHelper.Indent("return null;", 3));
		sb.AppendLine(IndentHelper.Indent("}", 2));
		sb.AppendLine(IndentHelper.Indent("}"));
		sb.AppendLine("}");

		return sb.ToString();
	}

	private string PrintInstantiationFunction(FormWrapperNode template)
	{
		var templateName = template.GetNormalizedName();
		var sb = new StringBuilder();
		sb.AppendLine($"public bool TryInstantiate{templateName}(out {template.GetTypeName()}? newInstance)");
		sb.AppendLine("{");
		sb.AppendLine(IndentHelper.Indent("newInstance = null;"));
		sb.AppendLine(IndentHelper.Indent("if ("));
		sb.AppendLine(IndentHelper.Indent($"{_nodePropertyName} is {{ }} node", 2));
		sb.AppendLine(IndentHelper.Indent($"&& {templateName}Template.Node is {{ }} template", 2));
		sb.AppendLine(IndentHelper.Indent(")"));
		sb.AppendLine(IndentHelper.Indent("{"));
		sb.AppendLine(
			IndentHelper.Indent($"newInstance = new {template.GetTypeName()}(node.Instantiate(template));", 2)
		);
		sb.AppendLine(IndentHelper.Indent("return true;", 2));
		sb.AppendLine(IndentHelper.Indent("}"));
		sb.AppendLine(IndentHelper.Indent("return false;"));
		sb.AppendLine("}");
		return sb.ToString();
	}

	private string PrintInstanceProperty(InstanceProperty instanceProperty)
	{
		if (instanceProperty.IsCollection)
		{
			var sb = new StringBuilder();
			sb.Append($"public IEnumerable<RobinEpple.Common.Forms.IFormWrapper> {instanceProperty.Name} => ");
			sb.Append(
				$"({_nodePropertyName}?.{instanceProperty.Name} ?? []).OfType<RobinEpple.Common.Forms.Nodes.IForm>()"
			);
			sb.Append($".Select(WrapInstance).OfType<RobinEpple.Common.Forms.IFormWrapper>();");
			sb.AppendLine();
			return sb.ToString();
		}
		else
		{
			var sb = new StringBuilder();
			sb.Append($"public RobinEpple.Common.Forms.IFormWrapper? {instanceProperty.Name} => ");
			sb.Append(
				$"WrapInstance({_nodePropertyName}?.{instanceProperty.Name} as RobinEpple.Common.Forms.Nodes.IForm);"
			);
			sb.AppendLine();
			return sb.ToString();
		}
	}

	private string PrintWrapperProperty(bool nodeIsTemplate)
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
		var typeName = GetFullyQualifiedTypeName();
		sb.Append($"public {typeName} {propertyName} => ");
		sb.Append(@$"new {typeName}({nodeAccessor});");
		sb.AppendLine();
		return sb.ToString();
	}
}
