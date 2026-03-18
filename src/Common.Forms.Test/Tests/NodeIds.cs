namespace RobinEpple.Common.Forms.Test.Tests;

using RobinEpple.Common.Forms;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Util;

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

		Assert.AreEqual(form.FindFirst("Boolean")!.GetId(), $"Test{IFormNode.PathSeparator}Boolean");
		Assert.AreEqual(form.FindFirst("Collection")!.GetId(), $"Test{IFormNode.PathSeparator}Collection");
		Assert.AreEqual(form.FindFirst("File")!.GetId(), $"Test{IFormNode.PathSeparator}File");
		Assert.AreEqual(form.FindFirst("Number")!.GetId(), $"Test{IFormNode.PathSeparator}Number");
		Assert.AreEqual(form.FindFirst("Section")!.GetId(), $"Test{IFormNode.PathSeparator}Section");
		Assert.AreEqual(form.FindFirst("TemplatedSection")!.GetId(), $"Test{IFormNode.PathSeparator}TemplatedSection");
		Assert.AreEqual(form.FindFirst("Text")!.GetId(), $"Test{IFormNode.PathSeparator}Text");
		Assert.AreEqual(form.FindFirst("Timestamp")!.GetId(), $"Test{IFormNode.PathSeparator}Timestamp");
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
			$"Test{IFormNode.PathSeparator}Section{IFormNode.PathSeparator}TemplatedSection{IFormNode.PathSeparator}SectionTemplate{IFormNode.PathSeparator}Collection{IFormNode.IndexIdentifier.Format(0)}{IFormNode.PathSeparator}CollectionTemplate{IFormNode.PathSeparator}Boolean"
		);
		Assert.AreEqual(
			instance1.FindFirst("File")!.GetId(),
			$"Test{IFormNode.PathSeparator}Section{IFormNode.PathSeparator}TemplatedSection{IFormNode.PathSeparator}SectionTemplate{IFormNode.PathSeparator}Collection{IFormNode.IndexIdentifier.Format(0)}{IFormNode.PathSeparator}CollectionTemplate{IFormNode.PathSeparator}File"
		);
		Assert.AreEqual(
			instance1.FindFirst("Number")!.GetId(),
			$"Test{IFormNode.PathSeparator}Section{IFormNode.PathSeparator}TemplatedSection{IFormNode.PathSeparator}SectionTemplate{IFormNode.PathSeparator}Collection{IFormNode.IndexIdentifier.Format(0)}{IFormNode.PathSeparator}CollectionTemplate{IFormNode.PathSeparator}Number"
		);
		Assert.AreEqual(
			instance1.FindFirst("Text")!.GetId(),
			$"Test{IFormNode.PathSeparator}Section{IFormNode.PathSeparator}TemplatedSection{IFormNode.PathSeparator}SectionTemplate{IFormNode.PathSeparator}Collection{IFormNode.IndexIdentifier.Format(0)}{IFormNode.PathSeparator}CollectionTemplate{IFormNode.PathSeparator}Text"
		);
		Assert.AreEqual(
			instance1.FindFirst("Timestamp")!.GetId(),
			$"Test{IFormNode.PathSeparator}Section{IFormNode.PathSeparator}TemplatedSection{IFormNode.PathSeparator}SectionTemplate{IFormNode.PathSeparator}Collection{IFormNode.IndexIdentifier.Format(0)}{IFormNode.PathSeparator}CollectionTemplate{IFormNode.PathSeparator}Timestamp"
		);

		Assert.AreEqual(
			instance2.FindFirst("Boolean")!.GetId(),
			$"Test{IFormNode.PathSeparator}Section{IFormNode.PathSeparator}TemplatedSection{IFormNode.PathSeparator}SectionTemplate{IFormNode.PathSeparator}Collection{IFormNode.IndexIdentifier.Format(1)}{IFormNode.PathSeparator}CollectionTemplate{IFormNode.PathSeparator}Boolean"
		);
		Assert.AreEqual(
			instance2.FindFirst("File")!.GetId(),
			$"Test{IFormNode.PathSeparator}Section{IFormNode.PathSeparator}TemplatedSection{IFormNode.PathSeparator}SectionTemplate{IFormNode.PathSeparator}Collection{IFormNode.IndexIdentifier.Format(1)}{IFormNode.PathSeparator}CollectionTemplate{IFormNode.PathSeparator}File"
		);
		Assert.AreEqual(
			instance2.FindFirst("Number")!.GetId(),
			$"Test{IFormNode.PathSeparator}Section{IFormNode.PathSeparator}TemplatedSection{IFormNode.PathSeparator}SectionTemplate{IFormNode.PathSeparator}Collection{IFormNode.IndexIdentifier.Format(1)}{IFormNode.PathSeparator}CollectionTemplate{IFormNode.PathSeparator}Number"
		);
		Assert.AreEqual(
			instance2.FindFirst("Text")!.GetId(),
			$"Test{IFormNode.PathSeparator}Section{IFormNode.PathSeparator}TemplatedSection{IFormNode.PathSeparator}SectionTemplate{IFormNode.PathSeparator}Collection{IFormNode.IndexIdentifier.Format(1)}{IFormNode.PathSeparator}CollectionTemplate{IFormNode.PathSeparator}Text"
		);
		Assert.AreEqual(
			instance2.FindFirst("Timestamp")!.GetId(),
			$"Test{IFormNode.PathSeparator}Section{IFormNode.PathSeparator}TemplatedSection{IFormNode.PathSeparator}SectionTemplate{IFormNode.PathSeparator}Collection{IFormNode.IndexIdentifier.Format(1)}{IFormNode.PathSeparator}CollectionTemplate{IFormNode.PathSeparator}Timestamp"
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
			$"Test{IFormNode.PathSeparator}Section{IFormNode.PathSeparator}TemplatedSection{IFormNode.PathSeparator}SectionTemplate{IFormNode.PathSeparator}Collection{IFormNode.IndexIdentifier.Format(1)}{IFormNode.PathSeparator}CollectionTemplate{IFormNode.PathSeparator}Boolean"
		);
		Assert.AreEqual(
			instance2.FindFirst("File")!.GetId(),
			$"Test{IFormNode.PathSeparator}Section{IFormNode.PathSeparator}TemplatedSection{IFormNode.PathSeparator}SectionTemplate{IFormNode.PathSeparator}Collection{IFormNode.IndexIdentifier.Format(1)}{IFormNode.PathSeparator}CollectionTemplate{IFormNode.PathSeparator}File"
		);
		Assert.AreEqual(
			instance2.FindFirst("Number")!.GetId(),
			$"Test{IFormNode.PathSeparator}Section{IFormNode.PathSeparator}TemplatedSection{IFormNode.PathSeparator}SectionTemplate{IFormNode.PathSeparator}Collection{IFormNode.IndexIdentifier.Format(1)}{IFormNode.PathSeparator}CollectionTemplate{IFormNode.PathSeparator}Number"
		);
		Assert.AreEqual(
			instance2.FindFirst("Text")!.GetId(),
			$"Test{IFormNode.PathSeparator}Section{IFormNode.PathSeparator}TemplatedSection{IFormNode.PathSeparator}SectionTemplate{IFormNode.PathSeparator}Collection{IFormNode.IndexIdentifier.Format(1)}{IFormNode.PathSeparator}CollectionTemplate{IFormNode.PathSeparator}Text"
		);
		Assert.AreEqual(
			instance2.FindFirst("Timestamp")!.GetId(),
			$"Test{IFormNode.PathSeparator}Section{IFormNode.PathSeparator}TemplatedSection{IFormNode.PathSeparator}SectionTemplate{IFormNode.PathSeparator}Collection{IFormNode.IndexIdentifier.Format(1)}{IFormNode.PathSeparator}CollectionTemplate{IFormNode.PathSeparator}Timestamp"
		);

		collection.RemoveItem(instance1);

		Assert.AreEqual(
			instance2.FindFirst("Boolean")!.GetId(),
			$"Test{IFormNode.PathSeparator}Section{IFormNode.PathSeparator}TemplatedSection{IFormNode.PathSeparator}SectionTemplate{IFormNode.PathSeparator}Collection{IFormNode.IndexIdentifier.Format(0)}{IFormNode.PathSeparator}CollectionTemplate{IFormNode.PathSeparator}Boolean"
		);
		Assert.AreEqual(
			instance2.FindFirst("File")!.GetId(),
			$"Test{IFormNode.PathSeparator}Section{IFormNode.PathSeparator}TemplatedSection{IFormNode.PathSeparator}SectionTemplate{IFormNode.PathSeparator}Collection{IFormNode.IndexIdentifier.Format(0)}{IFormNode.PathSeparator}CollectionTemplate{IFormNode.PathSeparator}File"
		);
		Assert.AreEqual(
			instance2.FindFirst("Number")!.GetId(),
			$"Test{IFormNode.PathSeparator}Section{IFormNode.PathSeparator}TemplatedSection{IFormNode.PathSeparator}SectionTemplate{IFormNode.PathSeparator}Collection{IFormNode.IndexIdentifier.Format(0)}{IFormNode.PathSeparator}CollectionTemplate{IFormNode.PathSeparator}Number"
		);
		Assert.AreEqual(
			instance2.FindFirst("Text")!.GetId(),
			$"Test{IFormNode.PathSeparator}Section{IFormNode.PathSeparator}TemplatedSection{IFormNode.PathSeparator}SectionTemplate{IFormNode.PathSeparator}Collection{IFormNode.IndexIdentifier.Format(0)}{IFormNode.PathSeparator}CollectionTemplate{IFormNode.PathSeparator}Text"
		);
		Assert.AreEqual(
			instance2.FindFirst("Timestamp")!.GetId(),
			$"Test{IFormNode.PathSeparator}Section{IFormNode.PathSeparator}TemplatedSection{IFormNode.PathSeparator}SectionTemplate{IFormNode.PathSeparator}Collection{IFormNode.IndexIdentifier.Format(0)}{IFormNode.PathSeparator}CollectionTemplate{IFormNode.PathSeparator}Timestamp"
		);
	}
}
