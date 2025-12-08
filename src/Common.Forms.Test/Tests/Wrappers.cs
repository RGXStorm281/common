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
		BuildBoundFields();
		BuildSubstructure();
		BuildTemplates();
		BuildLargeForm();
		BuildRecursive();
		BuildOptional(true);
		BuildDynamic(1, 2, ["Number1", "Number2", "Number3"]);
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
	public void WrapFormStructure_ShouldGeneratePropertiesForFields()
	{
		Assert.IsTrue(FieldsWrapper.Text is ITextNode);
		Assert.IsTrue(FieldsWrapper.Number is INumberNode);
		Assert.IsTrue(FieldsWrapper.Boolean is IBooleanNode);
		Assert.IsTrue(FieldsWrapper.Timestamp is ITimestampNode);
		Assert.IsTrue(FieldsWrapper.File is IFileNode);
	}

	public string Email { get; set; } = string.Empty;
	public IForm BoundFields { get; private set; }

	[MemberNotNull(nameof(BoundFields))]
	[WrapFormStructure(nameof(BoundFields))]
	private void BuildBoundFields()
	{
		BoundFields = new FormBuilder(nameof(BoundFields))
			.WithTextNode(nameof(Email), email => email.UseLabel("E-Mail").UsePropertyBinding(() => Email))
			.Build();
	}

	[TestMethod]
	public void WrapFormStructure_ShouldHandleNameof()
	{
		const string testEmail = "test@mail.de";
		Email = testEmail;
		BoundFields.LoadFromBinding();
		Assert.AreEqual(BoundFieldsWrapper.Email?.Value, testEmail);
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
	public void WrapFormStructure_ShouldGeneratePropertiesForSubstructure()
	{
		Assert.IsTrue(SubstructureWrapper.Section.Node is IForm);
		Assert.IsTrue(SubstructureWrapper.Section.Text is ITextNode);
		Assert.IsTrue(SubstructureWrapper.Section.Number is INumberNode);
		Assert.IsTrue(SubstructureWrapper.Section.Boolean is IBooleanNode);
		Assert.IsTrue(SubstructureWrapper.Section.Timestamp is ITimestampNode);
		Assert.IsTrue(SubstructureWrapper.Section.File is IFileNode);
	}

	public IForm Templates { get; private set; }

	[MemberNotNull(nameof(Templates))]
	[WrapFormStructure(nameof(Templates))]
	private void BuildTemplates()
	{
		Templates = new FormBuilder(nameof(Templates))
			.WithTemplatedSection(
				"TemplatedSection",
				(templates, _) =>
					templates
						.UseTemplate("First", firstTemplate => firstTemplate.WithBooleanNode("BooleanNode"))
						.UseTemplate("Second", firstTemplate => firstTemplate.WithTextNode("TextNode"))
			)
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
	public void WrapFormStructure_ShouldGenerateTemplateWrappers()
	{
		Assert.IsTrue(
			TemplatesWrapper.TemplatedSection.FirstTemplate
				is TemplatesStruct.TemplatedSectionStruct.FirstStruct { BooleanNode: IBooleanNode }
		);

		Assert.IsTrue(
			TemplatesWrapper.TemplatedSection.SecondTemplate
				is TemplatesStruct.TemplatedSectionStruct.SecondStruct { TextNode: ITextNode }
		);

		Assert.IsTrue(
			TemplatesWrapper.Collection.FirstTemplate
				is TemplatesStruct.CollectionStruct.FirstStruct { BooleanNode: IBooleanNode }
		);

		Assert.IsTrue(
			TemplatesWrapper.Collection.SecondTemplate
				is TemplatesStruct.CollectionStruct.SecondStruct { TextNode: ITextNode }
		);
	}

	[TestMethod]
	public void WrapFormStructure_ShouldWrapInstances()
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

		TemplatesWrapper.Collection.TryInstantiateFirst(out var firstInstance);

		Assert.IsTrue(
			TemplatesWrapper.Collection.Instances.First()
				is TemplatesStruct.CollectionStruct.FirstStruct { BooleanNode: IBooleanNode }
		);

		TemplatesWrapper.Collection.Node!.RemoveItem(firstInstance!.Value.Node!);
		TemplatesWrapper.Collection.TryInstantiateSecond(out _);

		Assert.IsTrue(
			TemplatesWrapper.Collection.Instances.First()
				is TemplatesStruct.CollectionStruct.SecondStruct { TextNode: ITextNode }
		);
	}

	private void ClassLevelConfigure(ITemplateNodeBuilder templates, IForm parent)
	{
		templates
			.UseTemplate("First", firstTemplate => firstTemplate.WithBooleanNode("BooleanNode"))
			.UseTemplate("Second", firstTemplate => firstTemplate.WithTextNode("TextNode"));
	}

	public IForm LargeForm { get; private set; }

	[MemberNotNull(nameof(LargeForm))]
	[WrapFormStructure(nameof(LargeForm))]
	private void BuildLargeForm()
	{
		void LocalConfigure(IFormBuilder sectionBuilder, IForm parent)
		{
			sectionBuilder.WithTemplatedSection("templatedSection", ClassLevelConfigure);
		}
		LargeForm = new FormBuilder(nameof(LargeForm)).WithSection("Section", LocalConfigure).Build();
	}

	[TestMethod]
	public void WrapFormStructure_ShouldHandleMethodGroupsForSubstructure()
	{
		Assert.IsNotNull(LargeFormWrapper.Section.Node);
		Assert.IsNotNull(LargeFormWrapper.Section.TemplatedSection.Node);
		Assert.IsNotNull(LargeFormWrapper.Section.TemplatedSection.FirstTemplate.Node);
	}

	public IForm Recursive { get; private set; }

	[MemberNotNull(nameof(Recursive))]
	[WrapFormStructure(nameof(Recursive))]
	private void BuildRecursive()
	{
		Recursive = new FormBuilder(nameof(Recursive))
			.WithTemplatedSection(
				"TemplatedSection",
				(templates, recursiveTemplate) => templates.UsePreConfiguredTemplate(recursiveTemplate)
			)
			.WithCollectionNode(
				"Collection",
				(collection, recursiveTemplate) => collection.UsePreConfiguredTemplate(recursiveTemplate)
			)
			.Build();
	}

	[TestMethod]
	public void WrapFormStructure_ShouldHandleRecursiveTemplates()
	{
		RecursiveWrapper.TemplatedSection.TryInstantiateRecursive(out _);

		Assert.IsTrue(RecursiveWrapper.TemplatedSection.Instance is RecursiveStruct);

		RecursiveWrapper.Collection.TryInstantiateRecursive(out _);

		Assert.IsTrue(RecursiveWrapper.Collection.Instances.First() is RecursiveStruct);
	}

	public IForm Optional { get; private set; }

	[MemberNotNull(nameof(Optional))]
	[WrapFormStructure(nameof(Optional))]
	private void BuildOptional(bool includeTextNode)
	{
		var builder = new FormBuilder(nameof(Optional));

		if (includeTextNode)
		{
			builder.WithBooleanNode("Text");
		}

		Optional = builder.Build();
	}

	[TestMethod]
	public void WrapFormStructure_ShouldIncludeOptionalNodes()
	{
		Assert.IsNotNull(OptionalWrapper.Text);
	}

	public IForm DynamicGeneratedForm { get; private set; }

	[MemberNotNull(nameof(DynamicGeneratedForm))]
	[WrapFormStructure(nameof(DynamicGeneratedForm))]
	private void BuildDynamic(int numTextNodes, int numDateNodes, string[] numberNodes)
	{
		var builder = new FormBuilder(nameof(DynamicGeneratedForm));

		for (int textNodeCounter = 0; textNodeCounter < numTextNodes; textNodeCounter++)
		{
			builder.WithTextNode("Text" + textNodeCounter);
		}

		var dateNodeCounter = 0;
		while (dateNodeCounter < numDateNodes)
		{
			builder.WithTimestampNode("Date" + dateNodeCounter);

			dateNodeCounter++;
		}

		foreach (var numberNode in numberNodes)
		{
			builder.WithNumberNode(numberNode);
		}

		DynamicGeneratedForm = builder.Build();
	}

	[TestMethod]
	public void WrapFormStructure_ShouldIgnoreNonStaticStructureElements()
	{
		var properties = typeof(DynamicGeneratedFormStruct).GetProperties();
		Assert.IsFalse(properties.Any(property => property.Name.StartsWith("Text")));
		Assert.IsFalse(properties.Any(property => property.Name.StartsWith("Date")));
		Assert.IsFalse(properties.Any(property => property.Name.StartsWith("Number")));
	}
}
