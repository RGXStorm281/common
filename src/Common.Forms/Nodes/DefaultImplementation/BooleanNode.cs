namespace RobinEpple.Common.Forms.Nodes.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes.Formatters;

internal class BooleanNode : FieldNode, IBooleanNode
{
	public BooleanNode(string name, IParentNode parent)
		: base(name, parent, new BooleanFormatter("yes", "no")) { }

	/// <inheritdoc />
	public bool? Value { get; set; }

	/// <inheritdoc />
	public override void Reset()
	{
		base.Reset();
		Value = null;
	}

	/// <inheritdoc />
	public override async Task ResetAsync()
	{
		await base.ResetAsync();
		Value = null;
	}
}
