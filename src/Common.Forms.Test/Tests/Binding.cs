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
	public void ExternalGetterSetterBinding_ShouldAccessValuesFromBuildingContext() { }

	[TestMethod]
	public void ExternalPropertyBinding_ShouldAccessPropertyFromBuildingContext() { }

	[TestMethod]
	public void InstanceGetterSetterBinding_ShouldAccessValuesInInstanceModel() { }

	[TestMethod]
	public void InstanceGetterSetterBinding_ShouldFindTargetMultipleLayersAbove() { }

	[TestMethod]
	public void InstancePropertyBinding_ShouldAccessPropertyInInstanceModel() { }

	[TestMethod]
	public void InstancePropertyBinding_ShouldFindTargetMultipleLayersAbove() { }
}
