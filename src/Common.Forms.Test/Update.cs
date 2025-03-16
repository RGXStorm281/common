namespace RobinEpple.Common.Forms.Test;

using RobinEpple.Common.Forms.Nodes;

[TestClass]
public class Update
{
	[TestMethod]
	public void UpdateWithoutVisibilityExpression_ShouldKeepManuallySetVisibility()
	{
		var form = new FormBuilder("Test").Build();
		form.IsVisible = false;
		form.Update();
		Assert.IsFalse(form.IsVisible);
	}

	[TestMethod]
	public void Update_ShouldKeepReadonly()
	{
		var form = new FormBuilder("Test").Build();
		form.IsReadonly = true;
		form.Update();
		Assert.IsTrue(form.IsReadonly);
	}

	[TestMethod]
	public void Update_ShouldOverwriteManualVisibilityFromCondition()
	{
		var form = new FormBuilder("Test").UseVisibilityCondition(new FalseMockCondition()).Build();
		form.IsVisible = true;
		form.Update();
		Assert.IsFalse(form.IsVisible);
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
		var form = new FormBuilder("Test").Build();
		form.SetValidationError("error", "message");
		form.Update();
		Assert.IsFalse(form.IsValid);
	}

	[TestMethod]
	public void InvisibleNode_ShouldSetChildrenInvisible()
	{
		var form = new FormBuilder("Test").WithTextNode("TestText").Build();
		form.IsVisible = false;
		form.Update();
		Assert.IsFalse(form.Nodes.First().IsVisible);

		form = new FormBuilder("Test")
			.WithTemplatedSection(
				"Template",
				(node, _) =>
					node.UseTemplate(
						"SectionTemplate",
						template =>
							template.WithTextNode(
								"SectionText",
								node => node.UseVisibilityCondition(new TrueMockCondition())
							)
					)
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

		templateNode.IsVisible = false;
		collectionNode.IsVisible = false;
		collectionText.IsVisible = true;
		form.Update();

		// Parent being invisible should also overwrite manual settings and conditions.
		Assert.IsFalse(sectionInstance.IsVisible);
		Assert.IsFalse(sectionText.IsVisible);
		Assert.IsFalse(collectionInstance.IsVisible);
		Assert.IsFalse(collectionText.IsVisible);
	}

	[TestMethod]
	public void InvalidChildren_ShouldSetContainerInvalid()
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

		sectionText.SetValidationError("error", "text");
		collectionText.SetValidationError("error", "text");

		form.Update();
		Assert.IsFalse(sectionTemplate.IsValid);
		Assert.IsFalse(templateNode.IsValid);
		Assert.IsFalse(collectionInstance.IsValid);
		Assert.IsFalse(collectionNode.IsValid);
		Assert.IsFalse(form.IsValid);
	}

	[TestMethod]
	public void AllChildrenValid_ShouldAllowContainerToBeValid()
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
}
