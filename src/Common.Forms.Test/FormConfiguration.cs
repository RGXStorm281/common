namespace Common.Forms.Test;

using System.Globalization;
using RobinEpple.Common.Forms;
using RobinEpple.Common.Forms.Nodes;

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
	public void UseDefaultVisilibity_ShouldReplaceDefaultVisibility()
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
