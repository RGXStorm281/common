namespace RobinEpple.Common.Forms.Test;

using RobinEpple.Common.Forms.Nodes;

[TestClass]
public class Cloning
{
	[TestMethod]
	public void CloneNode_ShouldDeepCloneAndHaveAllConfigurationsEqual()
	{
		// Build structure.
		var form = new FormBuilder("Test")
			.WithBooleanNode("Boolean")
			.WithCollectionNode(
				"Collection",
				(builder, recursiveTemplate) => builder.UsePreconfiguredTemplate(recursiveTemplate)
			)
			.WithFileNode("File")
			.WithNumberNode("Number", fieldBuilder => fieldBuilder.UseFormatter(new TestEuroFormatter()))
			.WithTemplatedSection(
				"TemplatedSection",
				(builder, recursiveTemplate) => builder.UsePreconfiguredTemplate(recursiveTemplate)
			)
			.WithTextNode("Text")
			.WithTimestampNode("Timestamp")
			.UseLabel("TestLabel")
			.UseDefaultVisibility(false)
			.UseDefaultReadonly(true)
			.UseVisibilityCondition(new FalseMockCondition())
			.UseValidator(new ValidMockValidator())
			.UseExtension(new MockExtension())
			.Build();

		// Get individual nodes.
		var booleanNode = (IBooleanNode)form.Nodes.First(node => node.Name == "Boolean");
		var collectionNode = (ICollectionNode)form.Nodes.First(node => node.Name == "Collection");
		var fileNode = (IFileNode)form.Nodes.First(node => node.Name == "File");
		var numberNode = (INumberNode)form.Nodes.First(node => node.Name == "Number");
		var templateNode = (ITemplateNode)form.Nodes.First(node => node.Name == "TemplatedSection");
		var textNode = (ITextNode)form.Nodes.First(node => node.Name == "Text");
		var timestampNode = (ITimestampNode)form.Nodes.First(node => node.Name == "Timestamp");

		// Clone.
		var clone = (IForm)form.Clone();
		var clonedBooleanNode = (IBooleanNode)clone.Nodes.First(node => node.Name == "Boolean");
		var clonedCollectionNode = (ICollectionNode)clone.Nodes.First(node => node.Name == "Collection");
		var clonedFileNode = (IFileNode)clone.Nodes.First(node => node.Name == "File");
		var clonedNumberNode = (INumberNode)clone.Nodes.First(node => node.Name == "Number");
		var clonedTemplateNode = (ITemplateNode)clone.Nodes.First(node => node.Name == "TemplatedSection");
		var clonedTextNode = (ITextNode)clone.Nodes.First(node => node.Name == "Text");
		var clonedTimestampNode = (ITimestampNode)clone.Nodes.First(node => node.Name == "Timestamp");

		// Reset and Compare.
		clone.Reset();
		Assert.IsFalse(ReferenceEquals(form, clone));
		Assert.IsFalse(ReferenceEquals(booleanNode, clonedBooleanNode));
		Assert.IsFalse(ReferenceEquals(collectionNode, clonedCollectionNode));
		Assert.IsFalse(ReferenceEquals(fileNode, clonedFileNode));
		Assert.IsFalse(ReferenceEquals(numberNode, clonedNumberNode));
		Assert.IsFalse(ReferenceEquals(templateNode, clonedTemplateNode));
		Assert.IsFalse(ReferenceEquals(textNode, clonedTextNode));
		Assert.IsFalse(ReferenceEquals(timestampNode, clonedTimestampNode));

		Assert.AreEqual(form.Name, clone.Name);
		Assert.AreEqual(clone.Parent, null);
		Assert.AreEqual(form, form.Root);
		Assert.AreEqual(clone, clone.Root);
		Assert.AreEqual(form.IsVisible, clone.IsVisible);
		Assert.AreEqual(form.IsReadonly, clone.IsReadonly);
		Assert.AreEqual(form.VisibilityCondition, clone.VisibilityCondition);
		Assert.AreEqual(form.NodeValidators.Count(), clone.NodeValidators.Count());
		Assert.AreEqual(form.NodeValidators.First(), clone.NodeValidators.First());
		Assert.AreEqual(form.Extensions.Count(), clone.Extensions.Count());
		Assert.AreEqual(form.Extensions.First(), clone.Extensions.First());

		Assert.AreEqual(form.Nodes.Count(), clone.Nodes.Count());
		Assert.AreEqual(collectionNode.Templates.Count(), clonedCollectionNode.Templates.Count());
		Assert.AreEqual(numberNode.Formatter, clonedNumberNode.Formatter);
		Assert.AreEqual(templateNode.Templates.Count(), clonedTemplateNode.Templates.Count());
		Assert.IsTrue(clonedNumberNode.Formatter is TestEuroFormatter);
	}

	[TestMethod]
	public void CloneNode_ShouldCopyNodeState()
	{
		// Build structure.
		var form = new FormBuilder("Test")
			.WithBooleanNode("Boolean")
			.WithCollectionNode(
				"Collection",
				(builder, recursiveTemplate) => builder.UsePreconfiguredTemplate(recursiveTemplate)
			)
			.WithFileNode("File")
			.WithNumberNode("Number", fieldBuilder => fieldBuilder.UseFormatter(new TestEuroFormatter()))
			.WithTemplatedSection(
				"TemplatedSection",
				(builder, recursiveTemplate) => builder.UsePreconfiguredTemplate(recursiveTemplate)
			)
			.WithTextNode("Text")
			.WithTimestampNode("Timestamp")
			.UseLabel("TestLabel")
			.UseDefaultVisibility(false)
			.UseDefaultReadonly(true)
			.UseVisibilityCondition(new FalseMockCondition())
			.UseValidator(new ValidMockValidator())
			.UseExtension(new MockExtension())
			.Build();

		// Get individual nodes.
		var booleanNode = (IBooleanNode)form.Nodes.First(node => node.Name == "Boolean");
		var collectionNode = (ICollectionNode)form.Nodes.First(node => node.Name == "Collection");
		var fileNode = (IFileNode)form.Nodes.First(node => node.Name == "File");
		var numberNode = (INumberNode)form.Nodes.First(node => node.Name == "Number");
		var templateNode = (ITemplateNode)form.Nodes.First(node => node.Name == "TemplatedSection");
		var textNode = (ITextNode)form.Nodes.First(node => node.Name == "Text");
		var timestampNode = (ITimestampNode)form.Nodes.First(node => node.Name == "Timestamp");

		// Modify state.
		form.IsVisible = false;
		form.IsReadonly = true;
		form.SetValidationError("test", "error");
		booleanNode.HasUserInteraction = true;
		booleanNode.Value = false;
		collectionNode.Instantiate(form);
		fileNode.Value = new FileValue() { FileContents = [1, 2, 3], FileName = "TestFile" };
		numberNode.Value = 42;
		templateNode.Instantiate(form);
		textNode.Value = "TestText";
		timestampNode.Value = DateTime.Now;

		// Clone.
		var clone = (IForm)form.Clone();
		var clonedBooleanNode = (IBooleanNode)clone.Nodes.First(node => node.Name == "Boolean");
		var clonedCollectionNode = (ICollectionNode)clone.Nodes.First(node => node.Name == "Collection");
		var clonedFileNode = (IFileNode)clone.Nodes.First(node => node.Name == "File");
		var clonedNumberNode = (INumberNode)clone.Nodes.First(node => node.Name == "Number");
		var clonedTemplateNode = (ITemplateNode)clone.Nodes.First(node => node.Name == "TemplatedSection");
		var clonedTextNode = (ITextNode)clone.Nodes.First(node => node.Name == "Text");
		var clonedTimestampNode = (ITimestampNode)clone.Nodes.First(node => node.Name == "Timestamp");

		// Compare.
		Assert.AreEqual(form.IsVisible, clone.IsVisible);
		Assert.AreEqual(form.IsReadonly, clone.IsReadonly);
		Assert.AreEqual(form.IsValid, clone.IsValid);
		Assert.AreEqual(form.ValidationErrors.First(), clone.ValidationErrors.First());
		Assert.AreEqual(booleanNode.HasUserInteraction, clonedBooleanNode.HasUserInteraction);
		Assert.AreEqual(booleanNode.Value, clonedBooleanNode.Value);
		Assert.AreEqual(collectionNode.Instances.Count(), clonedCollectionNode.Instances.Count());
		Assert.IsFalse(ReferenceEquals(collectionNode.Instances.First(), clonedCollectionNode.Instances.First()));
		Assert.AreEqual(fileNode.Value.FileContents, clonedFileNode.Value.FileContents);
		Assert.AreEqual(fileNode.Value.FileName, clonedFileNode.Value.FileName);
		Assert.AreEqual(numberNode.Value, clonedNumberNode.Value);
		Assert.IsTrue(clonedTemplateNode.Instance != null);
		Assert.IsFalse(ReferenceEquals(templateNode.Instance, clonedTemplateNode.Instance));
		Assert.AreEqual(textNode.Value, clonedTextNode.Value);
		Assert.AreEqual(timestampNode.Value, clonedTimestampNode.Value);
	}

	[TestMethod]
	public void ClonedNodes_ShouldNotInfluenceEachOther()
	{
		// Build structure.
		var form = new FormBuilder("Test")
			.WithBooleanNode("Boolean")
			.WithCollectionNode(
				"Collection",
				(builder, recursiveTemplate) => builder.UsePreconfiguredTemplate(recursiveTemplate)
			)
			.WithFileNode("File")
			.WithNumberNode("Number", fieldBuilder => fieldBuilder.UseFormatter(new TestEuroFormatter()))
			.WithTemplatedSection(
				"TemplatedSection",
				(builder, recursiveTemplate) => builder.UsePreconfiguredTemplate(recursiveTemplate)
			)
			.WithTextNode("Text")
			.WithTimestampNode("Timestamp")
			.UseLabel("TestLabel")
			.UseDefaultVisibility(false)
			.UseDefaultReadonly(true)
			.UseVisibilityCondition(new FalseMockCondition())
			.UseValidator(new ValidMockValidator())
			.UseExtension(new MockExtension())
			.Build();

		// Get individual nodes.
		var booleanNode = (IBooleanNode)form.Nodes.First(node => node.Name == "Boolean");
		var collectionNode = (ICollectionNode)form.Nodes.First(node => node.Name == "Collection");
		var fileNode = (IFileNode)form.Nodes.First(node => node.Name == "File");
		var numberNode = (INumberNode)form.Nodes.First(node => node.Name == "Number");
		var templateNode = (ITemplateNode)form.Nodes.First(node => node.Name == "TemplatedSection");
		var textNode = (ITextNode)form.Nodes.First(node => node.Name == "Text");
		var timestampNode = (ITimestampNode)form.Nodes.First(node => node.Name == "Timestamp");

		// Modify state.
		form.IsVisible = false;
		form.IsReadonly = true;
		form.SetValidationError("test", "error");
		booleanNode.HasUserInteraction = true;
		booleanNode.Value = false;
		collectionNode.Instantiate(form);
		fileNode.Value = new FileValue() { FileContents = [1, 2, 3], FileName = "TestFile" };
		numberNode.Value = 42;
		templateNode.Instantiate(form);
		textNode.Value = "TestText";
		var initialTimestamp = DateTime.Now;
		timestampNode.Value = initialTimestamp;

		// Clone.
		var clone = (IForm)form.Clone();
		var clonedBooleanNode = (IBooleanNode)clone.Nodes.First(node => node.Name == "Boolean");
		var clonedCollectionNode = (ICollectionNode)clone.Nodes.First(node => node.Name == "Collection");
		var clonedFileNode = (IFileNode)clone.Nodes.First(node => node.Name == "File");
		var clonedNumberNode = (INumberNode)clone.Nodes.First(node => node.Name == "Number");
		var clonedTemplateNode = (ITemplateNode)clone.Nodes.First(node => node.Name == "TemplatedSection");
		var clonedTextNode = (ITextNode)clone.Nodes.First(node => node.Name == "Text");
		var clonedTimestampNode = (ITimestampNode)clone.Nodes.First(node => node.Name == "Timestamp");

		// Modify Clone state.
		clone.IsVisible = true;
		clone.IsReadonly = false;
		clone.SetValidationError("test", "error 2");
		clonedBooleanNode.HasUserInteraction = false;
		clonedBooleanNode.Value = true;
		clonedCollectionNode.Clear();
		clonedFileNode.Value = new FileValue() { FileContents = [1, 2, 3, 4], FileName = "TestFile 2" };
		clonedNumberNode.Value = 43;
		clonedTemplateNode.Clear();
		clonedTextNode.Value = "TestText 2";
		clonedTimestampNode.Value = DateTime.Today;

		// Ensure original didn't change.
		Assert.AreEqual(false, form.IsVisible);
		Assert.AreEqual(true, form.IsReadonly);
		Assert.AreEqual("error", form.ValidationErrors.First());
		Assert.AreEqual(true, booleanNode.HasUserInteraction);
		Assert.AreEqual(false, booleanNode.Value);
		Assert.AreEqual(1, collectionNode.Instances.Count());
		Assert.AreEqual([1, 2, 3], fileNode.Value.FileContents);
		Assert.AreEqual("TestFile", fileNode.Value.FileName);
		Assert.AreEqual(42, numberNode.Value);
		Assert.IsTrue(templateNode.Instance != null);
		Assert.AreEqual("TestText", textNode.Value);
		Assert.AreEqual(initialTimestamp, timestampNode.Value);
	}

	[TestMethod]
	public void ClonedNodeRecursiveTemplates_ShouldBeRedirectedToAppropriateParent()
	{
		// Build structure.
		var form = new FormBuilder("Test")
			.WithCollectionNode(
				"Collection",
				(builder, recursiveTemplate) => builder.UsePreconfiguredTemplate(recursiveTemplate)
			)
			.WithTemplatedSection(
				"TemplatedSection",
				(builder, recursiveTemplate) => builder.UsePreconfiguredTemplate(recursiveTemplate)
			)
			.Build();

		// Get individual nodes.
		var collectionNode = (ICollectionNode)form.Nodes.First(node => node.Name == "Collection");
		var templateNode = (ITemplateNode)form.Nodes.First(node => node.Name == "TemplatedSection");

		// Clone.
		var clone = (IForm)form.Clone();
		var clonedCollectionNode = (ICollectionNode)clone.Nodes.First(node => node.Name == "Collection");
		var clonedTemplateNode = (ITemplateNode)clone.Nodes.First(node => node.Name == "TemplatedSection");

		Assert.AreEqual(form, collectionNode.Templates.First());
		Assert.AreEqual(form, templateNode.Templates.First());
		Assert.AreEqual(clone, clonedCollectionNode.Templates.First());
		Assert.AreEqual(clone, templateNode.Templates.First());
	}
}
