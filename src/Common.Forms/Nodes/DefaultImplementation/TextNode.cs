namespace RobinEpple.Common.Forms.Nodes.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes.Formatters;

internal class TextNode : FieldNode, ITextNode
{
	public TextNode(string name, IParentNode parent)
		: base(name, parent, new TrimTextFormatter()) { }

	/// <inheritdoc />
	public string? Value { get; set; }

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
