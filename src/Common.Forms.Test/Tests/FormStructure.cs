namespace RobinEpple.Common.Forms.Test.Tests;

using RobinEpple.Common.Forms;
using RobinEpple.Common.Forms.Nodes;

[TestClass]
public sealed class FormStructure
{
	[TestMethod]
	public void EmptyForm_ShouldNotThrow()
	{
		var builder = new FormBuilder("Test");
		var form = builder.Build();

		Assert.IsTrue(form != null);
	}

	[TestMethod]
	public void WithTextNode_ShouldAddSingleTextNode()
	{
		var builder = new FormBuilder("Test").WithTextNode("Text");
		var form = builder.Build();

		Assert.IsTrue(form.Nodes.Count() == 1);
		Assert.IsTrue(form.Nodes.First() is ITextNode { Name: "Text" });
	}

	[TestMethod]
	public void WithNumberNode_ShouldAddSingleNumberNode()
	{
		var builder = new FormBuilder("Test").WithNumberNode("Number");
		var form = builder.Build();

		Assert.IsTrue(form.Nodes.Count() == 1);
		Assert.IsTrue(form.Nodes.First() is INumberNode { Name: "Number" });
	}

	[TestMethod]
	public void WithTimestampNode_ShouldAddSingleTimestampNode()
	{
		var builder = new FormBuilder("Test").WithTimestampNode("Timestamp");
		var form = builder.Build();

		Assert.IsTrue(form.Nodes.Count() == 1);
		Assert.IsTrue(form.Nodes.First() is ITimestampNode { Name: "Timestamp" });
	}

	[TestMethod]
	public void WithBooleanNode_ShouldAddSingleBooleanNode()
	{
		var builder = new FormBuilder("Test").WithBooleanNode("Boolean");
		var form = builder.Build();

		Assert.IsTrue(form.Nodes.Count() == 1);
		Assert.IsTrue(form.Nodes.First() is IBooleanNode { Name: "Boolean" });
	}

	[TestMethod]
	public void WithFileNode_ShouldAddSingleFileNode()
	{
		var builder = new FormBuilder("Test").WithFileNode("File");
		var form = builder.Build();

		Assert.IsTrue(form.Nodes.Count() == 1);
		Assert.IsTrue(form.Nodes.First() is IFileNode { Name: "File" });
	}

	[TestMethod]
	public void WithCollectionNode_ShouldAddSingleCollectionNode()
	{
		var builder = new FormBuilder("Test").WithCollectionNode("Collection", (_, _) => { });
		var form = builder.Build();

		Assert.IsTrue(form.Nodes.Count() == 1);
		Assert.IsTrue(form.Nodes.First() is ICollectionNode { Name: "Collection" });
	}

	[TestMethod]
	public void WithTemplatedSection_ShouldAddSingleTemplatedSectionNode()
	{
		var builder = new FormBuilder("Test").WithTemplatedSection("TemplatedSection", (_, _) => { });
		var form = builder.Build();

		Assert.IsTrue(form.Nodes.Count() == 1);
		Assert.IsTrue(form.Nodes.First() is ITemplateNode { Name: "TemplatedSection" });
	}

	[TestMethod]
	public void MultipleNodes_ShouldStayInOrder()
	{
		var builder = new FormBuilder("Test")
			.WithTextNode("Text1")
			.WithNumberNode("Number1")
			.WithCollectionNode("Collection1", (_, _) => { })
			.WithTextNode("Text2");
		var form = builder.Build();

		var nodeList = form.Nodes.ToList();
		Assert.IsTrue(nodeList.Count == 4);
		Assert.IsTrue(nodeList[0] is ITextNode { Name: "Text1" });
		Assert.IsTrue(nodeList[1] is INumberNode { Name: "Number1" });
		Assert.IsTrue(nodeList[2] is ICollectionNode { Name: "Collection1" });
		Assert.IsTrue(nodeList[3] is ITextNode { Name: "Text2" });
	}

	[TestMethod]
	public void IdenticalName_ShouldThrowInvalidOperationException()
	{
		var builder = new FormBuilder("Test").WithTextNode("Text");
		Assert.ThrowsException<InvalidOperationException>(() => builder.WithNumberNode("Text"));
	}

	[TestMethod]
	public void InvalidCharacterInName_ShouldThrowInvalidOperationException()
	{
		var builder = new FormBuilder("Test");
		Assert.ThrowsException<InvalidOperationException>(
			() => builder.WithNumberNode($"Tex{IFormNode.PathSeparator}t")
		);
		Assert.ThrowsException<InvalidOperationException>(
			() => builder.WithNumberNode($"Tex{IFormNode.IndexIdentifer.First()}t")
		);
		Assert.ThrowsException<InvalidOperationException>(
			() => builder.WithNumberNode($"Tex{IFormNode.IndexIdentifer.Last()}t")
		);
		Assert.ThrowsException<InvalidOperationException>(() => builder.WithNumberNode($"$Text"));
		Assert.ThrowsException<InvalidOperationException>(() => builder.WithNumberNode($"Text "));
	}

	[TestMethod]
	public void TemplatedNodes_ShouldSupportMultipleTemplatesAndKeepOrder()
	{
		var builder = new FormBuilder("Test").WithTemplatedSection(
			"TemplatedSection",
			(builder, _) => builder.UseTemplate("Template1").UseTemplate("Template2")
		);

		var form = builder.Build();

		Assert.IsTrue(form.Nodes.Count() == 1);
		var templateNode = (ITemplateNode)form.Nodes.First();
		var templateList = templateNode.Templates.ToList();
		Assert.IsTrue(templateList.Count == 2);
		Assert.IsTrue(templateList[0] is IForm { Name: "Template1" });
		Assert.IsTrue(templateList[1] is IForm { Name: "Template2" });
	}

	[TestMethod]
	public void TemplatedNodes_ShouldSupportRecursion()
	{
		var builder = new FormBuilder("Test").WithTemplatedSection(
			"TemplatedSection",
			(builder, recursiveTemplate) => builder.UsePreconfiguredTemplate(recursiveTemplate)
		);

		var form = builder.Build();
		Assert.IsTrue(form.Nodes.Count() == 1);
		var templateNode = (ITemplateNode)form.Nodes.First();
		var templateList = templateNode.Templates.ToList();
		Assert.IsTrue(templateList.Count == 1);
		Assert.AreEqual(templateList[0], form);
	}

	[TestMethod]
	public void IdenticalTemplateNames_ShouldThrowInvalidOperationException()
	{
		var builder = new FormBuilder("Test").WithTemplatedSection(
			"TemplatedSection",
			(builder, recursiveTemplate) =>
			{
				builder.UsePreconfiguredTemplate(recursiveTemplate);
				Assert.ThrowsException<InvalidOperationException>(() => builder.UseTemplate("Test"));
			}
		);
	}

	[TestMethod]
	public void IdenticalNamesInTemplates_ShouldNotThrow()
	{
		var builder = new FormBuilder("Test")
			.WithBooleanNode("Boolean")
			.WithTemplatedSection(
				"TemplatedSection",
				(builder, _) =>
					builder
						.UseTemplate("Template1", templateBuilder => templateBuilder.WithBooleanNode("Boolean"))
						.UseTemplate("Template2", templateBuilder => templateBuilder.WithBooleanNode("Boolean"))
			);
	}
}
