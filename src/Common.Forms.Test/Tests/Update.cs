namespace RobinEpple.Common.Forms.Test.Tests;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.Test.Mocks;

[TestClass]
public class Update
{
	[TestMethod]
	public void Update_ShouldResetToDefaultReadonly()
	{
		var form = new FormBuilder("Test").UseDefaultReadonly(true).Build();
		form.Update();
		Assert.IsTrue(form.IsVisible);

		form = new FormBuilder("Test").UseDefaultReadonly(false).Build();
		form.Update();
		Assert.IsFalse(form.IsReadonly);
	}

	[TestMethod]
	public void Update_ReadonlyCondition_ShouldOverwriteDefaultReadonly()
	{
		var form = new FormBuilder("Test")
			.UseDefaultReadonly(true)
			.UseReadonlyCondition(new FalseMockCondition())
			.Build();
		form.Update();
		Assert.IsFalse(form.IsReadonly);
	}

	[TestMethod]
	public void Update_ParentReadonly_ShouldSetChildReadonly()
	{
		var form = new FormBuilder("Test")
			.UseDefaultReadonly(false)
			.UseReadonlyCondition(new TrueMockCondition())
			.WithBooleanNode("BooleanNode", node => node.UseDefaultReadonly(false))
			.Build();

		var booleanNode = form.Nodes.First(node => node.Name == "BooleanNode");

		form.Update();

		Assert.IsTrue(form.IsReadonly);
		Assert.IsTrue(booleanNode.IsReadonly);
	}

	[TestMethod]
	public void Update_ParentReadonly_ShouldOverruleReadonlyCondition()
	{
		var form = new FormBuilder("Test")
			.UseDefaultReadonly(true)
			.WithBooleanNode(
				"BooleanNode",
				node => node.UseDefaultReadonly(false).UseReadonlyCondition(new FalseMockCondition())
			)
			.Build();

		var booleanNode = form.Nodes.First(node => node.Name == "BooleanNode");

		form.Update();

		Assert.IsTrue(form.IsReadonly);
		Assert.IsTrue(booleanNode.IsReadonly);
	}

	[TestMethod]
	public void Update_ShouldAlwaysExecuteReadonlyExtensionEvents()
	{
		var mockExtension = new MockExtension();
		var form = new FormBuilder("Test")
			.UseDefaultReadonly(true)
			.UseReadonlyCondition(new FalseMockCondition())
			.UseExtension(mockExtension)
			.Build();
		form.Update();

		Assert.IsTrue(mockExtension.OnBeforeReadonlyStateEvaluationHasBeenCalled);
		Assert.IsTrue(mockExtension.OnBeforeReadonlyStateEvaluationReadonlyValue);
		Assert.IsTrue(mockExtension.OnAfterReadonlyStateEvaluationHasBeenCalled);
		Assert.IsFalse(mockExtension.OnAfterReadonlyStateEvaluationReadonlyValue);
	}

	[TestMethod]
	public void Update_ShouldResetToDefaultVisibility()
	{
		var form = new FormBuilder("Test").UseDefaultVisibility(true).Build();
		form.Update();
		Assert.IsTrue(form.IsVisible);

		form = new FormBuilder("Test").UseDefaultVisibility(false).Build();
		form.Update();
		Assert.IsFalse(form.IsVisible);
	}

	[TestMethod]
	public void Update_VisibilityCondition_ShouldOverwriteDefaultVisibility()
	{
		var form = new FormBuilder("Test")
			.UseDefaultVisibility(true)
			.UseVisibilityCondition(new FalseMockCondition())
			.Build();
		form.Update();
		Assert.IsFalse(form.IsVisible);
	}

	[TestMethod]
	public void Update_ParentInvisible_ShouldSetChildInvisible()
	{
		var form = new FormBuilder("Test")
			.UseDefaultVisibility(true)
			.UseVisibilityCondition(new FalseMockCondition())
			.WithBooleanNode("BooleanNode", node => node.UseDefaultVisibility(true))
			.Build();

		var booleanNode = form.Nodes.First(node => node.Name == "BooleanNode");

		form.Update();
		Assert.IsFalse(form.IsVisible);
		Assert.IsFalse(booleanNode.IsVisible);
	}

	[TestMethod]
	public void Update_ParentInvisible_ShouldOverruleInvisibilityCondition()
	{
		var form = new FormBuilder("Test")
			.UseDefaultVisibility(false)
			.WithBooleanNode(
				"BooleanNode",
				node => node.UseDefaultVisibility(true).UseVisibilityCondition(new TrueMockCondition())
			)
			.Build();

		var booleanNode = form.Nodes.First(node => node.Name == "BooleanNode");

		form.Update();
		Assert.IsFalse(form.IsVisible);
		Assert.IsFalse(booleanNode.IsVisible);
	}

	[TestMethod]
	public void Update_ShouldAlwaysExecuteVisibilityExtensionEvents()
	{
		var mockExtension = new MockExtension();
		var form = new FormBuilder("Test")
			.UseDefaultVisibility(true)
			.UseVisibilityCondition(new FalseMockCondition())
			.UseExtension(mockExtension)
			.Build();

		form.Update();
		Assert.IsTrue(mockExtension.OnBeforeVisibilityEvaluationHasBeenCalled);
		Assert.IsTrue(mockExtension.OnBeforeVisibilityEvaluationVisibilityValue);
		Assert.IsTrue(mockExtension.OnAfterVisibilityEvaluationHasBeenCalled);
		Assert.IsFalse(mockExtension.OnAfterVisibilityEvaluationVisibilityValue);
	}

	[TestMethod]
	public void Update_ShouldNotExecuteValidationIfNotVisible()
	{
		var mockExtension = new MockExtension();
		var mockValidator = new InvalidMockValidator();
		var form = new FormBuilder("Test")
			.UseDefaultVisibility(false)
			.UseExtension(mockExtension)
			.UseValidator(mockValidator)
			.Build();

		form.Update();
		Assert.IsFalse(mockExtension.OnBeforeValidationHasBeenCalled);
		Assert.IsFalse(mockValidator.HasBeenCalled);
		Assert.IsFalse(mockExtension.OnAfterValidationHasBeenCalled);
	}

	[TestMethod]
	public void Update_ShouldSetValidIfInvisible()
	{
		var form = new FormBuilder("Test").UseDefaultVisibility(false).UseValidator(new InvalidMockValidator()).Build();

		form.Update();
		Assert.IsTrue(form.IsValid);
	}

	[TestMethod]
	public void Update_ShouldSetValidIfNoErrors()
	{
		var form = new FormBuilder("Test").Build();
		form.Update();
		Assert.IsTrue(form.IsValid);
	}

	[TestMethod]
	public void Update_ShouldSetInvalidIfErrors()
	{
		var form = new FormBuilder("Test").UseValidator(new InvalidMockValidator()).Build();
		form.Update();
		Assert.IsFalse(form.IsValid);
	}

	[TestMethod]
	public void Update_InvalidChildren_ShouldSetContainerInvalid()
	{
		var form = new FormBuilder("Test")
			.WithTemplatedSection(
				"Template",
				(node, _) =>
					node.UseTemplate(
						"SectionTemplate",
						template =>
						{
							template.WithTextNode("SectionText", node => node.UseValidator(new InvalidMockValidator()));
						}
					)
			)
			.WithCollectionNode(
				"Collection",
				(node, _) =>
					node.UseTemplate(
						"CollectionTemplate",
						template =>
						{
							template.WithTextNode(
								"CollectionText",
								node => node.UseValidator(new InvalidMockValidator())
							);
						}
					)
			)
			.Build();

		var templateNode = (ITemplateNode)form.Nodes.First(node => node.Name == "Template");
		var sectionTemplate = templateNode.Templates.First();
		templateNode.Instantiate(sectionTemplate);
		var sectionInstance = templateNode.Instance!;
		var sectionText = (ITextNode)sectionInstance.Nodes.First(node => node.Name == "SectionText");

		var collectionNode = (ICollectionNode)form.Nodes.First(node => node.Name == "Collection");
		var collectionTemplate = collectionNode.Templates.First();
		collectionNode.Instantiate(collectionTemplate);
		var collectionInstance = collectionNode.Instances.First();
		var collectionText = collectionInstance.Nodes.First(node => node.Name == "CollectionText");

		sectionText.SetValidationError("error", "text");
		collectionText.SetValidationError("error", "text");

		form.Update();
		Assert.IsFalse(sectionInstance.IsValid);
		Assert.IsFalse(templateNode.IsValid);
		Assert.IsFalse(collectionInstance.IsValid);
		Assert.IsFalse(collectionNode.IsValid);
		Assert.IsFalse(form.IsValid);
	}

	[TestMethod]
	public void Update_AllChildrenValid_ShouldAllowContainerToBeValid()
	{
		var form = new FormBuilder("Test")
			.WithTemplatedSection(
				"Template",
				(node, _) => node.UseTemplate("SectionTemplate", template => template.WithTextNode("SectionText"))
			)
			.WithCollectionNode(
				"Collection",
				(node, _) => node.UseTemplate("CollectionTemplate", template => template.WithTextNode("CollectionText"))
			)
			.Build();

		var templateNode = (ITemplateNode)form.Nodes.First(node => node.Name == "Template");
		var sectionTemplate = templateNode.Templates.First();
		templateNode.Instantiate(sectionTemplate);
		var sectionInstance = templateNode.Instance!;
		var sectionText = (ITextNode)sectionInstance.Nodes.First(node => node.Name == "SectionText");

		var collectionNode = (ICollectionNode)form.Nodes.First(node => node.Name == "Collection");
		var collectionTemplate = collectionNode.Templates.First();
		collectionNode.Instantiate(collectionTemplate);
		var collectionInstance = collectionNode.Instances.First();
		var collectionText = collectionInstance.Nodes.First(node => node.Name == "CollectionText");

		form.Update();
		Assert.IsTrue(sectionTemplate.IsValid);
		Assert.IsTrue(templateNode.IsValid);
		Assert.IsTrue(collectionInstance.IsValid);
		Assert.IsTrue(collectionNode.IsValid);
		Assert.IsTrue(form.IsValid);
	}

	[TestMethod]
	public void Update_ShouldExecuteValidationExtensionEventsIfVisible()
	{
		var mockExtension = new MockExtension();
		var mockValidator = new InvalidMockValidator();
		var form = new FormBuilder("Test")
			.UseDefaultVisibility(true)
			.UseExtension(mockExtension)
			.UseValidator(mockValidator)
			.Build();

		form.Update();
		Assert.IsTrue(mockExtension.OnBeforeValidationHasBeenCalled);
		Assert.IsTrue(mockExtension.OnBeforeValidationIsValidValue);
		Assert.IsTrue(mockValidator.HasBeenCalled);
		Assert.IsTrue(mockExtension.OnBeforeValidationHasBeenCalled);
		Assert.IsFalse(mockExtension.OnAfterValidationIsValidValue);
	}

	[TestMethod]
	public void Update_ShouldNotTouchTags()
	{
		var form = new FormBuilder("Test").Build();
		form.SetTag("test", 42);

		form.Update();

		Assert.IsTrue(form.Tags["test"]?.Equals(42) ?? false);
	}
}
