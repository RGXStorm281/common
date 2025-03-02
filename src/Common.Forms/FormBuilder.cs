namespace RobinEpple.Common.Forms;

using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Extensions;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.Validation;

/// <summary>
/// Use this class to build and configure a form structure.
/// </summary>
public class FormBuilder : IFormBuilder
{
	/// <summary>
	/// Instantiates a new form builder.
	/// </summary>
	/// <param name="name">The name of the root form.</param>
	public FormBuilder(string name) { }

	/// <inheritdoc/>
	public IFormBuilder WithBooleanNode(string name, IFormBuilder.FieldBuilder? configure = null) =>
		throw new NotImplementedException();

	/// <inheritdoc/>
	public IFormBuilder WithCollectionNode(string name, IFormBuilder.CollectionBuilder? configure = null) =>
		throw new NotImplementedException();

	/// <inheritdoc/>
	public IFormBuilder WithFileNode(string name, IFormBuilder.FieldBuilder? configure = null) =>
		throw new NotImplementedException();

	/// <inheritdoc/>
	public IFormBuilder WithNumberNode(string name, IFormBuilder.FieldBuilder? configure = null) =>
		throw new NotImplementedException();

	/// <inheritdoc/>
	public IFormBuilder WithTemplatedSection(string name, IFormBuilder.TemplatedSectionBuilder? configure = null) =>
		throw new NotImplementedException();

	/// <inheritdoc/>
	public IFormBuilder WithTextNode(string name, IFormBuilder.FieldBuilder? configure = null) =>
		throw new NotImplementedException();

	/// <inheritdoc/>
	public IFormBuilder WithTimestampNode(string name, IFormBuilder.FieldBuilder? configure = null) =>
		throw new NotImplementedException();

	/// <inheritdoc/>
	public IForm Build() => throw new NotImplementedException();

	/// <inheritdoc/>
	public IFormBuilder UseDefaultReadonly(bool isReadonly) => throw new NotImplementedException();

	/// <inheritdoc/>
	public IFormBuilder UseDefaultVisibility(bool visible) => throw new NotImplementedException();

	/// <inheritdoc/>
	public IFormBuilder UseExtension(IFormNodeExtension extension) => throw new NotImplementedException();

	/// <inheritdoc/>
	public IFormBuilder UseLabel(string label) => throw new NotImplementedException();

	/// <inheritdoc/>
	public IFormBuilder UseValidator(INodeValidator validator) => throw new NotImplementedException();

	/// <inheritdoc/>
	public IFormBuilder UseVisibilityCondition(IFormExpression<bool> condition) => throw new NotImplementedException();
}
