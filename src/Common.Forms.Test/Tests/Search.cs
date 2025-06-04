namespace RobinEpple.Common.Forms.Test.Tests;

using RobinEpple.Common.Forms;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Util;

[TestClass]
public class Search
{
	[TestMethod]
	public void FindFirst_ShouldFindChildInForm()
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

		Assert.IsTrue(form.FindFirst("Boolean") is IBooleanNode);
		Assert.IsTrue(form.FindFirst("Collection") is ICollectionNode);
		Assert.IsTrue(form.FindFirst("File") is IFileNode);
		Assert.IsTrue(form.FindFirst("Number") is INumberNode);
		Assert.IsTrue(form.FindFirst("Section") is ITemplateNode);
		Assert.IsTrue(form.FindFirst("Text") is ITextNode);
		Assert.IsTrue(form.FindFirst("Timestamp") is ITimestampNode);
	}

	[TestMethod]
	public void FindFirst_ShouldReturnNullIfNodeDoesNotExist()
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

		Assert.AreEqual(null, form.FindFirst("Text2"));
	}

	[TestMethod]
	public void FindFirst_ShouldNotSearchTemplatedSectionTemplates()
	{
		var form = new FormBuilder("Test")
			.WithTemplatedSection(
				"Section",
				(node, _) => node.UseTemplate("Template", template => template.WithBooleanNode("Boolean"))
			)
			.Build();

		Assert.AreEqual(null, form.FindFirst("Boolean"));
	}

	[TestMethod]
	public void FindFirst_ShouldSearchTemplatedSectionInstances()
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

		Assert.IsTrue(form.FindFirst("Boolean") is IBooleanNode { Value: true });
	}

	[TestMethod]
	public void FindFirst_ShouldSearchCollections()
	{
		var form = new FormBuilder("Test")
			.WithCollectionNode(
				"Collection",
				(node, _) => node.UseTemplate("Template", template => template.WithBooleanNode("Boolean"))
			)
			.Build();

		var collection = (ICollectionNode)form.Nodes.First();
		var template = collection.Templates.First();
		var instance1 = collection.Instantiate(template);
		var booleanNode1 = instance1.Nodes.First();
		var instance2 = collection.Instantiate(template);
		var booleanNode2 = instance1.Nodes.First();

		// The search should find the first instance.
		Assert.AreEqual(booleanNode1, form.FindFirst("Boolean"));
	}

	[TestMethod]
	public void FindFirst_ShouldNotTraverseUp()
	{
		var form = new FormBuilder("Test")
			.WithTemplatedSection("Section", (node, _) => node.UseTemplate("Template"))
			.WithBooleanNode("Boolean")
			.Build();

		var section = (ITemplateNode)form.Nodes.First();
		var template = section.Templates.First();
		section.Instantiate(template);

		Assert.IsTrue(section.Instance!.FindFirst("Boolean") == null);
		Assert.IsTrue(form!.FindFirst("Boolean") is IBooleanNode);
	}

	[TestMethod]
	public void FindFirst_RecursiveTemplates_ShouldReturnUppermostInstance()
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

		Assert.IsTrue(ReferenceEquals(upperBooleanNode, form.FindFirst("Boolean")));
	}

	[TestMethod]
	public void FindFirst_EqualityComparer_ShouldTakeEffect()
	{
		var form = new FormBuilder("Test").WithBooleanNode("Boolean").Build();

		Assert.IsTrue(form.FindFirst("boolean", StringComparer.OrdinalIgnoreCase) is IBooleanNode);
	}

	[TestMethod]
	public void FindAll_ShouldReturnEmptyIfNoNodeIsFound()
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

		Assert.IsTrue(form.FindAll("Text2").None());
	}

	[TestMethod]
	public void FindAll_ShouldFindAllInstancesIncludingTemplatesAndCollections()
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
		Assert.IsTrue(form.FindAll("Boolean").Count() == 4);
		Assert.IsTrue(form.FindAll("Boolean").All(node => node is IBooleanNode));
	}

	[TestMethod]
	public void FindAll_ShouldNotTraverseUp()
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

		Assert.IsTrue(section.Instance!.FindAll("Boolean").None());
		Assert.IsTrue(collection.Instances.First().FindAll("Boolean").None());
		Assert.IsTrue(form!.FindAll("Boolean").Count() == 1);
	}
}
