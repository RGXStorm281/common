namespace RobinEpple.Common.Forms.Test.Tests;

using System.Diagnostics.CodeAnalysis;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.Wrappers.Abstractions;

[TestClass]
public partial class Wrappers
{
	public Wrappers()
	{
		BuildFields();
		BuildSubstructure();
		BuildTemplates();
	}

	public IForm Fields { get; private set; }

	[MemberNotNull(nameof(Fields))]
	[WrapFormStructure(nameof(Fields))]
	private void BuildFields()
	{
		Fields = new FormBuilder(nameof(Fields))
			.WithTextNode("Text")
			.WithNumberNode("Number", textNode => textNode.UseLabel("Please insert some number"))
			.WithBooleanNode("Boolean")
			.WithTimestampNode("Timestamp")
			.WithFileNode("File")
			.Build();
	}

	[TestMethod]
	public void GenerateFormWrapper_ShouldGeneratePropertiesForFields()
	{
		Assert.IsTrue(FieldsWrapper.Text is ITextNode);
		Assert.IsTrue(FieldsWrapper.Number is INumberNode);
		Assert.IsTrue(FieldsWrapper.Boolean is IBooleanNode);
		Assert.IsTrue(FieldsWrapper.Timestamp is ITimestampNode);
		Assert.IsTrue(FieldsWrapper.File is IFileNode);
	}

	public IForm Substructure { get; private set; }

	[MemberNotNull(nameof(Substructure))]
	[WrapFormStructure(nameof(Substructure))]
	private void BuildSubstructure()
	{
		Substructure = new FormBuilder(nameof(Substructure))
			.WithSection(
				"Section",
				(sectionBuilder, recursiveTemplate) =>
					sectionBuilder
						.WithTextNode("Text")
						.WithNumberNode("Number")
						.WithBooleanNode("Boolean")
						.WithTimestampNode("Timestamp")
						.WithFileNode("File")
			)
			.Build();
	}

	[TestMethod]
	public void GenerateFormWrapper_ShouldGeneratePropertiesForSubstructure()
	{
		Assert.IsTrue(SubstructureWrapper.Section.Node is IForm);
		Assert.IsTrue(SubstructureWrapper.Section.Text is ITextNode);
		Assert.IsTrue(SubstructureWrapper.Section.Number is INumberNode);
		Assert.IsTrue(SubstructureWrapper.Section.Boolean is IBooleanNode);
		Assert.IsTrue(SubstructureWrapper.Section.Timestamp is ITimestampNode);
		Assert.IsTrue(SubstructureWrapper.Section.File is IFileNode);
	}

	public IForm Templates { get; private set; }

	void Configure(ITemplateNodeBuilder templates, IForm parent)
	{
		templates
			.UseTemplate("First", firstTemplate => firstTemplate.WithBooleanNode("BooleanNode"))
			.UseTemplate("Second", firstTemplate => firstTemplate.WithTextNode("TextNode"))
			.UsePreConfiguredTemplate(parent);
	}

	[MemberNotNull(nameof(Templates))]
	[WrapFormStructure(nameof(Templates))]
	private void BuildTemplates()
	{
		Templates = new FormBuilder(nameof(Templates))
			.WithTemplatedSection("TemplatedSection", Configure)
			.WithCollectionNode(
				"Collection",
				(collection, _) =>
					collection
						.UseTemplate("First", firstTemplate => firstTemplate.WithBooleanNode("BooleanNode"))
						.UseTemplate("Second", firstTemplate => firstTemplate.WithTextNode("TextNode"))
			)
			.Build();
	}

	[TestMethod]
	public void GenerateFormWrapper_ShouldHandleTemplatesAndInstances()
	{
		TemplatesWrapper.TemplatedSection.TryInstantiateFirst(out _);

		Assert.IsTrue(
			TemplatesWrapper.TemplatedSection.Instance
				is TemplatesStruct.TemplatedSectionStruct.FirstStruct { BooleanNode: IBooleanNode }
		);

		TemplatesWrapper.TemplatedSection.TryInstantiateSecond(out _);

		Assert.IsTrue(
			TemplatesWrapper.TemplatedSection.Instance
				is TemplatesStruct.TemplatedSectionStruct.SecondStruct { TextNode: ITextNode }
		);

		TemplatesWrapper.TemplatedSection.TryInstantiateTemplates(out _);

		Assert.IsTrue(TemplatesWrapper.TemplatedSection.Instance is TemplatesStruct);

		TemplatesWrapper.Collection.TryInstantiateFirst(out _);

		Assert.IsTrue(
			TemplatesWrapper.Collection.Instances.First()
				is TemplatesStruct.CollectionStruct.FirstStruct { BooleanNode: IBooleanNode }
		);

		TemplatesWrapper.Collection.TryInstantiateSecond(out _);

		Assert.IsTrue(
			TemplatesWrapper.Collection.Instances.Skip(1).First()
				is TemplatesStruct.CollectionStruct.SecondStruct { TextNode: ITextNode }
		);
	}
}
