namespace RobinEpple.Common.Forms.Test;

using RobinEpple.Common.Forms.Nodes;

[TestClass]
public class Cloning
{
	[TestMethod]
	public void ClonedNode_ShouldDeepCloneAndHaveAllConfigurationsEqual()
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

		var booleanNode = (IBooleanNode)form.Nodes.First(node => node.Name == "Boolean");
		var collectionNode = (ICollectionNode)form.Nodes.First(node => node.Name == "Collection");
		var fileNode = (IFileNode)form.Nodes.First(node => node.Name == "File");
		var numberNode = (INumberNode)form.Nodes.First(node => node.Name == "Number");
		var templateNode = (ITemplateNode)form.Nodes.First(node => node.Name == "TemplatedSection");
		var textNode = (ITextNode)form.Nodes.First(node => node.Name == "Text");
		var timestampNode = (ITimestampNode)form.Nodes.First(node => node.Name == "Timestamp");

		var clone = (IForm)form.Clone();
		var clonedBooleanNode = (IBooleanNode)clone.Nodes.First(node => node.Name == "Boolean");
		var clonedCollectionNode = (ICollectionNode)clone.Nodes.First(node => node.Name == "Collection");
		var clonedFileNode = (IFileNode)clone.Nodes.First(node => node.Name == "File");
		var clonedNumberNode = (INumberNode)clone.Nodes.First(node => node.Name == "Number");
		var clonedTemplateNode = (ITemplateNode)clone.Nodes.First(node => node.Name == "TemplatedSection");
		var clonedTextNode = (ITextNode)clone.Nodes.First(node => node.Name == "Text");
		var clonedTimestampNode = (ITimestampNode)clone.Nodes.First(node => node.Name == "Timestamp");

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
		Assert.IsFalse(ReferenceEquals(collectionNode.Templates.First(), clonedCollectionNode.Templates.First()));
		Assert.AreEqual(numberNode.Formatter, clonedNumberNode.Formatter);
		Assert.AreEqual(templateNode.Templates.Count(), clonedTemplateNode.Templates.Count());
		Assert.IsFalse(ReferenceEquals(templateNode.Templates.First(), clonedTemplateNode.Templates.First()));
	}
}
