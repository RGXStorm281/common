namespace RobinEpple.Common.Forms.Test.Mocks;

using RobinEpple.Common.Forms.Binding;
using RobinEpple.Common.Forms.Nodes;

public class MockBinding : IFormNodeBinding
{
	public bool LoadFromBindingHasBeenCalled { get; set; }
	public bool WriteToModelHasBeenCalled { get; set; }

	public void LoadFromModel(IFormNode node)
	{
		LoadFromBindingHasBeenCalled = true;
	}

	public void WriteToModel(IFormNode node)
	{
		WriteToModelHasBeenCalled = true;
	}
}
