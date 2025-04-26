namespace RobinEpple.Common.Forms.Test.Tests;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.Test.Mocks;

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
				(builder, recursiveTemplate) => builder.UsePreConfiguredTemplate(recursiveTemplate)
			)
			.WithFileNode("File")
			.WithNumberNode("Number")
			.WithTemplatedSection(
				"TemplatedSection",
				(builder, recursiveTemplate) => builder.UsePreConfiguredTemplate(recursiveTemplate)
			)
			.WithTextNode("Text")
			.WithTimestampNode("Timestamp")
			.UseLabel("TestLabel")
			.UseDefaultVisibility(false)
			.UseDefaultReadonly(true)
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

		Assert.AreEqual(form.Nodes.Count(), clone.Nodes.Count());
		Assert.AreEqual(collectionNode.Templates.Count(), clonedCollectionNode.Templates.Count());
		Assert.AreEqual(templateNode.Templates.Count(), clonedTemplateNode.Templates.Count());
	}

	[TestMethod]
	public void StatelessDecorators_ShouldNotBeCloned()
	{
		// Build structure.
		var form = new FormBuilder("Test")
			.WithCollectionNode(
				"Collection",
				(builder, recursiveTemplate) =>
					builder.UsePreConfiguredTemplate(recursiveTemplate).UseBinding(new MockCollectionBinding())
			)
			.WithNumberNode(
				"Number",
				fieldBuilder => fieldBuilder.UseFormatter(new TestEuroFormatter()).UseBinding(new MockFieldBinding())
			)
			.WithTemplatedSection(
				"TemplatedSection",
				(builder, recursiveTemplate) =>
					builder.UsePreConfiguredTemplate(recursiveTemplate).UseBinding(new MockTemplateBinding())
			)
			.UseVisibilityCondition(new FalseMockCondition())
			.UseValidator(new ValidMockValidator())
			.UseExtension(new MockExtension())
			.Build();

		// Get individual nodes.
		var collectionNode = (ICollectionNode)form.Nodes.First(node => node.Name == "Collection");
		var numberNode = (INumberNode)form.Nodes.First(node => node.Name == "Number");
		var templateNode = (ITemplateNode)form.Nodes.First(node => node.Name == "TemplatedSection");

		// Clone.
		var clone = (IForm)form.Clone();
		var clonedCollectionNode = (ICollectionNode)clone.Nodes.First(node => node.Name == "Collection");
		var clonedNumberNode = (INumberNode)clone.Nodes.First(node => node.Name == "Number");
		var clonedTemplateNode = (ITemplateNode)clone.Nodes.First(node => node.Name == "TemplatedSection");

		// Compare.
		Assert.AreEqual(form.VisibilityCondition, clone.VisibilityCondition);
		Assert.AreEqual(form.NodeValidators.First(), clone.NodeValidators.First());
		Assert.AreEqual(form.Extensions.First(), clone.Extensions.First());
		Assert.AreEqual(collectionNode.Binding, clonedCollectionNode.Binding);
		Assert.AreEqual(numberNode.Formatter, clonedNumberNode.Formatter);
		Assert.AreEqual(numberNode.Binding, clonedNumberNode.Binding);
		Assert.AreEqual(templateNode.Binding, clonedTemplateNode.Binding);
	}

	[TestMethod]
	public void CloneNode_ShouldCopyNodeState()
	{
		// Build structure.
		var form = new FormBuilder("Test")
			.WithBooleanNode("Boolean")
			.WithCollectionNode(
				"Collection",
				(builder, recursiveTemplate) => builder.UsePreConfiguredTemplate(recursiveTemplate)
			)
			.WithFileNode("File")
			.WithNumberNode("Number", fieldBuilder => fieldBuilder.UseFormatter(new TestEuroFormatter()))
			.WithTemplatedSection(
				"TemplatedSection",
				(builder, recursiveTemplate) => builder.UsePreConfiguredTemplate(recursiveTemplate)
			)
			.WithTextNode("Text")
			.WithTimestampNode("Timestamp")
			.UseLabel("TestLabel")
			.UseDefaultVisibility(true)
			.UseDefaultReadonly(true)
			.UseVisibilityCondition(new FalseMockCondition())
			.UseReadonlyCondition(new FalseMockCondition())
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
		form.SetValidationError("test", "error");
		form.SetTag("test", 42);
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
		Assert.AreEqual(form.ValidationErrorsByKey.First(), clone.ValidationErrorsByKey.First());
		Assert.IsFalse(ReferenceEquals(form.ValidationErrorsByKey, clone.ValidationErrorsByKey));
		Assert.AreEqual(form.Tags.First(), clone.Tags.First());
		Assert.IsFalse(ReferenceEquals(form.Tags, clone.Tags));
		Assert.AreEqual(booleanNode.HasUserInteraction, clonedBooleanNode.HasUserInteraction);
		Assert.AreEqual(booleanNode.Value, clonedBooleanNode.Value);
		Assert.AreEqual(collectionNode.Instances.Count(), clonedCollectionNode.Instances.Count());
		Assert.IsFalse(ReferenceEquals(collectionNode.Instances.First(), clonedCollectionNode.Instances.First()));
		Assert.AreEqual(fileNode.Value.FileContents[0], clonedFileNode.Value.FileContents![0]);
		Assert.AreEqual(fileNode.Value.FileContents[1], clonedFileNode.Value.FileContents![1]);
		Assert.AreEqual(fileNode.Value.FileContents[2], clonedFileNode.Value.FileContents![2]);
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
				(builder, recursiveTemplate) => builder.UsePreConfiguredTemplate(recursiveTemplate)
			)
			.WithFileNode("File")
			.WithNumberNode("Number", fieldBuilder => fieldBuilder.UseFormatter(new TestEuroFormatter()))
			.WithTemplatedSection(
				"TemplatedSection",
				(builder, recursiveTemplate) => builder.UsePreConfiguredTemplate(recursiveTemplate)
			)
			.WithTextNode("Text")
			.WithTimestampNode("Timestamp")
			.UseLabel("TestLabel")
			.UseDefaultVisibility(true)
			.UseDefaultReadonly(true)
			.UseVisibilityCondition(new FalseMockCondition())
			.UseReadonlyCondition(new FalseMockCondition())
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
		form.SetValidationError("test", "error");
		form.SetTag("test", 42);
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
		clone.SetValidationError("test", "error 2");
		clone.SetTag("test", 43);
		clonedBooleanNode.HasUserInteraction = false;
		clonedBooleanNode.Value = true;
		clonedCollectionNode.Clear();
		clonedFileNode.Value = new FileValue() { FileContents = [1, 2, 3, 4], FileName = "TestFile 2" };
		clonedNumberNode.Value = 43;
		clonedTemplateNode.Clear();
		clonedTextNode.Value = "TestText 2";
		clonedTimestampNode.Value = DateTime.Today;

		// Ensure original didn't change.
		Assert.AreEqual(true, form.IsVisible);
		Assert.AreEqual(true, form.IsReadonly);
		Assert.IsTrue(form.ValidationErrorsByKey.TryGetValue("test", out var message) && message == "error");
		Assert.IsTrue(form.Tags.TryGetValue("test", out var tag) && (tag?.Equals(42) ?? false));
		Assert.AreEqual(true, booleanNode.HasUserInteraction);
		Assert.AreEqual(false, booleanNode.Value);
		Assert.AreEqual(1, collectionNode.Instances.Count());
		Assert.AreEqual(1, fileNode.Value.FileContents[0]);
		Assert.AreEqual(2, fileNode.Value.FileContents[1]);
		Assert.AreEqual(3, fileNode.Value.FileContents[2]);
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
				(builder, recursiveTemplate) => builder.UsePreConfiguredTemplate(recursiveTemplate)
			)
			.WithTemplatedSection(
				"TemplatedSection",
				(builder, recursiveTemplate) => builder.UsePreConfiguredTemplate(recursiveTemplate)
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
		Assert.AreEqual(clone, clonedTemplateNode.Templates.First());
	}
}
