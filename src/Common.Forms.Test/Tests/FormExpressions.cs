namespace RobinEpple.Common.Forms.Test.Tests;

using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Nodes;
using static RobinEpple.Common.Forms.Expressions.FormExpression;
using static RobinEpple.Common.Forms.Expressions.FormExpressionExtensions;

[TestClass]
public class FormExpressions
{
	private IEnumerable<TValue> _enumerate<TValue>(params TValue[] values) => values;

	# region utilities

	[TestMethod]
	public void StaticValue_ShouldReturnConfiguredValue()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsTrue(StaticValue(true).EvaluateOn(form));
		Assert.IsFalse(StaticValue(false).EvaluateOn(form));
		Assert.AreEqual(null, StaticValue<bool?>(null).EvaluateOn(form));
		Assert.AreEqual("testText", StaticValue("testText").EvaluateOn(form));
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
			"falseText",
			StaticValue(false).Conditional(StaticValue("trueText"), StaticValue("falseText")).EvaluateOn(form)
		);
	}

	[TestMethod]
	public void Single_Select_ShouldApplyToSourceValue()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsTrue(StaticValue<bool?>(true).Select(item => item ?? false).EvaluateOn(form));
		Assert.IsFalse(StaticValue<bool?>(null).Select(item => item ?? false).EvaluateOn(form));
	}

	[TestMethod]
	public void Multi_Select_ShouldTransformEachItem()
	{
		var form = new FormBuilder("Test").Build();

		var result = StaticValue(_enumerate<bool?>(true, null, false))
			.Select(item => item ?? false)
			.EvaluateOn(form)
			.ToList();

		Assert.AreEqual(true, result[0]);
		Assert.AreEqual(false, result[1]);
		Assert.AreEqual(false, result[2]);
	}

	[TestMethod]
	public void Contains_ShouldOnlyBeTrueIfItemContained()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsFalse(StaticValue(_enumerate(1, 2, 3)).Contains(StaticValue(0)).EvaluateOn(form));
		Assert.IsTrue(StaticValue(_enumerate(1, 2, 3)).Contains(StaticValue(1)).EvaluateOn(form));
		Assert.IsTrue(StaticValue(_enumerate(1, 2, 3)).Contains(StaticValue(2)).EvaluateOn(form));
		Assert.IsTrue(StaticValue(_enumerate(1, 2, 3)).Contains(StaticValue(3)).EvaluateOn(form));
		Assert.IsFalse(StaticValue(_enumerate(1, 2, 3)).Contains(StaticValue(4)).EvaluateOn(form));
	}

	[TestMethod]
	public void ContainsEqualityComparer_ShouldTakeEffect()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsFalse(
			StaticValue(_enumerate("Test")).Contains(StaticValue("test"), StringComparer.Ordinal).EvaluateOn(form)
		);
		Assert.IsTrue(
			StaticValue(_enumerate("Test"))
				.Contains(StaticValue("test"), StringComparer.OrdinalIgnoreCase)
				.EvaluateOn(form)
		);
	}

	# endregion

	# region logical operators

	[TestMethod]
	public void Not_ShouldInvertResult()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsTrue(Not(StaticValue(false)).EvaluateOn(form));
		Assert.IsFalse(Not(StaticValue(true)).EvaluateOn(form));
	}

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

		Assert.IsTrue(StaticValue(_enumerate(true, true, true)).All().EvaluateOn(form));
	}

	[TestMethod]
	public void All_ShouldBeFalseIfAnyValueNotTrue()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsFalse(StaticValue(_enumerate(false, true, true)).All().EvaluateOn(form));

		Assert.IsFalse(StaticValue(_enumerate(true, false, true)).All().EvaluateOn(form));

		Assert.IsFalse(StaticValue(_enumerate(true, true, false)).All().EvaluateOn(form));

		Assert.IsFalse(StaticValue(_enumerate(false, false, false)).All().EvaluateOn(form));
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

		Assert.IsTrue(StaticValue(_enumerate(true, true, true)).Any().EvaluateOn(form));

		Assert.IsTrue(StaticValue(_enumerate(true, false, false)).Any().EvaluateOn(form));

		Assert.IsTrue(StaticValue(_enumerate(false, true, false)).Any().EvaluateOn(form));

		Assert.IsTrue(StaticValue(_enumerate(false, false, true)).Any().EvaluateOn(form));
	}

	[TestMethod]
	public void Any_ShouldBeFalseIfAllValuesFalse()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsFalse(StaticValue(_enumerate(false, false, false)).Any().EvaluateOn(form));
	}

	# endregion

	# region field access

	[TestMethod]
	public void GetNode_ShouldAccessFieldsInForm()
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

		Assert.AreEqual(booleanNode, GetNode<IBooleanNode>("Boolean").EvaluateOn(form));
		Assert.AreEqual(fileNode, GetNode<IFileNode>("File").EvaluateOn(form));
		Assert.AreEqual(numberNode, GetNode<INumberNode>("Number").EvaluateOn(form));
		Assert.AreEqual(textNode, GetNode<ITextNode>("Text").EvaluateOn(form));
		Assert.AreEqual(timestampNode, GetNode<ITimestampNode>("Timestamp").EvaluateOn(form));
	}

	[TestMethod]
	public void GetNode_ShouldAccessNeighboringFields()
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

		Assert.AreEqual(booleanNode, GetNode<IBooleanNode>("Boolean").EvaluateOn(collectionNode));
		Assert.AreEqual(fileNode, GetNode<IFileNode>("File").EvaluateOn(collectionNode));
		Assert.AreEqual(numberNode, GetNode<INumberNode>("Number").EvaluateOn(collectionNode));
		Assert.AreEqual(textNode, GetNode<ITextNode>("Text").EvaluateOn(collectionNode));
		Assert.AreEqual(timestampNode, GetNode<ITimestampNode>("Timestamp").EvaluateOn(collectionNode));

		Assert.AreEqual(booleanNode, GetNode<IBooleanNode>("Boolean").EvaluateOn(templateNode));
		Assert.AreEqual(fileNode, GetNode<IFileNode>("File").EvaluateOn(templateNode));
		Assert.AreEqual(numberNode, GetNode<INumberNode>("Number").EvaluateOn(templateNode));
		Assert.AreEqual(textNode, GetNode<ITextNode>("Text").EvaluateOn(templateNode));
		Assert.AreEqual(timestampNode, GetNode<ITimestampNode>("Timestamp").EvaluateOn(templateNode));
	}

	[TestMethod]
	public void GetNode_ShouldNotAccessUpperScopes()
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
			() => GetNode<IBooleanNode>("Boolean").EvaluateOn(collectionNode.Instances.First())
		);
		Assert.ThrowsException<NodeNotFoundException>(
			() => GetNode<IFileNode>("File").EvaluateOn(collectionNode.Instances.First())
		);
		Assert.ThrowsException<NodeNotFoundException>(
			() => GetNode<INumberNode>("Number").EvaluateOn(collectionNode.Instances.First())
		);
		Assert.ThrowsException<NodeNotFoundException>(
			() => GetNode<ITextNode>("Text").EvaluateOn(collectionNode.Instances.First())
		);
		Assert.ThrowsException<NodeNotFoundException>(
			() => GetNode<ITimestampNode>("Timestamp").EvaluateOn(collectionNode.Instances.First())
		);

		Assert.ThrowsException<NodeNotFoundException>(
			() => GetNode<IBooleanNode>("Boolean").EvaluateOn(templateNode.Instance!)
		);
		Assert.ThrowsException<NodeNotFoundException>(
			() => GetNode<IFileNode>("File").EvaluateOn(templateNode.Instance!)
		);
		Assert.ThrowsException<NodeNotFoundException>(
			() => NumberFieldValue("Number").EvaluateOn(templateNode.Instance!)
		);
		Assert.ThrowsException<NodeNotFoundException>(
			() => GetNode<ITextNode>("Text").EvaluateOn(templateNode.Instance!)
		);
		Assert.ThrowsException<NodeNotFoundException>(
			() => GetNode<ITimestampNode>("Timestamp").EvaluateOn(templateNode.Instance!)
		);
	}

	[TestMethod]
	public void GetNode_ShouldAccessTemplatedSections()
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

		Assert.AreEqual(booleanNode, GetNode<IBooleanNode>("Boolean").EvaluateOn(form));
		Assert.AreEqual(fileNode, GetNode<IFileNode>("File").EvaluateOn(form));
		Assert.AreEqual(numberNode, GetNode<INumberNode>("Number").EvaluateOn(form));
		Assert.AreEqual(textNode, GetNode<ITextNode>("Text").EvaluateOn(form));
		Assert.AreEqual(timestampNode, GetNode<ITimestampNode>("Timestamp").EvaluateOn(form));
	}

	[TestMethod]
	public void GetNode_ShouldNotAccessCollections()
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

		Assert.ThrowsException<NodeNotFoundException>(() => GetNode<IBooleanNode>("Boolean").EvaluateOn(form));
		Assert.ThrowsException<NodeNotFoundException>(() => GetNode<IFileNode>("File").EvaluateOn(form));
		Assert.ThrowsException<NodeNotFoundException>(() => GetNode<INumberNode>("Number").EvaluateOn(form));
		Assert.ThrowsException<NodeNotFoundException>(() => GetNode<ITextNode>("Text").EvaluateOn(form));
		Assert.ThrowsException<NodeNotFoundException>(() => GetNode<ITimestampNode>("Timestamp").EvaluateOn(form));
	}

	[TestMethod]
	public void GetNode_ShouldPrioritizeHigherLayers()
	{
		var form = new FormBuilder("Test")
			.WithBooleanNode("Boolean")
			.WithFileNode("File")
			.WithNumberNode("Number")
			.WithTextNode("Text")
			.WithTimestampNode("Timestamp")
			.WithTemplatedSection(
				"Template",
				(node, recursiveTemplate) => node.UsePreConfiguredTemplate(recursiveTemplate)
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

		// Even though evaluated on the template node, the expression should first search its neighbors and return their values.
		Assert.AreEqual(booleanNode, GetNode<IBooleanNode>("Boolean").EvaluateOn(templateNode));
		Assert.AreEqual(fileNode, GetNode<IFileNode>("File").EvaluateOn(templateNode));
		Assert.AreEqual(numberNode, GetNode<INumberNode>("Number").EvaluateOn(templateNode));
		Assert.AreEqual(textNode, GetNode<ITextNode>("Text").EvaluateOn(templateNode));
		Assert.AreEqual(timestampNode, GetNode<ITimestampNode>("Timestamp").EvaluateOn(templateNode));
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
		Assert.AreEqual("Template", InTemplatedSection("Section", ScopeName()).EvaluateOn(templateNode));
	}

	[TestMethod]
	public void InSection_ShouldThrowIfNoInstanceExists()
	{
		var form = new FormBuilder("Test")
			.WithTemplatedSection("Section", (node, _) => node.UseTemplate("Template"))
			.Build();
		var templateNode = (ITemplateNode)form.Nodes.First(node => node.Name == "Section");

		Assert.ThrowsException<NodeNotFoundException>(
			() => InTemplatedSection("Section", ScopeName()).EvaluateOn(templateNode)
		);
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
	public void Elevate_ShouldMoveUpTheSpecifiedAmountOfScopes()
	{
		var form = new FormBuilder("Test")
			.WithBooleanNode("Boolean")
			.WithTemplatedSection(
				"Section",
				(node, _) =>
					node.UseTemplate(
						"Template",
						template =>
							template
								.WithTextNode("Text")
								.WithTemplatedSection("InnerSection", (node, _) => node.UseTemplate("InnerTemplate"))
					)
			)
			.Build();

		var booleanNode = (IBooleanNode)form.Nodes.First(node => node.Name == "Boolean");
		booleanNode.Value = true;
		var templateNode = (ITemplateNode)form.Nodes.First(node => node.Name == "Section");
		templateNode.Instantiate(templateNode.Templates.First());
		var textNode = (ITextNode)templateNode.Instance!.Nodes.First(node => node.Name == "Text");
		textNode.Value = "TestText";
		var innerTemplateNode = (ITemplateNode)templateNode.Instance!.Nodes.First(node => node.Name == "InnerSection");
		innerTemplateNode.Instantiate(innerTemplateNode.Templates.First());

		// Instance does see neither boolean nor text node in upper scopes.
		Assert.ThrowsException<NodeNotFoundException>(
			() => BooleanFieldValue("Boolean").EvaluateOn(innerTemplateNode.Instance!)
		);
		Assert.ThrowsException<NodeNotFoundException>(
			() => TextFieldValue("Text").EvaluateOn(innerTemplateNode.Instance!)
		);

		// Elevate should reject integers smaller than 1.
		Assert.ThrowsException<InvalidOperationException>(
			() => Elevate(StaticValue(0), BooleanFieldValue("Boolean")).EvaluateOn(innerTemplateNode.Instance!)
		);
		Assert.ThrowsException<InvalidOperationException>(
			() => Elevate(StaticValue(0), TextFieldValue("Text")).EvaluateOn(innerTemplateNode.Instance!)
		);

		// One scope up still no boolean node, but text node is visible.
		Assert.ThrowsException<NodeNotFoundException>(
			() => Elevate(StaticValue(1), BooleanFieldValue("Boolean")).EvaluateOn(innerTemplateNode.Instance!)
		);
		Assert.AreEqual(
			"TestText",
			Elevate(StaticValue(1), TextFieldValue("Text")).EvaluateOn(innerTemplateNode.Instance!)
		);

		// Two scopes up both are visible.
		Assert.AreEqual(
			true,
			Elevate(StaticValue(2), BooleanFieldValue("Boolean")).EvaluateOn(innerTemplateNode.Instance!)
		);
		Assert.AreEqual(
			"TestText",
			Elevate(StaticValue(2), TextFieldValue("Text")).EvaluateOn(innerTemplateNode.Instance!)
		);

		// There is no parent above the root.
		Assert.ThrowsException<NodeNotFoundException>(
			() => Elevate(StaticValue(3), BooleanFieldValue("Boolean")).EvaluateOn(innerTemplateNode.Instance!)
		);
		Assert.ThrowsException<NodeNotFoundException>(
			() => Elevate(StaticValue(3), TextFieldValue("Text")).EvaluateOn(innerTemplateNode.Instance!)
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

	# region comparisons

	[TestMethod]
	public void SmallerThan_ShouldReturnTrueOnlyIfSmaller()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsTrue(StaticValue(0).SmallerThan(StaticValue(1)).EvaluateOn(form));
		Assert.IsFalse(StaticValue(1).SmallerThan(StaticValue(1)).EvaluateOn(form));
		Assert.IsFalse(StaticValue(2).SmallerThan(StaticValue(1)).EvaluateOn(form));
	}

	[TestMethod]
	public void SmallerOrEqual_ShouldReturnTrueIfSmallerOrEqual()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsTrue(StaticValue(0).SmallerOrEqual(StaticValue(1)).EvaluateOn(form));
		Assert.IsTrue(StaticValue(1).SmallerOrEqual(StaticValue(1)).EvaluateOn(form));
		Assert.IsFalse(StaticValue(2).SmallerOrEqual(StaticValue(1)).EvaluateOn(form));
	}

	[TestMethod]
	public void EqualTo_ShouldReturnTrueOnlyIfEqual()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsFalse(StaticValue(0).EqualTo(StaticValue(1)).EvaluateOn(form));
		Assert.IsTrue(StaticValue(1).EqualTo(StaticValue(1)).EvaluateOn(form));
		Assert.IsFalse(StaticValue(2).EqualTo(StaticValue(1)).EvaluateOn(form));
	}

	[TestMethod]
	public void BiggerOrEqual_ShouldReturnTrueIfBiggerOrEqual()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsFalse(StaticValue(0).BiggerOrEqual(StaticValue(1)).EvaluateOn(form));
		Assert.IsTrue(StaticValue(1).BiggerOrEqual(StaticValue(1)).EvaluateOn(form));
		Assert.IsTrue(StaticValue(2).BiggerOrEqual(StaticValue(1)).EvaluateOn(form));
	}

	[TestMethod]
	public void BiggerThan_ShouldReturnTrueOnlyIfBigger()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsFalse(StaticValue(0).BiggerThan(StaticValue(1)).EvaluateOn(form));
		Assert.IsFalse(StaticValue(1).BiggerThan(StaticValue(1)).EvaluateOn(form));
		Assert.IsTrue(StaticValue(2).BiggerThan(StaticValue(1)).EvaluateOn(form));
	}

	[TestMethod]
	public void Min_ShouldReturnSmallestElement()
	{
		var form = new FormBuilder("Test").Build();

		Assert.AreEqual(1, StaticValue(_enumerate(1, 2, 1, 3)).Min().EvaluateOn(form));
		Assert.AreEqual(2, StaticValue(_enumerate(5, 2, 3)).Min().EvaluateOn(form));
		Assert.AreEqual(5, StaticValue(_enumerate(5, 256, 10)).Min().EvaluateOn(form));

		Assert.AreEqual(
			DateTime.Today.AddDays(-1),
			StaticValue(_enumerate(DateTime.Today, DateTime.Today.AddDays(-1), DateTime.Today.AddDays(1)))
				.Min()
				.EvaluateOn(form)
		);
	}

	[TestMethod]
	public void Max_ShouldReturnBiggestElement()
	{
		var form = new FormBuilder("Test").Build();

		Assert.AreEqual(3, StaticValue(_enumerate(1, 2, 1, 3)).Max().EvaluateOn(form));
		Assert.AreEqual(5, StaticValue(_enumerate(5, 2, 3)).Max().EvaluateOn(form));
		Assert.AreEqual(256, StaticValue(_enumerate(5, 256, 10)).Max().EvaluateOn(form));

		Assert.AreEqual(
			DateTime.Today.AddDays(1),
			StaticValue(_enumerate(DateTime.Today, DateTime.Today.AddDays(-1), DateTime.Today.AddDays(1)))
				.Max()
				.EvaluateOn(form)
		);
	}

	[TestMethod]
	public void Median_ShouldReturnMiddleElement()
	{
		var form = new FormBuilder("Test").Build();

		Assert.AreEqual(2, StaticValue(_enumerate(1, 2, 1, 3)).Median().EvaluateOn(form));
		Assert.AreEqual(1, StaticValue(_enumerate(1, 2, 1, 3)).Median(true).EvaluateOn(form));
		Assert.AreEqual(3, StaticValue(_enumerate(5, 2, 3)).Median().EvaluateOn(form));
		Assert.AreEqual(3, StaticValue(_enumerate(5, 2, 3)).Median(true).EvaluateOn(form));
		Assert.AreEqual(10, StaticValue(_enumerate(5, 256, 10)).Median().EvaluateOn(form));

		Assert.AreEqual(
			DateTime.Today,
			StaticValue(_enumerate(DateTime.Today, DateTime.Today.AddDays(-1), DateTime.Today.AddDays(1)))
				.Median()
				.EvaluateOn(form)
		);
	}

	# endregion

	# region number calculations

	[TestMethod]
	public void Number_Add_ShouldReturnSumOfTwoValues()
	{
		var form = new FormBuilder("Test").Build();

		Assert.AreEqual(8m, StaticValue(3m).Add(StaticValue(5m)).EvaluateOn(form));
		Assert.AreEqual(10.275m, StaticValue(3.175m).Add(StaticValue(7.1m)).EvaluateOn(form));
		Assert.AreEqual(-2m, StaticValue(5m).Add(StaticValue(-7m)).EvaluateOn(form));
	}

	[TestMethod]
	public void Number_Sum_ShouldReturnSumOfAllValues()
	{
		var form = new FormBuilder("Test").Build();

		Assert.AreEqual(8m, StaticValue(_enumerate(1m, 2m, 5m)).Sum().EvaluateOn(form));
		Assert.AreEqual(-4m, StaticValue(_enumerate(1m, -10m, 5m)).Sum().EvaluateOn(form));
	}

	[TestMethod]
	public void Number_Subtract_ShouldReturnRemainder()
	{
		var form = new FormBuilder("Test").Build();

		Assert.AreEqual(-2m, StaticValue(3m).Subtract(StaticValue(5m)).EvaluateOn(form));
		Assert.AreEqual(-3.925m, StaticValue(3.175m).Subtract(StaticValue(7.1m)).EvaluateOn(form));
		Assert.AreEqual(12m, StaticValue(5m).Subtract(StaticValue(-7m)).EvaluateOn(form));
	}

	[TestMethod]
	public void Number_MultiplyBy_ShouldMultiplyTwoValues()
	{
		var form = new FormBuilder("Test").Build();

		Assert.AreEqual(4m, StaticValue(2m).MultiplyBy(StaticValue(2m)).EvaluateOn(form));
		Assert.AreEqual(-5m, StaticValue(5m).MultiplyBy(StaticValue(-1m)).EvaluateOn(form));
		Assert.AreEqual(1m, StaticValue(2m).MultiplyBy(StaticValue(1m / 2m)).EvaluateOn(form));
	}

	[TestMethod]
	public void Number_Multiply_ShouldMultiplyAllValues()
	{
		var form = new FormBuilder("Test").Build();

		Assert.AreEqual(10m, StaticValue(_enumerate(1m, 2m, 5m)).Multiply().EvaluateOn(form));
		Assert.AreEqual(-10m, StaticValue(_enumerate(2m, -10m, 1m / 2m)).Multiply().EvaluateOn(form));
	}

	[TestMethod]
	public void Number_DivideBy_ShouldDivideBySecondValue()
	{
		var form = new FormBuilder("Test").Build();

		Assert.AreEqual(1m, StaticValue(2m).DivideBy(StaticValue(2m)).EvaluateOn(form));
		Assert.AreEqual(-5m, StaticValue(5m).DivideBy(StaticValue(-1m)).EvaluateOn(form));
		Assert.AreEqual(4m, StaticValue(2m).DivideBy(StaticValue(1m / 2m)).EvaluateOn(form));
	}

	[TestMethod]
	public void Number_CastInt_ShouldStripDigitsAfterComma()
	{
		var form = new FormBuilder("Test").Build();

		Assert.AreEqual(2, StaticValue(2m).CastInt().EvaluateOn(form));
		Assert.AreEqual(2, StaticValue(2.1m).CastInt().EvaluateOn(form));
		Assert.AreEqual(2, StaticValue(2.9m).CastInt().EvaluateOn(form));
		Assert.AreEqual(-3, StaticValue(-3.9m).CastInt().EvaluateOn(form));
	}

	[TestMethod]
	public void Number_CastDecimal_ShouldReturnDecimalRepresentation()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsTrue(StaticValue(2).CastDecimal().EvaluateOn(form).GetType() == typeof(decimal));
	}

	[TestMethod]
	public void Number_Ceil_ShouldReturnNextBiggerInt()
	{
		var form = new FormBuilder("Test").Build();

		Assert.AreEqual(2m, StaticValue(2m).Ceil().EvaluateOn(form));
		Assert.AreEqual(3m, StaticValue(2.1m).Ceil().EvaluateOn(form));
		Assert.AreEqual(3m, StaticValue(2.9m).Ceil().EvaluateOn(form));
		Assert.AreEqual(-3m, StaticValue(-3.9m).Ceil().EvaluateOn(form));
	}

	[TestMethod]
	public void Number_Floor_ShouldReturnNextSmallerInt()
	{
		var form = new FormBuilder("Test").Build();

		Assert.AreEqual(2m, StaticValue(2m).Floor().EvaluateOn(form));
		Assert.AreEqual(2m, StaticValue(2.1m).Floor().EvaluateOn(form));
		Assert.AreEqual(2m, StaticValue(2.9m).Floor().EvaluateOn(form));
		Assert.AreEqual(-4m, StaticValue(-3.9m).Floor().EvaluateOn(form));
	}

	[TestMethod]
	public void Number_Round_ShouldReturnClosestInt()
	{
		var form = new FormBuilder("Test").Build();

		Assert.AreEqual(2m, StaticValue(2m).Round().EvaluateOn(form));
		Assert.AreEqual(2m, StaticValue(2.1m).Round().EvaluateOn(form));
		Assert.AreEqual(3m, StaticValue(2.9m).Round().EvaluateOn(form));
		Assert.AreEqual(-4m, StaticValue(-3.9m).Round().EvaluateOn(form));
	}

	[TestMethod]
	public void Number_Modulo_ShouldTrimToField()
	{
		var form = new FormBuilder("Test").Build();

		Assert.AreEqual(2, StaticValue(12).Modulo(StaticValue(10)).EvaluateOn(form));
		Assert.AreEqual(0, StaticValue(12).Modulo(StaticValue(3)).EvaluateOn(form));
		Assert.AreEqual(2, StaticValue(12).Modulo(StaticValue(5)).EvaluateOn(form));

		// The deviation of mod in C# differs from the mathematical definition of allowing only
		// integer values (positive). It is apparently a "truncation-based remainder operation"
		// and the expression should stick to the % operator we are used to from programming.
		Assert.AreEqual(-2m, StaticValue(-12).Modulo(StaticValue(10)).EvaluateOn(form));
	}

	[TestMethod]
	public void Number_Average_ShouldComputeAverageWithDecimalPrecision()
	{
		var form = new FormBuilder("Test").Build();

		Assert.AreEqual(2m, StaticValue(_enumerate(1m, 3m)).Average().EvaluateOn(form));
		Assert.AreEqual(2m, StaticValue(_enumerate(1m, 2m, 3m)).Average().EvaluateOn(form));
		Assert.AreEqual(1.5m, StaticValue(_enumerate(1m, 2m)).Average().EvaluateOn(form));
	}

	# endregion

	# region date calculations

	[TestMethod]
	public void Date_Add_JumpsByGivenTimeSpan()
	{
		var form = new FormBuilder("Test").Build();

		Assert.AreEqual(
			DateTime.Today.AddDays(1),
			StaticValue(DateTime.Today).Add(StaticValue(TimeSpan.FromDays(1))).EvaluateOn(form)
		);
		Assert.AreEqual(
			DateTime.Today.AddDays(-1),
			StaticValue(DateTime.Today).Add(StaticValue(TimeSpan.FromDays(-1))).EvaluateOn(form)
		);
		Assert.AreEqual(
			DateTime.Today.AddHours(1),
			StaticValue(DateTime.Today).Add(StaticValue(TimeSpan.FromHours(1))).EvaluateOn(form)
		);
	}

	[TestMethod]
	public void Date_Subtract_JumpsByGivenTimeSpan()
	{
		var form = new FormBuilder("Test").Build();

		Assert.AreEqual(
			DateTime.Today.AddDays(-1),
			StaticValue(DateTime.Today).Subtract(StaticValue(TimeSpan.FromDays(1))).EvaluateOn(form)
		);
		Assert.AreEqual(
			DateTime.Today.AddDays(1),
			StaticValue(DateTime.Today).Subtract(StaticValue(TimeSpan.FromDays(-1))).EvaluateOn(form)
		);
		Assert.AreEqual(
			DateTime.Today.AddHours(-1),
			StaticValue(DateTime.Today).Subtract(StaticValue(TimeSpan.FromHours(1))).EvaluateOn(form)
		);
	}

	[TestMethod]
	public void Date_Difference_ReturnsTimeSpanBetween()
	{
		var form = new FormBuilder("Test").Build();

		Assert.AreEqual(
			TimeSpan.FromHours(2),
			StaticValue(DateTime.Today).Difference(StaticValue(DateTime.Today.AddHours(2))).EvaluateOn(form)
		);

		Assert.AreEqual(
			TimeSpan.FromHours(-2),
			StaticValue(DateTime.Today).Difference(StaticValue(DateTime.Today.AddHours(-2))).EvaluateOn(form)
		);

		Assert.AreEqual(
			TimeSpan.FromDays(-2),
			StaticValue(DateTime.Today).Difference(StaticValue(DateTime.Today.AddDays(-2))).EvaluateOn(form)
		);
	}

	[TestMethod]
	public void TimeSpan_MultiplyBy_IncreasesByFactor()
	{
		var form = new FormBuilder("Test").Build();

		Assert.AreEqual(
			TimeSpan.FromHours(5),
			StaticValue(TimeSpan.FromHours(1)).MultiplyBy(StaticValue(5m)).EvaluateOn(form)
		);

		Assert.AreEqual(
			TimeSpan.FromHours(-5),
			StaticValue(TimeSpan.FromHours(1)).MultiplyBy(StaticValue(-5m)).EvaluateOn(form)
		);

		Assert.AreEqual(
			TimeSpan.FromDays(1 / 5d),
			StaticValue(TimeSpan.FromDays(1)).MultiplyBy(StaticValue(1 / 5m)).EvaluateOn(form)
		);
	}

	[TestMethod]
	public void TimeSpan_DivideBy_DecreasesByFactor()
	{
		var form = new FormBuilder("Test").Build();

		Assert.AreEqual(
			TimeSpan.FromHours(1 / 5d),
			StaticValue(TimeSpan.FromHours(1)).DivideBy(StaticValue(5m)).EvaluateOn(form)
		);

		Assert.AreEqual(
			TimeSpan.FromHours(-1 / 5d),
			StaticValue(TimeSpan.FromHours(1)).DivideBy(StaticValue(-5m)).EvaluateOn(form)
		);

		Assert.AreEqual(
			TimeSpan.FromDays(5),
			StaticValue(TimeSpan.FromDays(1)).DivideBy(StaticValue(1 / 5m)).EvaluateOn(form)
		);
	}

	# endregion
}
