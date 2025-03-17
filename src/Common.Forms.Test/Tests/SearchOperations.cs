namespace Company.TestProject1;

using RobinEpple.Common.Forms;
using RobinEpple.Common.Forms.Nodes;

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

		var section = (ICollectionNode)form.Nodes.First();
		var template = section.Templates.First();
		section.Instantiate(template);

		Assert.AreEqual(null, form.FindNode("Boolean"));
	}
}
