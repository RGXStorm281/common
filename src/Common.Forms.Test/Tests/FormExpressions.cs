namespace RobinEpple.Common.Forms.Test.Tests;

using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Nodes;
using static RobinEpple.Common.Forms.Expressions.FormExpression;

[TestClass]
public class FormExpressions
{
	# region static values

	[TestMethod]
	public void StaticValue_ShouldReturnConfiguredValue()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsTrue(StaticValue(true).EvaluateOn(form));
		Assert.IsFalse(StaticValue(false).EvaluateOn(form));
		Assert.Equals(null, StaticValue<bool?>(null).EvaluateOn(form));
		Assert.Equals("testText", StaticValue("testText").EvaluateOn(form));
	}

	[TestMethod]
	public void Coalesce_ShouldReturnExpressionValueIfNotNull()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsTrue(StaticValue<bool?>(true).Coalesce(false).EvaluateOn(form));
		Assert.IsFalse(StaticValue<bool?>(false).Coalesce(true).EvaluateOn(form));
		Assert.AreEqual(
			"testText",
			StaticValue<string?>("testText").Coalesce(StaticValue("fallbackText")).EvaluateOn(form)
		);
	}

	[TestMethod]
	public void Coalesce_ShouldReturnFallbackValueIfNull()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsFalse(StaticValue<bool?>(null).Coalesce(false).EvaluateOn(form));
		Assert.IsTrue(StaticValue<bool?>(null).Coalesce(true).EvaluateOn(form));
		Assert.AreEqual(
			"fallbackText",
			StaticValue<string?>(null).Coalesce(StaticValue("fallbackText")).EvaluateOn(form)
		);
	}

	[TestMethod]
	public void Conditional_ShouldReturnValueDependingOnConditionResult()
	{
		var form = new FormBuilder("Test").Build();

		Assert.AreEqual(
			"trueText",
			StaticValue(true).Conditional(StaticValue("trueText"), StaticValue("falseText")).EvaluateOn(form)
		);

		Assert.AreEqual(
			"trueText",
			StaticValue(false).Conditional(StaticValue("trueText"), StaticValue("falseText")).EvaluateOn(form)
		);
	}

	# endregion

	# region logical operators

	[TestMethod]
	public void And_ShouldBeTrueIfBothValuesTrue()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsTrue(StaticValue(true).And(StaticValue(true)).EvaluateOn(form));
	}

	[TestMethod]
	public void And_ShouldBeFalseIfAnyValueNotTrue()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsFalse(StaticValue(true).And(StaticValue(false)).EvaluateOn(form));
		Assert.IsFalse(StaticValue(false).And(StaticValue(true)).EvaluateOn(form));
		Assert.IsFalse(StaticValue(false).And(StaticValue(false)).EvaluateOn(form));
	}

	[TestMethod]
	public void All_ShouldBeTrueIfAllValuesTrue()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsTrue(
			new IFormExpression<bool>[] { StaticValue(true), StaticValue(true), StaticValue(true) }
				.All()
				.EvaluateOn(form)
		);
	}

	[TestMethod]
	public void All_ShouldBeFalseIfAnyValueNotTrue()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsFalse(
			new IFormExpression<bool>[] { StaticValue(false), StaticValue(true), StaticValue(true) }
				.All()
				.EvaluateOn(form)
		);

		Assert.IsFalse(
			new IFormExpression<bool>[] { StaticValue(true), StaticValue(false), StaticValue(true) }
				.All()
				.EvaluateOn(form)
		);

		Assert.IsFalse(
			new IFormExpression<bool>[] { StaticValue(true), StaticValue(true), StaticValue(false) }
				.All()
				.EvaluateOn(form)
		);

		Assert.IsFalse(
			new IFormExpression<bool>[] { StaticValue(false), StaticValue(false), StaticValue(false) }
				.All()
				.EvaluateOn(form)
		);
	}

	[TestMethod]
	public void Or_ShouldBeTrueIfAnyValueTrue()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsTrue(StaticValue(true).Or(StaticValue(true)).EvaluateOn(form));
		Assert.IsTrue(StaticValue(true).Or(StaticValue(false)).EvaluateOn(form));
		Assert.IsTrue(StaticValue(false).Or(StaticValue(true)).EvaluateOn(form));
	}

	[TestMethod]
	public void Or_ShouldBeFalseIfBothValueNotTrue()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsFalse(StaticValue(false).Or(StaticValue(false)).EvaluateOn(form));
	}

	[TestMethod]
	public void Any_ShouldBeTrueIfAnyValueTrue()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsTrue(
			new IFormExpression<bool>[] { StaticValue(true), StaticValue(true), StaticValue(true) }
				.Any()
				.EvaluateOn(form)
		);

		Assert.IsTrue(
			new IFormExpression<bool>[] { StaticValue(true), StaticValue(false), StaticValue(false) }
				.Any()
				.EvaluateOn(form)
		);

		Assert.IsTrue(
			new IFormExpression<bool>[] { StaticValue(false), StaticValue(true), StaticValue(false) }
				.Any()
				.EvaluateOn(form)
		);

		Assert.IsTrue(
			new IFormExpression<bool>[] { StaticValue(false), StaticValue(false), StaticValue(true) }
				.Any()
				.EvaluateOn(form)
		);
	}

	[TestMethod]
	public void Any_ShouldBeFalseIfAllValuesFalse()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsFalse(
			new IFormExpression<bool>[] { StaticValue(false), StaticValue(false), StaticValue(false) }
				.Any()
				.EvaluateOn(form)
		);
	}

	# endregion

	# region field access

	[TestMethod]
	public void FieldValues_ShouldAccessFieldsInForm()
	{
		var form = new FormBuilder("Test")
			.WithBooleanNode("Boolean")
			.WithFileNode("File")
			.WithNumberNode("Number")
			.WithTextNode("Text")
			.WithTimestampNode("Timestamp")
			.Build();

		var booleanNode = (IBooleanNode)form.Nodes.First(node => node.Name == "Boolean");
		var fileNode = (IFileNode)form.Nodes.First(node => node.Name == "File");
		var numberNode = (INumberNode)form.Nodes.First(node => node.Name == "Number");
		var textNode = (ITextNode)form.Nodes.First(node => node.Name == "Text");
		var timestampNode = (ITimestampNode)form.Nodes.First(node => node.Name == "Timestamp");

		booleanNode.Value = true;
		fileNode.Value = new("test", [1, 2, 3]);
		numberNode.Value = 42;
		textNode.Value = "testText";
		timestampNode.Value = DateTime.Today;

		Assert.AreEqual(true, BooleanFieldValue("Boolean").EvaluateOn(form));
		Assert.AreEqual("test", FileFieldFileName("File").EvaluateOn(form));
		Assert.AreEqual(1, FileFieldFileContent("File").EvaluateOn(form)![0]);
		Assert.AreEqual(2, FileFieldFileContent("File").EvaluateOn(form)![1]);
		Assert.AreEqual(3, FileFieldFileContent("File").EvaluateOn(form)![2]);
		Assert.AreEqual(42, NumberFieldValue("Number").EvaluateOn(form));
		Assert.AreEqual("testText", TextFieldValue("Text").EvaluateOn(form));
		Assert.AreEqual(DateTime.Today, TimestampFieldValue("Timestamp").EvaluateOn(form));
	}

	[TestMethod]
	public void FieldValues_ShouldAccessNeighboringFields()
	{
		var form = new FormBuilder("Test")
			.WithBooleanNode("Boolean")
			.WithFileNode("File")
			.WithNumberNode("Number")
			.WithTextNode("Text")
			.WithTimestampNode("Timestamp")
			.WithCollectionNode("Collection")
			.WithTemplatedSection("Template")
			.Build();

		var booleanNode = (IBooleanNode)form.Nodes.First(node => node.Name == "Boolean");
		var fileNode = (IFileNode)form.Nodes.First(node => node.Name == "File");
		var numberNode = (INumberNode)form.Nodes.First(node => node.Name == "Number");
		var textNode = (ITextNode)form.Nodes.First(node => node.Name == "Text");
		var timestampNode = (ITimestampNode)form.Nodes.First(node => node.Name == "Timestamp");
		var collectionNode = (ICollectionNode)form.Nodes.First(node => node.Name == "Collection");
		var templateNode = (ITemplateNode)form.Nodes.First(node => node.Name == "Template");

		booleanNode.Value = true;
		fileNode.Value = new("test", [1, 2, 3]);
		numberNode.Value = 42;
		textNode.Value = "testText";
		timestampNode.Value = DateTime.Today;

		Assert.AreEqual(true, BooleanFieldValue("Boolean").EvaluateOn(collectionNode));
		Assert.AreEqual("test", FileFieldFileName("File").EvaluateOn(collectionNode));
		Assert.AreEqual(1, FileFieldFileContent("File").EvaluateOn(collectionNode)![0]);
		Assert.AreEqual(2, FileFieldFileContent("File").EvaluateOn(collectionNode)![1]);
		Assert.AreEqual(3, FileFieldFileContent("File").EvaluateOn(collectionNode)![2]);
		Assert.AreEqual(42, NumberFieldValue("Number").EvaluateOn(collectionNode));
		Assert.AreEqual("testText", TextFieldValue("Text").EvaluateOn(collectionNode));
		Assert.AreEqual(DateTime.Today, TimestampFieldValue("Timestamp").EvaluateOn(collectionNode));

		Assert.AreEqual(true, BooleanFieldValue("Boolean").EvaluateOn(templateNode));
		Assert.AreEqual("test", FileFieldFileName("File").EvaluateOn(templateNode));
		Assert.AreEqual(1, FileFieldFileContent("File").EvaluateOn(templateNode)![0]);
		Assert.AreEqual(2, FileFieldFileContent("File").EvaluateOn(templateNode)![1]);
		Assert.AreEqual(3, FileFieldFileContent("File").EvaluateOn(templateNode)![2]);
		Assert.AreEqual(42, NumberFieldValue("Number").EvaluateOn(templateNode));
		Assert.AreEqual("testText", TextFieldValue("Text").EvaluateOn(templateNode));
		Assert.AreEqual(DateTime.Today, TimestampFieldValue("Timestamp").EvaluateOn(templateNode));
	}

	[TestMethod]
	public void FieldValues_ShouldNotAccessUpperScopes()
	{
		var form = new FormBuilder("Test")
			.WithBooleanNode("Boolean")
			.WithFileNode("File")
			.WithNumberNode("Number")
			.WithTextNode("Text")
			.WithTimestampNode("Timestamp")
			.WithCollectionNode("Collection", (node, _) => node.UseTemplate("CollectionTemplate"))
			.WithTemplatedSection("Template", (node, _) => node.UseTemplate("SectionTemplate"))
			.Build();

		var booleanNode = (IBooleanNode)form.Nodes.First(node => node.Name == "Boolean");
		var fileNode = (IFileNode)form.Nodes.First(node => node.Name == "File");
		var numberNode = (INumberNode)form.Nodes.First(node => node.Name == "Number");
		var textNode = (ITextNode)form.Nodes.First(node => node.Name == "Text");
		var timestampNode = (ITimestampNode)form.Nodes.First(node => node.Name == "Timestamp");
		var collectionNode = (ICollectionNode)form.Nodes.First(node => node.Name == "Collection");
		collectionNode.Instantiate(collectionNode.Templates.First());
		var templateNode = (ITemplateNode)form.Nodes.First(node => node.Name == "Template");
		templateNode.Instantiate(templateNode.Templates.First());

		booleanNode.Value = true;
		fileNode.Value = new("test", [1, 2, 3]);
		numberNode.Value = 42;
		textNode.Value = "testText";
		timestampNode.Value = DateTime.Today;

		Assert.ThrowsException<FieldNotFoundException>(
			() => BooleanFieldValue("Boolean").EvaluateOn(collectionNode.Instances.First())
		);
		Assert.ThrowsException<FieldNotFoundException>(
			() => FileFieldFileName("File").EvaluateOn(collectionNode.Instances.First())
		);
		Assert.ThrowsException<FieldNotFoundException>(
			() => FileFieldFileContent("File").EvaluateOn(collectionNode.Instances.First())
		);
		Assert.ThrowsException<FieldNotFoundException>(
			() => NumberFieldValue("Number").EvaluateOn(collectionNode.Instances.First())
		);
		Assert.ThrowsException<FieldNotFoundException>(
			() => TextFieldValue("Text").EvaluateOn(collectionNode.Instances.First())
		);
		Assert.ThrowsException<FieldNotFoundException>(
			() => TimestampFieldValue("Timestamp").EvaluateOn(collectionNode.Instances.First())
		);

		Assert.ThrowsException<FieldNotFoundException>(
			() => BooleanFieldValue("Boolean").EvaluateOn(templateNode.Instance!)
		);
		Assert.ThrowsException<FieldNotFoundException>(
			() => FileFieldFileName("File").EvaluateOn(templateNode.Instance!)
		);
		Assert.ThrowsException<FieldNotFoundException>(
			() => FileFieldFileContent("File").EvaluateOn(templateNode.Instance!)
		);
		Assert.ThrowsException<FieldNotFoundException>(
			() => NumberFieldValue("Number").EvaluateOn(templateNode.Instance!)
		);
		Assert.ThrowsException<FieldNotFoundException>(() => TextFieldValue("Text").EvaluateOn(templateNode.Instance!));
		Assert.ThrowsException<FieldNotFoundException>(
			() => TimestampFieldValue("Timestamp").EvaluateOn(templateNode.Instance!)
		);
	}

	[TestMethod]
	public void FieldValues_ShouldAccessTemplatedSections()
	{
		var form = new FormBuilder("Test")
			.WithTemplatedSection(
				"Template",
				(node, _) =>
					node.UseTemplate(
						"SectionTemplate",
						template =>
							template
								.WithBooleanNode("Boolean")
								.WithFileNode("File")
								.WithNumberNode("Number")
								.WithTextNode("Text")
								.WithTimestampNode("Timestamp")
					)
			)
			.Build();

		var templateNode = (ITemplateNode)form.Nodes.First(node => node.Name == "Template");
		templateNode.Instantiate(templateNode.Templates.First());

		var booleanNode = (IBooleanNode)templateNode.Instance!.Nodes.First(node => node.Name == "Boolean");
		var fileNode = (IFileNode)templateNode.Instance!.Nodes.First(node => node.Name == "File");
		var numberNode = (INumberNode)templateNode.Instance!.Nodes.First(node => node.Name == "Number");
		var textNode = (ITextNode)templateNode.Instance!.Nodes.First(node => node.Name == "Text");
		var timestampNode = (ITimestampNode)templateNode.Instance!.Nodes.First(node => node.Name == "Timestamp");

		booleanNode.Value = true;
		fileNode.Value = new("test", [1, 2, 3]);
		numberNode.Value = 42;
		textNode.Value = "testText";
		timestampNode.Value = DateTime.Today;

		Assert.AreEqual(true, BooleanFieldValue("Boolean").EvaluateOn(form));
		Assert.AreEqual("test", FileFieldFileName("File").EvaluateOn(form));
		Assert.AreEqual(1, FileFieldFileContent("File").EvaluateOn(form)![0]);
		Assert.AreEqual(2, FileFieldFileContent("File").EvaluateOn(form)![1]);
		Assert.AreEqual(3, FileFieldFileContent("File").EvaluateOn(form)![2]);
		Assert.AreEqual(42, NumberFieldValue("Number").EvaluateOn(form));
		Assert.AreEqual("testText", TextFieldValue("Text").EvaluateOn(form));
		Assert.AreEqual(DateTime.Today, TimestampFieldValue("Timestamp").EvaluateOn(form));
	}

	[TestMethod]
	public void FieldValues_ShouldNotAccessCollections()
	{
		var form = new FormBuilder("Test")
			.WithCollectionNode(
				"Collection",
				(node, _) =>
					node.UseTemplate(
						"CollectionTemplate",
						template =>
							template
								.WithBooleanNode("Boolean")
								.WithFileNode("File")
								.WithNumberNode("Number")
								.WithTextNode("Text")
								.WithTimestampNode("Timestamp")
					)
			)
			.Build();

		var collectionNode = (ICollectionNode)form.Nodes.First(node => node.Name == "Collection");
		collectionNode.Instantiate(collectionNode.Templates.First());

		var booleanNode = (IBooleanNode)collectionNode.Instances.First().Nodes.First(node => node.Name == "Boolean");
		var fileNode = (IFileNode)collectionNode.Instances.First().Nodes.First(node => node.Name == "File");
		var numberNode = (INumberNode)collectionNode.Instances.First().Nodes.First(node => node.Name == "Number");
		var textNode = (ITextNode)collectionNode.Instances.First().Nodes.First(node => node.Name == "Text");
		var timestampNode = (ITimestampNode)
			collectionNode.Instances.First().Nodes.First(node => node.Name == "Timestamp");

		booleanNode.Value = true;
		fileNode.Value = new("test", [1, 2, 3]);
		numberNode.Value = 42;
		textNode.Value = "testText";
		timestampNode.Value = DateTime.Today;

		Assert.ThrowsException<FieldNotFoundException>(() => BooleanFieldValue("Boolean").EvaluateOn(form));
		Assert.ThrowsException<FieldNotFoundException>(() => FileFieldFileName("File").EvaluateOn(form));
		Assert.ThrowsException<FieldNotFoundException>(() => FileFieldFileContent("File").EvaluateOn(form));
		Assert.ThrowsException<FieldNotFoundException>(() => NumberFieldValue("Number").EvaluateOn(form));
		Assert.ThrowsException<FieldNotFoundException>(() => TextFieldValue("Text").EvaluateOn(form));
		Assert.ThrowsException<FieldNotFoundException>(() => TimestampFieldValue("Timestamp").EvaluateOn(form));
	}

	[TestMethod]
	public void FieldValues_ShouldPrioritizeHigherLayers()
	{
		var form = new FormBuilder("Test")
			.WithBooleanNode("Boolean")
			.WithFileNode("File")
			.WithNumberNode("Number")
			.WithTextNode("Text")
			.WithTimestampNode("Timestamp")
			.WithTemplatedSection(
				"Template",
				(node, recursiveTemplate) => node.UsePreconfiguredTemplate(recursiveTemplate)
			)
			.Build();

		var templateNode = (ITemplateNode)form.Nodes.First(node => node.Name == "Template");
		templateNode.Instantiate(templateNode.Templates.First());

		var booleanNode = (IBooleanNode)form.Nodes.First(node => node.Name == "Boolean");
		var fileNode = (IFileNode)form.Nodes.First(node => node.Name == "File");
		var numberNode = (INumberNode)form.Nodes.First(node => node.Name == "Number");
		var textNode = (ITextNode)form.Nodes.First(node => node.Name == "Text");
		var timestampNode = (ITimestampNode)form.Nodes.First(node => node.Name == "Timestamp");

		booleanNode.Value = true;
		fileNode.Value = new("test", [1, 2, 3]);
		numberNode.Value = 42;
		textNode.Value = "testText";
		timestampNode.Value = DateTime.Today;

		var instanceBooleanNode = (IBooleanNode)templateNode.Instance!.Nodes.First(node => node.Name == "Boolean");
		var instanceFileNode = (IFileNode)templateNode.Instance!.Nodes.First(node => node.Name == "File");
		var instanceNumberNode = (INumberNode)templateNode.Instance!.Nodes.First(node => node.Name == "Number");
		var instanceTextNode = (ITextNode)templateNode.Instance!.Nodes.First(node => node.Name == "Text");
		var instanceTimestampNode = (ITimestampNode)
			templateNode.Instance!.Nodes.First(node => node.Name == "Timestamp");

		instanceBooleanNode.Value = false;
		instanceFileNode.Value = new("instanceTest", [2, 3, 4]);
		instanceNumberNode.Value = 43;
		instanceTextNode.Value = "instanceTestText";
		instanceTimestampNode.Value = DateTime.Today.AddDays(1);

		// Even though evaluated on the template node, the expression should first search its neighbours and return their values.
		Assert.AreEqual(true, BooleanFieldValue("Boolean").EvaluateOn(templateNode));
		Assert.AreEqual("test", FileFieldFileName("File").EvaluateOn(templateNode));
		Assert.AreEqual(1, FileFieldFileContent("File").EvaluateOn(templateNode)![0]);
		Assert.AreEqual(2, FileFieldFileContent("File").EvaluateOn(templateNode)![1]);
		Assert.AreEqual(3, FileFieldFileContent("File").EvaluateOn(templateNode)![2]);
		Assert.AreEqual(42, NumberFieldValue("Number").EvaluateOn(templateNode));
		Assert.AreEqual("testText", TextFieldValue("Text").EvaluateOn(templateNode));
		Assert.AreEqual(DateTime.Today, TimestampFieldValue("Timestamp").EvaluateOn(templateNode));
	}

	# endregion
}
