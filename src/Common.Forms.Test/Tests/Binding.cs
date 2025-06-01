namespace RobinEpple.Common.Forms.Test.Tests;

using RobinEpple.Common.Forms.Binding;
using RobinEpple.Common.Forms.Building;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.Test.Mocks;

[TestClass]
public class Binding
{
	[TestMethod]
	public void LoadFromBinding_ShouldExecuteOnAllNodes()
	{
		// Build form.
		var invisibleMockBinding = new MockBinding();
		var visibleMockBinding = new MockBinding();
		var form = new FormBuilder("Test")
			.WithBooleanNode("Invisible", node => node.UseDefaultVisibility(false).UseBinding(invisibleMockBinding))
			.WithBooleanNode("Visible", node => node.UseDefaultVisibility(true).UseBinding(visibleMockBinding))
			.Build();

		// Check that loading is executed on all bindings.
		form.LoadFromBinding();
		Assert.IsTrue(invisibleMockBinding.LoadFromBindingHasBeenCalled);
		Assert.IsTrue(visibleMockBinding.LoadFromBindingHasBeenCalled);
	}

	[TestMethod]
	public void WriteToBinding_ShouldOnlyExecuteOnVisibleNodes()
	{
		// Build form.
		var invisibleMockBinding = new MockBinding();
		var visibleMockBinding = new MockBinding();
		var form = new FormBuilder("Test")
			.WithBooleanNode("Invisible", node => node.UseDefaultVisibility(false).UseBinding(invisibleMockBinding))
			.WithBooleanNode("Visible", node => node.UseDefaultVisibility(true).UseBinding(visibleMockBinding))
			.Build();

		// Check that writing is only executed on visible nodes.
		form.WriteToBinding();
		Assert.IsFalse(invisibleMockBinding.WriteToModelHasBeenCalled);
		Assert.IsTrue(visibleMockBinding.WriteToModelHasBeenCalled);
	}

	[TestMethod]
	public void InstanceModels_ShouldBeUniqueOnEachInstance()
	{
		var form = new FormBuilder("Test")
			.WithCollectionNode(
				"Collection",
				(collection, _) =>
					collection.UseTemplate(
						"Template",
						template => template.UseInstanceModel(() => Guid.NewGuid(), out var _)
					)
			)
			.Build();

		var collection = (ICollectionNode)form.Nodes.First(node => node.Name == "Collection");
		collection.Instantiate(collection.Templates.First());
		collection.Instantiate(collection.Templates.First());
		var instance1 = collection.Instances.First();
		var instance2 = collection.Instances.Skip(1).First();

		// Make sure the two instances have been initialized independently.
		var model1 = instance1.Tags[IFormNodeBinding.InstanceModelTagName]!;
		var model2 = instance2.Tags[IFormNodeBinding.InstanceModelTagName]!;
		Assert.AreNotEqual(model1, model2);
	}

	[TestMethod]
	public void ExternalGetterSetterBinding_ShouldAccessValuesFromBuildingContext()
	{
		// Define some variables to bind to.
		bool? booleanTarget = false;
		decimal? numberTarget = 1;
		string? textTarget = "initial";
		DateTime? timestampTarget = new DateTime(2000, 1, 1);
		FileValue fileTarget = new FileValue("initial", [1, 2, 3]);
		IEnumerable<int> collectionTarget = [1, 2];
		int? sectionTarget = 3;

		// Build the form with all the bindings.
		var form = new FormBuilder("Test")
			.WithBooleanNode(
				"Boolean",
				node => node.UseGetterSetterBinding(() => booleanTarget, value => booleanTarget = value)
			)
			.WithNumberNode(
				"Number",
				node => node.UseGetterSetterBinding(() => numberTarget, value => numberTarget = value)
			)
			.WithTextNode("Text", node => node.UseGetterSetterBinding(() => textTarget, value => textTarget = value))
			.WithTimestampNode(
				"Timestamp",
				node => node.UseGetterSetterBinding(() => timestampTarget, value => timestampTarget = value)
			)
			.WithFileNode("File", node => node.UseGetterSetterBinding(() => fileTarget, value => fileTarget = value))
			.WithCollectionNode(
				"Collection",
				(node, _) =>
					node.UseGetterSetterBinding(() => collectionTarget, value => collectionTarget = value)
						.UseTemplate("Template", template => template.UseInstanceModel(() => 4, out var _))
			)
			.WithTemplatedSection(
				"Section",
				(node, _) =>
					node.UseGetterSetterBinding(() => sectionTarget, value => sectionTarget = value)
						.UseTemplate("Template", template => template.UseInstanceModel(() => 5, out var _))
			)
			.Build();

		var booleanNode = (IBooleanNode)form.Nodes.First(node => node.Name == "Boolean");
		var numberNode = (INumberNode)form.Nodes.First(node => node.Name == "Number");
		var textNode = (ITextNode)form.Nodes.First(node => node.Name == "Text");
		var timestampNode = (ITimestampNode)form.Nodes.First(node => node.Name == "Timestamp");
		var fileNode = (IFileNode)form.Nodes.First(node => node.Name == "File");
		var collectionNode = (ICollectionNode)form.Nodes.First(node => node.Name == "Collection");
		var sectionNode = (ITemplateNode)form.Nodes.First(node => node.Name == "Section");

		// Load the state from the model.
		form.LoadFromBinding();
		Assert.AreEqual(booleanNode.Value, false);
		Assert.AreEqual(numberNode.Value, 1);
		Assert.AreEqual(textNode.Value, "initial");
		Assert.AreEqual(timestampNode.Value, new DateTime(2000, 1, 1));
		Assert.AreEqual(fileNode.Value.FileName, "initial");
		Assert.AreEqual(fileNode.Value.FileContents![0], 1);
		Assert.AreEqual(fileNode.Value.FileContents![1], 2);
		Assert.AreEqual(fileNode.Value.FileContents![2], 3);
		Assert.AreEqual(collectionNode.Instances.Count(), 2);
		Assert.AreEqual(collectionNode.Instances.First().Tags[IFormNodeBinding.InstanceModelTagName], 1);
		Assert.AreEqual(collectionNode.Instances.Skip(1).First().Tags[IFormNodeBinding.InstanceModelTagName], 2);
		Assert.AreEqual(sectionNode.Instance!.Tags[IFormNodeBinding.InstanceModelTagName], 3);

		// Alter the state in the form and write back.
		booleanNode.Value = true;
		numberNode.Value = 42;
		textNode.Value = "updated";
		timestampNode.Value = timestampNode.Value!.Value.AddDays(2);
		fileNode.Value = new("updated", [42]);
		collectionNode.Clear();
		collectionNode.Instantiate(collectionNode.Templates.First());
		sectionNode.Clear();
		sectionNode.Instantiate(sectionNode.Templates.First());

		form.WriteToBinding();

		Assert.AreEqual(booleanTarget, true);
		Assert.AreEqual(numberTarget, 42);
		Assert.AreEqual(textTarget, "updated");
		Assert.AreEqual(timestampTarget, new DateTime(2000, 1, 3));
		Assert.AreEqual(fileTarget.FileName, "updated");
		Assert.AreEqual(fileTarget.FileContents![0], 42);
		Assert.AreEqual(collectionTarget.First(), 4);
		Assert.AreEqual(sectionTarget, 5);
	}

	private BindingModel _model { get; set; } = new();

	[TestMethod]
	public void ExternalPropertyBinding_ShouldAccessPropertyFromBuildingContext()
	{
		// initialize properties.
		_model.BooleanProperty = false;
		_model.DecimalProperty = 1;
		_model.DoubleProperty = 1;
		_model.FloatProperty = 1;
		_model.LongProperty = 1;
		_model.IntProperty = 1;
		_model.TextProperty = "initial";
		_model.TimestampProperty = new DateTime(2000, 1, 1);
		_model.FileProperty = new FileValue("initial", [1, 2, 3]);
		_model.CollectionProperty = [1, 2];
		_model.SectionProperty = 3;

		// Build the form with all the bindings.
		var form = new FormBuilder("Test")
			.WithBooleanNode("Boolean", node => node.UsePropertyBinding(() => _model.BooleanProperty))
			.WithNumberNode("Decimal", node => node.UsePropertyBinding(() => _model.DecimalProperty))
			.WithNumberNode("Double", node => node.UsePropertyBinding(() => _model.DoubleProperty))
			.WithNumberNode("Float", node => node.UsePropertyBinding(() => _model.FloatProperty))
			.WithNumberNode("Long", node => node.UsePropertyBinding(() => _model.LongProperty))
			.WithNumberNode("Int", node => node.UsePropertyBinding(() => _model.IntProperty))
			.WithTextNode("Text", node => node.UsePropertyBinding(() => _model.TextProperty))
			.WithTimestampNode("Timestamp", node => node.UsePropertyBinding(() => _model.TimestampProperty))
			.WithFileNode("File", node => node.UsePropertyBinding(() => _model.FileProperty))
			.WithCollectionNode(
				"Collection",
				(node, _) =>
					node.UsePropertyBinding(() => _model.CollectionProperty)
						.UseTemplate("Template", template => template.UseInstanceModel(() => 4, out var _))
			)
			.WithTemplatedSection(
				"Section",
				(node, _) =>
					node.UsePropertyBinding(() => _model.SectionProperty)
						.UseTemplate("Template", template => template.UseInstanceModel(() => 5, out var _))
			)
			.Build();

		var booleanNode = (IBooleanNode)form.Nodes.First(node => node.Name == "Boolean");
		var decimalNode = (INumberNode)form.Nodes.First(node => node.Name == "Decimal");
		var doubleNode = (INumberNode)form.Nodes.First(node => node.Name == "Double");
		var floatNode = (INumberNode)form.Nodes.First(node => node.Name == "Float");
		var longNode = (INumberNode)form.Nodes.First(node => node.Name == "Long");
		var intNode = (INumberNode)form.Nodes.First(node => node.Name == "Int");
		var textNode = (ITextNode)form.Nodes.First(node => node.Name == "Text");
		var timestampNode = (ITimestampNode)form.Nodes.First(node => node.Name == "Timestamp");
		var fileNode = (IFileNode)form.Nodes.First(node => node.Name == "File");
		var collectionNode = (ICollectionNode)form.Nodes.First(node => node.Name == "Collection");
		var sectionNode = (ITemplateNode)form.Nodes.First(node => node.Name == "Section");

		// Load the state from the model.
		form.LoadFromBinding();
		Assert.AreEqual(booleanNode.Value, false);
		Assert.AreEqual(decimalNode.Value, 1);
		Assert.AreEqual(doubleNode.Value, 1);
		Assert.AreEqual(floatNode.Value, 1);
		Assert.AreEqual(longNode.Value, 1);
		Assert.AreEqual(intNode.Value, 1);
		Assert.AreEqual(textNode.Value, "initial");
		Assert.AreEqual(timestampNode.Value, new DateTime(2000, 1, 1));
		Assert.AreEqual(fileNode.Value.FileName, "initial");
		Assert.AreEqual(fileNode.Value.FileContents![0], 1);
		Assert.AreEqual(fileNode.Value.FileContents![1], 2);
		Assert.AreEqual(fileNode.Value.FileContents![2], 3);
		Assert.AreEqual(collectionNode.Instances.Count(), 2);
		Assert.AreEqual(collectionNode.Instances.First().Tags[IFormNodeBinding.InstanceModelTagName], 1);
		Assert.AreEqual(collectionNode.Instances.Skip(1).First().Tags[IFormNodeBinding.InstanceModelTagName], 2);
		Assert.AreEqual(sectionNode.Instance!.Tags[IFormNodeBinding.InstanceModelTagName], 3);

		// Alter the state in the form and write back.
		booleanNode.Value = true;
		decimalNode.Value = 42;
		doubleNode.Value = 42;
		floatNode.Value = 42;
		longNode.Value = 42;
		intNode.Value = 42;
		textNode.Value = "updated";
		timestampNode.Value = timestampNode.Value!.Value.AddDays(2);
		fileNode.Value = new("updated", [42]);
		collectionNode.Clear();
		collectionNode.Instantiate(collectionNode.Templates.First());
		sectionNode.Clear();
		sectionNode.Instantiate(sectionNode.Templates.First());

		form.WriteToBinding();

		Assert.AreEqual(_model.BooleanProperty, true);
		Assert.AreEqual(_model.DecimalProperty, 42);
		Assert.AreEqual(_model.DoubleProperty, 42);
		Assert.AreEqual(_model.FloatProperty, 42);
		Assert.AreEqual(_model.LongProperty, 42);
		Assert.AreEqual(_model.IntProperty, 42);
		Assert.AreEqual(_model.TextProperty, "updated");
		Assert.AreEqual(_model.TimestampProperty, new DateTime(2000, 1, 3));
		Assert.AreEqual(_model.FileProperty.FileName, "updated");
		Assert.AreEqual(_model.FileProperty.FileContents![0], 42);
		Assert.AreEqual(_model.CollectionProperty.First(), 4);
		Assert.AreEqual(_model.SectionProperty, 5);
	}

	[TestMethod]
	public void InstanceGetterSetterBinding_ShouldAccessValuesInInstanceModel() { }

	[TestMethod]
	public void InstanceGetterSetterBinding_ShouldFindTargetMultipleLayersAbove() { }

	[TestMethod]
	public void InstancePropertyBinding_ShouldAccessPropertyInInstanceModel() { }

	[TestMethod]
	public void InstancePropertyBinding_ShouldFindTargetMultipleLayersAbove() { }
}
