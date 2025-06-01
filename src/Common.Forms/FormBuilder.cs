namespace RobinEpple.Common.Forms;

using System.Globalization;
using System.Text.RegularExpressions;
using RobinEpple.Common.Forms.Binding;
using RobinEpple.Common.Forms.Building;
using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Extensions;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.Nodes.DefaultImplementation;
using RobinEpple.Common.Forms.Validation;
using RobinEpple.Common.Forms.Visitors;

/// <summary>
/// Use this class to build and configure a form structure.
/// </summary>
public class FormBuilder : IFormBuilder
{
	internal Form Form { get; }
	private readonly CultureInfo _defaultFormatCulture;

	/// <summary>
	/// Instantiates a new form builder.
	/// </summary>
	/// <param name="name">The name of the root form.</param>
	/// <param name="defaultFormatCulture">The culture to use for default formatting. If <see langword="null"/>, "de-DE" is used.</param>
	public FormBuilder(string name, CultureInfo? defaultFormatCulture = null)
	{
		ValidateName(name);
		Form = new Form(name, null);
		_defaultFormatCulture = defaultFormatCulture ?? new CultureInfo("de-DE");
	}

	/// <summary>
	/// Instantiates a new form builder.
	/// </summary>
	/// <param name="name">The name of the root form.</param>
	/// <param name="parent">The parent node for the built form.</param>
	/// <param name="defaultFormatCulture">The culture to use for default formatting. If <see langword="null"/>, "de-DE" is used.</param>
	internal FormBuilder(string name, IParentNode parent, CultureInfo? defaultFormatCulture = null)
	{
		ValidateName(name);
		Form = new Form(name, parent);
		_defaultFormatCulture = defaultFormatCulture ?? new CultureInfo("de-DE");
	}

	private static Regex? _invalidCharRegex;

	private void ValidateName(string name)
	{
		_invalidCharRegex ??= new Regex($"[^{IFormBuilder.ValidNameCharacters}]", RegexOptions.Compiled);
		if (_invalidCharRegex.IsMatch(name))
		{
			throw new InvalidOperationException($"The given name '{name}' contains invalid characters.");
		}
	}

	/// <inheritdoc/>
	public IFormBuilder WithBooleanNode(string name, IFormBuilder.BooleanFieldBuilder? configure = null)
	{
		ValidateName(name);
		var node = new BooleanNode(name, Form);
		var builder = new BooleanNodeBuilder(node);
		configure?.Invoke(builder);
		Form.AddNode(node);
		return this;
	}

	/// <inheritdoc/>
	public IFormBuilder WithCollectionNode(string name, IFormBuilder.CollectionBuilder? configure = null)
	{
		ValidateName(name);
		var node = new CollectionNode(name, Form);
		var builder = new CollectionNodeBuilder(node);
		configure?.Invoke(builder, Form);
		Form.AddNode(node);
		return this;
	}

	/// <inheritdoc/>
	public IFormBuilder WithFileNode(string name, IFormBuilder.FileFieldBuilder? configure = null)
	{
		ValidateName(name);
		var node = new FileNode(name, Form);
		var builder = new FileNodeBuilder(node);
		configure?.Invoke(builder);
		Form.AddNode(node);
		return this;
	}

	/// <inheritdoc/>
	public IFormBuilder WithNumberNode(string name, IFormBuilder.NumberFieldBuilder? configure = null)
	{
		ValidateName(name);
		var node = new NumberNode(name, Form, _defaultFormatCulture);
		var builder = new NumberNodeBuilder(node);
		configure?.Invoke(builder);
		Form.AddNode(node);
		return this;
	}

	/// <inheritdoc/>
	public IFormBuilder WithTemplatedSection(string name, IFormBuilder.TemplatedSectionBuilder? configure = null)
	{
		ValidateName(name);
		var node = new TemplateNode(name, Form);
		var builder = new TemplateNodeBuilder(node);
		configure?.Invoke(builder, Form);
		Form.AddNode(node);
		return this;
	}

	/// <inheritdoc/>
	public IFormBuilder WithTextNode(string name, IFormBuilder.TextFieldBuilder? configure = null)
	{
		ValidateName(name);
		var node = new TextNode(name, Form);
		var builder = new TextNodeBuilder(node);
		configure?.Invoke(builder);
		Form.AddNode(node);
		return this;
	}

	/// <inheritdoc/>
	public IFormBuilder WithTimestampNode(string name, IFormBuilder.TimestampFieldBuilder? configure = null)
	{
		ValidateName(name);
		var node = new TimestampNode(name, Form, _defaultFormatCulture);
		var builder = new TimestampNodeBuilder(node);
		configure?.Invoke(builder);
		Form.AddNode(node);
		return this;
	}

	/// <inheritdoc/>
	public IForm Build()
	{
		var initializer = new NodeInitializer();
		initializer.Visit(Form);
		return Form;
	}

	/// <inheritdoc/>
	public IFormBuilder UseDefaultReadonly(bool isReadonly)
	{
		Form.ReplaceDefaultReadonly(isReadonly);
		return this;
	}

	/// <inheritdoc/>
	public IFormBuilder UseDefaultVisibility(bool isVisible)
	{
		Form.ReplaceDefaultVisibility(isVisible);
		return this;
	}

	/// <inheritdoc/>
	public IFormBuilder UseVisibilityCondition(IFormExpression<bool> condition)
	{
		Form.UseVisibilityCondition(condition);
		return this;
	}

	/// <inheritdoc/>
	public IFormBuilder UseReadonlyCondition(IFormExpression<bool> condition)
	{
		Form.UseReadonlyCondition(condition);
		return this;
	}

	/// <inheritdoc/>
	public IFormBuilder UseExtension(IFormNodeExtension extension)
	{
		Form.UseExtension(extension);
		return this;
	}

	/// <inheritdoc/>
	public IFormBuilder UseLabel(string label)
	{
		Form.Label = label;
		Form.ReplaceDefaultLabel(label);
		return this;
	}

	/// <inheritdoc/>
	public IFormBuilder UseValidator(INodeValidator validator)
	{
		Form.UseValidator(validator);
		return this;
	}

	/// <inheritdoc/>
	public IFormBuilder UseBinding(IFormNodeBinding binding)
	{
		Form.UseBinding(binding);
		return this;
	}

	/// <inheritdoc/>
	public IFormBuilder WithNode(IFormNode node)
	{
		Form.AddNode(node);
		return this;
	}

	/// <inheritdoc/>
	public IFormBuilder WithSection(string name, IFormBuilder.SubFormBuilder? configure = null)
	{
		var builder = new FormBuilder(name, Form);
		configure?.Invoke(builder, Form);
		Form.AddNode(builder.Form);
		return this;
	}

	/// <inheritdoc/>
	public IFormBuilder WithPreConfiguredSubForm(IFormNode subForm)
	{
		Form.AddNode(subForm);
		return this;
	}

	/// <inheritdoc/>
	public string GetNodeName() => Form.Name;
}
