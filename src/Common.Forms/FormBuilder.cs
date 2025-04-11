namespace RobinEpple.Common.Forms;

using System.Globalization;
using System.Text.RegularExpressions;
using RobinEpple.Common.Forms.Building;
using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Extensions;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.Nodes.DefaultImplementation;
using RobinEpple.Common.Forms.Validation;

/// <summary>
/// Use this class to build and configure a form structure.
/// </summary>
public class FormBuilder : IFormBuilder
{
	private Form _form;
	private readonly CultureInfo _defaultFormatCulture;

	/// <summary>
	/// Instantiates a new form builder.
	/// </summary>
	/// <param name="name">The name of the root form.</param>
	/// <param name="defaultFormatCulture">The culture to use for default formatting. If <see langword="null"/>, "de-DE" is used.</param>
	public FormBuilder(string name, CultureInfo? defaultFormatCulture = null)
	{
		_validateName(name);
		_form = new Form(name, null);
		_defaultFormatCulture = defaultFormatCulture ?? new CultureInfo("de-DE");
	}

	private static Regex? _invalidCharRegex;

	private void _validateName(string name)
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
		_validateName(name);
		var node = new BooleanNode(name, _form);
		var builder = new BooleanNodeBuilder(node);
		configure?.Invoke(builder);
		_form.AddNode(node);
		return this;
	}

	/// <inheritdoc/>
	public IFormBuilder WithCollectionNode(string name, IFormBuilder.CollectionBuilder? configure = null)
	{
		_validateName(name);
		var node = new CollectionNode(name, _form);
		var builder = new CollectionNodeBuilder(node);
		configure?.Invoke(builder, _form);
		_form.AddNode(node);
		return this;
	}

	/// <inheritdoc/>
	public IFormBuilder WithFileNode(string name, IFormBuilder.FileFieldBuilder? configure = null)
	{
		_validateName(name);
		var node = new FileNode(name, _form);
		var builder = new FileNodeBuilder(node);
		configure?.Invoke(builder);
		_form.AddNode(node);
		return this;
	}

	/// <inheritdoc/>
	public IFormBuilder WithNumberNode(string name, IFormBuilder.NumberFieldBuilder? configure = null)
	{
		_validateName(name);
		var node = new NumberNode(name, _form, _defaultFormatCulture);
		var builder = new NumberNodeBuilder(node);
		configure?.Invoke(builder);
		_form.AddNode(node);
		return this;
	}

	/// <inheritdoc/>
	public IFormBuilder WithTemplatedSection(string name, IFormBuilder.TemplatedSectionBuilder? configure = null)
	{
		_validateName(name);
		var node = new TemplateNode(name, _form);
		var builder = new TemplateNodeBuilder(node);
		configure?.Invoke(builder, _form);
		_form.AddNode(node);
		return this;
	}

	/// <inheritdoc/>
	public IFormBuilder WithTextNode(string name, IFormBuilder.TextFieldBuilder? configure = null)
	{
		_validateName(name);
		var node = new TextNode(name, _form);
		var builder = new TextNodeBuilder(node);
		configure?.Invoke(builder);
		_form.AddNode(node);
		return this;
	}

	/// <inheritdoc/>
	public IFormBuilder WithTimestampNode(string name, IFormBuilder.TimestampFieldBuilder? configure = null)
	{
		_validateName(name);
		var node = new TimestampNode(name, _form, _defaultFormatCulture);
		var builder = new TimestampNodeBuilder(node);
		configure?.Invoke(builder);
		_form.AddNode(node);
		return this;
	}

	/// <inheritdoc/>
	public IForm Build() => _form;

	/// <inheritdoc/>
	public IFormBuilder UseDefaultReadonly(bool isReadonly)
	{
		_form.ReplaceDefaultReadonly(isReadonly);
		return this;
	}

	/// <inheritdoc/>
	public IFormBuilder UseDefaultVisibility(bool isVisible)
	{
		_form.ReplaceDefaultVisibility(isVisible);
		return this;
	}

	/// <inheritdoc/>
	public IFormBuilder UseVisibilityCondition(IFormExpression<bool> condition)
	{
		_form.UseVisibilityCondition(condition);
		return this;
	}

	/// <inheritdoc/>
	public IFormBuilder UseReadonlyCondition(IFormExpression<bool> condition)
	{
		_form.UseReadonlyCondition(condition);
		return this;
	}

	/// <inheritdoc/>
	public IFormBuilder UseExtension(IFormNodeExtension extension)
	{
		_form.UseExtension(extension);
		return this;
	}

	/// <inheritdoc/>
	public IFormBuilder UseLabel(string label)
	{
		_form.Label = label;
		_form.ReplaceDefaultLabel(label);
		return this;
	}

	/// <inheritdoc/>
	public IFormBuilder UseValidator(INodeValidator validator)
	{
		_form.UseValidator(validator);
		return this;
	}
}
