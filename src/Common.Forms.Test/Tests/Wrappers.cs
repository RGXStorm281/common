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
		Form = new FormBuilder("Test").WithTextNode("Text").Build();
	}

	[TestMethod]
	public void GenerateFormWrapper_ShouldGeneratePropertiesForFields() { }
}
