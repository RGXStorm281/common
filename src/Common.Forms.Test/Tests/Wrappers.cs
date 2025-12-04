namespace RobinEpple.Common.Forms.Test.Tests;

using System.Diagnostics.CodeAnalysis;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.Wrappers.Abstractions;

[TestClass]
public partial class Wrappers
{
	public Wrappers()
	{
		BuildFields();
		BuildSubstructure();
	}

	public IForm Fields { get; private set; }

	[MemberNotNull(nameof(Fields))]
	[GenerateFormWrapper(nameof(Fields))]
	private void BuildFields()
	{
		Fields = new FormBuilder("Test")
			.WithTextNode("Text")
			.WithNumberNode("Number", textNode => textNode.UseLabel("Please insert some number"))
			.WithBooleanNode("Boolean")
			.WithTimestampNode("Timestamp")
			.WithFileNode("File")
			.Build();
	}

	[TestMethod]
	public void GenerateFormWrapper_ShouldGeneratePropertiesForFields()
	{
		Assert.IsTrue(FieldsWrapper.Text is ITextNode);
		Assert.IsTrue(FieldsWrapper.Number is INumberNode);
		Assert.IsTrue(FieldsWrapper.Boolean is IBooleanNode);
		Assert.IsTrue(FieldsWrapper.Timestamp is ITimestampNode);
		Assert.IsTrue(FieldsWrapper.File is IFileNode);
	}

	public IForm Substructure { get; private set; }

	[MemberNotNull(nameof(Substructure))]
	[GenerateFormWrapper(nameof(Substructure))]
	private void BuildSubstructure()
	{
		Substructure = new FormBuilder("Test")
			.WithSection(
				"Section",
				(sectionBuilder, recursiveTemplate) =>
					sectionBuilder
						.WithTextNode("Text")
						.WithNumberNode("Number")
						.WithBooleanNode("Boolean")
						.WithTimestampNode("Timestamp")
						.WithFileNode("File")
			)
			.Build();
	}

	[TestMethod]
	public void GenerateFormWrapper_ShouldGeneratePropertiesForSubstructure()
	{
		Assert.IsTrue(SubstructureWrapper.Section.Node is IForm);
		Assert.IsTrue(SubstructureWrapper.Section.Text is ITextNode);
		Assert.IsTrue(SubstructureWrapper.Section.Number is INumberNode);
		Assert.IsTrue(SubstructureWrapper.Section.Boolean is IBooleanNode);
		Assert.IsTrue(SubstructureWrapper.Section.Timestamp is ITimestampNode);
		Assert.IsTrue(SubstructureWrapper.Section.File is IFileNode);
	}
}
