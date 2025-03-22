namespace RobinEpple.Common.Forms.Test.Tests;

using RobinEpple.Common.Forms;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.Test.Mocks;

[TestClass]
public class FormConfiguration
{
	[TestMethod]
	public void DefaultLabel_ShouldBeName()
	{
		var builder = new FormBuilder("Test").WithTextNode("Text");
		var form = builder.Build();
		var node = form.Nodes.First();
		Assert.IsTrue(node is ITextNode { Label: "Text" });
		node.Reset();
		Assert.IsTrue(node is ITextNode { Label: "Text" });
	}

	[TestMethod]
	public void UseLabel_ShouldReplaceDefaultLabel()
	{
		var builder = new FormBuilder("Test").WithTextNode("Text", nodeBuilder => nodeBuilder.UseLabel("Label"));
		var form = builder.Build();
		var node = form.Nodes.First();
		Assert.IsTrue(node is ITextNode { Label: "Label" });
		node.Reset();
		Assert.IsTrue(node is ITextNode { Label: "Label" });
	}

	[TestMethod]
	public void DefaultVisibility_ShouldBeTrue()
	{
		var builder = new FormBuilder("Test").WithTextNode("Text");
		var form = builder.Build();
		var node = form.Nodes.First();
		Assert.IsTrue(node is ITextNode { IsVisible: true });
		node.Reset();
		Assert.IsTrue(node is ITextNode { IsVisible: true });
	}

	[TestMethod]
	public void UseDefaultVisibility_ShouldReplaceDefaultVisibility()
	{
		var builder = new FormBuilder("Test").WithTextNode(
			"Text",
			nodeBuilder => nodeBuilder.UseDefaultVisibility(false)
		);
		var form = builder.Build();
		var node = form.Nodes.First();
		Assert.IsTrue(node is ITextNode { IsVisible: false });
		node.Reset();
		Assert.IsTrue(node is ITextNode { IsVisible: false });
	}

	[TestMethod]
	public void DefaultReadonly_ShouldBeFalse()
	{
		var builder = new FormBuilder("Test").WithTextNode("Text");
		var form = builder.Build();
		var node = form.Nodes.First();
		Assert.IsTrue(node is ITextNode { IsReadonly: false });
		node.Reset();
		Assert.IsTrue(node is ITextNode { IsReadonly: false });
	}

	[TestMethod]
	public void UseDefaultReadonly_ShouldReplaceDefaultReadonly()
	{
		var builder = new FormBuilder("Test").WithTextNode("Text", nodeBuilder => nodeBuilder.UseDefaultReadonly(true));
		var form = builder.Build();
		var node = form.Nodes.First();
		Assert.IsTrue(node is ITextNode { IsReadonly: true });
		node.Reset();
		Assert.IsTrue(node is ITextNode { IsReadonly: true });
	}

	[TestMethod]
	public void DefaultValue_ShouldBeNull()
	{
		var builder = new FormBuilder("Test")
			.WithBooleanNode("Boolean")
			.WithFileNode("File")
			.WithNumberNode("Number")
			.WithTextNode("Text")
			.WithTimestampNode("Timestamp");
		var form = builder.Build();
		var booleanNode = (IBooleanNode)form.Nodes.First(node => node.Name == "Boolean");
		var fileNode = (IFileNode)form.Nodes.First(node => node.Name == "File");
		var numberNode = (INumberNode)form.Nodes.First(node => node.Name == "Number");
		var textNode = (ITextNode)form.Nodes.First(node => node.Name == "Text");
		var timestampNode = (ITimestampNode)form.Nodes.First(node => node.Name == "Timestamp");

		// Make sure it starts with the right default.
		Assert.AreEqual(null, booleanNode.Value);
		Assert.AreEqual(null, fileNode.Value.FileContents);
		Assert.AreEqual(null, fileNode.Value.FileName);
		Assert.AreEqual(null, numberNode.Value);
		Assert.AreEqual(null, textNode.Value);
		Assert.AreEqual(null, timestampNode.Value);

		// just to make sure that reset reaches the children
		booleanNode.Value = true;
		fileNode.Value = new() { FileContents = [1, 2, 3], FileName = "TestFile" };
		numberNode.Value = 42;
		textNode.Value = "Test Text";
		timestampNode.Value = DateTime.Now;

		form.Reset();

		// Make sure it resets to the right default.
		Assert.AreEqual(null, booleanNode.Value);
		Assert.AreEqual(null, fileNode.Value.FileContents);
		Assert.AreEqual(null, fileNode.Value.FileName);
		Assert.AreEqual(null, numberNode.Value);
		Assert.AreEqual(null, textNode.Value);
		Assert.AreEqual(null, timestampNode.Value);
	}

	[TestMethod]
	public void UseDefaultValue_ShouldReplaceDefaultValue()
	{
		var builder = new FormBuilder("Test")
			.WithBooleanNode("Boolean", node => node.UseDefaultValue(false))
			.WithFileNode(
				"File",
				node => node.UseDefaultValue(new() { FileContents = [1, 2], FileName = "DefaultFile" })
			)
			.WithNumberNode("Number", node => node.UseDefaultValue(31))
			.WithTextNode("Text", node => node.UseDefaultValue("Default Text"))
			.WithTimestampNode("Timestamp", node => node.UseDefaultValue(DateTime.Today));
		var form = builder.Build();
		var booleanNode = (IBooleanNode)form.Nodes.First(node => node.Name == "Boolean");
		var fileNode = (IFileNode)form.Nodes.First(node => node.Name == "File");
		var numberNode = (INumberNode)form.Nodes.First(node => node.Name == "Number");
		var textNode = (ITextNode)form.Nodes.First(node => node.Name == "Text");
		var timestampNode = (ITimestampNode)form.Nodes.First(node => node.Name == "Timestamp");

		// Make sure it starts with the right default.
		Assert.AreEqual(false, booleanNode.Value);
		Assert.AreEqual(1, fileNode.Value.FileContents![0]);
		Assert.AreEqual(2, fileNode.Value.FileContents![1]);
		Assert.AreEqual("DefaultFile", fileNode.Value.FileName);
		Assert.AreEqual(31, numberNode.Value);
		Assert.AreEqual("Default Text", textNode.Value);
		Assert.AreEqual(DateTime.Today, timestampNode.Value);

		// just to make sure that reset reaches the children
		booleanNode.Value = true;
		fileNode.Value = new() { FileContents = [1, 2, 3], FileName = "TestFile" };
		numberNode.Value = 42;
		textNode.Value = "Test Text";
		timestampNode.Value = DateTime.Now;

		form.Reset();

		// Make sure it resets to the right default.
		Assert.AreEqual(false, booleanNode.Value);
		Assert.AreEqual(1, fileNode.Value.FileContents![0]);
		Assert.AreEqual(2, fileNode.Value.FileContents![1]);
		Assert.AreEqual("DefaultFile", fileNode.Value.FileName);
		Assert.AreEqual(31, numberNode.Value);
		Assert.AreEqual("Default Text", textNode.Value);
		Assert.AreEqual(DateTime.Today, timestampNode.Value);
	}

	[TestMethod]
	public void UseFormatter_ShouldReplaceDefaultFormatter()
	{
		var builder = new FormBuilder("Test").WithNumberNode(
			"Number",
			nodeBuilder => nodeBuilder.UseFormatter(new TestEuroFormatter())
		);
		var form = builder.Build();
		var node = (INumberNode)form.Nodes.First();
		node.Value = 10;
		var formattedValue = node.Formatter.Format(node.Value);
		Assert.AreEqual("10,00 €", formattedValue);
		node.Value = (decimal?)node.Formatter.Parse("20,00 €");
		Assert.AreEqual(20, node.Value);
	}
}
