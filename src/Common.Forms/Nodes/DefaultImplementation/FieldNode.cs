namespace RobinEpple.Common.Forms.Nodes.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Binding;

internal class FieldNode : NodeBase, IFieldNode
{
	public FieldNode(string name, IParentNode parent, IValueFormatter defaultFormatter)
		: base(name, parent.Root, parent)
	{
		Formatter = defaultFormatter;
	}

	/// <inheritdoc />
	public bool HasUserInteraction { get; set; }

	/// <inheritdoc />
	public IValueFormatter Formatter { get; private set; }

	internal void UseFormatter(IValueFormatter formatter) => Formatter = formatter;

	/// <inheritdoc />
	public override void Reset()
	{
		base.Reset();
		HasUserInteraction = false;
	}

	/// <inheritdoc />
	public override async Task ResetAsync()
	{
		await base.ResetAsync();
		HasUserInteraction = false;
	}

	/// <inheritdoc />
	public override object Clone()
	{
		var clone = (FieldNode)base.Clone();
		clone.HasUserInteraction = HasUserInteraction;

		// Do not clone stateless decorators.
		clone.Formatter = Formatter;
		return clone;
	}
}
