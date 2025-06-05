namespace RobinEpple.Common.Forms.Test.Tests;

using RobinEpple.Common.Forms;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.Test.Mocks;

[TestClass]
public class TreeTraversal
{
	[TestMethod]
	public void BreadthFirst_ShouldVisitLayersInOrder()
	{
		// Build the structure.
		var form = new FormBuilder("Test")
			.WithBooleanNode("Boolean")
			.WithTextNode("Text")
			.WithNumberNode("Number")
			.WithFileNode("File")
			.WithTimestampNode("Timestamp")
			.WithCollectionNode(
				"Collection",
				(builder, _) =>
					builder.UseTemplate("CollectionTemplate", builder => builder.WithBooleanNode("InstanceBoolean"))
			)
			.WithTemplatedSection(
				"Template",
				(builder, _) =>
					builder.UseTemplate("SectionTemplate", builder => builder.WithBooleanNode("InstanceBoolean"))
			)
			.WithSection("SubForm", (builder, _) => builder.WithBooleanNode("SubFormBoolean"))
			.Build();

		// Get all nodes.
		var booleanNode = (IBooleanNode)form.Nodes.First(node => node.Name == "Boolean");
		var textNode = (ITextNode)form.Nodes.First(node => node.Name == "Text");
		var numberNode = (INumberNode)form.Nodes.First(node => node.Name == "Number");
		var fileNode = (IFileNode)form.Nodes.First(node => node.Name == "File");
		var timestampNode = (ITimestampNode)form.Nodes.First(node => node.Name == "Timestamp");

		var collectionNode = (ICollectionNode)form.Nodes.First(node => node.Name == "Collection");
		collectionNode.Instantiate(collectionNode.Templates.First());
		collectionNode.Instantiate(collectionNode.Templates.First());
		var collectionInstance1 = collectionNode.Instances.First();
		var collectionInstance2 = collectionNode.Instances.Skip(1).First();
		var collectionInstance1Boolean = (IBooleanNode)
			collectionInstance1.Nodes.First(node => node.Name == "InstanceBoolean");
		var collectionInstance2Boolean = (IBooleanNode)
			collectionInstance2.Nodes.First(node => node.Name == "InstanceBoolean");

		var templateNode = (ITemplateNode)form.Nodes.First(node => node.Name == "Template");
		templateNode.Instantiate(templateNode.Templates.First());
		var templateInstance = templateNode.Instance!;
		var templateInstanceBoolean = (IBooleanNode)
			templateInstance.Nodes.First(node => node.Name == "InstanceBoolean");

		var subFormNode = (IForm)form.Nodes.First(node => node.Name == "SubForm");
		var subFormBoolean = (IBooleanNode)subFormNode.Nodes.First(node => node.Name == "SubFormBoolean");

		// Index nodes according to traversal.
		var indexer = new BreadthFirstIndexer();
		indexer.RunOn(form);

		// Make sure the nodes have been visited in the correct order.
		// Layer 1
		Assert.AreEqual(form.Tags[BreadthFirstIndexer.IndexTagName], 0);
		Assert.AreEqual(booleanNode.Tags[BreadthFirstIndexer.IndexTagName], 1);
		Assert.AreEqual(textNode.Tags[BreadthFirstIndexer.IndexTagName], 2);
		Assert.AreEqual(numberNode.Tags[BreadthFirstIndexer.IndexTagName], 3);
		Assert.AreEqual(fileNode.Tags[BreadthFirstIndexer.IndexTagName], 4);
		Assert.AreEqual(timestampNode.Tags[BreadthFirstIndexer.IndexTagName], 5);
		Assert.AreEqual(collectionNode.Tags[BreadthFirstIndexer.IndexTagName], 6);
		Assert.AreEqual(templateNode.Tags[BreadthFirstIndexer.IndexTagName], 7);
		Assert.AreEqual(subFormNode.Tags[BreadthFirstIndexer.IndexTagName], 8);
		// Layer 2
		Assert.AreEqual(collectionInstance1.Tags[BreadthFirstIndexer.IndexTagName], 9);
		Assert.AreEqual(collectionInstance2.Tags[BreadthFirstIndexer.IndexTagName], 10);
		Assert.AreEqual(templateInstance.Tags[BreadthFirstIndexer.IndexTagName], 11);
		Assert.AreEqual(subFormBoolean.Tags[BreadthFirstIndexer.IndexTagName], 12);
		// Layer 3
		Assert.AreEqual(collectionInstance1Boolean.Tags[BreadthFirstIndexer.IndexTagName], 13);
		Assert.AreEqual(collectionInstance2Boolean.Tags[BreadthFirstIndexer.IndexTagName], 14);
		Assert.AreEqual(templateInstanceBoolean.Tags[BreadthFirstIndexer.IndexTagName], 15);
	}

	[TestMethod]
	public void DepthFirst_ShouldVisitBranchesInOrder()
	{
		// Build the structure.
		var form = new FormBuilder("Test")
			.WithBooleanNode("Boolean")
			.WithTextNode("Text")
			.WithNumberNode("Number")
			.WithFileNode("File")
			.WithTimestampNode("Timestamp")
			.WithCollectionNode(
				"Collection",
				(builder, _) =>
					builder.UseTemplate("CollectionTemplate", builder => builder.WithBooleanNode("InstanceBoolean"))
			)
			.WithTemplatedSection(
				"Template",
				(builder, _) =>
					builder.UseTemplate("SectionTemplate", builder => builder.WithBooleanNode("InstanceBoolean"))
			)
			.WithSection("SubForm", (builder, _) => builder.WithBooleanNode("SubFormBoolean"))
			.Build();

		// Get all nodes.
		var booleanNode = (IBooleanNode)form.Nodes.First(node => node.Name == "Boolean");
		var textNode = (ITextNode)form.Nodes.First(node => node.Name == "Text");
		var numberNode = (INumberNode)form.Nodes.First(node => node.Name == "Number");
		var fileNode = (IFileNode)form.Nodes.First(node => node.Name == "File");
		var timestampNode = (ITimestampNode)form.Nodes.First(node => node.Name == "Timestamp");

		var collectionNode = (ICollectionNode)form.Nodes.First(node => node.Name == "Collection");
		collectionNode.Instantiate(collectionNode.Templates.First());
		collectionNode.Instantiate(collectionNode.Templates.First());
		var collectionInstance1 = collectionNode.Instances.First();
		var collectionInstance2 = collectionNode.Instances.Skip(1).First();
		var collectionInstance1Boolean = (IBooleanNode)
			collectionInstance1.Nodes.First(node => node.Name == "InstanceBoolean");
		var collectionInstance2Boolean = (IBooleanNode)
			collectionInstance2.Nodes.First(node => node.Name == "InstanceBoolean");

		var templateNode = (ITemplateNode)form.Nodes.First(node => node.Name == "Template");
		templateNode.Instantiate(templateNode.Templates.First());
		var templateInstance = templateNode.Instance!;
		var templateInstanceBoolean = (IBooleanNode)
			templateInstance.Nodes.First(node => node.Name == "InstanceBoolean");

		var subFormNode = (IForm)form.Nodes.First(node => node.Name == "SubForm");
		var subFormBoolean = (IBooleanNode)subFormNode.Nodes.First(node => node.Name == "SubFormBoolean");

		// Index nodes according to traversal.
		var indexer = new DepthFirstIndexer();
		indexer.RunOn(form);

		// Make sure the nodes have been visited in the correct order.
		Assert.AreEqual(form.Tags[DepthFirstIndexer.IndexTagName], 0);
		Assert.AreEqual(booleanNode.Tags[DepthFirstIndexer.IndexTagName], 1);
		Assert.AreEqual(textNode.Tags[DepthFirstIndexer.IndexTagName], 2);
		Assert.AreEqual(numberNode.Tags[DepthFirstIndexer.IndexTagName], 3);
		Assert.AreEqual(fileNode.Tags[DepthFirstIndexer.IndexTagName], 4);
		Assert.AreEqual(timestampNode.Tags[DepthFirstIndexer.IndexTagName], 5);
		// Branch 1
		Assert.AreEqual(collectionNode.Tags[DepthFirstIndexer.IndexTagName], 6);
		Assert.AreEqual(collectionInstance1.Tags[DepthFirstIndexer.IndexTagName], 7);
		Assert.AreEqual(collectionInstance1Boolean.Tags[DepthFirstIndexer.IndexTagName], 8);
		Assert.AreEqual(collectionInstance2.Tags[DepthFirstIndexer.IndexTagName], 9);
		Assert.AreEqual(collectionInstance2Boolean.Tags[DepthFirstIndexer.IndexTagName], 10);
		// Branch 2
		Assert.AreEqual(templateNode.Tags[DepthFirstIndexer.IndexTagName], 11);
		Assert.AreEqual(templateInstance.Tags[DepthFirstIndexer.IndexTagName], 12);
		Assert.AreEqual(templateInstanceBoolean.Tags[DepthFirstIndexer.IndexTagName], 13);
		// Branch 3
		Assert.AreEqual(subFormNode.Tags[DepthFirstIndexer.IndexTagName], 14);
		Assert.AreEqual(subFormBoolean.Tags[DepthFirstIndexer.IndexTagName], 15);
	}
}
