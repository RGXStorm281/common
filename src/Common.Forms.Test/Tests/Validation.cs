namespace RobinEpple.Common.Forms.Test.Tests;

using RobinEpple.Common.Forms;
using RobinEpple.Common.Forms.Building;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.Validation;
using RobinEpple.Common.Util;

[TestClass]
public class Validation
{
	[TestMethod]
	public void RequireTrueValidator_NonBooleanField_ShouldThrowInvalidOperationException()
	{
		var form = new FormBuilder("Test").UseValidator(new RequireTrueValidator()).Build();

		Assert.ThrowsException<InvalidOperationException>(form.Update);
	}

	[TestMethod]
	public void RequireTrueValidator_ValueNull_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test")
			.WithBooleanNode("BooleanNode", node => node.UseRequireTrueValidator())
			.Build();

		var node = (IBooleanNode)form.Nodes.First(node => node.Name == "BooleanNode");
		node.Value = null;

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(node.ValidationErrors.Contains(Resources.TheField_RequiresTheValueTrue.Format("BooleanNode")));
	}

	[TestMethod]
	public void RequireTrueValidator_ValueFalse_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test")
			.WithBooleanNode("BooleanNode", node => node.UseRequireTrueValidator())
			.Build();

		var node = (IBooleanNode)form.Nodes.First(node => node.Name == "BooleanNode");
		node.Value = false;

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(node.ValidationErrors.Contains(Resources.TheField_RequiresTheValueTrue.Format("BooleanNode")));
	}

	[TestMethod]
	public void RequireTrueValidator_ValueTrue_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithBooleanNode("BooleanNode", node => node.UseRequireTrueValidator())
			.Build();

		var node = (IBooleanNode)form.Nodes.First(node => node.Name == "BooleanNode");
		node.Value = true;

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(node.ValidationErrors.Contains(Resources.TheField_RequiresTheValueTrue.Format("BooleanNode")));
	}

	[TestMethod]
	public void FileRequiredValidator_NonFileField_ShouldThrowInvalidOperationException()
	{
		var form = new FormBuilder("Test").UseValidator(new FileRequiredValidator()).Build();

		Assert.ThrowsException<InvalidOperationException>(form.Update);
	}

	[TestMethod]
	public void FileRequiredValidator_ContentsNull_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test").WithFileNode("FileNode", node => node.UseRequiredValidator()).Build();

		var node = (IFileNode)form.Nodes.First(node => node.Name == "FileNode");
		node.Value = new FileValue { FileContents = null, FileName = "Any" };

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(node.ValidationErrors.Contains(Resources.TheField_RequiresAnInput.Format("FileNode")));
	}

	[TestMethod]
	public void FileRequiredValidator_NameNull_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test").WithFileNode("FileNode", node => node.UseRequiredValidator()).Build();

		var node = (IFileNode)form.Nodes.First(node => node.Name == "FileNode");
		node.Value = new FileValue { FileContents = [1, 2, 3], FileName = null };

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(node.ValidationErrors.Contains(Resources.TheField_RequiresAnInput.Format("FileNode")));
	}

	[TestMethod]
	public void FileRequiredValidator_HasValue_ShouldBeValid()
	{
		var form = new FormBuilder("Test").WithFileNode("FileNode", node => node.UseRequiredValidator()).Build();

		var node = (IFileNode)form.Nodes.First(node => node.Name == "FileNode");
		node.Value = new FileValue { FileContents = [1, 2, 3], FileName = "Any" };

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(node.ValidationErrors.Contains(Resources.TheField_RequiresAnInput.Format("FileNode")));
	}

	[TestMethod]
	public void NumberRequiredValidator_NonNumberField_ShouldThrowInvalidOperationException()
	{
		var form = new FormBuilder("Test").UseValidator(new NumberRequiredValidator()).Build();

		Assert.ThrowsException<InvalidOperationException>(form.Update);
	}

	[TestMethod]
	public void NumberRequiredValidator_ValueNull_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test").WithNumberNode("NumberNode", node => node.UseRequiredValidator()).Build();

		var node = (INumberNode)form.Nodes.First(node => node.Name == "NumberNode");
		node.Value = null;

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(node.ValidationErrors.Contains(Resources.TheField_RequiresAnInput.Format("NumberNode")));
	}

	[TestMethod]
	public void NumberRequiredValidator_HasValue_ShouldBeValid()
	{
		var form = new FormBuilder("Test").WithNumberNode("NumberNode", node => node.UseRequiredValidator()).Build();

		var node = (INumberNode)form.Nodes.First(node => node.Name == "NumberNode");
		node.Value = 42;

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(node.ValidationErrors.Contains(Resources.TheField_RequiresAnInput.Format("NumberNode")));
	}

	[TestMethod]
	public void TextRequiredValidator_NonTextField_ShouldThrowInvalidOperationException()
	{
		var form = new FormBuilder("Test").UseValidator(new TextRequiredValidator()).Build();

		Assert.ThrowsException<InvalidOperationException>(form.Update);
	}

	[TestMethod]
	public void TextRequiredValidator_ValueNull_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test").WithTextNode("TextNode", node => node.UseRequiredValidator()).Build();

		var node = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		node.Value = null;

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(node.ValidationErrors.Contains(Resources.TheField_RequiresAnInput.Format("TextNode")));
	}

	[TestMethod]
	public void TextRequiredValidator_ValueEmpty_ShouldBeInvalidExceptIfConfigured()
	{
		var form = new FormBuilder("Test").WithTextNode("TextNode", node => node.UseRequiredValidator()).Build();
		var node = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");

		// Should not accept by default.
		node.Value = string.Empty;
		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(node.ValidationErrors.Contains(Resources.TheField_RequiresAnInput.Format("TextNode")));

		node.Value = "  ";
		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(node.ValidationErrors.Contains(Resources.TheField_RequiresAnInput.Format("TextNode")));

		form = new FormBuilder("Test")
			.WithTextNode("TextNode", node => node.UseRequiredValidator(acceptWhitespace: true))
			.Build();
		node = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");

		// But can be configured to accept empty values.
		node.Value = string.Empty;
		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(node.ValidationErrors.Contains(Resources.TheField_RequiresAnInput.Format("TextNode")));

		node.Value = "  ";
		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(node.ValidationErrors.Contains(Resources.TheField_RequiresAnInput.Format("TextNode")));
	}

	[TestMethod]
	public void TextRequiredValidator_HasValue_ShouldBeValid()
	{
		var form = new FormBuilder("Test").WithTextNode("TextNode", node => node.UseRequiredValidator()).Build();

		var node = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		node.Value = "Test text";

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(node.ValidationErrors.Contains(Resources.TheField_RequiresAnInput.Format("TextNode")));
	}

	[TestMethod]
	public void TimestampRequiredValidator_NonTimestampField_ShouldThrowInvalidOperationException()
	{
		var form = new FormBuilder("Test").UseValidator(new TimestampRequiredValidator()).Build();

		Assert.ThrowsException<InvalidOperationException>(form.Update);
	}

	[TestMethod]
	public void TimestampRequiredValidator_ValueNull_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test")
			.WithTimestampNode("TimestampNode", node => node.UseRequiredValidator())
			.Build();

		var node = (ITimestampNode)form.Nodes.First(node => node.Name == "TimestampNode");
		node.Value = null;

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(node.ValidationErrors.Contains(Resources.TheField_RequiresAnInput.Format("TimestampNode")));
	}

	[TestMethod]
	public void TimestampRequiredValidator_HasValue_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithTimestampNode("TimestampNode", node => node.UseRequiredValidator())
			.Build();

		var node = (ITimestampNode)form.Nodes.First(node => node.Name == "TimestampNode");
		node.Value = DateTime.Now;

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(node.ValidationErrors.Contains(Resources.TheField_RequiresAnInput.Format("TimestampNode")));
	}

	[TestMethod]
	public void TemplateRequiredValidator_NonTemplateField_ShouldThrowInvalidOperationException()
	{
		var form = new FormBuilder("Test").UseValidator(new TemplateRequiredValidator()).Build();

		Assert.ThrowsException<InvalidOperationException>(form.Update);
	}

	[TestMethod]
	public void TemplateRequiredValidator_NoInstance_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test")
			.WithTemplatedSection("TemplateNode", (node, _) => node.UseRequiredValidator())
			.Build();

		var node = (ITemplateNode)form.Nodes.First(node => node.Name == "TemplateNode");

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(node.ValidationErrors.Contains(Resources.TheField_RequiresAnInput.Format("TemplateNode")));
	}

	[TestMethod]
	public void TemplateRequiredValidator_HasInstance_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithTemplatedSection("TemplateNode", (node, _) => node.UseTemplate("Template").UseRequiredValidator())
			.Build();

		var node = (ITemplateNode)form.Nodes.First(node => node.Name == "TemplateNode");
		node.Instantiate(node.Templates.First());

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(node.ValidationErrors.Contains(Resources.TheField_RequiresAnInput.Format("TemplateNode")));
	}

	[TestMethod]
	public void MaxFileSizeValidator_NonFileField_ShouldThrowInvalidOperationException()
	{
		var form = new FormBuilder("Test").UseValidator(new MaxFileSizeValidator(2)).Build();

		Assert.ThrowsException<InvalidOperationException>(form.Update);
	}

	[TestMethod]
	public void MaxFileSizeValidator_ContentsNull_ShouldNotValidate()
	{
		var form = new FormBuilder("Test").WithFileNode("FileNode", node => node.UseMaxFileSizeValidator(2)).Build();

		var node = (IFileNode)form.Nodes.First(node => node.Name == "FileNode");
		node.Value = new FileValue { FileContents = null, FileName = "Empty" };

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(Resources.TheFileInput_HasAMaximumFileSizeOf_.Format("FileNode", "2B"))
		);
	}

	[TestMethod]
	public void MaxFileSizeValidator_ContentsTooBig_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test").WithFileNode("FileNode", node => node.UseMaxFileSizeValidator(2)).Build();

		var node = (IFileNode)form.Nodes.First(node => node.Name == "FileNode");
		node.Value = new FileValue { FileContents = [1, 2, 3], FileName = "Too big" };

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(
			node.ValidationErrors.Contains(Resources.TheFileInput_HasAMaximumFileSizeOf_.Format("FileNode", "2B"))
		);
	}

	[TestMethod]
	public void MaxFileSizeValidator_ContentsValidSize_ShouldBeValid()
	{
		var form = new FormBuilder("Test").WithFileNode("FileNode", node => node.UseMaxFileSizeValidator(2)).Build();

		var node = (IFileNode)form.Nodes.First(node => node.Name == "FileNode");
		node.Value = new FileValue { FileContents = [1, 2], FileName = "Empty" };

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(Resources.TheFileInput_HasAMaximumFileSizeOf_.Format("FileNode", "2B"))
		);
	}

	[TestMethod]
	public void AllowedFileNameSymbolValidator_NonFileField_ShouldThrowInvalidOperationException()
	{
		var form = new FormBuilder("Test").UseValidator(new AllowedFileNameSymbolValidator("abc")).Build();

		Assert.ThrowsException<InvalidOperationException>(form.Update);
	}

	[TestMethod]
	public void AllowedFileNameSymbolValidator_NameNull_ShouldNotValidate()
	{
		var form = new FormBuilder("Test")
			.WithFileNode("FileNode", node => node.UseAllowedFileNameSymbolValidator("abc"))
			.Build();

		var node = (IFileNode)form.Nodes.First(node => node.Name == "FileNode");
		node.Value = new FileValue { FileContents = [1, 2, 3], FileName = null };

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(Resources.TheFollowingCharactersAreNotAllowedInAFileName_.Format("d"))
		);
	}

	[TestMethod]
	public void AllowedFileNameSymbolValidator_NameContainsInvalidChars_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test")
			.WithFileNode("FileNode", node => node.UseAllowedFileNameSymbolValidator("abc"))
			.Build();

		var node = (IFileNode)form.Nodes.First(node => node.Name == "FileNode");
		node.Value = new FileValue { FileContents = null, FileName = "abcdabc" };

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(
			node.ValidationErrors.Contains(Resources.TheFollowingCharactersAreNotAllowedInAFileName_.Format("d"))
		);
	}

	[TestMethod]
	public void AllowedFileNameSymbolValidator_ValidName_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithFileNode("FileNode", node => node.UseAllowedFileNameSymbolValidator("abc"))
			.Build();

		var node = (IFileNode)form.Nodes.First(node => node.Name == "FileNode");
		node.Value = new FileValue { FileContents = null, FileName = "aabbcc" };

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(Resources.TheFollowingCharactersAreNotAllowedInAFileName_.Format("d"))
		);
	}
}
