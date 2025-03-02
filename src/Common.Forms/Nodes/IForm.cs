namespace RobinEpple.Common.Forms.Nodes;

public interface IForm : IParentNode
{
	/// <summary>
	/// The list of nodes in this form.<br/>
	/// The order does not imply a visual arrangement.
	/// </summary>
	public IEnumerable<IFormNode> Nodes { get; }
}
