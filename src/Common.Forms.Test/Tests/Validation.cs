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
			.WithBooleanNode("BooleanNode", node => node.UseLabel("BooleanLabel").UseRequireTrueValidator())
			.Build();

		var node = (IBooleanNode)form.Nodes.First(node => node.Name == "BooleanNode");
		node.Value = null;

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(node.ValidationErrors.Contains(Resources.TheField_RequiresTheValueTrue.Format("BooleanLabel")));
	}

	[TestMethod]
	public void RequireTrueValidator_ValueFalse_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test")
			.WithBooleanNode("BooleanNode", node => node.UseLabel("BooleanLabel").UseRequireTrueValidator())
			.Build();

		var node = (IBooleanNode)form.Nodes.First(node => node.Name == "BooleanNode");
		node.Value = false;

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(node.ValidationErrors.Contains(Resources.TheField_RequiresTheValueTrue.Format("BooleanLabel")));
	}

	[TestMethod]
	public void RequireTrueValidator_ValueTrue_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithBooleanNode("BooleanNode", node => node.UseLabel("BooleanLabel").UseRequireTrueValidator())
			.Build();

		var node = (IBooleanNode)form.Nodes.First(node => node.Name == "BooleanNode");
		node.Value = true;

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(node.ValidationErrors.Contains(Resources.TheField_RequiresTheValueTrue.Format("BooleanLabel")));
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
		var form = new FormBuilder("Test")
			.WithFileNode("FileNode", node => node.UseLabel("FileLabel").UseRequiredValidator())
			.Build();

		var node = (IFileNode)form.Nodes.First(node => node.Name == "FileNode");
		node.Value = new FileValue { FileContents = null, FileName = "Any" };

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(node.ValidationErrors.Contains(Resources.TheField_RequiresAnInput.Format("FileLabel")));
	}

	[TestMethod]
	public void FileRequiredValidator_NameNull_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test")
			.WithFileNode("FileNode", node => node.UseLabel("FileLabel").UseRequiredValidator())
			.Build();

		var node = (IFileNode)form.Nodes.First(node => node.Name == "FileNode");
		node.Value = new FileValue { FileContents = [1, 2, 3], FileName = null };

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(node.ValidationErrors.Contains(Resources.TheField_RequiresAnInput.Format("FileLabel")));
	}

	[TestMethod]
	public void FileRequiredValidator_HasValue_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithFileNode("FileNode", node => node.UseLabel("FileLabel").UseRequiredValidator())
			.Build();

		var node = (IFileNode)form.Nodes.First(node => node.Name == "FileNode");
		node.Value = new FileValue { FileContents = [1, 2, 3], FileName = "Any" };

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(node.ValidationErrors.Contains(Resources.TheField_RequiresAnInput.Format("FileLabel")));
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
		var form = new FormBuilder("Test")
			.WithNumberNode("NumberNode", node => node.UseLabel("NumberLabel").UseRequiredValidator())
			.Build();

		var node = (INumberNode)form.Nodes.First(node => node.Name == "NumberNode");
		node.Value = null;

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(node.ValidationErrors.Contains(Resources.TheField_RequiresAnInput.Format("NumberLabel")));
	}

	[TestMethod]
	public void NumberRequiredValidator_HasValue_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithNumberNode("NumberNode", node => node.UseLabel("NumberLabel").UseRequiredValidator())
			.Build();

		var node = (INumberNode)form.Nodes.First(node => node.Name == "NumberNode");
		node.Value = 42;

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(node.ValidationErrors.Contains(Resources.TheField_RequiresAnInput.Format("NumberLabel")));
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
		var form = new FormBuilder("Test")
			.WithTextNode("TextNode", node => node.UseLabel("TextLabel").UseRequiredValidator())
			.Build();

		var node = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		node.Value = null;

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(node.ValidationErrors.Contains(Resources.TheField_RequiresAnInput.Format("TextLabel")));
	}

	[TestMethod]
	public void TextRequiredValidator_ValueEmpty_ShouldBeInvalidExceptIfConfigured()
	{
		var form = new FormBuilder("Test")
			.WithTextNode("TextNode", node => node.UseLabel("TextLabel").UseRequiredValidator())
			.Build();
		var node = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");

		// Should not accept by default.
		node.Value = string.Empty;
		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(node.ValidationErrors.Contains(Resources.TheField_RequiresAnInput.Format("TextLabel")));

		node.Value = "  ";
		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(node.ValidationErrors.Contains(Resources.TheField_RequiresAnInput.Format("TextLabel")));

		form = new FormBuilder("Test")
			.WithTextNode("TextNode", node => node.UseLabel("TextLabel").UseRequiredValidator(acceptWhitespace: true))
			.Build();
		node = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");

		// But can be configured to accept empty values.
		node.Value = string.Empty;
		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(node.ValidationErrors.Contains(Resources.TheField_RequiresAnInput.Format("TextLabel")));

		node.Value = "  ";
		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(node.ValidationErrors.Contains(Resources.TheField_RequiresAnInput.Format("TextLabel")));
	}

	[TestMethod]
	public void TextRequiredValidator_HasValue_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithTextNode("TextNode", node => node.UseLabel("TextLabel").UseRequiredValidator())
			.Build();

		var node = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		node.Value = "Test text";

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(node.ValidationErrors.Contains(Resources.TheField_RequiresAnInput.Format("TextLabel")));
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
			.WithTimestampNode("TimestampNode", node => node.UseLabel("TimestampLabel").UseRequiredValidator())
			.Build();

		var node = (ITimestampNode)form.Nodes.First(node => node.Name == "TimestampNode");
		node.Value = null;

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(node.ValidationErrors.Contains(Resources.TheField_RequiresAnInput.Format("TimestampLabel")));
	}

	[TestMethod]
	public void TimestampRequiredValidator_HasValue_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithTimestampNode("TimestampNode", node => node.UseLabel("TimestampLabel").UseRequiredValidator())
			.Build();

		var node = (ITimestampNode)form.Nodes.First(node => node.Name == "TimestampNode");
		node.Value = DateTime.Now;

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(node.ValidationErrors.Contains(Resources.TheField_RequiresAnInput.Format("TimestampLabel")));
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
			.WithTemplatedSection("TemplateNode", (node, _) => node.UseLabel("TemplateLabel").UseRequiredValidator())
			.Build();

		var node = (ITemplateNode)form.Nodes.First(node => node.Name == "TemplateNode");

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(node.ValidationErrors.Contains(Resources.TheField_RequiresAnInput.Format("TemplateLabel")));
	}

	[TestMethod]
	public void TemplateRequiredValidator_HasInstance_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithTemplatedSection(
				"TemplateNode",
				(node, _) => node.UseLabel("TemplateLabel").UseTemplate("Template").UseRequiredValidator()
			)
			.Build();

		var node = (ITemplateNode)form.Nodes.First(node => node.Name == "TemplateNode");
		node.Instantiate(node.Templates.First());

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(node.ValidationErrors.Contains(Resources.TheField_RequiresAnInput.Format("TemplateLabel")));
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
		var form = new FormBuilder("Test")
			.WithFileNode("FileNode", node => node.UseLabel("FileLabel").UseMaxFileSizeValidator(2))
			.Build();

		var node = (IFileNode)form.Nodes.First(node => node.Name == "FileNode");
		node.Value = new FileValue { FileContents = null, FileName = "Empty" };

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(Resources.TheFileInput_HasAMaximumFileSizeOf_.Format("FileLabel", "2 B"))
		);
	}

	[TestMethod]
	public void MaxFileSizeValidator_ContentsTooBig_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test")
			.WithFileNode("FileNode", node => node.UseLabel("FileLabel").UseMaxFileSizeValidator(2))
			.Build();

		var node = (IFileNode)form.Nodes.First(node => node.Name == "FileNode");
		node.Value = new FileValue { FileContents = [1, 2, 3], FileName = "Too big" };

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(
			node.ValidationErrors.Contains(Resources.TheFileInput_HasAMaximumFileSizeOf_.Format("FileLabel", "2 B"))
		);
	}

	[TestMethod]
	public void MaxFileSizeValidator_ContentsValidSize_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithFileNode("FileNode", node => node.UseLabel("FileLabel").UseMaxFileSizeValidator(2))
			.Build();

		var node = (IFileNode)form.Nodes.First(node => node.Name == "FileNode");
		node.Value = new FileValue { FileContents = [1, 2], FileName = "Empty" };

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(Resources.TheFileInput_HasAMaximumFileSizeOf_.Format("FileLabel", "2 B"))
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
			.WithFileNode("FileNode", node => node.UseLabel("FileLabel").UseAllowedFileNameSymbolValidator("abc"))
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
			.WithFileNode("FileNode", node => node.UseLabel("FileLabel").UseAllowedFileNameSymbolValidator("abc"))
			.Build();

		var node = (IFileNode)form.Nodes.First(node => node.Name == "FileNode");
		node.Value = new FileValue { FileContents = null, FileName = "abcdabcd" };

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(
			node.ValidationErrors.Contains(Resources.TheFollowingCharactersAreNotAllowedInAFileName_.Format("d"))
		);
	}

	[TestMethod]
	public void AllowedFileNameSymbolValidator_OnlyListedCharacters_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithFileNode("FileNode", node => node.UseLabel("FileLabel").UseAllowedFileNameSymbolValidator("abc[]."))
			.Build();

		var node = (IFileNode)form.Nodes.First(node => node.Name == "FileNode");
		node.Value = new FileValue { FileContents = null, FileName = "aabbcc[]." };

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
			.WithFileNode("FileNode", node => node.UseLabel("FileLabel").UseFileExtensionValidator(["jpg"]))
			.Build();

		var node = (IFileNode)form.Nodes.First(node => node.Name == "FileNode");
		node.Value = new FileValue { FileContents = null, FileName = "Empty" };

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(
				Resources.TheFileInput_OnlyAllowsFilesOfTheFollowingTypes_.Format("FileLabel", ".jpg")
			)
		);
	}

	[TestMethod]
	public void FileExtensionValidator_InvalidFileExtension_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test")
			.WithFileNode("FileNode", node => node.UseLabel("FileLabel").UseFileExtensionValidator(["png"]))
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
			node.ValidationErrors.Contains(Resources.TheFileInput_HasAMaximumFileSizeOf_.Format("FileLabel", ".png"))
		);
	}

	[TestMethod]
	public void FileExtensionValidator_ValidFileExtension_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithFileNode("FileNode", node => node.UseLabel("FileLabel").UseFileExtensionValidator(["jpg"]))
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
			node.ValidationErrors.Contains(Resources.TheFileInput_HasAMaximumFileSizeOf_.Format("FileLabel", ".jpg"))
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
				node =>
					node.UseLabel("NumberLabel").UseSelectListValidator(ISelectListSource<decimal>.ForValues([1, 2, 3]))
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
					"NumberLabel"
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
				node =>
					node.UseLabel("NumberLabel").UseSelectListValidator(ISelectListSource<decimal>.ForValues([1, 2, 3]))
			)
			.Build();

		var node = (INumberNode)form.Nodes.First(node => node.Name == "NumberNode");
		node.Value = 4;

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(
			node.ValidationErrors.Contains(
				Resources.TheValue_InTheField_IsNotAllowedPleaseSelectOneOfTheProvidedOptions.Format(4, "NumberLabel")
			)
		);
	}

	[TestMethod]
	public void NumberSelectListValidator_SelectionInList_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithNumberNode(
				"NumberNode",
				node =>
					node.UseLabel("NumberLabel").UseSelectListValidator(ISelectListSource<decimal>.ForValues([1, 2, 3]))
			)
			.Build();

		var node = (INumberNode)form.Nodes.First(node => node.Name == "NumberNode");
		node.Value = 3;

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(
				Resources.TheValue_InTheField_IsNotAllowedPleaseSelectOneOfTheProvidedOptions.Format(3, "NumberLabel")
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
					node.UseLabel("NumberLabel")
						.UseSelectListValidator(
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
				Resources.TheValue_InTheField_IsNotAllowedPleaseSelectOneOfTheProvidedOptions.Format(3, "NumberLabel")
			)
		);

		// For boolean node true should be valid.
		booleanNode.Value = true;

		form.Update();
		Assert.IsTrue(numberNode.IsValid);
		Assert.IsFalse(
			numberNode.ValidationErrors.Contains(
				Resources.TheValue_InTheField_IsNotAllowedPleaseSelectOneOfTheProvidedOptions.Format(3, "NumberLabel")
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
				node =>
					node.UseLabel("TextLabel")
						.UseSelectListValidator(ISelectListSource<string>.ForValues(["1", "2", "3"]))
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
					"TextLabel"
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
				node =>
					node.UseLabel("TextLabel")
						.UseSelectListValidator(ISelectListSource<string>.ForValues(["1", "2", "3"]))
			)
			.Build();

		var node = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		node.Value = "4";

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(
			node.ValidationErrors.Contains(
				Resources.TheValue_InTheField_IsNotAllowedPleaseSelectOneOfTheProvidedOptions.Format("4", "TextLabel")
			)
		);
	}

	[TestMethod]
	public void TextSelectListValidator_SelectionInList_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithTextNode(
				"TextNode",
				node =>
					node.UseLabel("TextLabel")
						.UseSelectListValidator(ISelectListSource<string>.ForValues(["1", "2", "3"]))
			)
			.Build();

		var node = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		node.Value = "3";

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(
				Resources.TheValue_InTheField_IsNotAllowedPleaseSelectOneOfTheProvidedOptions.Format("3", "TextLabel")
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
					node.UseLabel("TextLabel")
						.UseSelectListValidator(
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
				Resources.TheValue_InTheField_IsNotAllowedPleaseSelectOneOfTheProvidedOptions.Format("3", "TextLabel")
			)
		);

		// For boolean node true should be valid.
		booleanNode.Value = true;

		form.Update();
		Assert.IsTrue(numberNode.IsValid);
		Assert.IsFalse(
			numberNode.ValidationErrors.Contains(
				Resources.TheValue_InTheField_IsNotAllowedPleaseSelectOneOfTheProvidedOptions.Format("3", "TextLabel")
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
					node.UseLabel("TimestampLabel")
						.UseSelectListValidator(
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
					"TimestampLabel"
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
					node.UseLabel("TimestampLabel")
						.UseSelectListValidator(
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
					"TimestampLabel"
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
					node.UseLabel("TimestampLabel")
						.UseSelectListValidator(
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
					"TimestampLabel"
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
					node.UseLabel("TimestampLabel")
						.UseSelectListValidator(
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
					"TimestampLabel"
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
					"TimestampLabel"
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
		var form = new FormBuilder("Test")
			.WithNumberNode("NumberNode", node => node.UseLabel("NumberLabel").UseMinValueValidator(5))
			.Build();

		var node = (INumberNode)form.Nodes.First(node => node.Name == "NumberNode");
		node.Value = null;

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(Resources.TheField_RequiresAMinimumValueOf_.Format("NumberLabel", 5))
		);
	}

	[TestMethod]
	public void NumberMinValueValidator_ValueTooSmall_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test")
			.WithNumberNode("NumberNode", node => node.UseLabel("NumberLabel").UseMinValueValidator(5))
			.Build();

		var node = (INumberNode)form.Nodes.First(node => node.Name == "NumberNode");
		node.Value = 3;

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(
			node.ValidationErrors.Contains(Resources.TheField_RequiresAMinimumValueOf_.Format("NumberLabel", 5))
		);
	}

	[TestMethod]
	public void NumberMinValueValidator_ValueEqualsLowerBound_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithNumberNode("NumberNode", node => node.UseLabel("NumberLabel").UseMinValueValidator(5))
			.Build();

		var node = (INumberNode)form.Nodes.First(node => node.Name == "NumberNode");
		node.Value = 5;

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(Resources.TheField_RequiresAMinimumValueOf_.Format("NumberLabel", 5))
		);
	}

	[TestMethod]
	public void NumberMinValueValidator_ExpressionValueNull_ShouldNotValidate()
	{
		var form = new FormBuilder("Test")
			.WithNumberNode("LowerBound")
			.WithNumberNode(
				"NumberNode",
				node => node.UseLabel("NumberLabel").UseMinValueValidator(NumberFieldValue("LowerBound"))
			)
			.Build();

		var lowerBoundNode = (INumberNode)form.Nodes.First(node => node.Name == "LowerBound");
		lowerBoundNode.Value = null;
		var dependentNode = (INumberNode)form.Nodes.First(node => node.Name == "NumberNode");
		dependentNode.Value = 3;

		form.Update();
		Assert.IsTrue(dependentNode.IsValid);
		Assert.IsFalse(
			dependentNode.ValidationErrors.Contains(
				Resources.TheField_RequiresAMinimumValueOf_.Format("NumberLabel", string.Empty)
			)
		);
	}

	[TestMethod]
	public void NumberMinValueValidator_ExpressionValue_ShouldUpdateLowerBound()
	{
		var form = new FormBuilder("Test")
			.WithNumberNode("LowerBound")
			.WithNumberNode(
				"NumberNode",
				node => node.UseLabel("NumberLabel").UseMinValueValidator(NumberFieldValue("LowerBound"))
			)
			.Build();

		var lowerBoundNode = (INumberNode)form.Nodes.First(node => node.Name == "LowerBound");
		var dependentNode = (INumberNode)form.Nodes.First(node => node.Name == "NumberNode");
		dependentNode.Value = 3;

		// Lower bound too big renders node invalid.
		lowerBoundNode.Value = 5;

		form.Update();
		Assert.IsFalse(dependentNode.IsValid);
		Assert.IsTrue(
			dependentNode.ValidationErrors.Contains(
				Resources.TheField_RequiresAMinimumValueOf_.Format("NumberLabel", 5)
			)
		);

		// Decreasing the lower bound makes dependent node valid.
		lowerBoundNode.Value = 0;

		form.Update();
		Assert.IsTrue(dependentNode.IsValid);
		Assert.IsFalse(
			dependentNode.ValidationErrors.Contains(
				Resources.TheField_RequiresAMinimumValueOf_.Format("NumberLabel", 0)
			)
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
		var form = new FormBuilder("Test")
			.WithNumberNode("NumberNode", node => node.UseLabel("NumberLabel").UseMaxValueValidator(5))
			.Build();

		var node = (INumberNode)form.Nodes.First(node => node.Name == "NumberNode");
		node.Value = null;

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(Resources.TheField_AllowsAMaximumValueOf_.Format("NumberLabel", 5))
		);
	}

	[TestMethod]
	public void NumberMaxValueValidator_ValueTooBig_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test")
			.WithNumberNode("NumberNode", node => node.UseLabel("NumberLabel").UseMaxValueValidator(5))
			.Build();

		var node = (INumberNode)form.Nodes.First(node => node.Name == "NumberNode");
		node.Value = 6;

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(
			node.ValidationErrors.Contains(Resources.TheField_AllowsAMaximumValueOf_.Format("NumberLabel", 5))
		);
	}

	[TestMethod]
	public void NumberMaxValueValidator_ValueEqualsUpperBound_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithNumberNode("NumberNode", node => node.UseLabel("NumberLabel").UseMaxValueValidator(5))
			.Build();

		var node = (INumberNode)form.Nodes.First(node => node.Name == "NumberNode");
		node.Value = 5;

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(Resources.TheField_AllowsAMaximumValueOf_.Format("NumberLabel", 5))
		);
	}

	[TestMethod]
	public void NumberMaxValueValidator_ExpressionValueNull_ShouldNotValidate()
	{
		var form = new FormBuilder("Test")
			.WithNumberNode("UpperBound")
			.WithNumberNode(
				"NumberNode",
				node => node.UseLabel("NumberLabel").UseMaxValueValidator(NumberFieldValue("UpperBound"))
			)
			.Build();

		var upperBoundNode = (INumberNode)form.Nodes.First(node => node.Name == "UpperBound");
		upperBoundNode.Value = null;
		var dependentNode = (INumberNode)form.Nodes.First(node => node.Name == "NumberNode");
		dependentNode.Value = 3;

		form.Update();
		Assert.IsTrue(dependentNode.IsValid);
		Assert.IsFalse(
			dependentNode.ValidationErrors.Contains(
				Resources.TheField_AllowsAMaximumValueOf_.Format("NumberLabel", string.Empty)
			)
		);
	}

	[TestMethod]
	public void NumberMaxValueValidator_ExpressionValue_ShouldUpdateUpperBound()
	{
		var form = new FormBuilder("Test")
			.WithNumberNode("UpperBound")
			.WithNumberNode(
				"NumberNode",
				node => node.UseLabel("NumberLabel").UseMaxValueValidator(NumberFieldValue("UpperBound"))
			)
			.Build();

		var upperBoundNode = (INumberNode)form.Nodes.First(node => node.Name == "UpperBound");
		var dependentNode = (INumberNode)form.Nodes.First(node => node.Name == "NumberNode");
		dependentNode.Value = 3;

		// Upper bound too small renders node invalid.
		upperBoundNode.Value = 2;

		form.Update();
		Assert.IsFalse(dependentNode.IsValid);
		Assert.IsTrue(
			dependentNode.ValidationErrors.Contains(Resources.TheField_AllowsAMaximumValueOf_.Format("NumberLabel", 2))
		);

		// Increasing the upper bound makes dependent node valid.
		upperBoundNode.Value = 6;

		form.Update();
		Assert.IsTrue(dependentNode.IsValid);
		Assert.IsFalse(
			dependentNode.ValidationErrors.Contains(Resources.TheField_AllowsAMaximumValueOf_.Format("NumberLabel", 6))
		);
	}

	[TestMethod]
	public void TimestampMinValueValidator_NonTimestampField_ShouldThrowInvalidOperationException()
	{
		var form = new FormBuilder("Test")
			.UseValidator(new TimestampMinValueValidator(StaticValue<DateTime?>(DateTime.Today)))
			.Build();

		Assert.ThrowsException<InvalidOperationException>(form.Update);
	}

	[TestMethod]
	public void TimestampMinValueValidator_ContentsNull_ShouldNotValidate()
	{
		var form = new FormBuilder("Test")
			.WithTimestampNode(
				"TimestampNode",
				node => node.UseLabel("TimestampLabel").UseMinValueValidator(DateTime.Today)
			)
			.Build();

		var node = (ITimestampNode)form.Nodes.First(node => node.Name == "TimestampNode");
		node.Value = null;

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(
				Resources.TheField_RequiresAMinimumValueOf_.Format("TimestampLabel", DateTime.Today)
			)
		);
	}

	[TestMethod]
	public void TimestampMinValueValidator_ValueTooSmall_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test")
			.WithTimestampNode(
				"TimestampNode",
				node => node.UseLabel("TimestampLabel").UseMinValueValidator(DateTime.Today)
			)
			.Build();

		var node = (ITimestampNode)form.Nodes.First(node => node.Name == "TimestampNode");
		node.Value = DateTime.Today.AddDays(-1);

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(
			node.ValidationErrors.Contains(
				Resources.TheField_RequiresAMinimumValueOf_.Format("TimestampLabel", DateTime.Today)
			)
		);
	}

	[TestMethod]
	public void TimestampMinValueValidator_ValueEqualsLowerBound_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithTimestampNode(
				"TimestampNode",
				node => node.UseLabel("TimestampLabel").UseMinValueValidator(DateTime.Today)
			)
			.Build();

		var node = (ITimestampNode)form.Nodes.First(node => node.Name == "TimestampNode");
		node.Value = DateTime.Today;

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(
				Resources.TheField_RequiresAMinimumValueOf_.Format("TimestampLabel", DateTime.Today)
			)
		);
	}

	[TestMethod]
	public void TimestampMinValueValidator_ExpressionValueNull_ShouldNotValidate()
	{
		var form = new FormBuilder("Test")
			.WithTimestampNode("LowerBound")
			.WithTimestampNode(
				"TimestampNode",
				node => node.UseLabel("TimestampLabel").UseMinValueValidator(TimestampFieldValue("LowerBound"))
			)
			.Build();

		var lowerBoundNode = (ITimestampNode)form.Nodes.First(node => node.Name == "LowerBound");
		lowerBoundNode.Value = null;
		var dependentNode = (ITimestampNode)form.Nodes.First(node => node.Name == "TimestampNode");
		dependentNode.Value = DateTime.Today;

		form.Update();
		Assert.IsTrue(dependentNode.IsValid);
		Assert.IsFalse(
			dependentNode.ValidationErrors.Contains(
				Resources.TheField_RequiresAMinimumValueOf_.Format("TimestampLabel", string.Empty)
			)
		);
	}

	[TestMethod]
	public void TimestampMinValueValidator_ExpressionValue_ShouldUpdateLowerBound()
	{
		var form = new FormBuilder("Test")
			.WithTimestampNode("LowerBound")
			.WithTimestampNode(
				"TimestampNode",
				node => node.UseLabel("TimestampLabel").UseMinValueValidator(TimestampFieldValue("LowerBound"))
			)
			.Build();

		var lowerBoundNode = (ITimestampNode)form.Nodes.First(node => node.Name == "LowerBound");
		var dependentNode = (ITimestampNode)form.Nodes.First(node => node.Name == "TimestampNode");
		dependentNode.Value = DateTime.Today;

		// Lower bound too big renders node invalid.
		lowerBoundNode.Value = DateTime.Today.AddDays(1);

		form.Update();
		Assert.IsFalse(dependentNode.IsValid);
		Assert.IsTrue(
			dependentNode.ValidationErrors.Contains(
				Resources.TheField_RequiresAMinimumValueOf_.Format("TimestampLabel", DateTime.Today.AddDays(1))
			)
		);

		// Decreasing the lower bound makes dependent node valid.
		lowerBoundNode.Value = DateTime.Today.AddDays(-1);

		form.Update();
		Assert.IsTrue(dependentNode.IsValid);
		Assert.IsFalse(
			dependentNode.ValidationErrors.Contains(
				Resources.TheField_RequiresAMinimumValueOf_.Format("TimestampLabel", DateTime.Today.AddDays(-1))
			)
		);
	}

	[TestMethod]
	public void TimestampMaxValueValidator_NonTimestampField_ShouldThrowInvalidOperationException()
	{
		var form = new FormBuilder("Test")
			.UseValidator(new TimestampMaxValueValidator(StaticValue<DateTime?>(DateTime.Today)))
			.Build();

		Assert.ThrowsException<InvalidOperationException>(form.Update);
	}

	[TestMethod]
	public void TimestampMaxValueValidator_ContentsNull_ShouldNotValidate()
	{
		var form = new FormBuilder("Test")
			.WithTimestampNode(
				"TimestampNode",
				node => node.UseLabel("TimestampLabel").UseMaxValueValidator(DateTime.Today)
			)
			.Build();

		var node = (ITimestampNode)form.Nodes.First(node => node.Name == "TimestampNode");
		node.Value = null;

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(
				Resources.TheField_AllowsAMaximumValueOf_.Format("TimestampLabel", DateTime.Today)
			)
		);
	}

	[TestMethod]
	public void TimestampMaxValueValidator_ValueTooBig_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test")
			.WithTimestampNode(
				"TimestampNode",
				node => node.UseLabel("TimestampLabel").UseMaxValueValidator(DateTime.Today)
			)
			.Build();

		var node = (ITimestampNode)form.Nodes.First(node => node.Name == "TimestampNode");
		node.Value = DateTime.Today.AddDays(1);

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(
			node.ValidationErrors.Contains(
				Resources.TheField_AllowsAMaximumValueOf_.Format("TimestampLabel", DateTime.Today.AddDays(-1))
			)
		);
	}

	[TestMethod]
	public void TimestampMaxValueValidator_ValueEqualsUpperBound_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithTimestampNode(
				"TimestampNode",
				node => node.UseLabel("TimestampLabel").UseMaxValueValidator(DateTime.Today)
			)
			.Build();

		var node = (ITimestampNode)form.Nodes.First(node => node.Name == "TimestampNode");
		node.Value = DateTime.Today;

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(
				Resources.TheField_AllowsAMaximumValueOf_.Format("TimestampLabel", DateTime.Today)
			)
		);
	}

	[TestMethod]
	public void TimestampMaxValueValidator_ExpressionValueNull_ShouldNotValidate()
	{
		var form = new FormBuilder("Test")
			.WithTimestampNode("UpperBound")
			.WithTimestampNode(
				"TimestampNode",
				node => node.UseLabel("TimestampLabel").UseMaxValueValidator(TimestampFieldValue("UpperBound"))
			)
			.Build();

		var upperBoundNode = (ITimestampNode)form.Nodes.First(node => node.Name == "UpperBound");
		upperBoundNode.Value = null;
		var dependentNode = (ITimestampNode)form.Nodes.First(node => node.Name == "TimestampNode");
		dependentNode.Value = DateTime.Today;

		form.Update();
		Assert.IsTrue(dependentNode.IsValid);
		Assert.IsFalse(
			dependentNode.ValidationErrors.Contains(
				Resources.TheField_AllowsAMaximumValueOf_.Format("TimestampLabel", string.Empty)
			)
		);
	}

	[TestMethod]
	public void TimestampMaxValueValidator_ExpressionValue_ShouldUpdateUpperBound()
	{
		var form = new FormBuilder("Test")
			.WithTimestampNode("UpperBound")
			.WithTimestampNode(
				"TimestampNode",
				node => node.UseLabel("TimestampLabel").UseMaxValueValidator(TimestampFieldValue("UpperBound"))
			)
			.Build();

		var upperBoundNode = (ITimestampNode)form.Nodes.First(node => node.Name == "UpperBound");
		var dependentNode = (ITimestampNode)form.Nodes.First(node => node.Name == "TimestampNode");
		dependentNode.Value = DateTime.Today;

		// Upper bound too small renders node invalid.
		upperBoundNode.Value = DateTime.Today.AddDays(-1);

		form.Update();
		Assert.IsFalse(dependentNode.IsValid);
		Assert.IsTrue(
			dependentNode.ValidationErrors.Contains(
				Resources.TheField_AllowsAMaximumValueOf_.Format("TimestampLabel", DateTime.Today.AddDays(-1))
			)
		);

		// Increasing the upper bound makes dependent node valid.
		upperBoundNode.Value = DateTime.Today.AddDays(1);

		form.Update();
		Assert.IsTrue(dependentNode.IsValid);
		Assert.IsFalse(
			dependentNode.ValidationErrors.Contains(
				Resources.TheField_AllowsAMaximumValueOf_.Format("TimestampLabel", DateTime.Today.AddDays(1))
			)
		);
	}

	[TestMethod]
	public void MinLengthValidator_NonTextField_ShouldThrowInvalidOperationException()
	{
		var form = new FormBuilder("Test").UseValidator(new MinLengthValidator(StaticValue<decimal?>(5))).Build();

		Assert.ThrowsException<InvalidOperationException>(form.Update);
	}

	[TestMethod]
	public void MinLengthValidator_ContentsNull_ShouldNotValidate()
	{
		var form = new FormBuilder("Test")
			.WithTextNode("TextNode", node => node.UseLabel("TextLabel").UseMinLengthValidator(5))
			.Build();

		var node = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		node.Value = null;

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(Resources.TheField_RequiresAMinimumContentLengthOf_.Format("TextLabel", 5))
		);
	}

	[TestMethod]
	public void MinLengthValidator_ValueTooSmall_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test")
			.WithTextNode("TextNode", node => node.UseLabel("TextLabel").UseMinLengthValidator(5))
			.Build();

		var node = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		node.Value = "abc";

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(
			node.ValidationErrors.Contains(Resources.TheField_RequiresAMinimumContentLengthOf_.Format("TextLabel", 5))
		);
	}

	[TestMethod]
	public void MinLengthValidator_ValueEqualsLowerBound_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithTextNode("TextNode", node => node.UseLabel("TextLabel").UseMinLengthValidator(5))
			.Build();

		var node = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		node.Value = "abcde";

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(Resources.TheField_RequiresAMinimumContentLengthOf_.Format("TextLabel", 5))
		);
	}

	[TestMethod]
	public void MinLengthValidator_ExpressionValueNull_ShouldNotValidate()
	{
		var form = new FormBuilder("Test")
			.WithNumberNode("LowerBound")
			.WithTextNode(
				"TextNode",
				node => node.UseLabel("TextLabel").UseMinLengthValidator(NumberFieldValue("LowerBound"))
			)
			.Build();

		var lowerBoundNode = (INumberNode)form.Nodes.First(node => node.Name == "LowerBound");
		lowerBoundNode.Value = null;
		var dependentNode = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		dependentNode.Value = "abc";

		form.Update();
		Assert.IsTrue(dependentNode.IsValid);
		Assert.IsFalse(
			dependentNode.ValidationErrors.Contains(
				Resources.TheField_RequiresAMinimumContentLengthOf_.Format("TextLabel", string.Empty)
			)
		);
	}

	[TestMethod]
	public void MinLengthValidator_ExpressionValue_ShouldUpdateLowerBound()
	{
		var form = new FormBuilder("Test")
			.WithNumberNode("LowerBound")
			.WithTextNode(
				"TextNode",
				node => node.UseLabel("TextLabel").UseMinLengthValidator(NumberFieldValue("LowerBound"))
			)
			.Build();

		var lowerBoundNode = (INumberNode)form.Nodes.First(node => node.Name == "LowerBound");
		var dependentNode = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		dependentNode.Value = "abc";

		// Lower bound too big renders node invalid.
		lowerBoundNode.Value = 5;

		form.Update();
		Assert.IsFalse(dependentNode.IsValid);
		Assert.IsTrue(
			dependentNode.ValidationErrors.Contains(
				Resources.TheField_RequiresAMinimumContentLengthOf_.Format("TextLabel", 5)
			)
		);

		// Decreasing the lower bound makes dependent node valid.
		lowerBoundNode.Value = 0;

		form.Update();
		Assert.IsTrue(dependentNode.IsValid);
		Assert.IsFalse(
			dependentNode.ValidationErrors.Contains(
				Resources.TheField_RequiresAMinimumContentLengthOf_.Format("TextLabel", 0)
			)
		);
	}

	[TestMethod]
	public void MaxLengthValidator_NonTextField_ShouldThrowInvalidOperationException()
	{
		var form = new FormBuilder("Test").UseValidator(new MaxLengthValidator(StaticValue<decimal?>(5))).Build();

		Assert.ThrowsException<InvalidOperationException>(form.Update);
	}

	[TestMethod]
	public void MaxLengthValidator_ContentsNull_ShouldNotValidate()
	{
		var form = new FormBuilder("Test")
			.WithTextNode("TextNode", node => node.UseLabel("TextLabel").UseMaxLengthValidator(5))
			.Build();

		var node = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		node.Value = null;

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(Resources.TheField_AllowsAMaximumContentLengthOf_.Format("TextLabel", 5))
		);
	}

	[TestMethod]
	public void MaxLengthValidator_ValueTooBig_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test")
			.WithTextNode("TextNode", node => node.UseLabel("TextLabel").UseMaxLengthValidator(5))
			.Build();

		var node = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		node.Value = "abcdef";

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(
			node.ValidationErrors.Contains(Resources.TheField_AllowsAMaximumContentLengthOf_.Format("TextLabel", 5))
		);
	}

	[TestMethod]
	public void MaxLengthValidator_ValueEqualsUpperBound_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithTextNode("TextNode", node => node.UseLabel("TextLabel").UseMaxLengthValidator(5))
			.Build();

		var node = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		node.Value = "abcde";

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(Resources.TheField_AllowsAMaximumContentLengthOf_.Format("TextLabel", 5))
		);
	}

	[TestMethod]
	public void MaxLengthValidator_ExpressionValueNull_ShouldNotValidate()
	{
		var form = new FormBuilder("Test")
			.WithNumberNode("UpperBound")
			.WithTextNode(
				"TextNode",
				node => node.UseLabel("TextLabel").UseMaxLengthValidator(NumberFieldValue("UpperBound"))
			)
			.Build();

		var upperBoundNode = (INumberNode)form.Nodes.First(node => node.Name == "UpperBound");
		upperBoundNode.Value = null;
		var dependentNode = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		dependentNode.Value = "abc";

		form.Update();
		Assert.IsTrue(dependentNode.IsValid);
		Assert.IsFalse(
			dependentNode.ValidationErrors.Contains(
				Resources.TheField_AllowsAMaximumContentLengthOf_.Format("TextLabel", string.Empty)
			)
		);
	}

	[TestMethod]
	public void MaxLengthValidator_ExpressionValue_ShouldUpdateUpperBound()
	{
		var form = new FormBuilder("Test")
			.WithNumberNode("UpperBound")
			.WithTextNode(
				"TextNode",
				node => node.UseLabel("TextLabel").UseMaxLengthValidator(NumberFieldValue("UpperBound"))
			)
			.Build();

		var upperBoundNode = (INumberNode)form.Nodes.First(node => node.Name == "UpperBound");
		var dependentNode = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		dependentNode.Value = "abc";

		// Upper bound too small renders node invalid.
		upperBoundNode.Value = 2;

		form.Update();
		Assert.IsFalse(dependentNode.IsValid);
		Assert.IsTrue(
			dependentNode.ValidationErrors.Contains(
				Resources.TheField_AllowsAMaximumContentLengthOf_.Format("TextLabel", 2)
			)
		);

		// Increasing the upper bound makes dependent node valid.
		upperBoundNode.Value = 6;

		form.Update();
		Assert.IsTrue(dependentNode.IsValid);
		Assert.IsFalse(
			dependentNode.ValidationErrors.Contains(
				Resources.TheField_AllowsAMaximumContentLengthOf_.Format("TextLabel", 6)
			)
		);
	}

	[TestMethod]
	public void AllowedSymbolValidator_NonFileField_ShouldThrowInvalidOperationException()
	{
		var form = new FormBuilder("Test").UseValidator(new AllowedSymbolValidator("abc")).Build();

		Assert.ThrowsException<InvalidOperationException>(form.Update);
	}

	[TestMethod]
	public void AllowedSymbolValidator_ValueNull_ShouldNotValidate()
	{
		var form = new FormBuilder("Test")
			.WithTextNode("TextNode", node => node.UseLabel("TextLabel").UseAllowedSymbolValidator("abc"))
			.Build();

		var node = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		node.Value = null;

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(Resources.TheField_DoesNotAllowTheFollowingCharacters_.Format("d"))
		);
	}

	[TestMethod]
	public void AllowedSymbolValidator_ValueContainsInvalidChars_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test")
			.WithTextNode("TextNode", node => node.UseLabel("TextLabel").UseAllowedSymbolValidator("abc"))
			.Build();

		var node = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		node.Value = "abcdabc";

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(
			node.ValidationErrors.Contains(Resources.TheField_DoesNotAllowTheFollowingCharacters_.Format("d"))
		);
	}

	[TestMethod]
	public void AllowedSymbolValidator_OnlyListedCharacters_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithTextNode("TextNode", node => node.UseLabel("TextLabel").UseAllowedSymbolValidator("abc"))
			.Build();

		var node = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		node.Value = "aabbcc";

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(Resources.TheField_DoesNotAllowTheFollowingCharacters_.Format("d"))
		);
	}

	[TestMethod]
	public void EmailValidator_NonFileField_ShouldThrowInvalidOperationException()
	{
		var form = new FormBuilder("Test").UseValidator(new EmailValidator()).Build();

		Assert.ThrowsException<InvalidOperationException>(form.Update);
	}

	[TestMethod]
	public void EmailValidator_ValueNull_ShouldNotValidate()
	{
		var form = new FormBuilder("Test")
			.WithTextNode("TextNode", node => node.UseLabel("TextLabel").UseEmailValidator())
			.Build();

		var node = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		node.Value = null;

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(
				Resources.TheValue_CouldNotBeRecognizedAsAValidEmailFormat.Format(string.Empty)
			)
		);
	}

	[TestMethod]
	public void EmailValidator_RandomText_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test")
			.WithTextNode("TextNode", node => node.UseLabel("TextLabel").UseEmailValidator())
			.Build();

		var node = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		node.Value = "please send letter";

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(
			node.ValidationErrors.Contains(
				Resources.TheValue_CouldNotBeRecognizedAsAValidEmailFormat.Format("please send letter")
			)
		);
	}

	[TestMethod]
	public void EmailValidator_EmailAddress_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithTextNode("TextNode", node => node.UseLabel("TextLabel").UseEmailValidator())
			.Build();

		var node = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		node.Value = "test@mail.de";

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(
				Resources.TheValue_CouldNotBeRecognizedAsAValidEmailFormat.Format("test@mail.de")
			)
		);
	}

	[TestMethod]
	public void PhoneNumberValidator_NonFileField_ShouldThrowInvalidOperationException()
	{
		var form = new FormBuilder("Test").UseValidator(new PhoneNumberValidator()).Build();

		Assert.ThrowsException<InvalidOperationException>(form.Update);
	}

	[TestMethod]
	public void PhoneNumberValidator_ValueNull_ShouldNotValidate()
	{
		var form = new FormBuilder("Test")
			.WithTextNode("TextNode", node => node.UseLabel("TextLabel").UsePhoneNumberValidator())
			.Build();

		var node = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		node.Value = null;

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(
				Resources.TheValue_CouldNotBeRecognizedAsAValidPhoneNumberFormat.Format(string.Empty)
			)
		);
	}

	[TestMethod]
	public void PhoneNumberValidator_RandomText_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test")
			.WithTextNode("TextNode", node => node.UseLabel("TextLabel").UsePhoneNumberValidator())
			.Build();

		var node = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		node.Value = "please send letter";

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(
			node.ValidationErrors.Contains(
				Resources.TheValue_CouldNotBeRecognizedAsAValidPhoneNumberFormat.Format("please send letter")
			)
		);
	}

	[TestMethod]
	public void PhoneNumberValidator_EmailAddress_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithTextNode("TextNode", node => node.UseLabel("TextLabel").UsePhoneNumberValidator())
			.Build();

		var node = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");

		node.Value = "+4915712345678";
		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(
				Resources.TheValue_CouldNotBeRecognizedAsAValidPhoneNumberFormat.Format("+4915712345678")
			)
		);

		node.Value = "+49 157 12345678";
		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(
				Resources.TheValue_CouldNotBeRecognizedAsAValidPhoneNumberFormat.Format("+49 157 12345678")
			)
		);

		node.Value = "+49157123456";
		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(
				Resources.TheValue_CouldNotBeRecognizedAsAValidPhoneNumberFormat.Format("+49157123456")
			)
		);

		node.Value = "+49 157 123456";
		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(
				Resources.TheValue_CouldNotBeRecognizedAsAValidPhoneNumberFormat.Format("+49 157 123456")
			)
		);
	}

	[TestMethod]
	public void IbanValidator_NonFileField_ShouldThrowInvalidOperationException()
	{
		var form = new FormBuilder("Test").UseValidator(new IbanValidator()).Build();

		Assert.ThrowsException<InvalidOperationException>(form.Update);
	}

	[TestMethod]
	public void IbanValidator_ValueNull_ShouldNotValidate()
	{
		var form = new FormBuilder("Test")
			.WithTextNode("TextNode", node => node.UseLabel("TextLabel").UseIbanValidator())
			.Build();

		var node = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		node.Value = null;

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(Resources.TheValue_CouldNotBeRecognizedAsAValidIban.Format(string.Empty))
		);
	}

	[TestMethod]
	public void IbanValidator_RandomText_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test")
			.WithTextNode("TextNode", node => node.UseLabel("TextLabel").UseIbanValidator())
			.Build();

		var node = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		node.Value = "I'll pay by check";

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(
			node.ValidationErrors.Contains(
				Resources.TheValue_CouldNotBeRecognizedAsAValidIban.Format("I'll pay by check")
			)
		);
	}

	[TestMethod]
	public void IbanValidator_Iban_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithTextNode("TextNode", node => node.UseLabel("TextLabel").UseIbanValidator())
			.Build();

		var node = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");

		node.Value = "DE07500105172438863988";
		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(
				Resources.TheValue_CouldNotBeRecognizedAsAValidIban.Format("DE07500105172438863988")
			)
		);
	}

	[TestMethod]
	public void MinCountValidator_NonCollectionNode_ShouldThrowInvalidOperationException()
	{
		var form = new FormBuilder("Test").UseValidator(new MinCountValidator(StaticValue<decimal?>(5))).Build();

		Assert.ThrowsException<InvalidOperationException>(form.Update);
	}

	[TestMethod]
	public void MinCountValidator_NoInstance_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test")
			.WithCollectionNode(
				"CollectionNode",
				(node, _) => node.UseLabel("CollectionLabel").UseTemplate("Template").UseMinCountValidator(5)
			)
			.Build();

		var node = (ICollectionNode)form.Nodes.First(node => node.Name == "CollectionNode");

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(
			node.ValidationErrors.Contains(
				Resources.TheField_RequiresAtLeastTheFollowingNumberOfInstances_.Format("CollectionLabel", 5)
			)
		);
	}

	[TestMethod]
	public void MinCountValidator_TooLittleInstances_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test")
			.WithCollectionNode(
				"CollectionNode",
				(node, _) => node.UseLabel("CollectionLabel").UseTemplate("Template").UseMinCountValidator(5)
			)
			.Build();

		var node = (ICollectionNode)form.Nodes.First(node => node.Name == "CollectionNode");
		node.Instantiate(node.Templates.First());
		node.Instantiate(node.Templates.First());
		node.Instantiate(node.Templates.First());

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(
			node.ValidationErrors.Contains(
				Resources.TheField_RequiresAtLeastTheFollowingNumberOfInstances_.Format("CollectionLabel", 5)
			)
		);
	}

	[TestMethod]
	public void MinCountValidator_InstanceCountEqualsLowerBound_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithCollectionNode(
				"CollectionNode",
				(node, _) => node.UseLabel("CollectionLabel").UseTemplate("Template").UseMinCountValidator(5)
			)
			.Build();

		var node = (ICollectionNode)form.Nodes.First(node => node.Name == "CollectionNode");
		node.Instantiate(node.Templates.First());
		node.Instantiate(node.Templates.First());
		node.Instantiate(node.Templates.First());
		node.Instantiate(node.Templates.First());
		node.Instantiate(node.Templates.First());

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(
				Resources.TheField_RequiresAtLeastTheFollowingNumberOfInstances_.Format("CollectionLabel", 5)
			)
		);
	}

	[TestMethod]
	public void MinCountValidator_ExpressionValueNull_ShouldNotValidate()
	{
		var form = new FormBuilder("Test")
			.WithNumberNode("LowerBound")
			.WithCollectionNode(
				"CollectionNode",
				(node, _) =>
					node.UseLabel("CollectionLabel")
						.UseTemplate("Template")
						.UseMinCountValidator(NumberFieldValue("LowerBound"))
			)
			.Build();

		var lowerBoundNode = (INumberNode)form.Nodes.First(node => node.Name == "LowerBound");
		lowerBoundNode.Value = null;
		var dependentNode = (ICollectionNode)form.Nodes.First(node => node.Name == "CollectionNode");

		form.Update();
		Assert.IsTrue(dependentNode.IsValid);
		Assert.IsFalse(
			dependentNode.ValidationErrors.Contains(
				Resources.TheField_RequiresAtLeastTheFollowingNumberOfInstances_.Format("CollectionLabel", string.Empty)
			)
		);
	}

	[TestMethod]
	public void MinCountValidator_ExpressionValue_ShouldUpdateLowerBound()
	{
		var form = new FormBuilder("Test")
			.WithNumberNode("LowerBound")
			.WithCollectionNode(
				"CollectionNode",
				(node, _) =>
					node.UseLabel("CollectionLabel")
						.UseTemplate("Template")
						.UseMinCountValidator(NumberFieldValue("LowerBound"))
			)
			.Build();

		var lowerBoundNode = (INumberNode)form.Nodes.First(node => node.Name == "LowerBound");
		var dependentNode = (ICollectionNode)form.Nodes.First(node => node.Name == "CollectionNode");
		dependentNode.Instantiate(dependentNode.Templates.First());
		dependentNode.Instantiate(dependentNode.Templates.First());

		// Lower bound too big renders node invalid.
		lowerBoundNode.Value = 5;

		form.Update();
		Assert.IsFalse(dependentNode.IsValid);
		Assert.IsTrue(
			dependentNode.ValidationErrors.Contains(
				Resources.TheField_RequiresAtLeastTheFollowingNumberOfInstances_.Format("CollectionLabel", 5)
			)
		);

		// Decreasing the lower bound makes dependent node valid.
		lowerBoundNode.Value = 0;

		form.Update();
		Assert.IsTrue(dependentNode.IsValid);
		Assert.IsFalse(
			dependentNode.ValidationErrors.Contains(
				Resources.TheField_RequiresAtLeastTheFollowingNumberOfInstances_.Format("CollectionLabel", 0)
			)
		);
	}

	[TestMethod]
	public void MaxCountValidator_NonCollectionNode_ShouldThrowInvalidOperationException()
	{
		var form = new FormBuilder("Test").UseValidator(new MaxCountValidator(StaticValue<decimal?>(5))).Build();

		Assert.ThrowsException<InvalidOperationException>(form.Update);
	}

	[TestMethod]
	public void MaxCountValidator_NoInstance_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithCollectionNode(
				"CollectionNode",
				(node, _) => node.UseLabel("CollectionLabel").UseTemplate("Template").UseMaxCountValidator(5)
			)
			.Build();

		var node = (ICollectionNode)form.Nodes.First(node => node.Name == "CollectionNode");

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(Resources.TheField_AllowsAMaximumOf_Instances.Format("CollectionLabel", 5))
		);
	}

	[TestMethod]
	public void MaxCountValidator_TooManyInstances_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test")
			.WithCollectionNode(
				"CollectionNode",
				(node, _) => node.UseLabel("CollectionLabel").UseTemplate("Template").UseMaxCountValidator(5)
			)
			.Build();

		var node = (ICollectionNode)form.Nodes.First(node => node.Name == "CollectionNode");
		node.Instantiate(node.Templates.First());
		node.Instantiate(node.Templates.First());
		node.Instantiate(node.Templates.First());
		node.Instantiate(node.Templates.First());
		node.Instantiate(node.Templates.First());
		node.Instantiate(node.Templates.First());

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(
			node.ValidationErrors.Contains(Resources.TheField_AllowsAMaximumOf_Instances.Format("CollectionLabel", 5))
		);
	}

	[TestMethod]
	public void MaxCountValidator_InstanceCountEqualsUpperBound_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithCollectionNode(
				"CollectionNode",
				(node, _) => node.UseLabel("CollectionLabel").UseTemplate("Template").UseMaxCountValidator(5)
			)
			.Build();

		var node = (ICollectionNode)form.Nodes.First(node => node.Name == "CollectionNode");
		node.Instantiate(node.Templates.First());
		node.Instantiate(node.Templates.First());
		node.Instantiate(node.Templates.First());
		node.Instantiate(node.Templates.First());
		node.Instantiate(node.Templates.First());

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(Resources.TheField_AllowsAMaximumOf_Instances.Format("CollectionLabel", 5))
		);
	}

	[TestMethod]
	public void MaxCountValidator_ExpressionValueNull_ShouldNotValidate()
	{
		var form = new FormBuilder("Test")
			.WithNumberNode("LowerBound")
			.WithCollectionNode(
				"CollectionNode",
				(node, _) =>
					node.UseLabel("CollectionLabel")
						.UseTemplate("Template")
						.UseMaxCountValidator(NumberFieldValue("LowerBound"))
			)
			.Build();

		var lowerBoundNode = (INumberNode)form.Nodes.First(node => node.Name == "LowerBound");
		lowerBoundNode.Value = null;
		var dependentNode = (ICollectionNode)form.Nodes.First(node => node.Name == "CollectionNode");
		dependentNode.Instantiate(dependentNode.Templates.First());
		dependentNode.Instantiate(dependentNode.Templates.First());
		dependentNode.Instantiate(dependentNode.Templates.First());

		form.Update();
		Assert.IsTrue(dependentNode.IsValid);
		Assert.IsFalse(
			dependentNode.ValidationErrors.Contains(
				Resources.TheField_AllowsAMaximumOf_Instances.Format("CollectionLabel", string.Empty)
			)
		);
	}

	[TestMethod]
	public void MaxCountValidator_ExpressionValue_ShouldUpdateLowerBound()
	{
		var form = new FormBuilder("Test")
			.WithNumberNode("LowerBound")
			.WithCollectionNode(
				"CollectionNode",
				(node, _) =>
					node.UseLabel("CollectionLabel")
						.UseTemplate("Template")
						.UseMaxCountValidator(NumberFieldValue("LowerBound"))
			)
			.Build();

		var lowerBoundNode = (INumberNode)form.Nodes.First(node => node.Name == "LowerBound");
		var dependentNode = (ICollectionNode)form.Nodes.First(node => node.Name == "CollectionNode");
		dependentNode.Instantiate(dependentNode.Templates.First());
		dependentNode.Instantiate(dependentNode.Templates.First());
		dependentNode.Instantiate(dependentNode.Templates.First());

		// Upper bound too small renders node invalid.
		lowerBoundNode.Value = 2;

		form.Update();
		Assert.IsFalse(dependentNode.IsValid);
		Assert.IsTrue(
			dependentNode.ValidationErrors.Contains(
				Resources.TheField_AllowsAMaximumOf_Instances.Format("CollectionLabel", 2)
			)
		);

		// Decreasing the lower bound makes dependent node valid.
		lowerBoundNode.Value = 3;

		form.Update();
		Assert.IsTrue(dependentNode.IsValid);
		Assert.IsFalse(
			dependentNode.ValidationErrors.Contains(
				Resources.TheField_AllowsAMaximumOf_Instances.Format("CollectionLabel", 3)
			)
		);
	}

	[TestMethod]
	public void FileNameMaxLengthValidator_NonTextField_ShouldThrowInvalidOperationException()
	{
		var form = new FormBuilder("Test")
			.UseValidator(new FileNameMaxLengthValidator(StaticValue<decimal?>(5)))
			.Build();

		Assert.ThrowsException<InvalidOperationException>(form.Update);
	}

	[TestMethod]
	public void FileNameMaxLengthValidator_FileNameNull_ShouldNotValidate()
	{
		var form = new FormBuilder("Test")
			.WithFileNode("FileNode", node => node.UseLabel("FileLabel").UseFileNameMaxLengthValidator(5))
			.Build();

		var node = (IFileNode)form.Nodes.First(node => node.Name == "FileNode");
		node.Value = new() { FileContents = [1, 2, 3], FileName = null };

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(Resources.TheField_AllowsAMaximumFileNameLengthOf_.Format("FileLabel", 5))
		);
	}

	[TestMethod]
	public void FileNameMaxLengthValidator_FileNameTooBig_ShouldBeInvalid()
	{
		var form = new FormBuilder("Test")
			.WithFileNode("FileNode", node => node.UseLabel("FileLabel").UseFileNameMaxLengthValidator(5))
			.Build();

		var node = (IFileNode)form.Nodes.First(node => node.Name == "FileNode");
		node.Value = new() { FileContents = [1, 2, 3], FileName = "abcdef" };

		form.Update();
		Assert.IsFalse(node.IsValid);
		Assert.IsTrue(
			node.ValidationErrors.Contains(Resources.TheField_AllowsAMaximumFileNameLengthOf_.Format("FileLabel", 5))
		);
	}

	[TestMethod]
	public void FileNameMaxLengthValidator_FileNameLengthEqualsUpperBound_ShouldBeValid()
	{
		var form = new FormBuilder("Test")
			.WithFileNode("FileNode", node => node.UseLabel("FileLabel").UseFileNameMaxLengthValidator(5))
			.Build();

		var node = (IFileNode)form.Nodes.First(node => node.Name == "FileNode");
		node.Value = new() { FileContents = [1, 2, 3], FileName = "abcde" };

		form.Update();
		Assert.IsTrue(node.IsValid);
		Assert.IsFalse(
			node.ValidationErrors.Contains(Resources.TheField_AllowsAMaximumFileNameLengthOf_.Format("FileLabel", 5))
		);
	}

	[TestMethod]
	public void FileNameMaxLengthValidator_ExpressionValueNull_ShouldNotValidate()
	{
		var form = new FormBuilder("Test")
			.WithNumberNode("UpperBound")
			.WithFileNode(
				"FileNode",
				node => node.UseLabel("FileLabel").UseFileNameMaxLengthValidator(NumberFieldValue("UpperBound"))
			)
			.Build();

		var upperBoundNode = (INumberNode)form.Nodes.First(node => node.Name == "UpperBound");
		upperBoundNode.Value = null;
		var dependentNode = (IFileNode)form.Nodes.First(node => node.Name == "FileNode");
		dependentNode.Value = new() { FileContents = [1, 2, 3], FileName = "abc" };

		form.Update();
		Assert.IsTrue(dependentNode.IsValid);
		Assert.IsFalse(
			dependentNode.ValidationErrors.Contains(
				Resources.TheField_AllowsAMaximumFileNameLengthOf_.Format("FileLabel", string.Empty)
			)
		);
	}

	[TestMethod]
	public void FileNameMaxLengthValidator_ExpressionValue_ShouldUpdateUpperBound()
	{
		var form = new FormBuilder("Test")
			.WithNumberNode("UpperBound")
			.WithFileNode(
				"FileNode",
				node => node.UseLabel("FileLabel").UseFileNameMaxLengthValidator(NumberFieldValue("UpperBound"))
			)
			.Build();

		var upperBoundNode = (INumberNode)form.Nodes.First(node => node.Name == "UpperBound");
		var dependentNode = (IFileNode)form.Nodes.First(node => node.Name == "FileNode");
		dependentNode.Value = new() { FileContents = [1, 2, 3], FileName = "abc" };

		// Upper bound too small renders node invalid.
		upperBoundNode.Value = 2;

		form.Update();
		Assert.IsFalse(dependentNode.IsValid);
		Assert.IsTrue(
			dependentNode.ValidationErrors.Contains(
				Resources.TheField_AllowsAMaximumFileNameLengthOf_.Format("FileLabel", 2)
			)
		);

		// Increasing the upper bound makes dependent node valid.
		upperBoundNode.Value = 6;

		form.Update();
		Assert.IsTrue(dependentNode.IsValid);
		Assert.IsFalse(
			dependentNode.ValidationErrors.Contains(
				Resources.TheField_AllowsAMaximumFileNameLengthOf_.Format("FileLabel", 6)
			)
		);
	}

	[TestMethod]
	public void ExpressionValidator_ExpressionValue_ShouldDecideValidationState()
	{
		var form = new FormBuilder("Test")
			.WithBooleanNode("DeciderNode", node => node.UseLabel("DeciderLabel"))
			.WithTextNode(
				"TextNode",
				node =>
					node.UseLabel("TextLabel")
						.UseExpressionValidator(
							BooleanFieldValue("DeciderNode").Coalesce(true),
							"Field DeciderNode is true."
						)
			)
			.Build();

		var deciderNode = (IBooleanNode)form.Nodes.First(node => node.Name == "DeciderNode");
		var dependentNode = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		dependentNode.Value = "test";

		// Decider node true renders node invalid.
		deciderNode.Value = true;

		form.Update();
		Assert.IsFalse(dependentNode.IsValid);
		Assert.IsTrue(dependentNode.ValidationErrors.Contains("Field DeciderLabel is true"));

		// Decider node false makes dependent node valid.
		deciderNode.Value = false;

		form.Update();
		Assert.IsTrue(dependentNode.IsValid);
		Assert.IsFalse(dependentNode.ValidationErrors.Contains("Field DeciderLabel is true"));
	}
}
