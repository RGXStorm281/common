namespace RobinEpple.Common.Forms.Test.Tests;

using RobinEpple.Common.Forms;
using RobinEpple.Common.Forms.Building;
using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.SelectLists;
using RobinEpple.Common.Forms.Test.Mocks;
using RobinEpple.Common.Forms.Validation;
using RobinEpple.Common.Util;
using static RobinEpple.Common.Forms.Expressions.FormExpression;
using static RobinEpple.Common.Forms.Expressions.FormExpressionExtensions;

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

	[TestMethod]
	public void FileExtensionValidator_NonFileField_ShouldThrowInvalidOperationException()
	{
		var form = new FormBuilder("Test").UseValidator(new FileExtensionValidator(["jpg"])).Build();

		Assert.ThrowsException<InvalidOperationException>(form.Update);
	}

	[TestMethod]
	public void FileExtensionValidator_ContentsNull_ShouldNotValidate()
	{
		var form = new FormBuilder("Test")
			.WithFileNode("FileNode", node => node.UseFileExtensionValidator(["jpg"]))
			.Build();

		var node = (IFileNode)form.Nodes.First(node => node.Name == "FileNode");
		node.Value = new FileValue { FileContents = null, FileName = "Empty" };

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(
				Resources.TheFileInput_OnlyAllowsFilesOfTheFollowingTypes_.Format("FileNode", ".jpg")
			)
		);
	}

	[TestMethod]
	public void FileExtensionValidator_InvalidFileExtension_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test")
			.WithFileNode("FileNode", node => node.UseFileExtensionValidator(["png"]))
			.Build();

		var node = (IFileNode)form.Nodes.First(node => node.Name == "FileNode");
		node.Value = new FileValue
		{
			FileContents = File.ReadAllBytes("/workspaces/common/src/Common.Forms.Test/TestImage.jpg"),
			FileName = "Text file",
		};

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(
			node.ValidationErrors.Contains(Resources.TheFileInput_HasAMaximumFileSizeOf_.Format("FileNode", ".png"))
		);
	}

	[TestMethod]
	public void FileExtensionValidator_ValidFileExtension_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithFileNode("FileNode", node => node.UseFileExtensionValidator(["jpg"]))
			.Build();

		var node = (IFileNode)form.Nodes.First(node => node.Name == "FileNode");
		node.Value = new FileValue
		{
			FileContents = File.ReadAllBytes("/workspaces/common/src/Common.Forms.Test/TestImage.jpg"),
			FileName = "Text file",
		};

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(Resources.TheFileInput_HasAMaximumFileSizeOf_.Format("FileNode", ".jpg"))
		);
	}

	[TestMethod]
	public void NumberSelectListValidator_NonNumberField_ShouldThrowInvalidOperationException()
	{
		var form = new FormBuilder("Test")
			.UseValidator(new NumberSelectListValidator(ISelectListSource<decimal>.ForValues([1, 2, 3])))
			.Build();

		Assert.ThrowsException<InvalidOperationException>(form.Update);
	}

	[TestMethod]
	public void NumberSelectListValidator_ContentsNull_ShouldNotValidate()
	{
		var form = new FormBuilder("Test")
			.WithNumberNode(
				"NumberNode",
				node => node.UseSelectListValidator(ISelectListSource<decimal>.ForValues([1, 2, 3]))
			)
			.Build();

		var node = (INumberNode)form.Nodes.First(node => node.Name == "NumberNode");
		node.Value = null;

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(
				Resources.TheValue_InTheField_IsNotAllowedPleaseSelectOneOfTheProvidedOptions.Format(
					string.Empty,
					"NumberNode"
				)
			)
		);
	}

	[TestMethod]
	public void NumberSelectListValidator_SelectionOutOfRange_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test")
			.WithNumberNode(
				"NumberNode",
				node => node.UseSelectListValidator(ISelectListSource<decimal>.ForValues([1, 2, 3]))
			)
			.Build();

		var node = (INumberNode)form.Nodes.First(node => node.Name == "NumberNode");
		node.Value = 4;

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(
			node.ValidationErrors.Contains(
				Resources.TheValue_InTheField_IsNotAllowedPleaseSelectOneOfTheProvidedOptions.Format(4, "NumberNode")
			)
		);
	}

	[TestMethod]
	public void NumberSelectListValidator_SelectionInList_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithNumberNode(
				"NumberNode",
				node => node.UseSelectListValidator(ISelectListSource<decimal>.ForValues([1, 2, 3]))
			)
			.Build();

		var node = (INumberNode)form.Nodes.First(node => node.Name == "NumberNode");
		node.Value = 3;

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(
				Resources.TheValue_InTheField_IsNotAllowedPleaseSelectOneOfTheProvidedOptions.Format(3, "NumberNode")
			)
		);
	}

	[TestMethod]
	public void NumberSelectListValidator_ParentDependency_ShouldBeEvaluated()
	{
		var dependentListSource = new DependentSelectListMock<bool, decimal>(
			new Dictionary<bool, ISelectListSource<decimal>>()
			{
				{ false, ISelectListSource<decimal>.ForValues([1, 2]) },
				{ true, ISelectListSource<decimal>.ForValues([2, 3]) },
			}
		);

		var form = new FormBuilder("Test")
			.WithBooleanNode("BooleanNode")
			.WithNumberNode(
				"NumberNode",
				node =>
					node.UseSelectListValidator(
						dependentListSource,
						new Dictionary<string, IFormExpression<object?>>()
						{
							{
								DependentSelectListMock<bool, decimal>.ParentValueKey,
								BooleanFieldValue("BooleanNode").Select(value => (object?)value)
							},
						}
					)
			)
			.Build();

		var booleanNode = (IBooleanNode)form.Nodes.First(node => node.Name == "BooleanNode");
		var numberNode = (INumberNode)form.Nodes.First(node => node.Name == "NumberNode");
		numberNode.Value = 3;

		// For boolean node false should be invalid.
		booleanNode.Value = false;

		form.Update();
		Assert.IsFalse(numberNode.IsValid);
		Assert.IsTrue(
			numberNode.ValidationErrors.Contains(
				Resources.TheValue_InTheField_IsNotAllowedPleaseSelectOneOfTheProvidedOptions.Format(3, "NumberNode")
			)
		);

		// For boolean node true should be valid.
		booleanNode.Value = true;

		form.Update();
		Assert.IsTrue(numberNode.IsValid);
		Assert.IsFalse(
			numberNode.ValidationErrors.Contains(
				Resources.TheValue_InTheField_IsNotAllowedPleaseSelectOneOfTheProvidedOptions.Format(3, "NumberNode")
			)
		);
	}

	[TestMethod]
	public void TextSelectListValidator_NonTextField_ShouldThrowInvalidOperationException()
	{
		var form = new FormBuilder("Test")
			.UseValidator(new TextSelectListValidator(ISelectListSource<string>.ForValues(["1", "2", "3"])))
			.Build();

		Assert.ThrowsException<InvalidOperationException>(form.Update);
	}

	[TestMethod]
	public void TextSelectListValidator_ContentsNull_ShouldNotValidate()
	{
		var form = new FormBuilder("Test")
			.WithTextNode(
				"TextNode",
				node => node.UseSelectListValidator(ISelectListSource<string>.ForValues(["1", "2", "3"]))
			)
			.Build();

		var node = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		node.Value = null;

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(
				Resources.TheValue_InTheField_IsNotAllowedPleaseSelectOneOfTheProvidedOptions.Format(
					string.Empty,
					"TextNode"
				)
			)
		);
	}

	[TestMethod]
	public void TextSelectListValidator_SelectionOutOfRange_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test")
			.WithTextNode(
				"TextNode",
				node => node.UseSelectListValidator(ISelectListSource<string>.ForValues(["1", "2", "3"]))
			)
			.Build();

		var node = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		node.Value = "4";

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(
			node.ValidationErrors.Contains(
				Resources.TheValue_InTheField_IsNotAllowedPleaseSelectOneOfTheProvidedOptions.Format("4", "TextNode")
			)
		);
	}

	[TestMethod]
	public void TextSelectListValidator_SelectionInList_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithTextNode(
				"TextNode",
				node => node.UseSelectListValidator(ISelectListSource<string>.ForValues(["1", "2", "3"]))
			)
			.Build();

		var node = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		node.Value = "3";

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(
				Resources.TheValue_InTheField_IsNotAllowedPleaseSelectOneOfTheProvidedOptions.Format("3", "TextNode")
			)
		);
	}

	[TestMethod]
	public void TextSelectListValidator_ParentDependency_ShouldBeEvaluated()
	{
		var dependentListSource = new DependentSelectListMock<bool, string>(
			new Dictionary<bool, ISelectListSource<string>>()
			{
				{ false, ISelectListSource<string>.ForValues(["1", "2"]) },
				{ true, ISelectListSource<string>.ForValues(["2", "3"]) },
			}
		);

		var form = new FormBuilder("Test")
			.WithBooleanNode("BooleanNode")
			.WithTextNode(
				"TextNode",
				node =>
					node.UseSelectListValidator(
						dependentListSource,
						new Dictionary<string, IFormExpression<object?>>()
						{
							{
								DependentSelectListMock<bool, decimal>.ParentValueKey,
								BooleanFieldValue("BooleanNode").Select(value => (object?)value)
							},
						}
					)
			)
			.Build();

		var booleanNode = (IBooleanNode)form.Nodes.First(node => node.Name == "BooleanNode");
		var numberNode = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		numberNode.Value = "3";

		// For boolean node false should be invalid.
		booleanNode.Value = false;

		form.Update();
		Assert.IsFalse(numberNode.IsValid);
		Assert.IsTrue(
			numberNode.ValidationErrors.Contains(
				Resources.TheValue_InTheField_IsNotAllowedPleaseSelectOneOfTheProvidedOptions.Format("3", "TextNode")
			)
		);

		// For boolean node true should be valid.
		booleanNode.Value = true;

		form.Update();
		Assert.IsTrue(numberNode.IsValid);
		Assert.IsFalse(
			numberNode.ValidationErrors.Contains(
				Resources.TheValue_InTheField_IsNotAllowedPleaseSelectOneOfTheProvidedOptions.Format("3", "TextNode")
			)
		);
	}

	[TestMethod]
	public void TimestampSelectListValidator_NonTimestampField_ShouldThrowInvalidOperationException()
	{
		var form = new FormBuilder("Test")
			.UseValidator(
				new TimestampSelectListValidator(
					ISelectListSource<DateTime>.ForValues(
						[DateTime.Today.AddDays(-1), DateTime.Today, DateTime.Today.AddDays(1)]
					)
				)
			)
			.Build();

		Assert.ThrowsException<InvalidOperationException>(form.Update);
	}

	[TestMethod]
	public void TimestampSelectListValidator_ContentsNull_ShouldNotValidate()
	{
		var form = new FormBuilder("Test")
			.WithTimestampNode(
				"TimestampNode",
				node =>
					node.UseSelectListValidator(
						ISelectListSource<DateTime>.ForValues(
							[DateTime.Today.AddDays(-1), DateTime.Today, DateTime.Today.AddDays(1)]
						)
					)
			)
			.Build();

		var node = (ITimestampNode)form.Nodes.First(node => node.Name == "TimestampNode");
		node.Value = null;

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(
				Resources.TheValue_InTheField_IsNotAllowedPleaseSelectOneOfTheProvidedOptions.Format(
					string.Empty,
					"TimestampNode"
				)
			)
		);
	}

	[TestMethod]
	public void TimestampSelectListValidator_SelectionOutOfRange_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test")
			.WithTimestampNode(
				"TimestampNode",
				node =>
					node.UseSelectListValidator(
						ISelectListSource<DateTime>.ForValues(
							[DateTime.Today.AddDays(-1), DateTime.Today, DateTime.Today.AddDays(1)]
						)
					)
			)
			.Build();

		var node = (ITimestampNode)form.Nodes.First(node => node.Name == "TimestampNode");
		node.Value = DateTime.Today.AddDays(2);

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(
			node.ValidationErrors.Contains(
				Resources.TheValue_InTheField_IsNotAllowedPleaseSelectOneOfTheProvidedOptions.Format(
					DateTime.Today.AddDays(2),
					"TimestampNode"
				)
			)
		);
	}

	[TestMethod]
	public void TimestampSelectListValidator_SelectionInList_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithTimestampNode(
				"TimestampNode",
				node =>
					node.UseSelectListValidator(
						ISelectListSource<DateTime>.ForValues(
							[DateTime.Today.AddDays(-1), DateTime.Today, DateTime.Today.AddDays(1)]
						)
					)
			)
			.Build();

		var node = (ITimestampNode)form.Nodes.First(node => node.Name == "TimestampNode");
		node.Value = DateTime.Today.AddDays(1);

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(
				Resources.TheValue_InTheField_IsNotAllowedPleaseSelectOneOfTheProvidedOptions.Format(
					DateTime.Today.AddDays(1),
					"TimestampNode"
				)
			)
		);
	}

	[TestMethod]
	public void TimestampSelectListValidator_ParentDependency_ShouldBeEvaluated()
	{
		var dependentListSource = new DependentSelectListMock<bool, DateTime>(
			new Dictionary<bool, ISelectListSource<DateTime>>()
			{
				{ false, ISelectListSource<DateTime>.ForValues([DateTime.Today.AddDays(-1), DateTime.Today]) },
				{ true, ISelectListSource<DateTime>.ForValues([DateTime.Today, DateTime.Today.AddDays(1)]) },
			}
		);

		var form = new FormBuilder("Test")
			.WithBooleanNode("BooleanNode")
			.WithTimestampNode(
				"TimestampNode",
				node =>
					node.UseSelectListValidator(
						dependentListSource,
						new Dictionary<string, IFormExpression<object?>>()
						{
							{
								DependentSelectListMock<bool, decimal>.ParentValueKey,
								BooleanFieldValue("BooleanNode").Select(value => (object?)value)
							},
						}
					)
			)
			.Build();

		var booleanNode = (IBooleanNode)form.Nodes.First(node => node.Name == "BooleanNode");
		var numberNode = (ITimestampNode)form.Nodes.First(node => node.Name == "TimestampNode");
		numberNode.Value = DateTime.Today.AddDays(1);

		// For boolean node false should be invalid.
		booleanNode.Value = false;

		form.Update();
		Assert.IsFalse(numberNode.IsValid);
		Assert.IsTrue(
			numberNode.ValidationErrors.Contains(
				Resources.TheValue_InTheField_IsNotAllowedPleaseSelectOneOfTheProvidedOptions.Format(
					DateTime.Today.AddDays(1),
					"TimestampNode"
				)
			)
		);

		// For boolean node true should be valid.
		booleanNode.Value = true;

		form.Update();
		Assert.IsTrue(numberNode.IsValid);
		Assert.IsFalse(
			numberNode.ValidationErrors.Contains(
				Resources.TheValue_InTheField_IsNotAllowedPleaseSelectOneOfTheProvidedOptions.Format(
					DateTime.Today.AddDays(1),
					"TimestampNode"
				)
			)
		);
	}

	[TestMethod]
	public void NumberMinValueValidator_NonNumberField_ShouldThrowInvalidOperationException()
	{
		var form = new FormBuilder("Test").UseValidator(new NumberMinValueValidator(StaticValue<decimal?>(5))).Build();

		Assert.ThrowsException<InvalidOperationException>(form.Update);
	}

	[TestMethod]
	public void NumberMinValueValidator_ContentsNull_ShouldNotValidate()
	{
		var form = new FormBuilder("Test").WithNumberNode("NumberNode", node => node.UseMinValueValidator(5)).Build();

		var node = (INumberNode)form.Nodes.First(node => node.Name == "NumberNode");
		node.Value = null;

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(Resources.TheField_RequiresAMinimumValueOf_.Format("NumberNode", 5))
		);
	}

	[TestMethod]
	public void NumberMinValueValidator_ValueTooSmall_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test").WithNumberNode("NumberNode", node => node.UseMinValueValidator(5)).Build();

		var node = (INumberNode)form.Nodes.First(node => node.Name == "NumberNode");
		node.Value = 3;

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(
			node.ValidationErrors.Contains(Resources.TheField_RequiresAMinimumValueOf_.Format("NumberNode", 5))
		);
	}

	[TestMethod]
	public void NumberMinValueValidator_ValueEqualsLowerBound_ShouldBeValid()
	{
		var form = new FormBuilder("Test").WithNumberNode("NumberNode", node => node.UseMinValueValidator(5)).Build();

		var node = (INumberNode)form.Nodes.First(node => node.Name == "NumberNode");
		node.Value = 5;

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(Resources.TheField_RequiresAMinimumValueOf_.Format("NumberNode", 5))
		);
	}

	[TestMethod]
	public void NumberMinValueValidator_ExpressionValueNull_ShouldNotValidate()
	{
		var form = new FormBuilder("Test")
			.WithNumberNode("LowerBound")
			.WithNumberNode("NumberNode", node => node.UseMinValueValidator(NumberFieldValue("LowerBound")))
			.Build();

		var lowerBoundNode = (INumberNode)form.Nodes.First(node => node.Name == "LowerBound");
		lowerBoundNode.Value = null;
		var dependentNode = (INumberNode)form.Nodes.First(node => node.Name == "NumberNode");
		dependentNode.Value = 3;

		form.Update();
		Assert.IsTrue(dependentNode.IsValid);
		Assert.IsFalse(
			dependentNode.ValidationErrors.Contains(
				Resources.TheField_RequiresAMinimumValueOf_.Format("NumberNode", string.Empty)
			)
		);
	}

	[TestMethod]
	public void NumberMinValueValidator_ExpressionValue_ShouldUpdateLowerBound()
	{
		var form = new FormBuilder("Test")
			.WithNumberNode("LowerBound")
			.WithNumberNode("NumberNode", node => node.UseMinValueValidator(NumberFieldValue("LowerBound")))
			.Build();

		var lowerBoundNode = (INumberNode)form.Nodes.First(node => node.Name == "LowerBound");
		var dependentNode = (INumberNode)form.Nodes.First(node => node.Name == "NumberNode");
		dependentNode.Value = 3;

		// Lower bound too big renders node invalid.
		lowerBoundNode.Value = 5;

		form.Update();
		Assert.IsFalse(dependentNode.IsValid);
		Assert.IsTrue(
			dependentNode.ValidationErrors.Contains(Resources.TheField_RequiresAMinimumValueOf_.Format("NumberNode", 5))
		);

		// Decreasing the lower bound makes dependent node valid.
		lowerBoundNode.Value = 0;

		form.Update();
		Assert.IsTrue(dependentNode.IsValid);
		Assert.IsFalse(
			dependentNode.ValidationErrors.Contains(Resources.TheField_RequiresAMinimumValueOf_.Format("NumberNode", 0))
		);
	}

	[TestMethod]
	public void NumberMaxValueValidator_NonNumberField_ShouldThrowInvalidOperationException()
	{
		var form = new FormBuilder("Test").UseValidator(new NumberMaxValueValidator(StaticValue<decimal?>(5))).Build();

		Assert.ThrowsException<InvalidOperationException>(form.Update);
	}

	[TestMethod]
	public void NumberMaxValueValidator_ContentsNull_ShouldNotValidate()
	{
		var form = new FormBuilder("Test").WithNumberNode("NumberNode", node => node.UseMaxValueValidator(5)).Build();

		var node = (INumberNode)form.Nodes.First(node => node.Name == "NumberNode");
		node.Value = null;

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(Resources.TheField_OnlyAllowsAMaximumValueOf_.Format("NumberNode", 5))
		);
	}

	[TestMethod]
	public void NumberMaxValueValidator_ValueTooBig_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test").WithNumberNode("NumberNode", node => node.UseMaxValueValidator(5)).Build();

		var node = (INumberNode)form.Nodes.First(node => node.Name == "NumberNode");
		node.Value = 6;

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(
			node.ValidationErrors.Contains(Resources.TheField_OnlyAllowsAMaximumValueOf_.Format("NumberNode", 5))
		);
	}

	[TestMethod]
	public void NumberMaxValueValidator_ValueEqualsUpperBound_ShouldBeValid()
	{
		var form = new FormBuilder("Test").WithNumberNode("NumberNode", node => node.UseMaxValueValidator(5)).Build();

		var node = (INumberNode)form.Nodes.First(node => node.Name == "NumberNode");
		node.Value = 5;

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(Resources.TheField_OnlyAllowsAMaximumValueOf_.Format("NumberNode", 5))
		);
	}

	[TestMethod]
	public void NumberMaxValueValidator_ExpressionValueNull_ShouldNotValidate()
	{
		var form = new FormBuilder("Test")
			.WithNumberNode("UpperBound")
			.WithNumberNode("NumberNode", node => node.UseMaxValueValidator(NumberFieldValue("UpperBound")))
			.Build();

		var upperBoundNode = (INumberNode)form.Nodes.First(node => node.Name == "UpperBound");
		upperBoundNode.Value = null;
		var dependentNode = (INumberNode)form.Nodes.First(node => node.Name == "NumberNode");
		dependentNode.Value = 3;

		form.Update();
		Assert.IsTrue(dependentNode.IsValid);
		Assert.IsFalse(
			dependentNode.ValidationErrors.Contains(
				Resources.TheField_OnlyAllowsAMaximumValueOf_.Format("NumberNode", string.Empty)
			)
		);
	}

	[TestMethod]
	public void NumberMaxValueValidator_ExpressionValue_ShouldUpdateUpperBound()
	{
		var form = new FormBuilder("Test")
			.WithNumberNode("UpperBound")
			.WithNumberNode("NumberNode", node => node.UseMaxValueValidator(NumberFieldValue("UpperBound")))
			.Build();

		var upperBoundNode = (INumberNode)form.Nodes.First(node => node.Name == "UpperBound");
		var dependentNode = (INumberNode)form.Nodes.First(node => node.Name == "NumberNode");
		dependentNode.Value = 3;

		// Upper bound too small renders node invalid.
		upperBoundNode.Value = 2;

		form.Update();
		Assert.IsFalse(dependentNode.IsValid);
		Assert.IsTrue(
			dependentNode.ValidationErrors.Contains(
				Resources.TheField_OnlyAllowsAMaximumValueOf_.Format("NumberNode", 2)
			)
		);

		// Increasing the upper bound makes dependent node valid.
		upperBoundNode.Value = 6;

		form.Update();
		Assert.IsTrue(dependentNode.IsValid);
		Assert.IsFalse(
			dependentNode.ValidationErrors.Contains(
				Resources.TheField_OnlyAllowsAMaximumValueOf_.Format("NumberNode", 6)
			)
		);
	}
}
