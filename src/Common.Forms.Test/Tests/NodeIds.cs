namespace RobinEpple.Common.Forms.Test.Tests;

using RobinEpple.Common.Forms;
using RobinEpple.Common.Forms.Nodes;

[TestClass]
public class NodeIds
{
	[TestMethod]
	public void Ids_ShouldBeConstructedFromNodeNames()
	{
		var form = new FormBuilder("Test")
			.WithBooleanNode("Boolean")
			.WithCollectionNode("Collection")
			.WithFileNode("File")
			.WithNumberNode("Number")
			.WithSection("Section")
			.WithTemplatedSection("TemplatedSection")
			.WithTextNode("Text")
			.WithTimestampNode("Timestamp")
			.Build();

		Assert.AreEqual(form.FindFirst("Boolean")!.GetId(), "Test/Boolean");
		Assert.AreEqual(form.FindFirst("Collection")!.GetId(), "Test/Collection");
		Assert.AreEqual(form.FindFirst("File")!.GetId(), "Test/File");
		Assert.AreEqual(form.FindFirst("Number")!.GetId(), "Test/Number");
		Assert.AreEqual(form.FindFirst("Section")!.GetId(), "Test/Section");
		Assert.AreEqual(form.FindFirst("TemplatedSection")!.GetId(), "Test/TemplatedSection");
		Assert.AreEqual(form.FindFirst("Text")!.GetId(), "Test/Text");
		Assert.AreEqual(form.FindFirst("Timestamp")!.GetId(), "Test/Timestamp");
	}

	[TestMethod]
	public void Ids_ShouldBeUniqueToInstances()
	{
		var form = new FormBuilder("Test")
			.WithSection(
				"Section",
				(section, _) =>
					section.WithTemplatedSection(
						"TemplatedSection",
						(templatedSection, _) =>
							templatedSection.UseTemplate(
								"SectionTemplate",
								template =>
									template.WithCollectionNode(
										"Collection",
										(collection, _) =>
											collection.UseTemplate(
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
							)
					)
			)
			.Build();

		var template = (ITemplateNode)form.FindFirst("TemplatedSection")!;
		template.Instantiate(template.Templates.First());
		var collection = (ICollectionNode)form.FindFirst("Collection")!;
		var instance1 = collection.Instantiate(collection.Templates.First());
		var instance2 = collection.Instantiate(collection.Templates.First());

		Assert.AreEqual(
			instance1.FindFirst("Boolean")!.GetId(),
			"Test/Section/TemplatedSection/SectionTemplate/Collection[0]/CollectionTemplate/Boolean"
		);
		Assert.AreEqual(
			instance1.FindFirst("File")!.GetId(),
			"Test/Section/TemplatedSection/SectionTemplate/Collection[0]/CollectionTemplate/File"
		);
		Assert.AreEqual(
			instance1.FindFirst("Number")!.GetId(),
			"Test/Section/TemplatedSection/SectionTemplate/Collection[0]/CollectionTemplate/Number"
		);
		Assert.AreEqual(
			instance1.FindFirst("Text")!.GetId(),
			"Test/Section/TemplatedSection/SectionTemplate/Collection[0]/CollectionTemplate/Text"
		);
		Assert.AreEqual(
			instance1.FindFirst("Timestamp")!.GetId(),
			"Test/Section/TemplatedSection/SectionTemplate/Collection[0]/CollectionTemplate/Timestamp"
		);

		Assert.AreEqual(
			instance2.FindFirst("Boolean")!.GetId(),
			"Test/Section/TemplatedSection/SectionTemplate/Collection[1]/CollectionTemplate/Boolean"
		);
		Assert.AreEqual(
			instance2.FindFirst("File")!.GetId(),
			"Test/Section/TemplatedSection/SectionTemplate/Collection[1]/CollectionTemplate/File"
		);
		Assert.AreEqual(
			instance2.FindFirst("Number")!.GetId(),
			"Test/Section/TemplatedSection/SectionTemplate/Collection[1]/CollectionTemplate/Number"
		);
		Assert.AreEqual(
			instance2.FindFirst("Text")!.GetId(),
			"Test/Section/TemplatedSection/SectionTemplate/Collection[1]/CollectionTemplate/Text"
		);
		Assert.AreEqual(
			instance2.FindFirst("Timestamp")!.GetId(),
			"Test/Section/TemplatedSection/SectionTemplate/Collection[1]/CollectionTemplate/Timestamp"
		);
	}

	[TestMethod]
	public void Ids_ShouldAdjustToStructureChanges()
	{
		var form = new FormBuilder("Test")
			.WithSection(
				"Section",
				(section, _) =>
					section.WithTemplatedSection(
						"TemplatedSection",
						(templatedSection, _) =>
							templatedSection.UseTemplate(
								"SectionTemplate",
								template =>
									template.WithCollectionNode(
										"Collection",
										(collection, _) =>
											collection.UseTemplate(
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
							)
					)
			)
			.Build();

		var template = (ITemplateNode)form.FindFirst("TemplatedSection")!;
		template.Instantiate(template.Templates.First());
		var collection = (ICollectionNode)form.FindFirst("Collection")!;
		var instance1 = collection.Instantiate(collection.Templates.First());
		var instance2 = collection.Instantiate(collection.Templates.First());

		Assert.AreEqual(
			instance2.FindFirst("Boolean")!.GetId(),
			"Test/Section/TemplatedSection/SectionTemplate/Collection[1]/CollectionTemplate/Boolean"
		);
		Assert.AreEqual(
			instance2.FindFirst("File")!.GetId(),
			"Test/Section/TemplatedSection/SectionTemplate/Collection[1]/CollectionTemplate/File"
		);
		Assert.AreEqual(
			instance2.FindFirst("Number")!.GetId(),
			"Test/Section/TemplatedSection/SectionTemplate/Collection[1]/CollectionTemplate/Number"
		);
		Assert.AreEqual(
			instance2.FindFirst("Text")!.GetId(),
			"Test/Section/TemplatedSection/SectionTemplate/Collection[1]/CollectionTemplate/Text"
		);
		Assert.AreEqual(
			instance2.FindFirst("Timestamp")!.GetId(),
			"Test/Section/TemplatedSection/SectionTemplate/Collection[1]/CollectionTemplate/Timestamp"
		);

		collection.RemoveItem(instance1);

		Assert.AreEqual(
			instance2.FindFirst("Boolean")!.GetId(),
			"Test/Section/TemplatedSection/SectionTemplate/Collection[0]/CollectionTemplate/Boolean"
		);
		Assert.AreEqual(
			instance2.FindFirst("File")!.GetId(),
			"Test/Section/TemplatedSection/SectionTemplate/Collection[0]/CollectionTemplate/File"
		);
		Assert.AreEqual(
			instance2.FindFirst("Number")!.GetId(),
			"Test/Section/TemplatedSection/SectionTemplate/Collection[0]/CollectionTemplate/Number"
		);
		Assert.AreEqual(
			instance2.FindFirst("Text")!.GetId(),
			"Test/Section/TemplatedSection/SectionTemplate/Collection[0]/CollectionTemplate/Text"
		);
		Assert.AreEqual(
			instance2.FindFirst("Timestamp")!.GetId(),
			"Test/Section/TemplatedSection/SectionTemplate/Collection[0]/CollectionTemplate/Timestamp"
		);
	}
}
