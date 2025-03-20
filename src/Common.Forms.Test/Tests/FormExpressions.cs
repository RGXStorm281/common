namespace RobinEpple.Common.Forms.Test.Tests;

using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Nodes;
using static RobinEpple.Common.Forms.Expressions.FormExpression;

[TestClass]
public class FormExpressions
{
	private IEnumerable<TValue> Enumerate<TValue>(params TValue[] values) => values;

	# region utilities

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
	public void OnNotFound_ShouldReturnExpressionValueIfNoException()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsTrue(StaticValue(true).OnNotFound(StaticValue(false)).EvaluateOn(form));
	}

	[TestMethod]
	public void OnNotFound_ShouldReturnFallbackValueIfNodeNotFoundExceptionIsThrown()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsFalse(Throw<bool>(_ => new NodeNotFoundException()).OnNotFound(StaticValue(false)).EvaluateOn(form));
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

	[TestMethod]
	public void Select_ShouldTransformEachItem()
	{
		var form = new FormBuilder("Test").Build();

		var result = StaticValue(Enumerate<bool?>(true, null, false))
			.Select(item => item.Coalesce(false))
			.EvaluateOn(form)
			.ToList();

		Assert.AreEqual(true, result[0]);
		Assert.AreEqual(false, result[1]);
		Assert.AreEqual(false, result[2]);
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

		Assert.IsTrue(StaticValue(Enumerate(true, true, true)).All().EvaluateOn(form));
	}

	[TestMethod]
	public void All_ShouldBeFalseIfAnyValueNotTrue()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsFalse(StaticValue(Enumerate(false, true, true)).All().EvaluateOn(form));

		Assert.IsFalse(StaticValue(Enumerate(true, false, true)).All().EvaluateOn(form));

		Assert.IsFalse(StaticValue(Enumerate(true, true, false)).All().EvaluateOn(form));

		Assert.IsFalse(StaticValue(Enumerate(false, false, false)).All().EvaluateOn(form));
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

		Assert.IsTrue(StaticValue(Enumerate(true, true, true)).Any().EvaluateOn(form));

		Assert.IsTrue(StaticValue(Enumerate(true, false, false)).Any().EvaluateOn(form));

		Assert.IsTrue(StaticValue(Enumerate(false, true, false)).Any().EvaluateOn(form));

		Assert.IsTrue(StaticValue(Enumerate(false, false, true)).Any().EvaluateOn(form));
	}

	[TestMethod]
	public void Any_ShouldBeFalseIfAllValuesFalse()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsFalse(StaticValue(Enumerate(false, false, false)).Any().EvaluateOn(form));
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

		Assert.ThrowsException<NodeNotFoundException>(
			() => BooleanFieldValue("Boolean").EvaluateOn(collectionNode.Instances.First())
		);
		Assert.ThrowsException<NodeNotFoundException>(
			() => FileFieldFileName("File").EvaluateOn(collectionNode.Instances.First())
		);
		Assert.ThrowsException<NodeNotFoundException>(
			() => FileFieldFileContent("File").EvaluateOn(collectionNode.Instances.First())
		);
		Assert.ThrowsException<NodeNotFoundException>(
			() => NumberFieldValue("Number").EvaluateOn(collectionNode.Instances.First())
		);
		Assert.ThrowsException<NodeNotFoundException>(
			() => TextFieldValue("Text").EvaluateOn(collectionNode.Instances.First())
		);
		Assert.ThrowsException<NodeNotFoundException>(
			() => TimestampFieldValue("Timestamp").EvaluateOn(collectionNode.Instances.First())
		);

		Assert.ThrowsException<NodeNotFoundException>(
			() => BooleanFieldValue("Boolean").EvaluateOn(templateNode.Instance!)
		);
		Assert.ThrowsException<NodeNotFoundException>(
			() => FileFieldFileName("File").EvaluateOn(templateNode.Instance!)
		);
		Assert.ThrowsException<NodeNotFoundException>(
			() => FileFieldFileContent("File").EvaluateOn(templateNode.Instance!)
		);
		Assert.ThrowsException<NodeNotFoundException>(
			() => NumberFieldValue("Number").EvaluateOn(templateNode.Instance!)
		);
		Assert.ThrowsException<NodeNotFoundException>(() => TextFieldValue("Text").EvaluateOn(templateNode.Instance!));
		Assert.ThrowsException<NodeNotFoundException>(
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

		Assert.ThrowsException<NodeNotFoundException>(() => BooleanFieldValue("Boolean").EvaluateOn(form));
		Assert.ThrowsException<NodeNotFoundException>(() => FileFieldFileName("File").EvaluateOn(form));
		Assert.ThrowsException<NodeNotFoundException>(() => FileFieldFileContent("File").EvaluateOn(form));
		Assert.ThrowsException<NodeNotFoundException>(() => NumberFieldValue("Number").EvaluateOn(form));
		Assert.ThrowsException<NodeNotFoundException>(() => TextFieldValue("Text").EvaluateOn(form));
		Assert.ThrowsException<NodeNotFoundException>(() => TimestampFieldValue("Timestamp").EvaluateOn(form));
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

	# region change of scope

	[TestMethod]
	public void ScopeName_ShouldReturnNameOfClosestForm()
	{
		var form = new FormBuilder("Test")
			.WithTemplatedSection("Section", (node, _) => node.UseTemplate("Template"))
			.Build();
		var templateNode = (ITemplateNode)form.Nodes.First(node => node.Name == "Section");
		templateNode.Instantiate(templateNode.Templates.First());

		Assert.AreEqual("Test", ScopeName().EvaluateOn(form));
		Assert.AreEqual("Test", ScopeName().EvaluateOn(templateNode));
		Assert.AreEqual("Template", ScopeName().EvaluateOn(templateNode.Instance!));
	}

	[TestMethod]
	public void InSection_ShouldSwitchScopeToInstanceInTemplatedSection()
	{
		var form = new FormBuilder("Test")
			.WithTemplatedSection("Section", (node, _) => node.UseTemplate("Template"))
			.Build();
		var templateNode = (ITemplateNode)form.Nodes.First(node => node.Name == "Section");
		templateNode.Instantiate(templateNode.Templates.First());

		Assert.AreEqual("Test", ScopeName().EvaluateOn(form));
		Assert.AreEqual("Template", InSection("Section", ScopeName()).EvaluateOn(templateNode));
	}

	[TestMethod]
	public void InSection_ShouldThrowIfNoInstanceExists()
	{
		var form = new FormBuilder("Test")
			.WithTemplatedSection("Section", (node, _) => node.UseTemplate("Template"))
			.Build();
		var templateNode = (ITemplateNode)form.Nodes.First(node => node.Name == "Section");

		Assert.ThrowsException<NodeNotFoundException>(() => InSection("Section", ScopeName()).EvaluateOn(templateNode));
	}

	[TestMethod]
	public void ForEachCollectionItem_ShouldReturnEmptyListIfNoItems()
	{
		var form = new FormBuilder("Test")
			.WithCollectionNode(
				"Collection",
				(node, _) => node.UseTemplate("Template", template => template.WithBooleanNode("ItemBool"))
			)
			.Build();
		var collectionNode = (ICollectionNode)form.Nodes.First(node => node.Name == "Collection");

		Assert.AreEqual(0, ForEachCollectionItem("Collection", BooleanFieldValue("ItemBool")).EvaluateOn(form).Count());
	}

	[TestMethod]
	public void ForEachCollectionItem_ShouldExecuteTheExpressionForEachItem()
	{
		var form = new FormBuilder("Test")
			.WithCollectionNode(
				"Collection",
				(node, _) => node.UseTemplate("Template", template => template.WithBooleanNode("ItemBool"))
			)
			.Build();
		var collectionNode = (ICollectionNode)form.Nodes.First(node => node.Name == "Collection");
		collectionNode.Instantiate(collectionNode.Templates.First());
		collectionNode.Instantiate(collectionNode.Templates.First());

		var instance1 = collectionNode.Instances.ElementAt(0);
		var firstBool = (IBooleanNode)instance1.Nodes.First(node => node.Name == "ItemBool");
		firstBool.Value = false;

		var instance2 = collectionNode.Instances.ElementAt(1);
		var secondBool = (IBooleanNode)instance2.Nodes.First(node => node.Name == "ItemBool");
		secondBool.Value = true;

		var expressionResult = ForEachCollectionItem("Collection", BooleanFieldValue("ItemBool"))
			.EvaluateOn(form)
			.ToList();

		Assert.AreEqual(false, expressionResult[0]);
		Assert.AreEqual(true, expressionResult[1]);
	}

	[TestMethod]
	public void InParentScope_ShouldMoveUpTheSpecifiedAmountOfLayers()
	{
		var form = new FormBuilder("Test")
			.WithBooleanNode("Boolean")
			.WithTemplatedSection("Section", (node, _) => node.UseTemplate("Template"))
			.Build();

		var booleanNode = (IBooleanNode)form.Nodes.First(node => node.Name == "Boolean");
		booleanNode.Value = true;
		var templateNode = (ITemplateNode)form.Nodes.First(node => node.Name == "Section");
		templateNode.Instantiate(templateNode.Templates.First());

		// Instance does not see boolean node in upper scope.
		Assert.ThrowsException<NodeNotFoundException>(
			() => BooleanFieldValue("Boolean").EvaluateOn(templateNode.Instance!)
		);

		// Template Node does see its neighbor.
		Assert.AreEqual(true, InParentScope(0, BooleanFieldValue("Boolean")).EvaluateOn(templateNode.Instance!));

		// Root form does see its child.
		Assert.AreEqual(true, InParentScope(1, BooleanFieldValue("Boolean")).EvaluateOn(templateNode.Instance!));

		// There is no parent above the root.
		Assert.ThrowsException<NodeNotFoundException>(
			() => InParentScope(2, BooleanFieldValue("Boolean")).EvaluateOn(templateNode.Instance!)
		);
	}

	[TestMethod]
	public void InRootScope_ShouldExecuteOnRootNode()
	{
		var form = new FormBuilder("Test")
			.WithBooleanNode("Boolean")
			.WithTemplatedSection("Section", (node, _) => node.UseTemplate("Template"))
			.Build();

		var booleanNode = (IBooleanNode)form.Nodes.First(node => node.Name == "Boolean");
		booleanNode.Value = true;
		var templateNode = (ITemplateNode)form.Nodes.First(node => node.Name == "Section");
		templateNode.Instantiate(templateNode.Templates.First());

		// Instance does not see boolean node in upper scope.
		Assert.ThrowsException<NodeNotFoundException>(
			() => BooleanFieldValue("Boolean").EvaluateOn(templateNode.Instance!)
		);

		// Root form does see its child.
		Assert.AreEqual(true, InRootScope(BooleanFieldValue("Boolean")).EvaluateOn(templateNode.Instance!));
	}

	# endregion
}
