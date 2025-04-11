namespace RobinEpple.Common.Forms.Test.Tests;

using RobinEpple.Common.Forms;
using RobinEpple.Common.Forms.Nodes;

[TestClass]
public class TemplateInstantiation
{
	[TestMethod]
	public void TemplateNodeInstantiate_ShouldInstantiateTemplate()
	{
		var form = new FormBuilder("Test")
			.WithTemplatedSection(
				"TemplateSection",
				(builder, _) =>
					builder.UseTemplate(
						"Template",
						templateBuilder =>
						{
							templateBuilder.WithTextNode("TemplateText");
						}
					)
			)
			.Build();

		var templateNode = (ITemplateNode)form.Nodes.First();
		var template = templateNode.Templates.First(template => template.Name == "Template");

		Assert.IsTrue(templateNode.Instance == null);
		templateNode.Instantiate(template);
		var instance = templateNode.Instance;
		Assert.IsTrue(instance != null);
		Assert.IsTrue(instance.Nodes.First() is ITextNode { Name: "TemplateText" });
	}

	[TestMethod]
	public void TemplateNodeInstantiate_ShouldReplaceExistingInstance()
	{
		var form = new FormBuilder("Test")
			.WithTemplatedSection(
				"TemplateSection",
				(builder, _) =>
					builder
						.UseTemplate(
							"Template1",
							templateBuilder =>
							{
								templateBuilder.WithTextNode("TemplateText");
							}
						)
						.UseTemplate(
							"Template2",
							templateBuilder =>
							{
								templateBuilder.WithNumberNode("TemplateNumber");
							}
						)
			)
			.Build();

		var templateNode = (ITemplateNode)form.Nodes.First();
		var template1 = templateNode.Templates.First(template => template.Name == "Template1");
		var template2 = templateNode.Templates.First(template => template.Name == "Template2");

		Assert.IsTrue(templateNode.Instance == null);
		templateNode.Instantiate(template1);
		templateNode.Instantiate(template2);
		var instance = templateNode.Instance;
		Assert.IsTrue(instance != null);
		Assert.IsTrue(instance.Nodes.First() is INumberNode { Name: "TemplateNumber" });
	}

	[TestMethod]
	public void TemplateNodeClear_ShouldRemoveInstance()
	{
		var form = new FormBuilder("Test")
			.WithTemplatedSection(
				"TemplateSection",
				(builder, _) =>
					builder.UseTemplate(
						"Template",
						templateBuilder =>
						{
							templateBuilder.WithTextNode("TemplateText");
						}
					)
			)
			.Build();

		var templateNode = (ITemplateNode)form.Nodes.First();
		var template = templateNode.Templates.First(template => template.Name == "Template");

		Assert.IsTrue(templateNode.Instance == null);
		templateNode.Instantiate(template);
		templateNode.Clear();
		var instance = templateNode.Instance;
		Assert.IsTrue(instance == null);
	}

	[TestMethod]
	public void TemplateNodeChangesOnInstance_ShouldNotAffectTemplate()
	{
		var form = new FormBuilder("Test")
			.WithTemplatedSection(
				"TemplateSection",
				(builder, _) =>
					builder.UseTemplate(
						"Template",
						templateBuilder =>
						{
							templateBuilder.WithTextNode("TemplateText");
						}
					)
			)
			.Build();

		var templateNode = (ITemplateNode)form.Nodes.First();
		var template = templateNode.Templates.First(template => template.Name == "Template");
		var templateTextNode = (ITextNode)template.Nodes.First();

		templateNode.Instantiate(template);
		var instance = templateNode.Instance!;
		var instanceTextNode = (ITextNode)instance.Nodes.First();

		instanceTextNode.Value = "Test";

		Assert.IsTrue(instanceTextNode.Value == "Test");
		Assert.IsTrue(templateTextNode.Value == null);
	}

	[TestMethod]
	public void TemplateNodeRecursiveLayers_ShouldNotAffectEachOther()
	{
		var form = new FormBuilder("Test")
			.WithTextNode("TextNode")
			.WithTemplatedSection(
				"TemplatedSection",
				(builder, recursiveTemplate) =>
				{
					builder.UsePreConfiguredTemplate(recursiveTemplate);
				}
			)
			.Build();

		var outerTextNode = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		var outerTemplateNode = (ITemplateNode)form.Nodes.First(node => node.Name == "TemplatedSection");
		var recursiveTemplate = outerTemplateNode.Templates.First();

		outerTextNode.Value = "outer Text";
		outerTemplateNode.Instantiate(recursiveTemplate);
		var instance = outerTemplateNode.Instance!;
		var innerTextNode = (ITextNode)instance.Nodes.First(node => node.Name == "TextNode");
		var innerTemplateNode = (ITemplateNode)instance.Nodes.First(node => node.Name == "TemplatedSection");

		Assert.IsTrue(outerTextNode.Value == "outer Text");
		Assert.IsTrue(outerTemplateNode.Instance != null);
		Assert.IsTrue(innerTextNode.Value == null);
		Assert.IsTrue(innerTemplateNode.Instance == null);

		innerTextNode.Value = "inner Text";
		Assert.IsTrue(innerTextNode.Value == "inner Text");
		Assert.IsTrue(outerTextNode.Value == "outer Text");
	}

	[TestMethod]
	public void CollectionNode_ShouldAllowMultipleInstancesOfSameTemplate()
	{
		var form = new FormBuilder("Test")
			.WithCollectionNode(
				"CollectionNode",
				(builder, _) =>
					builder.UseTemplate(
						"Template",
						templateBuilder =>
						{
							templateBuilder.WithTextNode("TextNode");
						}
					)
			)
			.Build();

		var collectionNode = (ICollectionNode)form.Nodes.First(node => node.Name == "CollectionNode");
		var template = collectionNode.Templates.First(template => template.Name == "Template");

		collectionNode.Instantiate(template);
		collectionNode.Instantiate(template);

		var instances = collectionNode.Instances.ToList();
		Assert.IsTrue(instances.Count == 2);
	}

	[TestMethod]
	public void CollectionNode_ShouldAllowMultipleInstancesOfDifferentTemplates()
	{
		var form = new FormBuilder("Test")
			.WithCollectionNode(
				"CollectionNode",
				(builder, _) =>
					builder
						.UseTemplate(
							"Template1",
							templateBuilder =>
							{
								templateBuilder.WithTextNode("TextNode");
							}
						)
						.UseTemplate(
							"Template2",
							templateBuilder =>
							{
								templateBuilder.WithNumberNode("NumberNode");
							}
						)
			)
			.Build();

		var collectionNode = (ICollectionNode)form.Nodes.First(node => node.Name == "CollectionNode");
		var template1 = collectionNode.Templates.First(template => template.Name == "Template1");
		var Template2 = collectionNode.Templates.First(template => template.Name == "Template2");

		collectionNode.Instantiate(template1);
		collectionNode.Instantiate(Template2);

		var instances = collectionNode.Instances.ToList();
		Assert.IsTrue(instances.Count == 2);
		Assert.IsTrue(instances[0].Name == "Template1");
		Assert.IsTrue(instances[1].Name == "Template2");
	}

	[TestMethod]
	public void CollectionNodeClear_ShouldRemoveAllInstances()
	{
		var form = new FormBuilder("Test")
			.WithCollectionNode(
				"CollectionNode",
				(builder, _) =>
					builder.UseTemplate(
						"Template",
						templateBuilder =>
						{
							templateBuilder.WithTextNode("TextNode");
						}
					)
			)
			.Build();

		var collectionNode = (ICollectionNode)form.Nodes.First(node => node.Name == "CollectionNode");
		var template = collectionNode.Templates.First(template => template.Name == "Template");

		collectionNode.Instantiate(template);
		collectionNode.Instantiate(template);

		collectionNode.Clear();

		Assert.IsTrue(collectionNode.Instances.Count() == 0);
	}

	[TestMethod]
	public void CollectionNodeInstances_ShouldNotAffectEachOtherOrTemplate()
	{
		var form = new FormBuilder("Test")
			.WithCollectionNode(
				"CollectionNode",
				(builder, _) =>
					builder.UseTemplate(
						"Template",
						templateBuilder =>
						{
							templateBuilder.WithTextNode("TextNode");
						}
					)
			)
			.Build();

		var collectionNode = (ICollectionNode)form.Nodes.First(node => node.Name == "CollectionNode");
		var template = collectionNode.Templates.First(template => template.Name == "Template");

		collectionNode.Instantiate(template);
		collectionNode.Instantiate(template);

		var instances = collectionNode.Instances.ToList();
		var instance1 = instances[0];
		var instance2 = instances[1];

		var templateTextNode = (ITextNode)template.Nodes.First(node => node.Name == "TextNode");
		var instance1TextNode = (ITextNode)instance1.Nodes.First(node => node.Name == "TextNode");
		var instance2TextNode = (ITextNode)instance2.Nodes.First(node => node.Name == "TextNode");

		instance1TextNode.Value = "Text 1";
		instance2TextNode.Value = "Text 2";

		Assert.IsTrue(templateTextNode.Value == null);
		Assert.IsTrue(instance1TextNode.Value == "Text 1");
		Assert.IsTrue(instance2TextNode.Value == "Text 2");
	}

	[TestMethod]
	public void CollectionNodeRecursiveLayers_ShouldNotAffectEachOther()
	{
		var form = new FormBuilder("Test")
			.WithTextNode("TextNode")
			.WithCollectionNode(
				"CollectionNode",
				(builder, recursiveTemplate) => builder.UsePreConfiguredTemplate(recursiveTemplate)
			)
			.Build();

		var outerTextNode = (ITextNode)form.Nodes.First(node => node.Name == "TextNode");
		var outerCollectionNode = (ICollectionNode)form.Nodes.First(node => node.Name == "CollectionNode");

		outerTextNode.Value = "outer Text";
		outerCollectionNode.Instantiate(form);
		var instances = outerCollectionNode.Instances.ToList();
		var innerForm = instances[0];
		var innerTextNode = (ITextNode)innerForm.Nodes.First(node => node.Name == "TextNode");
		var innerCollectionNode = (ICollectionNode)innerForm.Nodes.First(node => node.Name == "CollectionNode");

		Assert.IsTrue(outerTextNode.Value == "outer Text");
		Assert.IsTrue(innerTextNode.Value == null);

		innerTextNode.Value = "inner Text";
		Assert.IsTrue(outerTextNode.Value == "outer Text");
		Assert.IsTrue(innerTextNode.Value == "inner Text");
	}
}
