namespace RobinEpple.Common.Forms.Test.Tests;

using System.Diagnostics.CodeAnalysis;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.Wrappers.Abstractions;

[TestClass]
public partial class Wrappers
{
	public Wrappers()
	{
		BuildForm();
	}

	public IForm Form { get; private set; }

	[MemberNotNull(nameof(Form))]
	[GenerateFormWrapper(nameof(Form))]
	private void BuildForm()
	{
		Form = new FormBuilder("Test")
			.WithTextNode("Text")
			.WithNumberNode("Number")
			.WithBooleanNode("Boolean")
			.WithTimestampNode("Timestamp")
			.WithFileNode("File")
			.Build();
	}

	[TestMethod]
	public void GenerateFormWrapper_ShouldGeneratePropertiesForFields()
	{
		Assert.IsTrue(FormWrapper.Text is ITextNode);
		Assert.IsTrue(FormWrapper.Number is INumberNode);
		Assert.IsTrue(FormWrapper.Boolean is IBooleanNode);
		Assert.IsTrue(FormWrapper.Timestamp is ITimestampNode);
		Assert.IsTrue(FormWrapper.File is IFileNode);
	}
}
