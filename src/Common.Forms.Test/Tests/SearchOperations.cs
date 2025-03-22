namespace RobinEpple.Common.Forms.Test.Tests;

using RobinEpple.Common.Forms;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Util;

[TestClass]
public class SearchOperations
{
	[TestMethod]
	public void FindNode_ShouldFindChildInForm()
	{
		var form = new FormBuilder("Test")
			.WithBooleanNode("Boolean")
			.WithCollectionNode("Collection")
			.WithFileNode("File")
			.WithNumberNode("Number")
			.WithTemplatedSection("Section")
			.WithTextNode("Text")
			.WithTimestampNode("Timestamp")
			.Build();

		Assert.IsTrue(form.FindNode("Boolean") is IBooleanNode);
		Assert.IsTrue(form.FindNode("Collection") is ICollectionNode);
		Assert.IsTrue(form.FindNode("File") is IFileNode);
		Assert.IsTrue(form.FindNode("Number") is INumberNode);
		Assert.IsTrue(form.FindNode("Section") is ITemplateNode);
		Assert.IsTrue(form.FindNode("Text") is ITextNode);
		Assert.IsTrue(form.FindNode("Timestamp") is ITimestampNode);
	}

	[TestMethod]
	public void FindNode_ShouldReturnNullIfNodeDoesNotExist()
	{
		var form = new FormBuilder("Test")
			.WithBooleanNode("Boolean")
			.WithCollectionNode("Collection")
			.WithFileNode("File")
			.WithNumberNode("Number")
			.WithTemplatedSection("Section")
			.WithTextNode("Text")
			.WithTimestampNode("Timestamp")
			.Build();

		Assert.AreEqual(null, form.FindNode("Text2"));
	}

	[TestMethod]
	public void FindNode_ShouldNotSearchTemplatedSectionTemplates()
	{
		var form = new FormBuilder("Test")
			.WithTemplatedSection(
				"Section",
				(node, _) => node.UseTemplate("Template", template => template.WithBooleanNode("Boolean"))
			)
			.Build();

		Assert.AreEqual(null, form.FindNode("Boolean"));
	}

	[TestMethod]
	public void FindNode_ShouldSearchTemplatedSectionInstances()
	{
		var form = new FormBuilder("Test")
			.WithTemplatedSection(
				"Section",
				(node, _) => node.UseTemplate("Template", template => template.WithBooleanNode("Boolean"))
			)
			.Build();

		var section = (ITemplateNode)form.Nodes.First();
		var template = section.Templates.First();
		section.Instantiate(template);
		var booleanInstance = (IBooleanNode)section.Instance!.Nodes.First();
		// Set value to make sure the node is from the instance and not the template.
		booleanInstance.Value = true;

		Assert.IsTrue(form.FindNode("Boolean") is IBooleanNode { Value: true });
	}

	[TestMethod]
	public void FindNode_ShouldNotSearchCollections()
	{
		var form = new FormBuilder("Test")
			.WithCollectionNode(
				"Collection",
				(node, _) => node.UseTemplate("Template", template => template.WithBooleanNode("Boolean"))
			)
			.Build();

		var collection = (ICollectionNode)form.Nodes.First();
		var template = collection.Templates.First();
		collection.Instantiate(template);

		// The collection should not search all instances.
		Assert.AreEqual(null, form.FindNode("Boolean"));

		// But if a specific instance is searched the (now unique) node should be found.
		Assert.IsTrue(collection.Instances.First().FindNode("Boolean") is IBooleanNode);
	}

	[TestMethod]
	public void FindNode_ShouldNotTraverseUp()
	{
		var form = new FormBuilder("Test")
			.WithTemplatedSection("Section", (node, _) => node.UseTemplate("Template"))
			.WithBooleanNode("Boolean")
			.Build();

		var section = (ITemplateNode)form.Nodes.First();
		var template = section.Templates.First();
		section.Instantiate(template);

		Assert.IsTrue(section.Instance!.FindNode("Boolean") == null);
		Assert.IsTrue(form!.FindNode("Boolean") is IBooleanNode);
	}

	[TestMethod]
	public void FindNodeRecursiveTemplates_ShouldReturnUppermostInstance()
	{
		var form = new FormBuilder("Test")
			.WithTemplatedSection(
				"Section",
				(node, recursiveTemplate) => node.UsePreConfiguredTemplate(recursiveTemplate)
			)
			.WithBooleanNode("Boolean")
			.Build();

		var section = (ITemplateNode)form.Nodes.First();
		var template = section.Templates.First();
		section.Instantiate(template);
		var upperBooleanNode = form.Nodes.First(node => node.Name == "Boolean");

		Assert.IsTrue(ReferenceEquals(upperBooleanNode, form.FindNode("Boolean")));
	}

	[TestMethod]
	public void FindNodeEqualityComparer_ShouldTakeEffect()
	{
		var form = new FormBuilder("Test").WithBooleanNode("Boolean").Build();

		Assert.IsTrue(form.FindNode("boolean", StringComparer.OrdinalIgnoreCase) is IBooleanNode);
	}

	[TestMethod]
	public void FindNodes_ShouldReturnEmptyIfNoNodeIsFound()
	{
		var form = new FormBuilder("Test")
			.WithBooleanNode("Boolean")
			.WithCollectionNode("Collection")
			.WithFileNode("File")
			.WithNumberNode("Number")
			.WithTemplatedSection("Section")
			.WithTextNode("Text")
			.WithTimestampNode("Timestamp")
			.Build();

		Assert.IsTrue(form.FindNodes("Text2").None());
	}

	[TestMethod]
	public void FindNodes_ShouldFindAllInstancesIncludingTemplatesAndCollections()
	{
		var form = new FormBuilder("Test")
			.WithBooleanNode("Boolean")
			.WithCollectionNode(
				"Collection",
				(node, recursiveTemplate) => node.UsePreConfiguredTemplate(recursiveTemplate)
			)
			.WithTemplatedSection(
				"Section",
				(node, recursiveTemplate) => node.UsePreConfiguredTemplate(recursiveTemplate)
			)
			.Build();

		var collectionNode = (ICollectionNode)form.Nodes.First(node => node.Name == "Collection");
		var templateNode = (ITemplateNode)form.Nodes.First(node => node.Name == "Section");
		collectionNode.Instantiate(form);
		collectionNode.Instantiate(form);
		templateNode.Instantiate(form);

		// Should find one on top level, one in templated section and two in collection.
		Assert.IsTrue(form.FindNodes("Boolean").Count() == 4);
		Assert.IsTrue(form.FindNodes("Boolean").All(node => node is IBooleanNode));
	}

	[TestMethod]
	public void FindNodes_ShouldNotTraverseUp()
	{
		var form = new FormBuilder("Test")
			.WithTemplatedSection("Section", (node, _) => node.UseTemplate("Template"))
			.WithCollectionNode("Collection", (node, _) => node.UseTemplate("Template"))
			.WithBooleanNode("Boolean")
			.Build();

		var section = (ITemplateNode)form.Nodes.First(node => node.Name == "Section");
		section.Instantiate(section.Templates.First());
		var collection = (ICollectionNode)form.Nodes.First(node => node.Name == "Collection");
		collection.Instantiate(collection.Templates.First());

		Assert.IsTrue(section.Instance!.FindNodes("Boolean").None());
		Assert.IsTrue(collection.Instances.First().FindNodes("Boolean").None());
		Assert.IsTrue(form!.FindNodes("Boolean").Count() == 1);
	}
}
